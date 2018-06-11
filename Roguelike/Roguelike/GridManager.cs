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

        public void SetInitialPlayerAndExitPosition(Player player)
        {
            int exitRnd = rnd.Next(0, 8);
            int playerRnd = rnd.Next(0, 8);

            gameGrid[playerRnd, 0][0] = player;
            oldPlayerPos = new Position(playerRnd, 0);
            player.playerPos = new Position(playerRnd, 0);

            for (int i = 0; i < ObjectsPerTile; i++)
            {
                gameGrid[exitRnd, 7][i] = exit;
            }
        }

        public void UpdatePlayerPosition(Player player)
        {
            gameGrid[player.playerPos.X, player.playerPos.Y].Explored = true;
            player.Health--;

            // Remove Player from current tile
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Remove(player);
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Add(new EmptyTile());
            oldPlayerPos = new Position(player.playerPos.X, player.playerPos.Y);

            // Add Player to new tile
            gameGrid[player.playerPos.X, player.playerPos.Y].Insert(0, player);
            // If tile has more than 10 objects, remove last
            if (gameGrid[player.playerPos.X, player.playerPos.Y].Count > 10)
            {
                gameGrid[player.playerPos.X, player.playerPos.Y]
                    .RemoveAt(gameGrid[player.playerPos.X, player.playerPos.Y].Count - 1);
            }

            // If current tile contains player and exit
            if (gameGrid[player.playerPos.X, player.playerPos.Y].Contains(exit) &&
               gameGrid[player.playerPos.X, player.playerPos.Y].Contains(player))
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
