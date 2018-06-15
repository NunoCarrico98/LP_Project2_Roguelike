using System;

namespace Roguelike
{
    public class Trap : IGameObject
    {
        public int MaxDamage { get; private set; } 
        public double Damage { get; private set; }
        public TypesOfTraps TrapType { get; set; }
        public Position TrapPos { get; set; }
        public bool FallenInto { get; set; }
        public bool WroteMessage { get; set; }
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public Trap(Position pos)
        {
            TrapType = (TypesOfTraps)rnd.Next
                (0, Enum.GetNames(typeof(TypesOfTraps)).Length);
            SetMaxDamage();
            TrapPos = pos;
            FallenInto = false;
            WroteMessage = false;
        }

        public Trap(TypesOfTraps type)
        {
            TrapType = type;
            SetMaxDamage();
        }

        public void SetMaxDamage()
        {
            switch (TrapType)
            {
                case TypesOfTraps.BearTrap:
                    MaxDamage = 5;
                    break;
                case TypesOfTraps.CeilingTrap:
                    MaxDamage = 15;
                    break;
                case TypesOfTraps.LandMine:
                    MaxDamage = 30;
                    break;
                case TypesOfTraps.Spikes:
                    MaxDamage = 20;
                    break;
                case TypesOfTraps.WallCrusher:
                    MaxDamage = 25;
                    break;
            }

            Damage = rnd.NextDouble() * MaxDamage;
        }

        public override string ToString()
        {

            return ($"{TrapType,-14}| {MaxDamage,14}|");
        }
    }
}
