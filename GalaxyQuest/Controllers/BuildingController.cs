using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Buildings;
using GalaxyQuest.RequestBodies;
using GalaxyQuest.Services;
using GalaxyQuest.Units.CombatUnits;
using GalaxyQuest.Units.PeacefulUnits;
using GalaxyQuest.Utilities;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;

namespace GalaxyQuest.Controllers
{
    public class BuildingController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly MapRepository mapRepository;
        private readonly IClock clock;

        public BuildingController(UserRepository userRepository, MapRepository mapRepository, IClock clock)
        {
            this.userRepository = userRepository;
            this.mapRepository = mapRepository;
            this.clock = clock;
        }

        /// <summary>
        /// Creates a new building for the specified user.
        /// </summary>
        [HttpPost("users/{userId}/Buildings")]
        public async Task<IActionResult> CreateBuilding([FromRoute] string userId, [FromBody] PostRequestBuildingBody buildingRequest)
        {
            // Check if the user exists.
            UserDto? userDTO = userRepository.Users.FirstOrDefault(u => u.Id == userId);
            if (userDTO == null) return NotFound("BuildingController_CreateBuilding : User not found.");

            // Check if the user has a builder unit and its destination is set.
            Unit unit = userDTO.Units.FirstOrDefault(u => u.Type == Unit.UnitType.Builder.ToString().ToLower());
            if (unit == null ||
                unit.Type != Unit.UnitType.Builder.ToString().ToLower() ||
                string.IsNullOrEmpty(unit.DestinationSystem) ||
                string.IsNullOrEmpty(unit.DestinationPlanet))
            {
                return BadRequest("BuildingController_CreateBuilding : Invalid builder unit or missing destination.");
            }

            // Validate the building request.
            if (buildingRequest != null &&
                buildingRequest.BuilderId != null &&
                buildingRequest.BuilderId == unit.Id)
            {
                if (buildingRequest.Type == Building.BuildingType.Mine.ToString().ToLower())
                {

                    // Validate the resource category.
                    if (buildingRequest.ResourceCategory != Building.BuildingResourceCategory.Liquid.ToString().ToLower() &&
                        buildingRequest.ResourceCategory != Building.BuildingResourceCategory.Gaseous.ToString().ToLower() &&
                        buildingRequest.ResourceCategory != Building.BuildingResourceCategory.Solid.ToString().ToLower())
                    {
                        return BadRequest("BuildingController_CreateBuilding : Invalid resource category.");
                    }

                    // Create a new building of "mine" type and add it to the user's list of buildings.
                    MineBuilding mine = new(
                        unit.Id,
                        buildingRequest.ResourceCategory,
                        unit.System,
                        unit.Planet,
                        clock,
                        userRepository,
                        mapRepository
                    );
                    
                    userDTO.Buildings.Add(mine);
                    
                    BuildingDto mineDto = new BuildingDto(mine.Id, mine.Type, mine.BuilderId, mine.ResourceCategory, mine.System, mine.Planet, mine.IsBuilt, mine.EstimatedBuildTime);

                    // Return the building or an error response.
                    return buildingRequest.BuilderId == null || buildingRequest.BuilderId != unit.Id
                        ? BadRequest("BuildingController_CreateBuilding : Invalid builder ID format or mismatch for mine creation.")
                        : Ok(mineDto);
                }
                else if (buildingRequest.Type == Building.BuildingType.Starport.ToString().ToLower())
                {
                    // Create a new building of "mine" type and add it to the user's list of buildings.
                    StarportBuilding starport = new(
                        unit.Id,
                        buildingRequest.ResourceCategory,
                        unit.System,
                        unit.Planet,
                        clock,
                        userRepository,
                        mapRepository
                    );
                    
                    userDTO.Buildings.Add(starport);

                    BuildingDto buildingDto = new BuildingDto(starport.Id, starport.Type, starport.BuilderId, starport.ResourceCategory, starport.System, starport.Planet, starport.IsBuilt, starport.EstimatedBuildTime);
                    
                    // Return the building or an error response.
                    return buildingRequest.BuilderId == null || buildingRequest.BuilderId != unit.Id
                        ? BadRequest("BuildingController_CreateBuilding : Invalid builder ID format or mismatch for starport creation.")
                        : Ok(buildingDto);
                }
                return BadRequest("BuildingController_CreateBuilding : Invalid building request.");
            }
            return BadRequest("BuildingController_CreateBuilding : Invalid building request.");
        }

