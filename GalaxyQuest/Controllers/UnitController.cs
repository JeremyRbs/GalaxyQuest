using System.Net.Http.Headers;
using System.Text;
using GalaxyQuest.Buildings;
using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using GalaxyQuest.RequestBodies;
using GalaxyQuest.Units.CombatUnits;
using GalaxyQuest.Units.PeacefulUnits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace GalaxyQuest.Controllers
{
    public class UnitController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly MapRepository mapRepository;
        private readonly IClock clock;
        private Dictionary<string, Wormholes> wormholes;
        private readonly IHttpClientFactory httpClientFactory;

        public UnitController(UserRepository userRepository, MapRepository mapRepository, IClock clock, IOptions<Dictionary<string, Wormholes>> wormholes, IHttpClientFactory httpClientFactory)
        {
            (this.userRepository, this.mapRepository, this.clock, this.wormholes, this.httpClientFactory) = (userRepository, mapRepository, clock, wormholes.Value, httpClientFactory);
        }

        /// <summary>
        /// Gets all units for a specific user.
        /// </summary>
        [HttpGet("users/{userId}/Units")]
        public IActionResult GetUserUnits([FromRoute] string userId)
        {
            bool isResource = false;
            
            UserDto? userDTO = userRepository.Users.FirstOrDefault(u => u.Id == userId);
            
            foreach (var resourceKind in userDTO.ResourcesQuantity.Keys)
                if(userDTO.ResourcesQuantity[resourceKind] != 0)
                    isResource = true;
            
            return ResponseHelper.HandleResponse(userDTO?.Units, notFoundMessage: $"UnitController_GetUserUnits : User with ID '{userId}' not found ...");
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
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="unitId">The unique identifier of the unit.</param>
        /// <param name="postRequestUnitBody">The request body containing updated information for the unit.</param>
        /// <returns>An IActionResult representing the result of the unit status update.</returns>
        [HttpPut("users/{userId}/Units/{unitId}")]
        public async Task<IActionResult> ChangeUnitStatus(string userId, string unitId, [FromBody] PostRequestUnitBody postRequestUnitBody)
        {
            // Retrieve userDto and unit based on provided userId and unitId
            UserDto? userDto = userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDto?.Units.FirstOrDefault(u => u.Id == unitId);

            if (unit != null)
            {
                // Handle specific logic for Builder units with different destination planet
                if (unit.Type == Unit.UnitType.Builder.ToString().ToLower() && unit.Planet != postRequestUnitBody.DestinationPlanet)
                {
                    // Check for a building under construction and cancel it
                    Building buildingOnConstruction = userDto.Buildings.FirstOrDefault(b => b.IsBuilt == false && b.BuilderId == unit.Id);
                    if (buildingOnConstruction != null)
                    {
                        userDto.Buildings.Remove(buildingOnConstruction);
                        buildingOnConstruction.CancelBuildingProcess();
                    }
                }

                // Check if resources are equal before processing
                bool areResourcesEqual = unit.ResourcesQuantity.SequenceEqual(postRequestUnitBody.ResourcesQuantity);
                if (areResourcesEqual)
                {
                    // Handle the case where DestinationShard is specified
                    if (postRequestUnitBody.DestinationShard != null)
                    {
                        // Retrieve the first wormhole from the dictionary
                        var wormhole = wormholes.FirstOrDefault().Value;

                        // Create an HttpClient and configure it
                        var client = httpClientFactory.CreateClient();
                        client.BaseAddress = new Uri(wormhole.BaseUri.ToString());
                        client.DefaultRequestHeaders.Authorization = CreateShardAuthorizationHeader(wormhole.User, wormhole.SharedPassword);

                        // Update userDto and unit on the remote server
                        var response = await client.PutAsJsonAsync($"users/{userId}", userDto);
                        response = await client.PutAsJsonAsync($"users/{userId}/units/{unitId}", unit);

                        // Remove the unit from the local userDto
                        userDto.Units.Remove(unit);

                        // Redirect to the updated unit's location
                        return new RedirectResult(wormhole.BaseUri.ToString() + $"users/{userId}/units/{unitId}", true, true);
                    }
                    else
                    {
                        // Handle the case where DestinationShard is not specified
                        unit.MoveUnit(postRequestUnitBody.DestinationSystem, postRequestUnitBody.DestinationPlanet);
                        unit.TaskUnit = unit.MoveAsync(postRequestUnitBody, unit, clock);
                    }
                }
                else
                {
                    // Handle the case where resources are not equal
                    if (postRequestUnitBody.ResourcesQuantity != null &&
                        postRequestUnitBody.ResourcesQuantity.Any(resource => resource.Value > 0))
                    {
                        if (unit.Type == Unit.UnitType.Cargo.ToString().ToLower())
                        {
                            // Handle resource deduction for Cargo units
                            CargoUnit cargo = (CargoUnit)unit;
                            List<Building> userStarports = userDto.GetBuildingByType("starport");

                            if (userStarports != null && userStarports.Any(starport => starport.Planet == cargo.Planet) && !cargo.IsMoving)
                            {
                                foreach (var resource in postRequestUnitBody.ResourcesQuantity)
                                {
                                    int bodyQuantity = resource.Value;
                                    int cargoQuantity = cargo.ResourcesQuantity[resource.Key];
                                    int differenceQuantity = bodyQuantity - cargoQuantity;

                                    if (differenceQuantity > 0)
                                    {
                                        // Deduct resources from the userDto and update CargoUnit resources
                                        if (userDto.ResourcesQuantity[resource.Key] <= differenceQuantity)
                                        {
                                            return BadRequest("UnitController_ChangeUnitStatus: User has not enough resources to build this unit.");
                                        }

                                        cargo.ResourcesQuantity[resource.Key] += differenceQuantity;
                                        userDto.ResourcesQuantity[resource.Key] -= differenceQuantity;
                                    }
                                    else if (differenceQuantity < 0)
                                    {
                                        // Deduct resources from the CargoUnit and update userDto resources
                                        userDto.ResourcesQuantity[resource.Key] -= differenceQuantity;
                                        cargo.ResourcesQuantity[resource.Key] += differenceQuantity;
                                    }
                                }

                                // Handle the response for the updated CargoUnit
                                return ResponseHelper.HandleResponse(cargo);
                            }

                            return BadRequest("UnitController_ChangeUnitStatus: Not the same planet as cargo");
                        }

                        return BadRequest("UnitController_ChangeUnitStatus: Invalid unit type (expected: Cargo).");
                    }
                }

                // Handle the response for the updated unit
                return ResponseHelper.HandleResponse(unit);
            }

            // Handle the case where the unit is not found
            if (User.IsInRole("admin"))
            {
                // Handle the case where the user is an admin and create a new unit based on the specified type
                switch (postRequestUnitBody.Type)
                {
                    case "scout":
                        ScoutUnit scout = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId);
                        userDto.AddUnit(scout);
                        return ResponseHelper.HandleResponse(scout);
                    case "builder":
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
                    case "cargo":
                        CargoUnit cargo = new(unitId, postRequestUnitBody.System, postRequestUnitBody.Planet,
                            postRequestUnitBody.System, postRequestUnitBody.Planet, userId,
                            postRequestUnitBody.ResourcesQuantity, postRequestUnitBody.Health);
                        userDto.AddUnit(cargo);
                        return ResponseHelper.HandleResponse(cargo);
                    default:
                        break;
                }
            }

            // Handle the case where the user is a shard and create a new CargoUnit with wormholes information
            if (User.IsInRole("shard"))
            {
                string wormHolesSystem = wormholes.FirstOrDefault().Value.System.ToString();
                CargoUnit cargo = new(postRequestUnitBody.Id, wormHolesSystem, null, null, null, userId, postRequestUnitBody.ResourcesQuantity, postRequestUnitBody.Health);
                userDto.AddUnit(cargo);

                // Handle the response for the newly created CargoUnit
                return ResponseHelper.HandleResponse(cargo);
            }

            // Handle the case where the user credentials are unauthorized
            return Unauthorized("UnitController_ChangeUnitStatus: Bad user credentials.");
        }

        /// <summary>
        /// Creates an authentication header for a shard using Basic authentication.
        /// </summary>
        /// <param name="shardName">The name of the shard.</param>
        /// <param name="sharedKey">The shared key associated with the shard.</param>
        /// <returns>An AuthenticationHeaderValue for Basic authentication.</returns>
        private static AuthenticationHeaderValue CreateShardAuthorizationHeader(string shardName, string sharedKey)
            => new("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"shard-{shardName}:{sharedKey}")));


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
