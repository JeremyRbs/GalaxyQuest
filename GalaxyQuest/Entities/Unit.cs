using Shard.Shared.Core;

namespace GalaxyQuest.Entities
{
    public class Unit
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string System { get; set; }
        public string? Planet { get; set; }
        public string? DestinationSystem { get; set; }
        public string? DestinationPlanet { get; set; }
        public DateTime? EstimatedTimeOfArrival { get; set; }
        public Task? TaskUnit { get; set; }
        public Unit(string type, string system, string planet) => (Id, Type, System, Planet, DestinationSystem, DestinationPlanet, EstimatedTimeOfArrival, TaskUnit) = (Guid.NewGuid().ToString(), type, system, planet, null, null, null, null);
        public void MoveUnit(string systemName, string planetName) => (DestinationSystem, DestinationPlanet) = (systemName, planetName);

        public async Task MoveAsync(Unit unitBody, Unit? unit, IClock _clock)
        {
            unit.DestinationPlanet = unitBody.DestinationPlanet;
            unit.DestinationSystem = unitBody.DestinationSystem;

            if (unitBody.DestinationSystem == unitBody.System)
            {
                if (unitBody.DestinationPlanet != null)
                {
                    unit.EstimatedTimeOfArrival = _clock.Now.AddSeconds(15);
                    await _clock.Delay(15000);
                }
                else
                {
                    unit.EstimatedTimeOfArrival = _clock.Now;
                }

                unit.Planet = unitBody.DestinationPlanet;
            }
            else if (unitBody.DestinationSystem != unit.System)
            {
                unit.EstimatedTimeOfArrival = _clock.Now.AddSeconds(60);
                await _clock.Delay(60000);

                unit.System = unitBody.DestinationSystem;

                if (unitBody.DestinationPlanet != null)
                {
                    unit.EstimatedTimeOfArrival = _clock.Now.AddSeconds(15);
                    await _clock.Delay(15000);
                }

                unit.Planet = unitBody.DestinationPlanet;
            }
        }
    }
}
