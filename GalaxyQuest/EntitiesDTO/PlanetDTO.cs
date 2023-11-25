using GalaxyQuest.Entities;

namespace GalaxyQuest.EntitiesDTO
{
    /// <summary>
    /// Represents a DTO (Data Transfer Object) for a planet.
    /// </summary>
    public class PlanetDTO
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
        /// Gets the resource quantity on the planet.
        /// </summary>
        public Dictionary<string, int>? ResourceQuantity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanetDTO"/> class.
        /// </summary>
        /// <param name="name">The name of the planet.</param>
        /// <param name="size">The size of the planet.</param>
        /// <param name="resourceQuantity">The resource quantity on the planet.</param>
        public PlanetDTO(string name, int size, Dictionary<string, int>? resourceQuantity) => (Name, Size, ResourceQuantity) = (name, size, resourceQuantity);

        /// <summary>
        /// Gets the resource with the maximum quantity, excluding specified resources.
        /// </summary>
        /// <param name="resource">The resource to exclude from consideration.</param>
        /// <returns>The resource with the maximum quantity.</returns>
        public string GetResourceWithMaxQuantity(String resource)
        {
            if (ResourceQuantity == null || ResourceQuantity.Count == 0)
            {
                return null;
            }

            var resourceWithMaxQuantity = ResourceQuantity
                .Where(kv => kv.Value > 0 && kv.Key != "water" && kv.Key != "oxygen" && kv.Key != resource)
                .OrderByDescending(kv => kv.Value)
                .FirstOrDefault();

            if (resourceWithMaxQuantity.Key == null && ResourceQuantity.GetValueOrDefault(resource) != 0)
            {
                resourceWithMaxQuantity = ResourceQuantity.FirstOrDefault(r => r.Key == resource);
            }


            return resourceWithMaxQuantity.Key;
        }

        /// <summary>
        /// Gets the second resource with the maximum quantity, excluding specified resources.
        /// </summary>
        /// <param name="resource">The resource to exclude from consideration.</param>
        /// <returns>The second resource with the maximum quantity.</returns>
        public string GetSecondResourceWithMaxQuantity(String resource)
        {
            if (ResourceQuantity == null || ResourceQuantity.Count < 1)
            {
                return null;
            }

            var firstPair = ResourceQuantity.OrderByDescending(kv => kv.Value).First();

            var secondResourceWithMaxQuantity = ResourceQuantity
                .Where(kv => kv.Key != firstPair.Key && kv.Key != resource && kv.Value < firstPair.Value)
                .OrderByDescending(kv => kv.Value)
                .FirstOrDefault();

            return secondResourceWithMaxQuantity.Key ?? firstPair.Key;
        }

    }
}
