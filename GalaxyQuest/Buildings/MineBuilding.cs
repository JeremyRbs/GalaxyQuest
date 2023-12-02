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

        /// <summary>
        /// Extracts resources based on the resource category asynchronously.
        /// </summary>
        /// <param name="_clock">The clock interface.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task ExtractResources(IClock _clock)
        {
            var (unit, userDTO, planetDTO) = GetUnitUserAndPlanet();
            int quantityToExtract = 1;

            // Local function to extract a specific resource
            async Task ExtractResourceAsync(string resourceName, int quantity)
            {
                while (planetDTO.ResourceQuantity[resourceName] > 0)
                {
                    await _clock.Delay(TimeSpan.FromMinutes(1));
                    userDTO.ResourcesQuantity[resourceName] += quantity;
                    planetDTO.ResourceQuantity[resourceName] -= quantity;
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
                string? mostPresentResource = planetDTO.GetResourceWithMaxQuantity(null);
                string? secondPresentResource = planetDTO.GetSecondResourceWithMaxQuantity(mostPresentResource);

                while (mostPresentResource != null && (mostPresentResource != "water" || mostPresentResource != "oxygen"))
                {
                    bool canExtract = true;

                    while (canExtract)
                    {
                        await _clock.Delay(TimeSpan.FromMinutes(1));
                        userDTO.ResourcesQuantity[mostPresentResource] += quantityToExtract;
                        planetDTO.ResourceQuantity[mostPresentResource] -= quantityToExtract;

                        if (planetDTO.ResourceQuantity[mostPresentResource] == planetDTO.ResourceQuantity[secondPresentResource] ||
                            planetDTO.ResourceQuantity[mostPresentResource] == planetDTO.ResourceQuantity[secondPresentResource] - quantityToExtract ||
                            planetDTO.ResourceQuantity[mostPresentResource] == 0)
                        {
                            canExtract = false;
                        }
                    }

                    mostPresentResource = planetDTO.GetResourceWithMaxQuantity(mostPresentResource);
                    secondPresentResource = planetDTO.GetSecondResourceWithMaxQuantity(mostPresentResource);
                }
            }
        }
    }
}