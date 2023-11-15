using GalaxyQuest.EntitiesDTO;
using Shard.Shared.Core;

namespace GalaxyQuest.Services
{
    public class MapFactory
    {
        public List<StarSystemDTO> Generate()
        {
            SectorSpecification sectorSpecification = new MapGenerator(new MapGeneratorOptions { Seed = "Seed" }).Generate();
            List<StarSystemDTO> res = sectorSpecification.Systems.Select(system =>
                new StarSystemDTO(system.Name, system.Planets.Select(planet => new PlanetDTO(planet.Name,planet.Size, planet.ResourceQuantity.ToDictionary(
                    kvp => kvp.Key.ToString().ToLower(),
                    kvp => kvp.Value
                ))).ToList())).ToList();
            return res;
        }
    }
}
