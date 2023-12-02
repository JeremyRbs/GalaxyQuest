using GalaxyQuest.Entities;

namespace GalaxyQuest.Units;

public class FighterUnit : Unit
{
    public FighterUnit(string system, string planet, string userId) : base("fighter", system, planet, userId)
    {
    }
}