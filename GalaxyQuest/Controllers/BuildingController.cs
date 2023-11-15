using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Services;
using GalaxyQuest.Utilities;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;

namespace GalaxyQuest.Controllers
{
    public class BuildingController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IClock _clock;

        public BuildingController(UserRepository userRepository, IClock clock) { 
            _userRepository = userRepository;
            _clock = clock;
        }

        [HttpPost("users/{userId}/Buildings")]
        public async Task<IActionResult> CreateBuilding([FromRoute] string userId, [FromBody] Building building)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            if (userDTO == null) return NotFound();

            Unit unit = userDTO.Units.FirstOrDefault(u => u.Type == "builder");
            if (unit == null || 
                unit.Type != "builder" || 
                string.IsNullOrEmpty(unit.DestinationSystem) || 
                string.IsNullOrEmpty(unit.DestinationPlanet))
            {
                return BadRequest();
            }

            if (building == null || 
                building.BuilderId == null || 
                building.BuilderId != unit.Id || 
                building.Type != "mine"
                ) return BadRequest();
            
            Building newBuilding = new Building
            {
                Id = Guid.NewGuid().ToString(),
                Type = building.Type,
                ResourceCategory = building.ResourceCategory,
                System = unit.System,
                Planet = unit.Planet,
                IsBuilt = false,
                EstimatedBuildTime = _clock.Now.AddMinutes(5),
            };
            newBuilding.Built(userDTO, _clock);
            userDTO.AddBuilding(newBuilding);

            return building.BuilderId == null || building.BuilderId != unit.Id ? BadRequest() : Ok(newBuilding);
        }

        [HttpGet("users/{userId}/Buildings")]
        public IActionResult GetBuildings([FromRoute] string userId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return ResponseHelper.HandleResponse(userDTO?.Buildings, $"User with ID '{userId}' not found ...");
        }

        [HttpGet("users/{userId}/Buildings/{buildingId}")]
        public IActionResult GetOneBuilding(string userId, string buildingId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            if (userDTO == null) return NotFound();
            Building? building = userDTO?.Buildings.FirstOrDefault(u => u.Id == buildingId);
            if (building == null) { return NotFound(); }

            return ResponseHelper.HandleResponse(building);
        }
    }
}
