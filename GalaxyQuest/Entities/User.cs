namespace GalaxyQuest.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Pseudo { get; set; }
        public User(string id, string pseudo) => (Id, Pseudo) = (id, pseudo);
    }
}