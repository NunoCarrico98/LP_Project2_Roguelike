using System.Collections.Generic;

namespace Roguelike
{
    /// <summary>
    /// Classe that defines a game tile. Each is a list of IGameObjects
    /// </summary>
    public class GameTile : List<IGameObject>
    {
        /// <summary>
        /// Constructor that initialises each game tile
        /// </summary>
        public GameTile()
        {
            // Add null to the respective tile
            for (int posInTile = 0; posInTile < 100;
                         posInTile++)
            {
                Add(null);
            }
        }

        /// <summary>
        /// Add game object in the position of the first null found
        /// </summary>
        /// <param name="go">Game object to add to list</param>
        public void AddObject(IGameObject go)
        {
            // Create and Initialise Index
            int i = 0;

            // If tile contains a null element
            if (Contains(null))
            {
                // Go through the full list
                foreach (IGameObject gameObject in this)
                {
                    // If we find a null
                    if (gameObject == null)
                    {
                        // Get out of cycle
                        break;
                    }
                    // If we don't find a null
                    else
                    {
                        // Increment 1 to index
                        i++;
                    }
                }

                // Put gameObject in position defined by index
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
