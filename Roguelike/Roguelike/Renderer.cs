using System;
using System.Collections.Generic;

namespace Roguelike
{
    public class Renderer
    {

        public void RenderBoard(GridManager grid, Player player)
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
            Console.WriteLine($"------------------------------------  LP1 - RPG : Level " +
                $"{grid.Level}  ------------------------------------\n");
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

            ShowPlayerStats(player);
            ShowGameInterface(grid, player);
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
                else if (go is Trap) gameSymbol = "T";
            }
            else
            {
                gameSymbol = "~";
            }
            return gameSymbol;
        }

        public void ShowPlayerStats(Player p)
        {
            int xCursor = Console.CursorTop;
            int yCursor = Console.CursorLeft;
            char c = '-';
            Console.SetCursorPosition(70, 2);
            Console.WriteLine("Player Stats");
            Console.SetCursorPosition(70, 3);
            Console.WriteLine("--------------");
            Console.SetCursorPosition(70, 4);
            Console.WriteLine($"HP{c,10}{p.Health, 7:f1}");
            Console.SetCursorPosition(70, 5);
            Console.WriteLine($"Weapon{c,6}");
            Console.SetCursorPosition(yCursor, xCursor);
        }

        public void ShowGameInterface(GridManager grid, Player p)
        {
            ShowMessages(p.Input);
            ShowTrapMessages(grid, p);
            ShowTileObjects(grid, p);
            ShowsOptions();
        }

        public void ShowMessages(string input)
        {
            Console.WriteLine("Messages");
            Console.WriteLine("----------");
            switch (input)
            {
                case "w":
                    Console.WriteLine("* You moved NORTH");
                    break;
                case "s":
                    Console.WriteLine("* You moved SOUTH");
                    break;
                case "a":
                    Console.WriteLine("* You moved WEST");
                    break;
                case "d":
                    Console.WriteLine("* You moved EAST");
                    break;
            }
        }

        public void ShowTrapMessages(GridManager grid, Player p)
        {
            // Cycle through all objects in tile backwards
            foreach(IGameObject go in grid.gameGrid[p.PlayerPos.X, p.PlayerPos.Y])
            {
                // If object is a Trap and it wasn't activated yet
                if (go is Trap && (go as Trap).WroteMessage == false)
                {
                    Console.WriteLine($"* You got hit by a " +
                        $"{(go as Trap).TrapType} and lost " +
                        $"{(go as Trap).Damage:f1} HP");
                    (go as Trap).WroteMessage = true;
                }
            }
        }

        public void ShowTileObjects(GridManager grid, Player p)
        {
            Position pos1 = grid.RestrictToMap(p.PlayerPos.X - 1, p.PlayerPos.Y);
            Position pos2 = grid.RestrictToMap(p.PlayerPos.X + 1, p.PlayerPos.Y);
            Position pos3 = grid.RestrictToMap(p.PlayerPos.X, p.PlayerPos.Y - 1);
            Position pos4 = grid.RestrictToMap(p.PlayerPos.X, p.PlayerPos.Y + 1);

            Console.WriteLine("\nWhat do I see?");
            Console.WriteLine("----------------");

            char c = ':';
            Console.Write($"* NORTH{c,2} ");
            ObjectsInTile(grid, pos1);
            Console.WriteLine();

            Console.Write($"* EAST{c,3} ");
            ObjectsInTile(grid, pos4);
            Console.WriteLine();

            Console.Write($"* WEST{c,3} ");
            ObjectsInTile(grid, pos3);
            Console.WriteLine();

            Console.Write($"* SOUTH{c,2} ");
            ObjectsInTile(grid, pos2);
            Console.WriteLine();

            Console.Write($"* HERE{c,3} ");
            ObjectsInTile(grid, p.PlayerPos);
            Console.WriteLine();
        }

        public void ObjectsInTile(GridManager grid, Position pos)
        {
            bool empty = true;
            if (grid.gameGrid[pos.X, pos.Y].Contains(grid.Exit))
                Console.Write("Exit");
            foreach (IGameObject go in grid.gameGrid[pos.X, pos.Y])
            {
                if (go != null) empty = false;
                if (go is Trap) Console.Write($"Trap " +
                    $"({(go as Trap).TrapType}) ");
                else if (go is Map) Console.Write("Map ");
            }

            if (empty) Console.Write("Empty");
        }

        public void ShowsOptions()
        {
            Console.WriteLine("\nOptions");
            Console.WriteLine("---------");
            Console.WriteLine("(W) Move NORTH  (A) Move WEST  " +
                "  (S) Move SOUTH (D) Move EAST");
            Console.WriteLine("(F) Attack NPC  (E) Pick up item " +
                "(U) Use item   (V) Drop item");
            Console.WriteLine("(I) Information (Q) Quit game");
        }

        public void InfoInterface()
        {
            ShowTrapInfo();
        }

        public void ShowTrapInfo()
        {
            List<Trap> TrapList = new List<Trap>();
            Console.WriteLine("Trap          |      MaxDamage\n"); //10 spaces
            for (int i = 0; i < Enum.GetNames(typeof(TypesOfTraps)).Length; i++)
            {
                TrapList.Add(new Trap((TypesOfTraps)(i)));
                Console.WriteLine(TrapList[i]);
            }
        }

        public void AddNewHighscoreInterface(GridManager grid)
        {
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
