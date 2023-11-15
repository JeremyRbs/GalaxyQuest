using GalaxyQuest.EntitiesDTO;
using Shard.Shared.Core;

namespace GalaxyQuest.Entities
{
    public class Building
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string BuilderId { get; set; }
        public string ResourceCategory { get; set; }
        public string System { get; set; }
        public string Planet { get; set; }
        public bool IsBuilt { get; set; }
        public DateTime? EstimatedBuildTime { get; set; }
        public Task? TaskBuilding { get; set; }

        public async Task Built(UserDTO userDTO, IClock _clock)
        {
            await _clock.Delay(TimeSpan.FromMinutes(5));
            this.EstimatedBuildTime = null;
            this.IsBuilt = true;
            await _clock.Delay(TimeSpan.FromMinutes(6));
            userDTO.ResourcesQuantity["carbon"]++;
        }
    }
}
