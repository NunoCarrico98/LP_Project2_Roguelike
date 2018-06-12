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
        private Map map = new Map();
        private Exit exit = new Exit();
        private Position oldPlayerPos;

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

            gameGrid[rnd.Next(0, 8), rnd.Next(0, 8)].AddObject(map);

            // Add Exit to tile on grid
            for (int i = 0; i < ObjectsPerTile; i++)
            {
                gameGrid[exitRnd, 7][i] = exit;
            }

<<<<<<< HEAD
            /* Testing the trap*/
            gameGrid[5, 5].AddObject(trap);
            Explore(player);
=======
            for (int i = 0; i < 5; i++)
            {
                Trap trap = new Trap(new Position(rnd.Next(0,8), rnd.Next(0,8)));
                gameGrid[trap.TrapPos.X, trap.TrapPos.Y].AddObject(trap);
            }
>>>>>>> ianis
        }

        public void UpdatePlayerPosition(Player player)
        {
            Explore(player);

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
            for (int i = ObjectsPerTile - 1; i >= 0; i--)
            {
                IGameObject go = gameGrid[player.PlayerPos.X, player.PlayerPos.Y][i];
                if (go is Trap)
                {
                    if (gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(go as Trap))
                    {
                        player.Health -= (go as Trap).Damage;
                        gameGrid[player.PlayerPos.X, player.PlayerPos.Y].RemoveAt(i);
                        gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Add(null);
                    }
                }
            }

        }

        public void PickUpMap(Player player)
        {
            if (gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(map) &&
               gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(player))
            {
                gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Remove(map);
                gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Add(null);
                foreach(GameTile gt in gameGrid) gt.Explored = true;
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

        public void Explore(Player player)
        {
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Explored = true;

            Position pos1 = Verify(player.PlayerPos.X - 1, player.PlayerPos.Y);
            Position pos2 = Verify(player.PlayerPos.X + 1, player.PlayerPos.Y);
            Position pos3 = Verify(player.PlayerPos.X, player.PlayerPos.Y - 1);
            Position pos4 = Verify(player.PlayerPos.X, player.PlayerPos.Y + 1);

            gameGrid[pos1.X, pos1.Y].Explored = true;
            gameGrid[pos2.X, pos2.Y].Explored = true;
            gameGrid[pos3.X, pos3.Y].Explored = true;
            gameGrid[pos4.X, pos4.Y].Explored = true;
        }

        public Position Verify(int x, int y)
        {
            if (x < 0)
            {
                x = 0;
            }
            if (y < 0)
            {
                y = 0;
            }
            if (y > Columns - 1)
            {
                y = Columns - 1;
            }
            if (x > Rows - 1)
            {
                x = Rows - 1;
            }

            return new Position(x, y);
        }

        public GameTile GetTile(int x, int y)
        {
            return gameGrid[x, y];
        }

        public IGameObject GetGO(int x, int y, int posIntTile)
        {
            // Return game object on grid
            return gameGrid[x, y][posIntTile];
        }
    }
}
