using GalaxyQuest.Services;
using GalaxyQuest.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyQuest.Controllers
{
    public class GalaxyController : Controller
    {
        private readonly MapRepository _mapRepository;

        public GalaxyController(MapRepository mapRepository) => _mapRepository = mapRepository;

        /// <summary>
        /// Gets all systems without resource details.
        /// </summary>
        [HttpGet("Systems")]
        public IActionResult GetSystems()
        {
            var systems = _mapRepository.GetAllSystemsWithoutResources();
            return ResponseHelper.HandleResponse(systems);
        }

        /// <summary>
        /// Gets details of a specific system by name.
        /// </summary>
        [HttpGet("Systems/{systemName}")]
        public IActionResult GetSystemName([FromRoute] string systemName)
        {
            var system = _mapRepository.GetSystem(systemName);
            return ResponseHelper.HandleResponse(system, $"'{systemName}' not found ...");
        }

        /// <summary>
        /// Gets all planets in a specific system.
        /// </summary>
        [HttpGet("Systems/{systemName}/planets")]
        public IActionResult GetAllPlanetsOneSystem([FromRoute] string systemName)
        {
            var system = _mapRepository.GetPlanets(systemName);
            return ResponseHelper.HandleResponse(system.Planets, $"'{systemName}' not found ...");
        }

        /// <summary>
        /// Gets details of a specific planet in a system.
        /// </summary>
        [HttpGet("Systems/{systemName}/planets/{planetName}")]
        public IActionResult GetSinglePlanet([FromRoute] string systemName, string planetName)
        {
            var planet = _mapRepository.GetOnePlanet(systemName, planetName);
            return ResponseHelper.HandleResponse(planet, $"'{planetName}' not found ...");
        }
    }
}
