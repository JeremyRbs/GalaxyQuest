using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;

namespace GalaxyQuest.Services
{
    public class UserRepository
    {
        public List<UserDTO> Users { get; }
        public UserRepository() => Users = new List<UserDTO>();
        public List<Unit> GetUnits() => Users.SelectMany(user => user.Units).ToList();
    }
}
