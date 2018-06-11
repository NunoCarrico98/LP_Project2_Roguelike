using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    class Trap : Item
    {

        public int MaxDamage { get; } 
        public bool FallenInto { get; set; }

        public Trap()
        {
            MaxDamage = 100;
            FallenInto = false;
        }

        public bool CheckForTraps()
        {
            if(FallenInto)
            {
                return false;
            }
            else
            {
                FallenInto = true;
                return true;    
            }
        }

    }
}
