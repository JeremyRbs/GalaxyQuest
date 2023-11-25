using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;

namespace GalaxyQuest.Services
{
    public class MapRepository
    {
        // List to store star systems with resources
        public List<StarSystemDTO> Systems;

        // Constructor that initializes Systems using a MapFactory
        public MapRepository(MapFactory mapFactory) => Systems = mapFactory.Generate();

        /// <summary>
        /// Gets all star systems without resource details.
        /// </summary>
        /// <returns>List of star systems without resource details.</returns>
        public List<StarSystem> GetAllSystemsWithoutResources() =>
            Systems.Select(system => new StarSystem(system.Name, system.Planets.Select(planet => new Planet(planet.Name, planet.Size)).ToList())).ToList();

        /// <summary>
        /// Gets all star systems with resources.
        /// </summary>
        /// <returns>List of star systems with resources.</returns>
        public List<StarSystemDTO> GetAllSystemsWithResources() => Systems;

        /// <summary>
        /// Gets a specific star system without resource details.
        /// </summary>
        /// <param name="systemName">Name of the star system.</param>
        /// <returns>The star system without resource details.</returns>
        public StarSystem? GetSystem(string systemName) =>
            GetAllSystemsWithoutResources().FirstOrDefault(s => s.Name == systemName);

        /// <summary>
        /// Gets a specific star system with its planets.
        /// </summary>
        /// <param name="systemName">Name of the star system.</param>
        /// <returns>The star system with its planets.</returns>
        public StarSystem? GetPlanets(string systemName) =>
            GetAllSystemsWithoutResources().FirstOrDefault(s => s.Name == systemName);

        /// <summary>
        /// Gets a specific planet within a star system without resource details.
        /// </summary>
        /// <param name="systemName">Name of the star system.</param>
        /// <param name="planetName">Name of the planet.</param>
        /// <returns>The planet without resource details.</returns>
        public Entities.Planet? GetOnePlanet(string systemName, string planetName) =>
            GetAllSystemsWithoutResources().FirstOrDefault(s => s.Name == systemName)?.Planets.FirstOrDefault(p => p.Name == planetName);

        /// <summary>
        /// Gets a specific planet within a star system with resource details.
        /// </summary>
        /// <param name="systemName">Name of the star system.</param>
        /// <param name="planetName">Name of the planet.</param>
        /// <returns>The planet with resource details.</returns>
        public PlanetDTO? GetOnePlanetWithResources(string systemName, string planetName) =>
            GetAllSystemsWithResources().FirstOrDefault(s => s.Name == systemName)?.Planets.FirstOrDefault(p => p.Name == planetName);

        /// <summary>
        /// Gets the name of a randomly selected star system.
        /// </summary>
        /// <returns>The name of a randomly selected star system.</returns>
        public string GetRandomSystemName() => Systems[new Random().Next(0, Systems.Count)].Name;
    }
}
