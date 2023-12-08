using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Services;
using Shard.Shared.Core;

namespace GalaxyQuest.Entities
{
    /// <summary>
    /// Represents a building in the galaxy.
    /// </summary>
    public abstract class Building
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
        /// Gets or sets the cancellation token source for the building process.
        /// </summary>
        private CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>
        /// Gets or sets the task representing the building process.
        /// </summary>
        public Task TaskBuilding { get; }

        /// <summary>
        /// Gets or sets the user repository.
        /// </summary>
        private UserRepository UserRepository { get; set; }

        /// <summary>
        /// Gets or sets the map repository.
        /// </summary>
        private MapRepository MapRepository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        public Building(string type, string builderId, string resourceCategory, string system, string planet, IClock clock, UserRepository userRepository, MapRepository mapRepository)
        {
            // Initialization of properties
            Id = Guid.NewGuid().ToString();
            Type = type;
            BuilderId = builderId;
            ResourceCategory = resourceCategory;
            System = system;
            Planet = planet;
            IsBuilt = false;
            EstimatedBuildTime = clock.Now.AddMinutes(5);
            CancellationTokenSource = new CancellationTokenSource();
            UserRepository = userRepository;
            MapRepository = mapRepository;
            TaskBuilding = BuiltProcess(clock, CancellationTokenSource.Token);
        }

        /// <summary>
        /// Handles the building process asynchronously.
        /// </summary>
        /// <param name="clock">The clock interface.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task BuiltProcess(IClock clock, CancellationToken cancellationToken)
        {
            var (unit, userDTO, planetDTO) = GetUnitUserAndPlanet();

            try
            {
                // Delay for 5 minutes or until canceled
                await clock.Delay(TimeSpan.FromMinutes(5), cancellationToken);
                this.EstimatedBuildTime = null;
                this.IsBuilt = true;
                // Start resource extraction
                BuildingWork(clock);
            }
            catch (OperationCanceledException)
            {
                this.EstimatedBuildTime = null;
                this.IsBuilt = false;
            }
        }

        /// <summary>
        /// Cancels the building process.
        /// </summary>
        public void CancelBuildingProcess()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }

        /// <summary>
        /// Virtual method for extracting resources from the built building.
        /// </summary>
        protected abstract void BuildingWork(IClock clock);

        /// <summary>
        /// Gets unit, userDTO, and planetDTO based on the builder ID.
        /// </summary>
        /// <returns>A tuple containing the unit, userDTO, and planetDTO.</returns>
        protected (Unit unit, UserDTO userDTO, PlanetDTO planetDTO) GetUnitUserAndPlanet()
        {
            var unit = UserRepository.GetUnits().FirstOrDefault(u => u.Id == BuilderId);
            var userDto = UserRepository.Users.FirstOrDefault(u => u.Id == unit.UserId);
            var planetDto = (PlanetDTO?)MapRepository.GetOnePlanetWithResources(System, Planet);

            return (unit, userDto, planetDto);
        }
    }
}