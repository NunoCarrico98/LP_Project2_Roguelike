using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    class Trap : Item
    {
        public int MaxDamage { get; private set; } 
        public TypesOfTraps TrapType { get; set; }
        Random rnd = new Random();

        public Trap()
        {
            TrapType = (TypesOfTraps)rnd.Next(0, Enum.GetNames(typeof(TypesOfTraps)).Length + 1);
            SetMaxDamage();
        }

        public Trap(TypesOfTraps typetrap)
        {
            TrapType = typetrap;
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
        }

        public override string ToString()
        {

            return ($"{TrapType,-14}| {MaxDamage,14} \n");
        }
    }
}
