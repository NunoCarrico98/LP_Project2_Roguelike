using System;
using System.Collections.Generic;

namespace Roguelike
{
    /// <summary>
    /// Classe that defines a game tile. Each tile is a list of IGameObjects.
    /// </summary>
    [Serializable()]
    public class GameTile : List<IGameObject>
    {
        /// <summary>
        /// Property that defines if a tile is explored or not.
        /// </summary>
        public bool Explored { get; set; }

        /// <summary>
        /// Constructor that initialises each game tile.
        /// </summary>
        public GameTile() : base(10)
        {
            // Add null to the respective tile
            for (int posInTile = 0; posInTile < 10;
                         posInTile++)
            {
                Add(null);
            }
            // Set the tile as unexplored
            Explored = false;
        }

        /// <summary>
        /// Add game object in the position of the first null found.
        /// </summary>
        /// <param name="go">Game object to add to list.</param>
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
            // If tile does not contain a null
            else
            {
                // Add gameObject to the end of the list
                Add(go);
            }
        }

        /// <summary>
        /// Remove nulls that exist but are not being rendered by the grid.
        /// </summary>
        public void RemoveNullsOutsideView()
        {
            // Do cycle while there nulls outside view
            while (Count > 10 && Contains(null))
            {
                // Remove a null from list
                Remove(null);
            }
        }
    }
}
