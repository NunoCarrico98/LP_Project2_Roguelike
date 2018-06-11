using System;

namespace Roguelike
{
    public class Renderer
    {

        public void RenderBoard(GridManager grid)
        {
            string[,][] gameSymbols = new string[grid.Rows, grid.Columns][];

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    gameSymbols[i, j] = new string[grid.ObjectsPerTile];
                }
            }

            Console.Clear();
            Console.WriteLine($"********************* LP1 RPG : Level " +
                $"{grid.Level} *********************\n");
            for (int x = 0; x < grid.Rows; x++)
            {
                for (int y = 0; y < grid.Columns; y++)
                {
                    for (int posInTile = 0; posInTile < grid.ObjectsPerTile / 2;
                        posInTile++)
                    {
                        gameSymbols[x, y][posInTile] =
                            DefineGameSymbol(grid.GetGO(x, y, posInTile), grid.GetTile(x, y));

                        Console.Write(gameSymbols[x, y][posInTile]);
                    }
                    Console.Write("\t");
                }
                Console.WriteLine();
                for (int y = 0; y < grid.Columns; y++)
                {
                    for (int posInTile = grid.ObjectsPerTile / 2;
                        posInTile < grid.ObjectsPerTile; posInTile++)
                    {
                        gameSymbols[x, y][posInTile] =
                            DefineGameSymbol(grid.GetGO(x, y, posInTile), grid.GetTile(x, y));

                        Console.Write(gameSymbols[x, y][posInTile]);
                    }
                    Console.Write("\t");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }       

        public string DefineGameSymbol(IGameObject go, GameTile gametile)
        {
            
            string gameSymbol = "";
            if (gametile.Explored == true)
            {
                if (go is null) gameSymbol = ".";
                else if (go is Player) gameSymbol = "P";
                else if (go is Exit) gameSymbol = "E";
                else if (go is Map) gameSymbol = "M";
            }
            else
            {
                gameSymbol = "~";
            }
            return gameSymbol;
        }

        public void AddNewHighscoreInterface(GridManager grid)
        {
            Console.WriteLine("GOOD JOB!");
            Console.WriteLine("You're 1 of the 10 best players!");
            Console.WriteLine($"Score: {grid.Level}");
            Console.Write("What's your name? ");
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
