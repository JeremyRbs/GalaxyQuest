using GalaxyQuest.EntitiesDTO;
using Shard.Shared.Core;

namespace GalaxyQuest.Services
{
    public class MapFactory
    {
        /// <summary>
        /// Generates a list of star systems with planets and resources based on a specified sector specification.
        /// </summary>
        /// <returns>The list of generated star systems.</returns>
        public List<StarSystemDTO> Generate()
        {
            // Generate a sector specification using the MapGenerator
            SectorSpecification sectorSpecification = new MapGenerator(new MapGeneratorOptions { Seed = "Test application" }).Generate();

            // Convert the sector specification into a list of StarSystemDTO
            List<StarSystemDTO> starSystems = sectorSpecification.Systems.Select(system =>
                new StarSystemDTO(
                    system.Name,
                    system.Planets.Select(planet => new PlanetDTO(
                        planet.Name,
                        planet.Size,
                        planet.ResourceQuantity.ToDictionary(
                            kvp => kvp.Key.ToString().ToLower(),
                            kvp => kvp.Value
                        )
                    )).ToList()
                )
            ).ToList();

            return starSystems;
        }
    }
}
