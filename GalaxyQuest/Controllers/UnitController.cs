using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyQuest.Controllers
{
    public class UnitController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MapRepository _mapRepository;
        public UnitController(UserRepository userRepository, MapRepository mapRepository) => (_userRepository, _mapRepository) = (userRepository, mapRepository);

        [HttpGet("users/{userId}/Units")]
        public IActionResult GetUserUnits([FromRoute] string userId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return ResponseHelper.HandleResponse(userDTO?.Units, $"User with ID '{userId}' not found ...");
        }

        [HttpGet("users/{userId}/Units/{unitId}")]
        public IActionResult GetSingleUserUnit([FromRoute] string userId, string unitId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            return ResponseHelper.HandleResponse(unit);
        }

        [HttpPut("users/{userId}/Units/{unitId}")]
        public IActionResult ChangeUnitStatus(string userId, string unitId, [FromBody] Unit unit)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? newUnit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            if (newUnit != null)
            {
                newUnit.MoveUnit(unit.System, unit.Planet);
                return Ok(newUnit);
            }
            return NotFound();
        }

        [HttpGet("users/{userId}/Units/{unitId}/location")]
        public IActionResult GetSingleUserUnitLocation([FromRoute] string userId, string unitId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            if (unit != null)
            {
                PlanetDTO? planetDTO = _mapRepository.GetOnePlanetWithResources(unit.System, unit.Planet);
                if(planetDTO != null) {
                    Location? unitLocation = new(unit.System, unit.Planet, planetDTO?.ResourceQuantity);
                    return ResponseHelper.HandleResponse(unitLocation);
                }
            }
            return NotFound();
        }
    }
}
