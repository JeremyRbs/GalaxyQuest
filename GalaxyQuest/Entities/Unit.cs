using Shard.Shared.Core;
using GalaxyQuest.RequestBodies;

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
        /// Gets or sets the destination shard of the unit.
        /// </summary>
        public string? DestinationShard { get; set; }
        
        /// <summary>
        /// Gets or sets the resources quantity dictionary associated with the unit.
        /// </summary>
        public Dictionary<string, int>? ResourcesQuantity { get; set; } = new Dictionary<string, int>() 
        {
            { "carbon", 0 },
            { "iron", 0 },
            { "gold", 0 },
            { "aluminium", 0 },
            { "titanium", 0 },
            { "water", 0 },
            { "oxygen", 0 }
        };
        
        /// <summary>
        /// Gets or sets the health of the unit.
        /// </summary>
        public int Health { get; set; }
        
        /// <summary>
        /// Enumeration of unit type (e.g., "Scout").
        /// </summary>
        public enum UnitType
        {
            Scout,
            Builder,
            Cruiser,
            Fighter,
            Bomber,
            Cargo
        }

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
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="id">The id of the unit.</param>
        /// <param name="type">The type of the unit.</param>
        /// <param name="system">The current system of the unit.</param>
        /// <param name="planet">The current planet of the unit.</param>
        /// <param name="destinationSystem">The destination system of the unit.</param>
        /// <param name="destinationPlanet">The destination planet of the unit.</param>
        /// <param name="userId">The user ID associated with the unit.</param>
        public Unit(string id, string type, string system, string planet, string destinationSystem, string destinationPlanet, string userId) =>
            (Id, Type, System, Planet, DestinationSystem, DestinationPlanet, EstimatedTimeOfArrival, TaskUnit, UserId, IsMoving) =
            (id, type, system, planet, destinationSystem, destinationPlanet, null, null, userId, false);

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
        /// <param name="postRequestUnitBody">The unit body containing destination information.</param>
        /// <param name="unit">The unit to be moved.</param>
        /// <param name="clock">The clock instance for handling delays.</param>
        public async Task MoveAsync(PostRequestUnitBody postRequestUnitBody, Unit? unit, IClock clock)
        {
            unit.DestinationPlanet = postRequestUnitBody.DestinationPlanet;
            unit.DestinationSystem = postRequestUnitBody.DestinationSystem;
            unit.IsMoving = true;

            if (postRequestUnitBody.DestinationSystem == postRequestUnitBody.System)
            {
                if (postRequestUnitBody.DestinationPlanet != null)
                {
                    unit.EstimatedTimeOfArrival = clock.Now.AddSeconds(15);
                    await clock.Delay(15000);
                }
                else
                {
                    unit.EstimatedTimeOfArrival = clock.Now;
                }

                unit.Planet = postRequestUnitBody.DestinationPlanet;
            }
            else if (postRequestUnitBody.DestinationSystem != unit.System)
            {
                unit.EstimatedTimeOfArrival = clock.Now.AddSeconds(60);
                await clock.Delay(60000);

                unit.System = postRequestUnitBody.DestinationSystem;

                if (postRequestUnitBody.DestinationPlanet != null)
                {
                    unit.EstimatedTimeOfArrival = clock.Now.AddSeconds(15);
                    await clock.Delay(15000);
                }

                unit.Planet = postRequestUnitBody.DestinationPlanet;
            }
            unit.IsMoving = false;
        }
    }
}
