using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    public class Exit : IGameObject
    {
        public GameObjects Type { get; }

        public Exit()
        {
            Type = GameObjects.Exit;
        }
    }
}
