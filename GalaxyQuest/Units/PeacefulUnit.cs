using GalaxyQuest.Entities;

namespace GalaxyQuest.Units
{
    /// <summary>
    /// Represents a peaceful unit without combat capabilities.
    /// </summary>
    public class PeacefulUnit : Unit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeacefulUnit"/> class.
        /// </summary>
        public PeacefulUnit(string type, string system, string planet, string userId) : base(type, system, planet, userId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeacefulUnit"/> class.
        /// </summary>
        public PeacefulUnit(string id, string type, string system, string planet, string destinationSystem, string destinationPlanet, string userId) : base(id, type, system, planet, destinationSystem, destinationPlanet, userId)
        {
        }
    }
}