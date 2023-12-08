using GalaxyQuest.Entities;

namespace GalaxyQuest.Units;

public class BomberUnit : Unit
{
    public BomberUnit(string system, string planet, string userId) : base("bomber", system, planet, userId)
    {
    }
    
    /*
    /// <summary>
    /// Initializes a new instance of the <see cref="BomberUnit"/> class.
    /// </summary>
    public BomberUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet, string userId)
        : base(id, "bomber", system, planet, destinationSystem, destinationPlanet, userId)
    { }
    */
}