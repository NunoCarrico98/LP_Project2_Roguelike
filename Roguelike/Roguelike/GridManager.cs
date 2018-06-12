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

            for (int i = 0; i < 5; i++)
            {
                Trap trap = new Trap(new Position(rnd.Next(0,8), rnd.Next(0,8)));
                gameGrid[trap.TrapPos.X, trap.TrapPos.Y].AddObject(trap);
            }
        }

        public void UpdatePlayerPosition(Player player)
        {
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
            return gameGrid[x, y][posIntTile];
        }
    }
}
