using System;

namespace Roguelike
{
    public class BoardManager
    {
        private Constants consts = new Constants();
        private GameTile[,][] gameGrid;
        private Random rnd = new Random();

        public BoardManager()
        {
            gameGrid = new GameTile[consts.Rows, consts.Columns][];
        }

        public void SetPlayerPosition(Player player)
        {
            gameGrid[consts.Rows, rnd.Next(0, 9)][0].Add(player);
        }
    }
}