        /// <summary>
        /// Gets all buildings for the specified user.
        /// </summary>
        [HttpGet("users/{userId}/Buildings")]
        public IActionResult GetBuildings([FromRoute] string userId)
        {
            UserDto? userDTO = userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return ResponseHelper.HandleResponse(userDTO?.Buildings.Select(b => new BuildingDto(b.Id, b.Type, b.BuilderId,  b.ResourceCategory, b.System, b.Planet, b.IsBuilt, b.EstimatedBuildTime)).ToList(), $"BuildingController_GetBuildings : User with ID '{userId}' not found ...");
        }

        /// <summary>
        /// Gets details of a specific building for the specified user.
        /// </summary>
        [HttpGet("users/{userId}/Buildings/{buildingId}")]
        public async Task<IActionResult> GetOneBuilding(string userId, string buildingId)
        {
            UserDto? userDTO = userRepository.Users.FirstOrDefault(u => u.Id == userId);
            if (userDTO == null) return NotFound("BuildingController_GetOneBuilding : User not found.");
            Building? building = userDTO?.Buildings.FirstOrDefault(u => u.Id == buildingId);
            if (building == null) { return NotFound("BuildingController_GetOneBuilding : Building not found."); }
            Unit? unit = userRepository.GetUnits().FirstOrDefault(u => u.Id == building.BuilderId);

            // Check if the building is still under construction.
            if (building.EstimatedBuildTime.HasValue)
            {
                TimeSpan timeRemaining = (TimeSpan)(building.EstimatedBuildTime - clock.Now);
                if (!building.IsBuilt && timeRemaining <= TimeSpan.FromSeconds(2))
                {
                    await building.TaskBuilding;
                }
            }

            // If the building is no longer in the user's list, it means it was canceled during construction.
            if (!userDTO.Buildings.Contains(building))
            {
                return NotFound("BuildingController_GetOneBuilding : Building construction canceled.");
            }

            BuildingDto buildingDto = new BuildingDto(building.Id, building.Type, building.BuilderId, building.ResourceCategory, building.System, building.Planet, building.IsBuilt, building.EstimatedBuildTime);
            
            // Return the building or an error response.
            return ResponseHelper.HandleResponse(buildingDto);
        }

        /// <summary>
        /// Handles the queuing of unit creation in a starport building for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="starportId">The unique identifier of the starport building.</param>
        /// <param name="postRequestUnitBody">The request body containing information for unit creation.</param>
        /// <returns>An asynchronous task representing the HTTP action result.</returns>
        [HttpPost("users/{userId}/Buildings/{starportId}/queue")]
        public async Task<IActionResult> QueueBuilding([FromRoute] string userId, string starportId, [FromBody] PostRequestUnitBody postRequestUnitBody)
        {
            // Retrieve the user from the repository based on the provided userId
            UserDto? userDto = userRepository.Users.FirstOrDefault(u => u.Id == userId);
            
            // Check if the user exists
            if (userDto == null)
                return NotFound("BuildingController_QueueBuilding : User not found.");

            // Retrieve the building (starport) from the user's buildings based on the provided starportId
            Building building = userDto?.Buildings.FirstOrDefault(u => u.Id == starportId);
            
            // Check if the building exists and is of the correct type (starport)
            if (building == null)
                return NotFound("BuildingController_QueueBuilding : Building not found.");
            
            if (building.Type != "starport")
                return BadRequest("BuildingController_QueueBuilding : Building isn't a starport");

            // Convert the building to a StarportBuilding for additional operations
            StarportBuilding starport = (StarportBuilding)(building);

            // Check if the starport is built
            if (starport.IsBuilt == false)
                return BadRequest("BuildingController_QueueBuilding : Starport is not built yet");

            // Process different unit types
            switch (postRequestUnitBody.Type)
            {
                case "scout":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        ScoutUnit scout = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(scout);
                        return Ok(scout);
                    }
                    return BadRequest("BuildingController_QueueBuilding : Not enough resources to build scout unit.");
                case "builder":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        BuilderUnit builder = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(builder);
                        return Ok(builder);
                    }
                    return BadRequest("BuildingController_QueueBuilding : Not enough resources to build builder unit.");
                case "fighter":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        FighterUnit fighter = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(fighter);
                        return Ok(fighter);
                    }
                    return BadRequest("BuildingController_QueueBuilding : Not enough resources to build fighter unit.");
                case "bomber":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        BomberUnit bomber = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(bomber);
                        return Ok(bomber);
                    }
                    return BadRequest("BuildingController_QueueBuilding : Not enough resources to build bomber unit.");
                case "cruiser":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        CruiserUnit cruiser = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(cruiser);
                        return Ok(cruiser);
                    }
                    return BadRequest("BuildingController_QueueBuilding : Not enough resources to build cruiser unit.");
                default:
                    return BadRequest("BuildingController_QueueBuilding : Unit type doesn't exist");
            }
        }
    }
}
