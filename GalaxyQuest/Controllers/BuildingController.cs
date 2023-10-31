using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyQuest.Controllers
{
    public class BuildingController : Controller
    {
        private readonly UserRepository _userRepository;

        public BuildingController(UserRepository userRepository) { 
            _userRepository = userRepository;
        }

        [HttpPost("users/{userId}/Buildings")]
        public IActionResult CreateBuilding([FromRoute] string userId, [FromBody] Building building)
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

            if (building == null || building.BuilderId == null || building.BuilderId != unit.Id || building.Type != "mine") return BadRequest();

            Building newBuilding = new Building
            {
                Id = Guid.NewGuid().ToString(),
                Type = building.Type,
                System = unit.System,
                Planet = unit.Planet
            };

            return building.BuilderId == null || building.BuilderId != unit.Id ? BadRequest() : Ok(newBuilding);
        }
    }
}
