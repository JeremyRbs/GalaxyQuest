using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;

namespace GalaxyQuest.Controllers
{
    public class UnitController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MapRepository _mapRepository;
        private readonly IClock _clock;
        public UnitController(UserRepository userRepository, MapRepository mapRepository, IClock clock) => (_userRepository, _mapRepository, _clock) = (userRepository, mapRepository, clock);

        [HttpGet("users/{userId}/Units")]
        public IActionResult GetUserUnits([FromRoute] string userId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return ResponseHelper.HandleResponse(userDTO?.Units, $"User with ID '{userId}' not found ...");
        }

        [HttpGet("users/{userId}/Units/{unitId}")]
        public async Task<IActionResult> GetSingleUserUnit([FromRoute]string userId, string unitId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            if (unit == null) { return NotFound(); }
            if (unit.EstimatedTimeOfArrival.HasValue) {
                var timeOfArrival = (unit.EstimatedTimeOfArrival.Value - _clock.Now).TotalSeconds;
                if (timeOfArrival <= 2)
                {
                    await unit.TaskUnit;
                } else
                {
                    unit.Planet = null;
                }
            }
            return ResponseHelper.HandleResponse(unit);
        }

        [HttpPut("users/{userId}/Units/{unitId}")]
        public IActionResult ChangeUnitStatus(string userId, string unitId, [FromBody] Unit unit)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? newUnit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            if (newUnit != null)
            {
                newUnit.MoveUnit(unit.DestinationSystem, unit.DestinationPlanet);
                newUnit.TaskUnit = newUnit.MoveAsync(unit, newUnit, _clock);
                return Ok(newUnit);
            }
            return NotFound();
        }

        [HttpGet("users/{userId}/Units/{unitId}/location")]
        public IActionResult GetSingleUserUnitLocation([FromRoute] string userId, string unitId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            PlanetDTO? planetDTO = _mapRepository.GetOnePlanetWithResources(unit.System, unit.Planet);
            if (planetDTO != null)
            {
                Location? unitLocation = new(unit.System, unit.DestinationPlanet, planetDTO?.ResourceQuantity);

                if (unit.Type == "builder")
                {
                    unitLocation.ResourcesQuantity = null;
                }
                return Ok(unitLocation);
            }
            return NotFound();
        }
    }
}
