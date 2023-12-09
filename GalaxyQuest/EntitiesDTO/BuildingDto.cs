using GalaxyQuest.Entities;

namespace GalaxyQuest.EntitiesDTO
{
    /// <summary>
    /// Represents a building in the galaxy.
    /// </summary>
    public class BuildingDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the building.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the building.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the builder unit.
        /// </summary>
        public string BuilderId { get; set; }

        /// <summary>
        /// Gets or sets the resource category of the building.
        /// </summary>
        public string ResourceCategory { get; set; }

        /// <summary>
        /// Gets or sets the star system where the building is located.
        /// </summary>
        public string System { get; set; }

        /// <summary>
        /// Gets or sets the planet where the building is located.
        /// </summary>
        public string Planet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the building is built.
        /// </summary>
        public bool IsBuilt { get; set; }

        /// <summary>
        /// Gets or sets the estimated build time for the building.
        /// </summary>
        public DateTime? EstimatedBuildTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        public BuildingDto(string id, string type, string builderId, string resourceCategory, string system, string planet, bool isBuilt, DateTime? estimatedBuildTime)
        {
            // Initialization of properties
            Id = id;
            Type = type;
            BuilderId = builderId;
            ResourceCategory = resourceCategory;
            System = system;
            Planet = planet;
            IsBuilt = isBuilt;
            EstimatedBuildTime = estimatedBuildTime;
        }
    }
}
