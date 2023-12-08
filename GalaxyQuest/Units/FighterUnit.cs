using GalaxyQuest.Entities;

namespace GalaxyQuest.Units;

public class FighterUnit : Unit
{
    public FighterUnit(string system, string planet, string userId) : base("fighter", system, planet, userId)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FighterUnit"/> class.
    /// </summary>
    public FighterUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet, string userId)
        : base(id, "fighter", system, planet, destinationSystem, destinationPlanet, userId)
    { }
    
}