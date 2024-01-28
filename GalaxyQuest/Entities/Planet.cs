namespace GalaxyQuest.Entities
{
    /// <summary>
    /// Represents a planet within a star system.
    /// </summary>
    public class Planet
    {
        /// <summary>
        /// Gets the name of the planet.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the size of the planet.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Planet"/> class.
        /// </summary>
        /// <param name="name">The name of the planet.</param>
        /// <param name="size">The size of the planet.</param>
        public Planet(string name, int size)
        {
            (Name, Size) = (name, size);
        }
    }
}
