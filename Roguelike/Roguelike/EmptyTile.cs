using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    public class EmptyTile : IGameObject
    {
        public GameObjects Type { get; }

        public EmptyTile()
        {
            Type = GameObjects.None;
        }
    }
}
