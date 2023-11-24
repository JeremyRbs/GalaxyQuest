using GalaxyQuest.Entities;

namespace GalaxyQuest.Units
{
    /// <summary>
    /// Represents a scout unit in the galaxy.
    /// </summary>
    public class ScoutUnit : Unit
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoutUnit"/> class.
        /// </summary>
        public ScoutUnit(string system, string planet, string userId)
            : base("scout", system, planet, userId)
        {}
    }
}
