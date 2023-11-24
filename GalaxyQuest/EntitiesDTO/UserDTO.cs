using GalaxyQuest.Entities;
using Shard.Shared.Core;

namespace GalaxyQuest.EntitiesDTO
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a user.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the pseudo (username) of the user.
        /// </summary>
        public string Pseudo { get; set; }

        /// <summary>
        /// Gets the date of creation of the user.
        /// </summary>
        public DateTimeOffset DateOfCreation { get; }

        /// <summary>
        /// Gets or sets the list of units associated with the user.
        /// </summary>
        public List<Unit> Units;

        /// <summary>
        /// Gets or sets the list of buildings associated with the user.
        /// </summary>
        public List<Building> Buildings;

        /// <summary>
        /// Gets or sets the resources quantity dictionary associated with the user.
        /// </summary>
        public Dictionary<string, int>? ResourcesQuantity { get; set; } = new Dictionary<string, int>()
        {
            {ResourceKind.Aluminium.ToString().ToLower(), 0},
            {ResourceKind.Carbon.ToString().ToLower(), 20},
            {ResourceKind.Iron.ToString().ToLower(), 10},
            {ResourceKind.Gold.ToString().ToLower(), 0},
            {ResourceKind.Titanium.ToString().ToLower(), 0},
            {ResourceKind.Water.ToString().ToLower(), 50},
            {ResourceKind.Oxygen.ToString().ToLower(), 50}
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDTO"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="pseudo">The pseudo (username) of the user.</param>
        public UserDTO(string id, string pseudo) => (Id, Pseudo, DateOfCreation, Units, Buildings, ResourcesQuantity) = (id, pseudo, DateTimeOffset.Now, new List<Unit>(), new List<Building>(), ResourcesQuantity);

        /// <summary>
        /// Adds a unit to the user's list of units.
        /// </summary>
        /// <param name="unit">The unit to be added.</param>
        public void AddUnit(Unit unit) => Units.Add(unit);
    }
}
