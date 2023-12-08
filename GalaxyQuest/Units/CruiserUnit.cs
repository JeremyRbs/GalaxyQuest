using GalaxyQuest.Entities;

namespace GalaxyQuest.Units;

public class CruiserUnit : Unit
{
    public CruiserUnit(string system, string planet, string userId) : base("cruiser", system, planet, userId)
    {
    }
    
    /*
    /// <summary>
    /// Initializes a new instance of the <see cref="CruiserUnit"/> class.
    /// </summary>
    public CruiserUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet, string userId)
        : base(id, "cruiser", system, planet, destinationSystem, destinationPlanet, userId)
    { }
    */
}