using Shard.Shared.Core;

namespace GalaxyQuest.Entities
{
    /// <summary>
    /// Represents a user entity.
    /// </summary>
    public class User
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
        /// Gets or sets the date of creation of the user.
        /// </summary>
        public DateTimeOffset? DateOfCreation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="pseudo">The pseudo (username) of the user.</param>
        public User(string id, string pseudo) => (Id, Pseudo) = (id, pseudo);
    }
}
