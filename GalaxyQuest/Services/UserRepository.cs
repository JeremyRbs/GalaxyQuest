using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;

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
        public List<UserDTO> Users { get; }

        /// <summary>
        /// Constructor to initialize the Users list.
        /// </summary>
        public UserRepository() => Users = new List<UserDTO>();

        /// <summary>
        /// Gets a list of all units associated with users.
        /// </summary>
        /// <returns>List of Unit objects representing user units.</returns>
        public List<Unit> GetUnits() => Users.SelectMany(user => user.Units).ToList();
    }
}
