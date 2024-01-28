using GalaxyQuest.Entities;
using GalaxyQuest.Services;
using Shard.Shared.Core;

namespace GalaxyQuest.Buildings
{
    /// <summary>
    /// Represents a mine building in the galaxy.
    /// </summary>
    public class MineBuilding : Building
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MineBuilding"/> class.
        /// </summary>
        /// <param name="builderId">The identifier of the builder unit.</param>
        /// <param name="resourceCategory">The resource category of the building.</param>
        /// <param name="system">The star system where the building is located.</param>
        /// <param name="planet">The planet where the building is located.</param>
        /// <param name="_clock">The clock interface.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="mapRepository">The map repository.</param>
        public MineBuilding(string builderId, string resourceCategory, string system, string planet, IClock _clock, UserRepository userRepository, MapRepository mapRepository) : base("mine", builderId, resourceCategory, system, planet, _clock, userRepository, mapRepository)
        {
        }

        protected override void BuildingWork(IClock clock)
        {
            ExtractResources(clock);
        }

        /// <summary>
        /// Extracts resources based on the resource category asynchronously.
        /// </summary>
        /// <param name="clock">The clock interface.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExtractResources(IClock clock)
        {
            var (unit, userDto, planetDto) = GetUnitUserAndPlanet();
            int quantityToExtract = 1;

            // Local function to extract a specific resource
            async Task ExtractResourceAsync(string resourceName, int quantity)
            {
                while (planetDto.ResourceQuantity != null && planetDto.ResourceQuantity[resourceName] > 0)
                {
                    await clock.Delay(TimeSpan.FromMinutes(1));
                    if (userDto.ResourcesQuantity != null)
                    {
                        userDto.ResourcesQuantity[resourceName] += quantity;
                    }
                    planetDto.ResourceQuantity[resourceName] -= quantity;
                }
            }

            if (this.ResourceCategory == "gaseous")
            {
                await ExtractResourceAsync("oxygen", quantityToExtract);
            }
            else if (this.ResourceCategory == "liquid")
            {
                await ExtractResourceAsync("water", quantityToExtract);
            }
            else
            {
                string? mostPresentResource = planetDto.GetResourceWithMaxQuantity(null);
                string? secondPresentResource = planetDto.GetSecondResourceWithMaxQuantity(mostPresentResource);

                while (mostPresentResource != null && (mostPresentResource != "water" || mostPresentResource != "oxygen"))
                {
                    bool canExtract = true;

                    while (canExtract)
                    {
                        await clock.Delay(TimeSpan.FromMinutes(1));
                        userDto.ResourcesQuantity[mostPresentResource] += quantityToExtract;
                        planetDto.ResourceQuantity[mostPresentResource] -= quantityToExtract;

                        if (planetDto.ResourceQuantity[mostPresentResource] == planetDto.ResourceQuantity[secondPresentResource] ||
                            planetDto.ResourceQuantity[mostPresentResource] == planetDto.ResourceQuantity[secondPresentResource] - quantityToExtract ||
                            planetDto.ResourceQuantity[mostPresentResource] == 0)
                        {
                            canExtract = false;
                        }
                    }

                    mostPresentResource = planetDto.GetResourceWithMaxQuantity(mostPresentResource);
                    secondPresentResource = planetDto.GetSecondResourceWithMaxQuantity(mostPresentResource);
                }
            }
        }
    }
}