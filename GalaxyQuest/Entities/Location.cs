namespace GalaxyQuest.Entities
{
    /// <summary>
    /// Represents the location information, including the star system, planet, and resources quantity.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets or sets the name of the star system.
        /// </summary>
        public string System { get; set; }

        /// <summary>
        /// Gets or sets the name of the planet.
        /// </summary>
        public string Planet { get; set; }

        /// <summary>
        /// Gets or sets the quantity of resources on the planet.
        /// </summary>
        public Dictionary<string, int>? ResourcesQuantity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="system">The name of the star system.</param>
        /// <param name="planet">The name of the planet.</param>
        /// <param name="resourcesQuantity">The quantity of resources on the planet.</param>
        public Location(string system, string planet, Dictionary<string, int>? resourcesQuantity)
        {
            (System, Planet, ResourcesQuantity) = (system, planet, resourcesQuantity);
        }
    }
}
