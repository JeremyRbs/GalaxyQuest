using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Units;
using Shard.Shared.Core;

namespace GalaxyQuest.Services
{
    /// <summary>
    /// Background service for handling combat between combat units.
    /// </summary>
    public class CombatUnitService : IHostedService, IDisposable
    {
        private readonly UserRepository userRepository;
        private int executionCount = 0;
        private ITimer? timer = null;
        private IClock clock;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatService"/> class.
        /// </summary>
        public CombatUnitService(UserRepository userRepository, IClock clock)
        {
            this.userRepository = userRepository;
            this.clock = clock;
        }

        /// <summary>
        /// Starts the background service.
        /// </summary>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            timer = clock.CreateTimer(DoWork, null, TimeSpan.FromSeconds(6),
                TimeSpan.FromSeconds(6));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Performs the periodic combat check.
        /// </summary>
        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);
            
            List<CombatUnit> combatUnits = userRepository.GetCombatUnits();

            CombatUnitsFight(combatUnits);
        }

        /// <summary>
        /// Handles combat between combat units.
        /// </summary>
        private void CombatUnitsFight(List<CombatUnit> combatUnits)
        {
            foreach (var unit in combatUnits)
            {
                var orderedTargets = combatUnits
                    .Where(u => u != unit && u.System == unit.System && u.Planet == unit.Planet && u.UserId != unit.UserId)
                    .OrderBy(u => unit.AttackPriority.IndexOf(u.Type))
                    .ToList();

                CombatUnit? targetUnit = orderedTargets.FirstOrDefault();
                if (targetUnit != null)
                {
                    unit.Attack(unit, targetUnit, clock.Now.Second);

                    if (targetUnit.Health <= 0 && !userRepository.Graveyard.Contains(targetUnit))
                    {
                        userRepository.Graveyard.Add(targetUnit);
                    }
                }
            }
            RemoveDeadUnits();
        }

        /// <summary>
        /// Removes dead units from the user's units list.
        /// </summary>
        private void RemoveDeadUnits()
        {
            foreach (var combatUnit in userRepository.Graveyard)
            {
                UserDto userDto = userRepository.Users.FirstOrDefault(u => u.Id == combatUnit.UserId);
                userDto?.Units.Remove(combatUnit);
            }
        }
        
        /// <summary>
        /// Stops the background service.
        /// </summary>
        public Task StopAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Disposes of resources used by the service.
        /// </summary>
        public void Dispose(){}
    }
}
