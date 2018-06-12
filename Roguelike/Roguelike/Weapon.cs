using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    class Weapon : Item
    {
        public float AttackPower { get; private set; }
        public double Durability { get; private set; }    
    }
}
