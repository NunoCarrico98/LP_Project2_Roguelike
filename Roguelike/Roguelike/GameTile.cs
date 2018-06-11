using System.Collections.Generic;

namespace Roguelike
{
    public class GameTile : List<IGameObject>
    {
        public bool Explored{ get; set; }

        public GameTile() : base(10)
        {
            for (int posInTile = 0; posInTile < 100;
                         posInTile++)
            {
                Add(null);
            }
        }

        public void AddObject(IGameObject go)
        {
            int i = 0;

            if (Contains(null))
            {
                foreach (IGameObject gameObject in this)
                {
                    if (gameObject == null)
                    {
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }

                this[i] = go;
            }
        }

        public void RemoveNullsOutsideView()
        {
            while (Count > 10 && Contains(null))
            {
                Remove(null);
            }
        }
    }
}
