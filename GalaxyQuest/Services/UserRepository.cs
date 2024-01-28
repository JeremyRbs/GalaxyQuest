using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Units;

namespace GalaxyQuest.Services
{
    /// <summary>
    /// Repository class for managing user-related data.
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// List of UserDTO objects representing users in the system.
        /// </summary>
        public List<UserDto> Users { get; }
        
        /// <summary>
        /// List of CombatUnit objects representing CombatUnit in the graveyard.
        /// </summary>
        public List<CombatUnit> Graveyard = new List<CombatUnit>();

        /// <summary>
        /// Constructor to initialize the Users list.
        /// </summary>
        public UserRepository() => Users = new List<UserDto>();

        /// <summary>
        /// Gets a list of all units associated with users.
        /// </summary>
        /// <returns>List of Unit objects representing user units.</returns>
        public List<Unit> GetUnits() => Users.SelectMany(user => user.Units).ToList();
        
        /// <summary>
        /// Gets a list of all combat units.
        /// </summary>
        /// <returns>List of CombatUnit objects.</returns>
        public List<CombatUnit> GetCombatUnits() => Users.SelectMany(user => user.Units.OfType<CombatUnit>()).ToList();
    }
}
