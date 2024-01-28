using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using GalaxyQuest.RequestBodies;
using GalaxyQuest.Units.CombatUnits;
using GalaxyQuest.Units.PeacefulUnits;

namespace GalaxyQuest.Controllers
{
    public class UnitController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly MapRepository mapRepository;
        private readonly IClock clock;

        public UnitController(UserRepository userRepository, MapRepository mapRepository, IClock clock)
        {
            (this.userRepository, this.mapRepository, this.clock) = (userRepository, mapRepository, clock);
        }

        /// <summary>
        /// Gets all units for a specific user.
        /// </summary>
        [HttpGet("users/{userId}/Units")]
        public IActionResult GetUserUnits([FromRoute] string userId)
        {
            UserDto? userDTO = userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return ResponseHelper.HandleResponse(userDTO?.Units, $"UnitController_GetUserUnits : User with ID '{userId}' not found ...");
        }

        /// <summary>
        /// Gets details of a specific unit for a user.
        /// </summary>
        [HttpGet("users/{userId}/Units/{unitId}")]
        public async Task<IActionResult> GetSingleUserUnit([FromRoute] string userId, string unitId)
        {
            UserDto? userDTO = userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);

            if (unit == null)
            {
                return ResponseHelper.HandleResponse<Unit>(null, notFoundMessage: "UnitController_GetSingleUserUnit : Unit not found");
            }

            if (unit.EstimatedTimeOfArrival.HasValue)
            {
                var timeOfArrival = (unit.EstimatedTimeOfArrival.Value - clock.Now).TotalSeconds;
                await (timeOfArrival <= 2 ? unit.TaskUnit : Task.CompletedTask);
                unit.Planet = timeOfArrival > 2 ? null : unit.Planet;
            }

            return ResponseHelper.HandleResponse(unit);
        }

        /// <summary>
        /// Updates the status of a specific unit for a user.
        /// </summary>
        [HttpPut("users/{userId}/Units/{unitId}")]
        public IActionResult ChangeUnitStatus(string userId, string unitId, [FromBody] PostRequestUnitBody postRequestUnitBody)
        {
            UserDto? userDto = userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDto?.Units.FirstOrDefault(u => u.Id == unitId);

            if (unit != null)
            {
                if (unit.Type == Unit.UnitType.Builder.ToString().ToLower() && unit.Planet != postRequestUnitBody.DestinationPlanet)
                {
                    Building buildingOnConstruction =
                        userDto.Buildings.FirstOrDefault(b => b.IsBuilt == false && b.BuilderId == unit.Id);
                    if (buildingOnConstruction != null)
                    {
                        userDto.Buildings.Remove(buildingOnConstruction);
                        buildingOnConstruction.CancelBuildingProcess();
                    }
                }

                unit.MoveUnit(postRequestUnitBody.DestinationSystem, postRequestUnitBody.DestinationPlanet);
                unit.TaskUnit = unit.MoveAsync(postRequestUnitBody, unit, clock);

                return ResponseHelper.HandleResponse(unit);
            }

            if (User.Identity.IsAuthenticated)
            {
                switch (postRequestUnitBody.Type)
                {
                    case "scout":
                        ScoutUnit scout = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDto.AddUnit(scout);
                        return ResponseHelper.HandleResponse(scout);
                    case "builder" :
                        BuilderUnit builder = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDto.AddUnit(builder);
                        return ResponseHelper.HandleResponse(builder);
                    case "cruiser":
                        CruiserUnit cruiser = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDto.AddUnit(cruiser);
                        return ResponseHelper.HandleResponse(cruiser);
                    case "fighter":
                        FighterUnit fighter = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDto.AddUnit(fighter);
                        return ResponseHelper.HandleResponse(fighter);
                    case "bomber":
                        BomberUnit bomber = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDto.AddUnit(bomber);
                        return ResponseHelper.HandleResponse(bomber);
                    default:
                        break;
                }
                return ResponseHelper.HandleResponse<Unit>(null, notFoundMessage: "UnitController_ChangeUnitStatus : Unit not found");
            }

            return Unauthorized();
        }

        /// <summary>
        /// Gets the location details of a specific unit for a user.
        /// </summary>
        [HttpGet("users/{userId}/Units/{unitId}/location")]
        public IActionResult GetSingleUserUnitLocation([FromRoute] string userId, string unitId)
        {
            UserDto? userDTO = userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            PlanetDto? planetDTO = mapRepository.GetOnePlanetWithResources(unit.System, unit.Planet);

            if (planetDTO != null)
            {
                Location? unitLocation = new Location(unit.System, unit.DestinationPlanet, planetDTO?.ResourceQuantity);

                if (unit.Type == Unit.UnitType.Builder.ToString().ToLower())
                {
                    unitLocation.ResourcesQuantity = null;
                }

                return ResponseHelper.HandleResponse(unitLocation);
            }

            return ResponseHelper.HandleResponse<Location>(null, notFoundMessage: "UnitController_GetSingleUserUnitLocation : Location not found");
        }
    }
}
