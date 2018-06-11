using System;
using System.Collections.Generic;

namespace Roguelike
{
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

        public void Update(Player player)
        {
            UpdatePlayerPosition(player);
            CheckForTraps(player);
            WinLevel(player);
        }

        public void SetInitialPlayerAndExitPosition(Player player)
        {
            int exitRnd = rnd.Next(0, 8);
            int playerRnd = rnd.Next(0, 8);

            gameGrid[playerRnd, 0][0] = player;
            oldPlayerPos = new Position(playerRnd, 0);
            player.PlayerPos = new Position(playerRnd, 0);

            for (int i = 0; i < ObjectsPerTile; i++)
            {
                gameGrid[exitRnd, 7][i] = exit;
            }
            /* Testing the trap*/
            gameGrid[5, 5].AddObject(trap);
            Explore(player);
        }

        public void UpdatePlayerPosition(Player player)
        {
            Explore(player);

            player.Health--;

            // Remove Player from current tile
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Remove(player);
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Insert(0, null);
            oldPlayerPos = new Position(player.PlayerPos.X, player.PlayerPos.Y);

            // Add Player to new tile
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Insert(0, player);
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].RemoveNullsOutsideView();
        }

        public void CheckForTraps(Player player)
        {
            /* Testing the trap */
            if (gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(trap))
            {
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
            return gameGrid[x, y][posIntTile];
        }
    }
}
