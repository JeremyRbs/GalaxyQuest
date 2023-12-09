namespace GalaxyQuest.Entities;

public class Weapon
{
    /// <summary>
    /// Gets or sets the unique identifier of the weapon.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the weapon (e.g., "canon").
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the type of the weapon (e.g., "defensive").
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// Enumeration of weapon name (e.g., "Deflector").
    /// </summary>
    public enum WeaponName
    {
        Deflector,
        BombBay,
        Canon
    }
    
    /// <summary>
    /// Enumeration of weapon type (e.g., "Offensive").
    /// </summary>
    public enum WeaponType
    {
        Offensive,
        Defensive
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Weapon"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the weapon.</param>
    /// <param name="name">The name of the weapon.</param>
    /// <param name="type">The type of the weapon (e.g., "canon").</param>
    /// <param name="damage">The damage value of the weapon.</param>
    public Weapon(string name, string type)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Type = type;
    }
}