using System;

namespace Roguelike
{
    /// <summary>
    /// Class that defines a trap. It is a Game Object.
    /// </summary>
    [Serializable()]
    public class Trap : IGameObject
    {
        /// <summary>
        /// Property that defines trap max damage.
        /// </summary>
        public int MaxDamage { get; private set; } 

        /// <summary>
        /// Property that defines trap damage.
        /// </summary>
        public double Damage { get; private set; }

        /// <summary>
        /// Property that defines trap type.
        /// </summary>
        public TypesOfTraps TrapType { get; set; }

        /// <summary>
        /// Property that defines if a trap is active or not.
        /// </summary>
        public bool FallenInto { get; set; }

        /// <summary>
        /// Property that defines if a message was written for a trap or not.
        /// </summary>
        public bool WroteMessage { get; set; }

        /// <summary>
        /// Instance variable to be able to randomize numbers.
        /// </summary>
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Constructor to initialise trap properties.
        /// </summary>
        public Trap()
        {
            // Initialise trap type randomly from all types of enum
            TrapType = (TypesOfTraps)
                rnd.Next(Enum.GetNames(typeof(TypesOfTraps)).Length);

            // Set trap max damage
            SetMaxDamage();

            // Initialise trap as active
            FallenInto = false;

            // Initialise message as false
            WroteMessage = false;
        }

        /// <summary>
        /// Alternative constructor to initialise trap properties.
        /// </summary>
        /// <param name="type">Trap type we want the trap to be.</param>
        public Trap(TypesOfTraps type)
        {
            // Initialise trap type
            TrapType = type;

            // Set trap max damage
            SetMaxDamage();
        }

        /// <summary>
        /// Method to set the trap max damage.
        /// </summary>
        public void SetMaxDamage()
        {
            // Depending on trap type
            switch (TrapType)
            {
                // If it's a Bear Trap
                case TypesOfTraps.BearTrap:
                    MaxDamage = 5;
                    break;
                // If it's a Ceiling Trap
                case TypesOfTraps.CeilingTrap:
                    MaxDamage = 15;
                    break;
                // If it's a Land Mine
                case TypesOfTraps.LandMine:
                    MaxDamage = 30;
                    break;
                // If it's Spikes
                case TypesOfTraps.Spikes:
                    MaxDamage = 20;
                    break;
                // If it's a Wall Crusher
                case TypesOfTraps.WallCrusher:
                    MaxDamage = 25;
                    break;
            }

            // Set Damage as a random between 0 and max damage
            Damage = rnd.NextDouble() * MaxDamage;
        }

        /// <summary>
        /// Override method ToString() from class object.
        /// </summary>
        /// <returns>Returns a string with a formatted line.</returns>
        public override string ToString()
        {
            // Return a formatted line with trap type and max damage
            return ($"{TrapType,-14}| {MaxDamage,14}|");
        }
    }
}
