using GalaxyQuest.Entities;

namespace GalaxyQuest.Stuff.Weapons
{
    /// <summary>
    /// Represents a defensive weapon, a type of weapon.
    /// </summary>
    public class DefensiveWeapon : Weapon
    {
        /// <summary>
        /// Gets or sets the damage reduction value of the weapon.
        /// </summary>
        public int DamageReduction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefensiveWeapon"/> class.
        /// </summary>
        /// <param name="name">The name of the weapon.</param>
        /// <param name="type">The type of the weapon.</param>
        /// <param name="damageReduction">The damage reduction value of the weapon.</param>
        public DefensiveWeapon(string name, string type, int damageReduction) : base(name, type)
        {
            DamageReduction = damageReduction;
        }
    }
}