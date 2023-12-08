using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Services;
using Shard.Shared.Core;

namespace GalaxyQuest.Buildings;

public class StarportBuilding : Building
{
    public List<Unit> QueueUnitsList { get; set; }

    private TaskCompletionSource<bool> TaskAddUnitToQueue = new TaskCompletionSource<bool>();
    
    public StarportBuilding(string builderId, string resourceCategory, string system, string planet, IClock _clock, UserRepository userRepository, MapRepository mapRepository) : base("starport", builderId, resourceCategory, system, planet, _clock, userRepository, mapRepository)
    {
        QueueUnitsList = new List<Unit>();
    }

    protected override void BuildingWork(IClock clock)
    {
        WaitingForUnitToBuild();
    }
    
    public async void WaitingForUnitToBuild()
    {
        var (unit, userDto, planetDto) = GetUnitUserAndPlanet();
        
        while (true)
        {
            await TaskAddUnitToQueue.Task;
            if (QueueUnitsList.Any())
            {
                userDto.Units.Add(QueueUnitsList.First());
                QueueUnitsList.RemoveAt(0);
            }
            else
            {
                TaskAddUnitToQueue = new TaskCompletionSource<bool>();
            }
        }
    }

    public void AddUnitToQueue(Unit unit)
    {
        QueueUnitsList.Add(unit);
        TaskAddUnitToQueue.TrySetResult(true);
    }
}