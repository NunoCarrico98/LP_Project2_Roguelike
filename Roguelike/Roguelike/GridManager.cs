using System;
using System.Collections.Generic;

namespace Roguelike
{
    /// <summary>
    /// Class that controls all grid movement and management
    /// </summary>
    public class GridManager
    {
        public int Rows { get; } = 8;
        public int Columns { get; } = 8;
        public int ObjectsPerTile { get; } = 10;
        public int Level { get; set; } = 1;

        private GameTile[,] gameGrid;
        private Random rnd = new Random();
        private Exit exit = new Exit();
        private Position oldPlayerPos;
        private Trap trap = new Trap();

        /// <summary>
        /// Constructor to Initialise Level
        /// </summary>
        public GridManager()
        {
            gameGrid = new GameTile[Rows, Columns];
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    gameGrid[x, y] = new GameTile();
                }
            }
        }

        /// <summary>
        /// Update Grid
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            // Update player position
            UpdatePlayerPosition(player);
            //Check current tile for traps
            CheckForTraps(player);
            // Check if player won level
            WinLevel(player);
        }

        public void SetInitialPlayerAndExitPosition(Player player)
        {
            // Create and Initialise Player and Exit randoms
            int exitRnd = rnd.Next(0, 8);
            int playerRnd = rnd.Next(0, 8);

            // Add player to grid
            gameGrid[playerRnd, 0][0] = player;
            // Save player position
            oldPlayerPos = new Position(playerRnd, 0);
            player.PlayerPos = new Position(playerRnd, 0);

            // Add Exit to tile on grid
            for (int i = 0; i < ObjectsPerTile; i++)
            {
                gameGrid[exitRnd, 7][i] = exit;
            }

            /* Testing the trap*/
            gameGrid[5, 5].AddObject(trap);
        }

        public void UpdatePlayerPosition(Player player)
        {
            // Remove Player from current tile
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Remove(player);
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Insert(0, null);
            oldPlayerPos = new Position(player.PlayerPos.X, player.PlayerPos.Y);

            // Add Player to new tile
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Insert(0, player);
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].RemoveNullsOutsideView();

            // Take 1 health from player
            player.Health--;
        }

        public void CheckForTraps(Player player)
        {
            /* Testing the trap */
            if (gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(trap))
            {
                // Take random Health from player
                player.Health -= rnd.Next(0, trap.MaxDamage);
                gameGrid[5, 5].Remove(trap);
                gameGrid[5, 5].Add(null);
            }
        }

        public void WinLevel(Player player)
        {
            // If current tile contains player and exit
            if (gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(exit) &&
               gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(player))
            {
                // Remove everything from grid
                for (int x = 0; x < Rows; x++)
                {
                    for (int y = 0; y < Columns; y++)
                    {
                        gameGrid[x, y] = new GameTile();
                    }
                }

                // Add level
                Level++;

                // Begin new level
                SetInitialPlayerAndExitPosition(player);
            }
        }

        public IGameObject GetGO(int x, int y, int posIntTile)
        {
            // Return game object on grid
            return gameGrid[x, y][posIntTile];
        }
    }
}
