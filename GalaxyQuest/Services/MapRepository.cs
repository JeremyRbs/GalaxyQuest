using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;

namespace GalaxyQuest.Services
{
    public class MapRepository
    {
        public List<StarSystemDTO> Systems;
        public MapRepository(MapFactory mapFactory) => Systems = mapFactory.Generate();
        public List<StarSystem> GetAllSystemsWithoutResources() => Systems.Select(system => new StarSystem(system.Name, system.Planets.Select(planet => new Planet(planet.Name, planet.Size)).ToList())).ToList();
        public List<StarSystemDTO> GetAllSystemsWithResources() => Systems;
        public StarSystem? GetSystem(string systemName) => GetAllSystemsWithoutResources().FirstOrDefault(s => s.Name == systemName);
        public StarSystem? GetPlanets(string systemName) => GetAllSystemsWithoutResources().FirstOrDefault(s => s.Name == systemName);
        public Entities.Planet? GetOnePlanet(string systemName, string planetName) => GetAllSystemsWithoutResources().FirstOrDefault(s => s.Name == systemName)?.Planets.FirstOrDefault(p => p.Name == planetName);
        public PlanetDTO? GetOnePlanetWithResources(string systemName, string planetName) => GetAllSystemsWithResources().FirstOrDefault(s => s.Name == systemName)?.Planets.FirstOrDefault(p => p.Name == planetName);
        public string GetRandomSystemName() => Systems[new Random().Next(0, Systems.Count)].Name;
<<<<<<< HEAD
=======

        /*
        public bool CanBuildMineOnPlanet(string systemName, string planetName)
        {
            var system = GetSystem(systemName);
            if (system == null) { return false; }

            var planet = GetOnePlanet(systemName, planetName);
            if (planet == null) { return false; }

            if (planet.HasMine) {  return false; }

            return true;
        }
        */
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
    }
}
