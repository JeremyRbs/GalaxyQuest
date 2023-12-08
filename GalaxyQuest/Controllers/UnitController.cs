using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using System.Threading.Tasks;
using GalaxyQuest.RequestBodies;
using GalaxyQuest.Units;
using Microsoft.AspNetCore.Authorization;

namespace GalaxyQuest.Controllers
{
    public class UnitController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MapRepository _mapRepository;
        private readonly IClock _clock;

        public UnitController(UserRepository userRepository, MapRepository mapRepository, IClock clock)
        {
            (_userRepository, _mapRepository, _clock) = (userRepository, mapRepository, clock);
        }

        /// <summary>
        /// Gets all units for a specific user.
        /// </summary>
        [HttpGet("users/{userId}/Units")]
        public IActionResult GetUserUnits([FromRoute] string userId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return ResponseHelper.HandleResponse(userDTO?.Units, $"User with ID '{userId}' not found ...");
        }

        /// <summary>
        /// Gets details of a specific unit for a user.
        /// </summary>
        [HttpGet("users/{userId}/Units/{unitId}")]
        public async Task<IActionResult> GetSingleUserUnit([FromRoute] string userId, string unitId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);

            if (unit == null)
            {
                return ResponseHelper.HandleResponse<Unit>(null, notFoundMessage: "Unit not found");
            }

            if (unit.EstimatedTimeOfArrival.HasValue)
            {
                var timeOfArrival = (unit.EstimatedTimeOfArrival.Value - _clock.Now).TotalSeconds;
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
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);

            if (unit != null)
            {
                if (unit.Type == "builder" && unit.Planet != postRequestUnitBody.DestinationPlanet)
                {
                    Building buildingOnConstruction =
                        userDTO.Buildings.FirstOrDefault(b => b.IsBuilt == false && b.BuilderId == unit.Id);
                    if (buildingOnConstruction != null)
                    {
                        userDTO.Buildings.Remove(buildingOnConstruction);
                        buildingOnConstruction.CancelBuildingProcess();
                    }
                }

                unit.MoveUnit(postRequestUnitBody.DestinationSystem, postRequestUnitBody.DestinationPlanet);
                unit.TaskUnit = unit.MoveAsync(postRequestUnitBody, unit, _clock);

                return ResponseHelper.HandleResponse(unit);
            }

            if (User.Identity.IsAuthenticated)
            {
                switch (postRequestUnitBody.Type)
                {
                    case "scout":
                        Unit scout = new(unitId, "scout", postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDTO.AddUnit(scout);
                        return ResponseHelper.HandleResponse(scout);
                    case "builder":
                        Unit builder = new(unitId, "builder", postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDTO.AddUnit(builder);
                        return ResponseHelper.HandleResponse(builder);
                    /*
                    case "cruiser":
                        CruiserUnit cruiser = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDTO.AddUnit(cruiser);
                        return ResponseHelper.HandleResponse(cruiser);
                    case "fighter":
                        FighterUnit fighter = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDTO.AddUnit(fighter);
                        return ResponseHelper.HandleResponse(fighter);
                    case "bomber":
                        BomberUnit bomber = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDTO.AddUnit(bomber);
                        return ResponseHelper.HandleResponse(bomber);
                        */
                    default:
                        break;
                }
                return ResponseHelper.HandleResponse<Unit>(null, notFoundMessage: "Unit not found");
            }

            return Unauthorized();
        }

        /// <summary>
        /// Gets the location details of a specific unit for a user.
        /// </summary>
        [HttpGet("users/{userId}/Units/{unitId}/location")]
        public IActionResult GetSingleUserUnitLocation([FromRoute] string userId, string unitId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            PlanetDTO? planetDTO = _mapRepository.GetOnePlanetWithResources(unit.System, unit.Planet);

            if (planetDTO != null)
            {
                Location? unitLocation = new Location(unit.System, unit.DestinationPlanet, planetDTO?.ResourceQuantity);

                if (unit.Type == "builder")
                {
                    unitLocation.ResourcesQuantity = null;
                }

                return ResponseHelper.HandleResponse(unitLocation);
            }

            return ResponseHelper.HandleResponse<Location>(null, notFoundMessage: "Location not found");
        }
    }
}
