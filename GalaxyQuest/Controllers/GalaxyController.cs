using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;

namespace GalaxyQuest.Controllers
{
    //[ApiController]
    public class GalaxyController : Controller
    {

        private SectorSpecification _sectorSpecification;

        public GalaxyController()
        {
            MapGeneratorOptions generatorOptions = new MapGeneratorOptions();
            generatorOptions.Seed = "Seed";     // Seed creation
            MapGenerator mapGenerator = new MapGenerator(generatorOptions);
            _sectorSpecification = (mapGenerator.Generate());    // Sector generation
        }

        // Get the systems
        [HttpGet("Systems")]
        public IActionResult GetSystems()
        {
            return Ok(_sectorSpecification.Systems);
        }

        // Get the system and all it planets
        [HttpGet("Systems/{systemName}")]
        public IActionResult GetSystemName([FromRoute] string systemName)
        {
            // Using foreach to find the 'systemName'
            foreach (var name in _sectorSpecification.Systems)
            {
                if (name.Name == systemName)
                {
                    // If 'systemName' is find, return HTTP 200 (OK)
                    return Ok(name);
                }
            }

            // If 'systemName' isn't find, return HTTP 404 (Not Found)
            return NotFound($"'{systemName}' not found ...");
        }

        // Get all planets of a single system
        [HttpGet("Systems/{systemName}/planets")]
        public IActionResult GetAllPlanetsOneSystem([FromRoute] string systemName)
        {
            // Using foreach to find the 'systemName'
            foreach (var name in _sectorSpecification.Systems)
            {
                if (name.Name == systemName)
                {
                    // If 'systemName' is find, return HTTP 200 (OK)
                    return Ok(name.Planets);
                }
            }

            // If 'systemName' isn't find, return HTTP 404 (Not Found)
            return NotFound($"'{systemName}' not found ...");
        }

        // Get a single planet of a system
        [HttpGet("Systems/{systemName}/planets/{planetName}")]
        public IActionResult GetSinglePlanet([FromRoute] string systemName, string planetName)
        {
            // Using foreach to find the 'systemName'
            foreach (var name in _sectorSpecification.Systems)
            {
                if (name.Name == systemName)
                {
                    // If 'systemName' is find
                    foreach (var planet in name.Planets)
                    {
                        if (planet.Name == planetName) { return Ok(planet); }
                    }
                    // If 'planetName' isn't find, return HTTP 404 (Not Found)
                    return NotFound($"'{planetName}' not found ...");
                }
            }

            // If 'systemName' isn't find, return HTTP 404 (Not Found)
            return NotFound($"'{systemName}' not found ...");
        }
    }
}
