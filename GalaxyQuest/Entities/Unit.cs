using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Services;
using Shard.Shared.Core;
using System;
using System.Threading.Tasks;

namespace GalaxyQuest.Entities
{
    /// <summary>
    /// Represents a unit entity.
    /// </summary>
    public class Unit
    {
        /// <summary>
        /// Gets or sets the unique identifier of the unit.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the unit.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the current system of the unit.
        /// </summary>
        public string System { get; set; }

        /// <summary>
        /// Gets or sets the current planet of the unit.
        /// </summary>
        public string? Planet { get; set; }

        /// <summary>
        /// Gets or sets the destination system of the unit.
        /// </summary>
        public string? DestinationSystem { get; set; }

        /// <summary>
        /// Gets or sets the destination planet of the unit.
        /// </summary>
        public string? DestinationPlanet { get; set; }

        /// <summary>
        /// Gets or sets the estimated time of arrival for the unit.
        /// </summary>
        public DateTime? EstimatedTimeOfArrival { get; set; }

        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the unit.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the unit is currently moving.
        /// </summary>
        public bool IsMoving { get; set; }

        /// <summary>
        /// Gets or sets the task associated with the unit.
        /// </summary>
        public Task? TaskUnit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="type">The type of the unit.</param>
        /// <param name="system">The current system of the unit.</param>
        /// <param name="planet">The current planet of the unit.</param>
        /// <param name="userId">The user ID associated with the unit.</param>
        public Unit(string type, string system, string planet, string userId) =>
            (Id, Type, System, Planet, DestinationSystem, DestinationPlanet, EstimatedTimeOfArrival, TaskUnit, UserId, IsMoving) =
            (Guid.NewGuid().ToString(), type, system, planet, null, null, null, null, userId, false);

        /// <summary>
        /// Moves the unit to the specified system and planet.
        /// </summary>
        /// <param name="systemName">The name of the destination system.</param>
        /// <param name="planetName">The name of the destination planet.</param>
        public void MoveUnit(string systemName, string planetName) =>
            (DestinationSystem, DestinationPlanet) = (systemName, planetName);

        /// <summary>
        /// Moves the unit asynchronously based on the provided unit body and clock.
        /// </summary>
        /// <param name="unitBody">The unit body containing destination information.</param>
        /// <param name="unit">The unit to be moved.</param>
        /// <param name="_clock">The clock instance for handling delays.</param>
        public async Task MoveAsync(Unit unitBody, Unit? unit, IClock _clock)
        {
            unit.DestinationPlanet = unitBody.DestinationPlanet;
            unit.DestinationSystem = unitBody.DestinationSystem;
            unit.IsMoving = true;

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
            unit.IsMoving = false;
        }
    }
}
