using GalaxyQuest.Entities;

namespace GalaxyQuest.Units;

public class CruiserUnit : Unit
{
    public CruiserUnit(string system, string planet, string userId) : base("cruiser", system, planet, userId)
    {
    }
}