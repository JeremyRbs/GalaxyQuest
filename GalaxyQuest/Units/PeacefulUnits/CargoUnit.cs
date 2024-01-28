using Shard.Shared.Core;

namespace GalaxyQuest.Units.PeacefulUnits;

public class CargoUnit : PeacefulUnit
{

    /// <summary>
    /// Initializes a new instance of the <see cref="CargoUnit"/> class.
    /// </summary>
    public CargoUnit(string system, string planet, string userId)
        : base(UnitType.Cargo.ToString().ToLower(), system, planet, userId)
    {
        Health = 100;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CargoUnit"/> class.
    /// </summary>
    public CargoUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet,
        string userId, Dictionary<string, int>? resourcesQuantity, int health)
        : base(id, UnitType.Cargo.ToString().ToLower(), system, planet, destinationSystem, destinationPlanet, userId)
    {
        ResourcesQuantity = resourcesQuantity;
        Health = health;
    }
}