namespace GalaxyQuest.EntitiesDTO
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a star system.
    /// </summary>
    public class StarSystemDto
    {
        /// <summary>
        /// Gets the name of the star system.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the list of planets in the star system.
        /// </summary>
        public List<PlanetDto> Planets { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StarSystemDto"/> class.
        /// </summary>
        /// <param name="name">The name of the star system.</param>
        /// <param name="planets">The list of planets in the star system.</param>
        public StarSystemDto(string name, List<PlanetDto> planets) => (Name, Planets) = (name, planets);
    }
}
