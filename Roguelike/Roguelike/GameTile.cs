using System.Collections.Generic;

namespace Roguelike
{
    public class GameTile : List<IGameObject>
    {
        public GameTile() : base(10)
        {
            for (int posInTile = 0; posInTile < 10;
                         posInTile++)
            {
                Add(new EmptyTile());
            }
        }
    }
}
