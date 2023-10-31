namespace GalaxyQuest.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Pseudo { get; set; }
<<<<<<< HEAD
=======
        public DateTimeOffset? DateOfCreation { get; set; }
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        public User(string id, string pseudo) => (Id, Pseudo) = (id, pseudo);
    }
}