using GalaxyQuest.Entities;

namespace GalaxyQuest.Units;

public class BomberUnit : Unit
{
    public BomberUnit(string system, string planet, string userId) : base("bomber", system, planet, userId)
    {
    }
}