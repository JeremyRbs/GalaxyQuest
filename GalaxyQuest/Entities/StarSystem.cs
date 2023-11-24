namespace GalaxyQuest.Entities
{
    /// <summary>
    /// Represents a star system entity.
    /// </summary>
    public class StarSystem
    {
        /// <summary>
        /// Gets or sets the name of the star system.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the list of planets in the star system.
        /// </summary>
        public List<Planet> Planets { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StarSystem"/> class.
        /// </summary>
        /// <param name="name">The name of the star system.</param>
        /// <param name="planets">The list of planets in the star system.</param>
        public StarSystem(string name, List<Planet> planets) =>
            (Name, Planets) = (name, planets);
    }
}
