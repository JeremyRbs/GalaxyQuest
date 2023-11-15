using GalaxyQuest.Entities;
using Shard.Shared.Core;

namespace GalaxyQuest.EntitiesDTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Pseudo { get; set; }
        public DateTimeOffset DateOfCreation { get; }
        public List<Unit> Units = new List<Unit>();
        public List<Building> Buildings = new List<Building>();
        public Dictionary<string, int>? ResourcesQuantity { get; set; } = new Dictionary<string, int>()
        {
            {ResourceKind.Aluminium.ToString().ToLower(), 0},
            {ResourceKind.Carbon.ToString().ToLower(), 20},
            {ResourceKind.Iron.ToString().ToLower(), 10},
            {ResourceKind.Gold.ToString().ToLower(), 0},
            {ResourceKind.Titanium.ToString().ToLower(), 0},
            {ResourceKind.Water.ToString().ToLower(), 50},
            {ResourceKind.Oxygen.ToString().ToLower(), 50}
        };
        public UserDTO(string id, string pseudo) => (Id, Pseudo, DateOfCreation, Units, ResourcesQuantity) = (id, pseudo, DateTimeOffset.Now, new List<Unit>(), ResourcesQuantity);
        public void AddUnit(Unit unit) => Units.Add(unit);
        public void AddBuilding(Building building) => Buildings.Add(building);
    }
}
