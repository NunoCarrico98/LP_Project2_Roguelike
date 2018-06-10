using System;

namespace Roguelike
{
    public class Renderer
    {
        public void RenderBoard(GridManager grid)
        {
            char[,][] gameSymbols = new char[grid.Rows, grid.Columns][];

            for (int x = 0; x < grid.Rows; x++)
            {
                for (int y = 0; y < grid.Columns; y++)
                {
                    for (int posInTile = 0; posInTile < grid.ObjectsPerTile;
                        posInTile++)
                    {
                        gameSymbols[x, y][posInTile] =
                            DefineGameSymbol(go: grid.GetGO(x, y, posInTile));
                    }
                }
            }
        }

        public char DefineGameSymbol(IGameObject go)
        {
            char gameSymbol = ' ';
            switch (go.Type)
            {
                case GameObjects.None:
                    gameSymbol = '.';
                    break;
                case GameObjects.Player:
                    gameSymbol = 'P';
                    break;
                case GameObjects.Exit:
                    gameSymbol = 'E';
                    break;
            }
            return gameSymbol;
        }

        public void MainMenuInterface()
        {
            Console.Clear();
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. High Scores");
            Console.WriteLine("3. Credits");
            Console.WriteLine("4. Exit\n");
        }

        public void Credits()
        {
            Console.Clear();
            Console.WriteLine("Trabalho Realizado Por:");
            Console.WriteLine("- Diogo Maia");
            Console.WriteLine("- Ianis Arquissandas");
            Console.WriteLine("- Nuno Carriço\n");
            Console.WriteLine("Press ENTER to go back\n");
            Console.ReadKey();
        }
    }
}
