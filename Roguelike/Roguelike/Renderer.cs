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
                else if (go is Food) gameSymbol = "F";
                else if (go is Weapon) gameSymbol = "W";
                else if (go is NPC)
                {
                    if((go as NPC).NpcType == StateOfNpc.Enemy) 
                        gameSymbol = "H";
                    if ((go as NPC).NpcType == StateOfNpc.Neutral)
                        gameSymbol = "N";
                }
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
            Console.WriteLine($"HP{c,10}{p.Health,7:f1}");
            Console.SetCursorPosition(70, 5);
            Console.WriteLine($"Weapon{c,6}");
            Console.SetCursorPosition(yCursor, xCursor);
        }

        public void ShowGameInterface(GridManager grid, Player p)
        {
            Console.WriteLine(p.Health);
            Console.WriteLine(p.Weight);
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
            foreach (IGameObject go in grid.gameGrid[p.PlayerPos.X, p.PlayerPos.Y])
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
            char c = ':';

            Console.WriteLine("\nWhat do I see?");
            Console.WriteLine("----------------");

            // Top of player
            if (RestrictToMap(grid, p.PlayerPos.X, p.PlayerPos.Y)[0])
            {
                Console.Write($"* NORTH{c,2} ");
                ObjectsInTile(grid, p.PlayerPos.X - 1, p.PlayerPos.Y);
                Console.WriteLine();
            }
            else Console.WriteLine($"* NORTH{c,2} ");

            // Right of Player
            if (RestrictToMap(grid, p.PlayerPos.X, p.PlayerPos.Y)[3])
            {
                Console.Write($"* EAST{c,3} ");
                ObjectsInTile(grid, p.PlayerPos.X, p.PlayerPos.Y + 1);
                Console.WriteLine();
            }
            else Console.WriteLine($"* EAST{c,3} ");

            // Left of player
            if (RestrictToMap(grid, p.PlayerPos.X, p.PlayerPos.Y)[2])
            {
                Console.Write($"* WEST{c,3} ");
                ObjectsInTile(grid, p.PlayerPos.X, p.PlayerPos.Y - 1);
                Console.WriteLine();
            }
            else Console.WriteLine($"* WEST{c,3} ");

            // Below Player
            if (RestrictToMap(grid, p.PlayerPos.X, p.PlayerPos.Y)[1])
            {
                Console.Write($"* SOUTH{c,2} ");
                ObjectsInTile(grid, p.PlayerPos.X + 1, p.PlayerPos.Y);
                Console.WriteLine();
            }
            else Console.WriteLine($"* SOUTH{c,2} ");

            // Player Position
            Console.Write($"* HERE{c,3} ");
            ObjectsInTile(grid, p.PlayerPos.X, p.PlayerPos.Y);
            Console.WriteLine();
        }

        public void ObjectsInTile(GridManager grid, int x, int y)
        {
            bool empty = true;
            if (grid.gameGrid[x, y].Contains(grid.Exit))
                Console.Write("Exit");
            foreach (IGameObject go in grid.gameGrid[x, y])
            {
                if (go != null) empty = false;
                if (go is Trap) Console.Write($"Trap " +
                    $"({(go as Trap).TrapType}) ");
                if (go is Map) Console.Write("Map ");
                if (go is Food) Console.Write($"Food " +
                    $"({(go as Food).FoodType}) ");
                if (go is Weapon) Console.Write($"Weapon " +
                    $"({(go as Weapon).WeaponType}) ");
                if (go is NPC) Console.Write($"NPC " +
                    $"({(go as NPC).NpcType}, HP = {(go as NPC).HP}, " +
                    $"AP = {(go as NPC).AP}) ");
            }
            if (empty) Console.Write("Empty");
        }

        public bool[] RestrictToMap(GridManager grid, int x, int y)
        {
            bool[] bools = new bool[4];

            // Top
            if (x - 1 < 0) bools[0] = false;
            else bools[0] = true;

            // Bottom
            if (x + 1 > grid.Rows - 1) bools[1] = false;
            else bools[1] = true;

            // Left
            if (y - 1 < 0) bools[2] = false;
            else bools[2] = true;

            // Right
            if (y + 1 > grid.Columns - 1) bools[3] = false;
            else bools[3] = true;

            return bools;
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
            ShowFoodInfo();
            ShowWeaponInfo();
            ShowTrapInfo();
        }

        public void PickUpScreen(GridManager grid, Player p)
        {
            int count = 1;
            Console.WriteLine($"\nSelect Item to pick up");
            Console.WriteLine("---------------");
            Console.WriteLine("0. Go back");

            for (int i = 0; i < grid.ObjectsPerTile; i++)
            {
                IGameObject go = grid.gameGrid[p.PlayerPos.X, p.PlayerPos.Y][i];
                if (go is Food)
                {
                    Console.WriteLine($"{count}. Food ({(go as Food).FoodType}) ");
                    count++;
                }
                else if (go is Weapon)
                {
                    Console.WriteLine($"{count}. Weapon ({(go as Weapon).WeaponType}) ");
                    count++;
                }
                else if (go is Map)
                {
                    Console.WriteLine($"{count}. Map");
                    count++;
                }
            }

            Console.Write("\n> ");
        }

        public void DropItemsScreen(GridManager grid, Player p)
        {
            int count = 1;
            Console.WriteLine($"\nSelect Item to drop");
            Console.WriteLine("---------------");
            Console.WriteLine("0. Go back");

            for (int i = 0; i < p.Inventory.Count; i++)
            {
                IGameObject go = p.Inventory[i];
                if (go is Food)
                {
                    Console.WriteLine($"{count}. Food ({(go as Food).FoodType}) ");
                    count++;
                }
                else if (go is Weapon)
                {
                    Console.WriteLine($"{count}. Weapon ({(go as Weapon).WeaponType}) ");
                    count++;
                }
            }
        }

        public void UseItemScreen(Player p)
        {
            int count = 1;
            Console.WriteLine($"\nSelect Item to use");
            Console.WriteLine("---------------");
            Console.WriteLine("0. Go back");

            for (int i = 0; i < p.Inventory.Count; i++)
            {
                IGameObject go = p.Inventory[i];
                if (go is Food)
                {
                    Console.WriteLine($"{count}. Food ({(go as Food).FoodType}) ");
                    count++;
                }
                else if (go is Weapon)
                {
                    Console.WriteLine($"{count}. Weapon ({(go as Weapon).WeaponType}) ");
                    count++;
                }
            }
        }


        public void ShowFoodInfo()
        {
            List<Food> FoodList = new List<Food>();
            Console.WriteLine("Food          |    Hp Increase|      Weight|\n");
            for (int i = 0; i < Enum.GetNames(typeof(TypesOfTraps)).Length; i++)
            {
                FoodList.Add(new Food((TypesOfFood)(i)));
                Console.WriteLine(FoodList[i]);
            }
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine();
        }

        public void ShowWeaponInfo()
        {
            List<Weapon> WeaponList = new List<Weapon>();
            Console.WriteLine("Weapon        |   Attack Power|      Weight|   Durability|\n");
            for (int i = 0; i < Enum.GetNames(typeof(TypesOfTraps)).Length; i++)
            {
                WeaponList.Add(new Weapon((TypesOfWeapon)(i)));
                Console.WriteLine(WeaponList[i]);
            }
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine();
        }

        public void ShowTrapInfo()
        {
            List<Trap> TrapList = new List<Trap>();
            Console.WriteLine("Trap          |      MaxDamage|\n");
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
