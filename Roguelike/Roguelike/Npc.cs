using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    public class NPC : IGameObject
    {
        public GameObjects Type { get; } = GameObjects.NPC;
    }
}
