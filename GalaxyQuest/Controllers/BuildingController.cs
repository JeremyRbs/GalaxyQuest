using System.Collections.Immutable;
using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Buildings;
using GalaxyQuest.RequestBodies;
using GalaxyQuest.Services;
using GalaxyQuest.Units;
using GalaxyQuest.Utilities;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;

namespace GalaxyQuest.Controllers
{
    public class BuildingController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MapRepository _mapRepository;
        private readonly IClock _clock;

        public BuildingController(UserRepository userRepository, MapRepository mapRepository, IClock clock)
        {
            _userRepository = userRepository;
            _mapRepository = mapRepository;
            _clock = clock;
        }

        /// <summary>
        /// Creates a new building for the specified user.
        /// </summary>
        [HttpPost("users/{userId}/Buildings")]
        public async Task<IActionResult> CreateBuilding([FromRoute] string userId, [FromBody] PostRequestBuildingBody buildingRequest)
        {
            // Check if the user exists.
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            if (userDTO == null) return NotFound("User not found.");

            // Check if the user has a builder unit and its destination is set.
            Unit unit = userDTO.Units.FirstOrDefault(u => u.Type == "builder");
            if (unit == null ||
                unit.Type != "builder" ||
                string.IsNullOrEmpty(unit.DestinationSystem) ||
                string.IsNullOrEmpty(unit.DestinationPlanet))
            {
                return BadRequest("Invalid builder unit or missing destination.");
            }

            // Validate the building request.
            if (buildingRequest != null &&
                buildingRequest.BuilderId != null &&
                buildingRequest.BuilderId == unit.Id)
            {
                if (buildingRequest.Type == "mine")
                {

                    // Validate the resource category.
                    if (buildingRequest.ResourceCategory != "liquid" &&
                        buildingRequest.ResourceCategory != "gaseous" &&
                        buildingRequest.ResourceCategory != "solid")
                    {
                        return BadRequest("Invalid resource category.");
                    }

                    // Create a new building of "mine" type and add it to the user's list of buildings.
                    MineBuilding mine = new(
                        unit.Id,
                        buildingRequest.ResourceCategory,
                        unit.System,
                        unit.Planet,
                        _clock,
                        _userRepository,
                        _mapRepository
                    );
                    
                    userDTO.Buildings.Add(mine);
                    
                    BuildingDTO mineDto = new BuildingDTO(mine.Id, mine.Type, mine.BuilderId, mine.ResourceCategory, mine.System, mine.Planet, mine.IsBuilt, mine.EstimatedBuildTime);

                    // Return the building or an error response.
                    return buildingRequest.BuilderId == null || buildingRequest.BuilderId != unit.Id
                        ? BadRequest("Invalid builder ID format or mismatch.")
                        : Ok(mineDto);
                }
                else if (buildingRequest.Type == "starport")
                {
                    // Create a new building of "mine" type and add it to the user's list of buildings.
                    StarportBuilding starport = new(
                        unit.Id,
                        buildingRequest.ResourceCategory,
                        unit.System,
                        unit.Planet,
                        _clock,
                        _userRepository,
                        _mapRepository
                    );
                    
                    userDTO.Buildings.Add(starport);

                    BuildingDTO buildingDto = new BuildingDTO(starport.Id, starport.Type, starport.BuilderId, starport.ResourceCategory, starport.System, starport.Planet, starport.IsBuilt, starport.EstimatedBuildTime);
                    
                    // Return the building or an error response.
                    return buildingRequest.BuilderId == null || buildingRequest.BuilderId != unit.Id
                        ? BadRequest("Invalid builder ID format or mismatch.")
                        : Ok(buildingDto);
                }
                return BadRequest("Invalid building request.");
            }
            return BadRequest("Invalid building request.");
        }

        /// <summary>
        /// Gets all buildings for the specified user.
        /// </summary>
        [HttpGet("users/{userId}/Buildings")]
        public IActionResult GetBuildings([FromRoute] string userId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return ResponseHelper.HandleResponse(userDTO?.Buildings.Select(b => new BuildingDTO(b.Id, b.Type, b.BuilderId,  b.ResourceCategory, b.System, b.Planet, b.IsBuilt, b.EstimatedBuildTime)).ToList(), $"User with ID '{userId}' not found ...");
        }

        /// <summary>
        /// Gets details of a specific building for the specified user.
        /// </summary>
        [HttpGet("users/{userId}/Buildings/{buildingId}")]
        public async Task<IActionResult> GetOneBuilding(string userId, string buildingId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            if (userDTO == null) return NotFound("User not found.");
            Building? building = userDTO?.Buildings.FirstOrDefault(u => u.Id == buildingId);
            if (building == null) { return NotFound("Building not found."); }
            Unit? unit = _userRepository.GetUnits().FirstOrDefault(u => u.Id == building.BuilderId);

            // Check if the building is still under construction.
            if (building.EstimatedBuildTime.HasValue)
            {
                TimeSpan timeRemaining = (TimeSpan)(building.EstimatedBuildTime - _clock.Now);
                if (!building.IsBuilt && timeRemaining <= TimeSpan.FromSeconds(2))
                {
                    await building.TaskBuilding;
                }
            }

            // If the building is no longer in the user's list, it means it was canceled during construction.
            if (!userDTO.Buildings.Contains(building))
            {
                return NotFound("Building construction canceled.");
            }

            BuildingDTO buildingDto = new BuildingDTO(building.Id, building.Type, building.BuilderId, building.ResourceCategory, building.System, building.Planet, building.IsBuilt, building.EstimatedBuildTime);
            
            // Return the building or an error response.
            return ResponseHelper.HandleResponse(buildingDto);
        }

        [HttpPost("users/{userId}/Buildings/{starportId}/queue")]
        public async Task<IActionResult> QueueBuilding([FromRoute] string userId, string starportId, [FromBody] PostRequestUnitBody postRequestUnitBody)
        {
            UserDTO? userDto = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            if (userDto == null) return NotFound("User not found.");

            Building building = userDto?.Buildings.FirstOrDefault(u => u.Id == starportId);
            if (building == null) {return NotFound("Building not found.");}
            if (building.Type != "starport") return BadRequest("Building isn't a starport");
            StarportBuilding starport = (StarportBuilding)(building);
            if (starport.IsBuilt == false) return BadRequest("Starport is not build yet");

            switch (postRequestUnitBody.Type)
            {
                case "scout":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        ScoutUnit scout = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(scout);
                        return Ok(scout);
                    }
                    return BadRequest("Not enough resources to build");
                case "builder":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        BuilderUnit builder = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(builder);
                        return Ok(builder);
                    }
                    return BadRequest("Not enough resources to build");
                case "fighter":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        FighterUnit fighter = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(fighter);
                        return Ok(fighter);
                    }
                    return BadRequest("Not enough resources to build");
                case "bomber":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        BomberUnit bomber = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(bomber);
                        return Ok(bomber);
                    }
                    return BadRequest("Not enough resources to build");
                case "cruiser":
                    if (userDto.BuildResources(postRequestUnitBody.Type))
                    {
                        CruiserUnit cruiser = new (starport.System, starport.Planet, userDto.Id);
                        starport.AddUnitToQueue(cruiser);
                        return Ok(cruiser);
                    }
                    return BadRequest("Not enough resources to build");
                default: 
                    return BadRequest("Unit type didn't exist");
            }
        }
    }
}
