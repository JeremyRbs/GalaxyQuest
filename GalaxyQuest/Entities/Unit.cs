<<<<<<< HEAD
﻿namespace GalaxyQuest.Entities
=======
﻿using Shard.Shared.Core;

namespace GalaxyQuest.Entities
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
{
    public class Unit
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string System { get; set; }
        public string? Planet { get; set; }
<<<<<<< HEAD
        public Unit(string type, string system) => (Id, Type, System, Planet) = (Guid.NewGuid().ToString(), type, system, null);
        public void MoveUnit(string systemName, string planetName) => (System, Planet) = (systemName, planetName);
=======
        public string? DestinationSystem { get; set; }
        public string? DestinationPlanet { get; set; }
        public DateTime? EstimatedTimeOfArrival { get; set; }
        public Task? TaskUnit { get; set; }
        public Unit(string type, string system, string planet) => (Id, Type, System, Planet, DestinationSystem, DestinationPlanet, EstimatedTimeOfArrival, TaskUnit) = (Guid.NewGuid().ToString(), type, system, planet, null, null, null, null);
        public void MoveUnit(string systemName, string planetName) => (DestinationSystem, DestinationPlanet) = (systemName, planetName);

>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
    }
}
