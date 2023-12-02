using GalaxyQuest.Entities;
using GalaxyQuest.Services;
using Shard.Shared.Core;

namespace GalaxyQuest.Buildings;

public class StarportBuilding : Building
{
    public StarportBuilding(string builderId, string resourceCategory, string system, string planet, IClock _clock, UserRepository userRepository, MapRepository mapRepository) : base("starport", builderId, resourceCategory, system, planet, _clock, userRepository, mapRepository)
    {
    }
}