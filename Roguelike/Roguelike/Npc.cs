using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    class NPC : IGameObject
    {
        public float Hp { get; private set; }
        public float Attack { get; private set; }
        public StateOfNpc NpcType { get; set; }
        public Position Pos { get; set; }
        double maxHPForThisLevel = 10;
        Random rnd = new Random();

        public NPC(Position pos)
        {
            Hp = (float)(rnd.NextDouble() * 10);
            Attack = (float)(rnd.NextDouble() * 10);
            NpcType = (StateOfNpc)rnd.Next(0, Enum.GetNames(typeof(StateOfNpc)).Length);
            Pos = pos;
        }

    }
}
