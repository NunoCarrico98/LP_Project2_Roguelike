using System;
using System.Collections.Generic;

namespace Roguelike
{
    public class GridManager
    {
        public int Rows { get; } = 8;
        public int Columns { get; } = 8;
        public int ObjectsPerTile { get; } = 10;
        public int Level { get; set; } = 0;

        private GameTile[,] gameGrid;
        private Random rnd = new Random();
        private Exit exit = new Exit();

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
            gameGrid[rnd.Next(0, 8), 0][0] = player;
            int exitRnd = rnd.Next(0, 8);
            for (int i = 0; i < ObjectsPerTile; i++)
            {
                gameGrid[exitRnd, 7][i] = exit;
            }
        }

        public IGameObject GetGO(int x, int y, int posIntTile)
        {
            return gameGrid[x, y][posIntTile];
        }
    }
}
