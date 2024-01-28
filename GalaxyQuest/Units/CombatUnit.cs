using GalaxyQuest.Entities;
using GalaxyQuest.Stuff.Weapons;
using GalaxyQuest.Units.CombatUnits;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyQuest.Units
{
    /// <summary>
    /// Represents a combat unit with attack capabilities.
    /// </summary>
    public class CombatUnit : Unit
    {
        
        /// <summary>
        /// Gets or sets the health of the combat unit.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the weapons equipped by the combat unit.
        /// </summary>
        public List<Weapon> Weapons { get; set; }

        /// <summary>
        /// Gets or sets the attack priority list for the combat unit.
        /// </summary>
        public List<string> AttackPriority { get; set; }

        /// <summary>
        /// Gets or sets the fire rate of the combat unit.
        /// </summary>
        public int FireRate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatUnit"/> class.
        /// </summary>
        protected CombatUnit(string type, string system, string planet, string userId, int health, List<Weapon> weapons, List<string> attackPriority, int fireRate)
            : base(type, system, planet, userId)
        {
            Health = health;
            Weapons = weapons;
            AttackPriority = attackPriority;
            FireRate = fireRate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatUnit"/> class.
        /// </summary>
        protected CombatUnit(string id, string type, string system, string planet, string destinationSystem, string destinationPlanet, string userId, int health, List<Weapon> weapons, List<string> attackPriority, int fireRate)
            : base(id, type, system, planet, destinationSystem, destinationPlanet, userId)
        {
            Health = health;
            Weapons = weapons;
            AttackPriority = attackPriority;
            FireRate = fireRate;
        }

        /// <summary>
        /// Performs an attack on a targeted combat unit.
        /// </summary>
        /// <param name="attackingCombatUnit">The combat unit initiating the attack.</param>
        /// <param name="targetedCombatUnit">The combat unit being targeted.</param>
        /// <param name="actualTime">The current time.</param>
        public void Attack(CombatUnit attackingCombatUnit, CombatUnit targetedCombatUnit, int actualTime)
        {
            if (actualTime % attackingCombatUnit.FireRate == 0)
            {
                int totalDamage = CalculateTotalDamage(attackingCombatUnit, targetedCombatUnit);
                targetedCombatUnit.Health -= totalDamage;
            }
        }

        private int CalculateTotalDamage(CombatUnit attackingCombatUnit, CombatUnit targetedCombatUnit)
        {
            int totalDamage = attackingCombatUnit.Weapons
                .OfType<OffensiveWeapon>()
                .Sum(weapon => weapon.Damage);

            if (attackingCombatUnit is CruiserUnit && targetedCombatUnit is BomberUnit)
            {
                List<DefensiveWeapon> defensiveWeapons = targetedCombatUnit.Weapons
                    .OfType<DefensiveWeapon>()
                    .ToList();

                foreach (var weapon in defensiveWeapons)
                {
                    totalDamage /= weapon.DamageReduction;
                }
            }

            return totalDamage;
        }
    }
}