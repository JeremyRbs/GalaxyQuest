using GalaxyQuest.Entities;

namespace GalaxyQuest.Units
{
    /// <summary>
    /// Represents a builder unit in the galaxy.
    /// </summary>
    public class BuilderUnit : Unit
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderUnit"/> class.
        /// </summary>
        public BuilderUnit(string system, string planet, string userId)
            : base("builder", system, planet, userId)
        { }
    }
}
