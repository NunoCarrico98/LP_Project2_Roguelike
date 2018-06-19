using System;

namespace Roguelike
{
    /// <summary>
    /// Class that defines a weapon. It is a Game Object.
    /// </summary>
    [Serializable()]
    public class Weapon : Item
    {
        /// <summary>
        /// Property that defines the weapon attack power.
        /// </summary>
        public int AttackPower { get; private set; }

        /// <summary>
        /// Prperty that defines weapon durability.
        /// </summary>
        public double Durability { get; private set; }

        /// <summary>
        /// Property that defines the weapon type.
        /// </summary>
        public TypesOfWeapon WeaponType { get; set; }

        /// <summary>
        /// Instance variable to be able to randomize numbers.
        /// </summary>
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Constructor to initialise weapon properties.
        /// </summary>
        public Weapon()
        {
            // Initialise weapon type randomly from all types of enum
            WeaponType = (TypesOfWeapon)
                rnd.Next(Enum.GetNames(typeof(TypesOfWeapon)).Length);

            // Set weapon durability and attack power
            SetDurAndAP();
        }

        /// <summary>
        /// Alternative constructor to initialise weapon properties.
        /// </summary>
        /// <param name="typeweapon">Weapon type we want the weapon to be.</param>
        public Weapon(TypesOfWeapon typeweapon)
        {
            // Set weapon type
            WeaponType = typeweapon;

            // Set weapon durability and attack power
            SetDurAndAP();
        }

        /// <summary>
        /// Method to set weapon durability, attack power and weight.
        /// </summary>
        public void SetDurAndAP()
        {
            // Depending on weapon type
            switch (WeaponType)
            {
                // If it's an Axe
                case TypesOfWeapon.Axe:
                    AttackPower = 15;
                    Durability = 0.85d;
                    Weight = 5f;
                    break;
                // If it's a Dagger
                case TypesOfWeapon.Dagger:
                    AttackPower = 5;
                    Durability = 0.9d;
                    Weight = 2f;
                    break;
                // If it's a Lance
                case TypesOfWeapon.Lance:
                    AttackPower = 25;
                    Durability = 0.75d;
                    Weight = 9f;
                    break;
                // If it's a Slingshot
                case TypesOfWeapon.SlingShot:
                    AttackPower = 3;
                    Durability = 0.95;
                    Weight = 1f;
                    break;
                // If it's a Sword
                case TypesOfWeapon.Sword:
                    AttackPower = 20;
                    Durability = 0.8d;
                    Weight = 7f;
                    break;
            }
        }

        /// <summary>
        /// Override method ToString() from class object.
        /// </summary>
        /// <returns>Returns a string with a formatted line.</returns>
        public override string ToString()
        {
            // Return a formatted line with weapon type, attack power and weight
            return ($"{WeaponType,-14}| {AttackPower,13} | {Weight,10} | " +
                $"{Durability,12:f1}|");
        }
    }
}
