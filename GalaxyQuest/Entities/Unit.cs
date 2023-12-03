using Shard.Shared.Core;

namespace GalaxyQuest.Entities
{
    public class Unit
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string System { get; set; }
        public string? Planet { get; set; }
        public string? DestinationSystem { get; set; }
        public string? DestinationPlanet { get; set; }
        public DateTime? EstimatedTimeOfArrival { get; set; }
        public Task? TaskUnit { get; set; }
        public Unit(string type, string system, string planet) => (Id, Type, System, Planet, DestinationSystem, DestinationPlanet, EstimatedTimeOfArrival, TaskUnit) = (Guid.NewGuid().ToString(), type, system, planet, null, null, null, null);
        public void MoveUnit(string systemName, string planetName) => (DestinationSystem, DestinationPlanet) = (systemName, planetName);

    }
}
