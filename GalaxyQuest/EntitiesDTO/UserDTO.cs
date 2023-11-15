using GalaxyQuest.Entities;

namespace GalaxyQuest.EntitiesDTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Pseudo { get; set; }
        public DateTimeOffset DateOfCreation { get; }
        public List<Unit> Units { get; set; }
        public UserDTO(string id, string pseudo) => (Id, Pseudo, DateOfCreation, Units) = (id, pseudo, DateTimeOffset.Now, new List<Unit>());
        public void AddUnit(Unit unit) => Units.Add(unit);
    }
}
