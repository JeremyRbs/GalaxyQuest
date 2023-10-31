using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
=======
using Shard.Shared.Core;
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

namespace GalaxyQuest.Controllers
{
    public class UnitController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MapRepository _mapRepository;
<<<<<<< HEAD
        public UnitController(UserRepository userRepository, MapRepository mapRepository) => (_userRepository, _mapRepository) = (userRepository, mapRepository);
=======
        private readonly IClock _clock;
        public UnitController(UserRepository userRepository, MapRepository mapRepository, IClock clock) => (_userRepository, _mapRepository, _clock) = (userRepository, mapRepository, clock);
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

        [HttpGet("users/{userId}/Units")]
        public IActionResult GetUserUnits([FromRoute] string userId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return ResponseHelper.HandleResponse(userDTO?.Units, $"User with ID '{userId}' not found ...");
        }

        [HttpGet("users/{userId}/Units/{unitId}")]
<<<<<<< HEAD
        public IActionResult GetSingleUserUnit([FromRoute] string userId, string unitId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
=======
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
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
            return ResponseHelper.HandleResponse(unit);
        }

        [HttpPut("users/{userId}/Units/{unitId}")]
        public IActionResult ChangeUnitStatus(string userId, string unitId, [FromBody] Unit unit)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? newUnit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
            if (newUnit != null)
            {
<<<<<<< HEAD
                newUnit.MoveUnit(unit.System, unit.Planet);
=======
                newUnit.MoveUnit(unit.DestinationSystem, unit.DestinationPlanet);
                newUnit.TaskUnit = MoveAsync(unit, newUnit);
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
                return Ok(newUnit);
            }
            return NotFound();
        }

        [HttpGet("users/{userId}/Units/{unitId}/location")]
        public IActionResult GetSingleUserUnitLocation([FromRoute] string userId, string unitId)
        {
            UserDTO? userDTO = _userRepository.Users.FirstOrDefault(s => s.Id == userId);
            Unit? unit = userDTO?.Units.FirstOrDefault(u => u.Id == unitId);
<<<<<<< HEAD
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
=======
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

        public async Task MoveAsync(Unit unitBody, Unit? unit)
        {
            unit.DestinationPlanet = unitBody.DestinationPlanet;
            unit.DestinationSystem = unitBody.DestinationSystem;

            if (unitBody.DestinationSystem == unitBody.System)
            {
                if (unitBody.DestinationPlanet != null)
                {
                    unit.EstimatedTimeOfArrival = _clock.Now.AddSeconds(15);
                    await _clock.Delay(15000);
                }
                else
                {
                    unit.EstimatedTimeOfArrival = _clock.Now;
                }

                unit.Planet = unitBody.DestinationPlanet;
            }
            else if (unitBody.DestinationSystem != unit.System)
            {
                unit.EstimatedTimeOfArrival = _clock.Now.AddSeconds(60);
                await _clock.Delay(60000);

                unit.System = unitBody.DestinationSystem;

                if (unitBody.DestinationPlanet != null)
                {
                    unit.EstimatedTimeOfArrival = _clock.Now.AddSeconds(15);
                    await _clock.Delay(15000);
                }

                unit.Planet = unitBody.DestinationPlanet;
            }
        }

>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
    }
}
