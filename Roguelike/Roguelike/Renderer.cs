using System;

namespace Roguelike
{
    public class Renderer
    {
        private Constants consts = new Constants();

        public void RenderBoard(BoardManager board)
        {
            char[,][] gameSymbols = new char[consts.Rows, consts.Columns][];

            for (int x = 0; x < consts.Rows; x++)
            {
                for (int y = 0; y < consts.Columns; y++)
                {
                    for (int posInTile = 0; posInTile < consts.ObjectsPerTile;
                        posInTile++)
                    {
                        gameSymbols = 
                    }
                }
            }
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
