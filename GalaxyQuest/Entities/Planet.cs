namespace GalaxyQuest.Entities
{
    public class Planet
    {
<<<<<<< HEAD
        public string Name { get; private set; }
        public int Size { get; private set; }
=======
        public string Name { get; }
        public int Size { get; }
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        public Planet(string name, int size) => (Name, Size) = (name, size);
    }
}
