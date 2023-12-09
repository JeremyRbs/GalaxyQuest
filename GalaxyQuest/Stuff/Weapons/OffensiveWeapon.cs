using GalaxyQuest.Entities;

namespace GalaxyQuest.Stuff.Weapons
{
    /// <summary>
    /// Represents an offensive weapon, a type of weapon.
    /// </summary>
    public class OffensiveWeapon : Weapon
    {
        /// <summary>
        /// Gets or sets the damage value of the weapon.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OffensiveWeapon"/> class.
        /// </summary>
        /// <param name="name">The name of the weapon.</param>
        /// <param name="type">The type of the weapon.</param>
        /// <param name="damage">The damage value of the weapon.</param>
        public OffensiveWeapon(string name, string type, int damage) : base(name, type)
        {
            Damage = damage;
        }
    }
}