using System;
using System.Collections.Generic;

namespace Roguelike
{
    /// <summary>
    /// Class that defines a renderer.
    /// </summary>
    [Serializable()]
    public class Renderer
    {
        /// <summary>
        /// Method to render the game grid and interface.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="player">Current player in game.</param>
        public void RenderBoard(GridManager grid, Player player)
        {
            // Create and Initialise a multidimensional array of arrays of strings
            string[,][] gameSymbols = new string[grid.Rows, grid.Columns][];

            // Initialise each array
            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    gameSymbols[i, j] = new string[grid.ObjectsPerTile];
                }
            }

            Console.Clear();
            Console.WriteLine($"---------------------------------------  LP1 " +
                $"- RPG : Level {grid.Level}  --------------------------------" +
                $"-------\n");

            // Cycle through al the rows
            for (int x = 0; x < grid.Rows; x++)
            {
                // Cycle through all the colums
                for (int y = 0; y < grid.Columns; y++)
                {
                    // Cycle through half the elements of the current tile
                    for (int posInTile = 0; posInTile < grid.ObjectsPerTile / 2;
                        posInTile++)
                    {
                        // Get Symbol of the respective object
                        gameSymbols[x, y][posInTile] =
                            DefineGameSymbol(grid.GetGO(x, y, posInTile),
                            grid.GetTile(x, y));

                        // Print symbol
                        Console.Write(gameSymbols[x, y][posInTile]);
                    }
                    Console.Write("\t");
                }
                Console.WriteLine();
                // Cycle through all the columns again
                for (int y = 0; y < grid.Columns; y++)
                {
                    // Cycle through the second half of the current tile
                    for (int posInTile = grid.ObjectsPerTile / 2;
                        posInTile < grid.ObjectsPerTile; posInTile++)
                    {
                        // Get Symbol of the respective object
                        gameSymbols[x, y][posInTile] =
                            DefineGameSymbol(grid.GetGO(x, y, posInTile),
                            grid.GetTile(x, y));

                        // Print symbol
                        Console.Write(gameSymbols[x, y][posInTile]);
                    }
                    Console.Write("\t");
                }
                Console.WriteLine();
                Console.WriteLine();
            }

            // Show player stats
            ShowPlayerStats(player);

            // Show Legend of game symbols
            ShowGuide();

            // Show game interface below grid
            ShowGameInterface(grid, player);
        }

        /// <summary>
        /// Method to define a game symbol according to each game object.
        /// </summary>
        /// <param name="go">Game Object being analysed.</param>
        /// <param name="gametile">Game tile being analysed.</param>
        /// <returns>Returns the game symbol corresponding to the analysed 
        /// game object or tile</returns>
        public string DefineGameSymbol(IGameObject go, GameTile gametile)
        {
            // Create and initialise variable to hold game symbol
            string gameSymbol = "";

            // If map is explored
            if (gametile.Explored == true)
            {
                if (go is null) gameSymbol = ".";
                else if (go is Player) gameSymbol = "\u0298";
                else if (go is Exit) gameSymbol = ">";
                else if (go is Map) gameSymbol = "X";
                else if (go is Trap) gameSymbol = "\u00A4";
                else if (go is Food) gameSymbol = "\u0277";
                else if (go is Weapon) gameSymbol = "\u2020";
                else if (go is NPC)
                {
                    if ((go as NPC).NpcType == StateOfNpc.Enemy)
                        gameSymbol = "\u0278";
                    if ((go as NPC).NpcType == StateOfNpc.Neutral)
                        gameSymbol = "\u00B6";
                }
            }
            // If map is unexplored
            else
            {
                gameSymbol = "~";
            }

            // Return game symbol
            return gameSymbol;
        }

        /// <summary>
        /// Method to show player stats.
        /// </summary>
        /// <param name="p">Current player in game.</param>
        public void ShowPlayerStats(Player p)
        {
            // Create and initialise variables to hold current cursor position
            int xCursor = Console.CursorTop;
            int yCursor = Console.CursorLeft;

            // Create and initialise to hold hyphen
            char c = '-';

            // Set cursor position and print player stats
            Console.SetCursorPosition(70, 2);
            Console.WriteLine("Player Stats");
            Console.SetCursorPosition(70, 3);
            Console.WriteLine("--------------");

            // Print player health
            Console.SetCursorPosition(70, 4);
            Console.WriteLine($"HP{c,10}   {p.Health:f1}");

            // Print the currently equipped weapon
            Console.SetCursorPosition(70, 5);
            Console.WriteLine($"Weapon{c,6}");

            // If there is no weapon equipped
            if(p.Equipped == null)
            {
                Console.SetCursorPosition(85, 5);
                Console.WriteLine($"None");
            }
            // If there is a weapon equipped
            else if (p.Equipped != null)
            {
                Console.SetCursorPosition(85, 5);
                Console.WriteLine($"{p.Equipped.WeaponType}");
            }

            // Print Current weight of player
            Console.SetCursorPosition(70, 6);
            Console.WriteLine($"Weight{c,6}   {p.Weight:f1} / {p.MaxWeight:f1}");
            Console.SetCursorPosition(yCursor, xCursor);
        }

        /// <summary>
        /// Method to print the guide to all game symbols in the game.
        /// </summary>
        public void ShowGuide()
        {
            int xCursor = Console.CursorTop;
            int yCursor = Console.CursorLeft;
            Console.SetCursorPosition(70, 10);
            Console.WriteLine("Guide");
            Console.SetCursorPosition(70, 11);
            Console.WriteLine("-------");
            Console.SetCursorPosition(70, 12);
            Console.WriteLine($"\u0298 - Player");
            Console.SetCursorPosition(70, 13);
            Console.WriteLine($"> - Exit");
            Console.SetCursorPosition(70, 14);
            Console.WriteLine($". - Empty");
            Console.SetCursorPosition(70, 15);
            Console.WriteLine($"~ - Unexplored");
            Console.SetCursorPosition(70, 16);
            Console.WriteLine($"\u00B6 - Neutral NPC");
            Console.SetCursorPosition(70, 17);
            Console.WriteLine($"\u0278 - Hostile NPC");
            Console.SetCursorPosition(70, 18);
            Console.WriteLine($"\u0277 - Food");
            Console.SetCursorPosition(70, 19);
            Console.WriteLine($"\u2020 - Weapon");
            Console.SetCursorPosition(70, 20);
            Console.WriteLine($"\u00A4 - Trap");
            Console.SetCursorPosition(70, 21);
            Console.WriteLine($"X - Map");
            Console.SetCursorPosition(yCursor, xCursor);
        }

        /// <summary>
        /// Method that shows the interface below the grid.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        public void ShowGameInterface(GridManager grid, Player p)
        {
            // Show movement messages
            ShowMessages(p.Input);

            // Show messages according with player interactions
            ShowObjectMessages(grid, p);

            // Show Objects in surrounding tiles
            ShowTileObjects(grid, p);

            // Show Options that player has
            ShowsOptions();
        }

        /// <summary>
        /// Method to show movement messages.
        /// </summary>
        /// <param name="input">Player input.</param>
        public void ShowMessages(string input)
        {
            Console.WriteLine("Messages");
            Console.WriteLine("----------");

            // Different message according to player input
            switch (input)
            {
                // If input is w
                case "w":
                    Console.WriteLine("* You moved NORTH");
                    break;
                // If input is w
                case "s":
                    Console.WriteLine("* You moved SOUTH");
                    break;
                // If input is w
                case "a":
                    Console.WriteLine("* You moved WEST");
                    break;
                // If input is w
                case "d":
                    Console.WriteLine("* You moved EAST");
                    break;
            }
        }

        /// <summary>
        /// Method to show messages according with player interactions.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        public void ShowObjectMessages(GridManager grid, Player p)
        {
            // Cycle through all objects in tile backwards
            foreach (IGameObject go in grid.GameGrid[p.PlayerPos.X, p.PlayerPos.Y])
            {
                // If game object is a Trap and it wasn't activated yet
                if (go is Trap && (go as Trap).WroteMessage == false)
                {
                    Console.WriteLine($"* You got hit by a " +
                        $"{(go as Trap).TrapType} and lost " +
                        $"{(go as Trap).Damage:f1} HP");

                    // No longer write this message
                    (go as Trap).WroteMessage = true;
                }

                // If player weapon broke
                if (p.Broken)
                {
                    Console.WriteLine("* Your weapon broke");
                    p.Broken = false;
                }

                // If player killed an NPC
                if (p.Killed)
                {
                    Console.WriteLine($"* You attacked an NPC and " +
                        $"killed it");
                    p.Killed = false;
                }

                // If game object is a NPC and it's an enemy
                if (go is NPC && (go as NPC).NpcType == StateOfNpc.Enemy)
                {
                    // If player input is f and player attacked
                    if (p.Input == "f" && p.Attacked)
                    {
                        Console.WriteLine($"* You attacked an NPC and " +
                                $"did {p.Damage:f1} damage");
                    }
                    // If player enters a tile with a NPC
                    Console.WriteLine("* You were attacked by an NPC and " +
                        $"lost {(go as NPC).Damage:f1} HP");
                }
            }
        }

        /// <summary>
        /// Method to show Objects in surrounding tiles.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        public void ShowTileObjects(GridManager grid, Player p)
        {
            // Create and initialise variable to hold ":"
            char c = ':';

            Console.WriteLine("\nWhat do I see?");
            Console.WriteLine("----------------");

            // Top of player
            // Make sure it's on the map. If yes
            if (RestrictToMap(grid, p.PlayerPos.X, p.PlayerPos.Y)[0])
            {
                Console.Write($"* NORTH{c,2} ");

                // Write information of all game objects in tile
                ObjectsInTile(p, grid, p.PlayerPos.X - 1, p.PlayerPos.Y);

                Console.WriteLine();
            }
            else Console.WriteLine($"* NORTH{c,2} ");

            // Right of Player
            // Make sure it's on the map. If yes
            if (RestrictToMap(grid, p.PlayerPos.X, p.PlayerPos.Y)[3])
            {
                Console.Write($"* EAST{c,3} ");

                // Write information of all game objects in tile
                ObjectsInTile(p, grid, p.PlayerPos.X, p.PlayerPos.Y + 1);

                Console.WriteLine();
            }
            else Console.WriteLine($"* EAST{c,3} ");

            // Left of player
            // Make sure it's on the map. If yes
            if (RestrictToMap(grid, p.PlayerPos.X, p.PlayerPos.Y)[2])
            {
                Console.Write($"* WEST{c,3} ");

                // Write information of all game objects in tile
                ObjectsInTile(p, grid, p.PlayerPos.X, p.PlayerPos.Y - 1);

                Console.WriteLine();
            }
            else Console.WriteLine($"* WEST{c,3} ");

            // Below Player
            // Make sure it's on the map. If yes
            if (RestrictToMap(grid, p.PlayerPos.X, p.PlayerPos.Y)[1])
            {
                Console.Write($"* SOUTH{c,2} ");

                // Write information of all game objects in tile
                ObjectsInTile(p, grid, p.PlayerPos.X + 1, p.PlayerPos.Y);

                Console.WriteLine();
            }
            else Console.WriteLine($"* SOUTH{c,2} ");

            // Player Position
            Console.Write($"* HERE{c,3} ");

            // Write information of all game objects in tile
            ObjectsInTile(p, grid, p.PlayerPos.X, p.PlayerPos.Y);

            Console.WriteLine();
        }

        /// <summary>
        /// Method that writes all information of tiles on the interface below 
        /// the grid.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        /// <param name="x">Given Row.</param>
        /// <param name="y">Given Column.</param>
        public void ObjectsInTile(Player p, GridManager grid, int x, int y)
        {
            // Create and initialise variable that makes sures tile is empty
            bool empty = true;

            // If tile contains exit
            if (grid.GameGrid[x, y].Contains(grid.Exit)) Console.Write("Exit");
            // If tile does not contain exit
            else
            {
                // Cycle through all game objects in tile
                for (int i = 0; i < grid.GameGrid[x, y].Count; i++)
                {
                    IGameObject go = grid.GameGrid[x, y][i];

                    // If there a game object in tile, tile is not empty
                    if (go != null) empty = false;

                    if (go is Trap) Console.Write($"Trap " +
                        $"({(go as Trap).TrapType})");
                    if (go is Map) Console.Write("Map");
                    if (go is Food) Console.Write($"Food " +
                        $"({(go as Food).FoodType})");
                    if (go is Weapon) Console.Write($"Weapon " +
                        $"({(go as Weapon).WeaponType})");
                    if (go is NPC) Console.Write($"NPC " +
                        $"({(go as NPC).NpcType}, HP = {(go as NPC).HP:f1}, " +
                        $"AP = {(go as NPC).AP:f1})");
                    if (i + 1 < grid.GameGrid[x, y].Count)
                    {
                        if (grid.GameGrid[x, y][i] != p &&
                            grid.GameGrid[x, y][i + 1] != null)
                        {
                            Console.Write(", ");
                        }
                    }
                }

                // If tile is empty
                if (empty) Console.Write("Empty");
            }
        }

        /// <summary>
        /// Method to make sure all verification of game objects are made 
        /// on the grid.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="x">Given Row.</param>
        /// <param name="y">Given Column.</param>
        /// <returns></returns>
        public bool[] RestrictToMap(GridManager grid, int x, int y)
        {
            bool[] bools = new bool[4];

            // Top
            // If it's outside the grid
            if (x - 1 < 0) bools[0] = false;
            // If it's on the grid
            else bools[0] = true;

            // Bottom
            // If it's outside the grid
            if (x + 1 > grid.Rows - 1) bools[1] = false;
            // If it's on the grid
            else bools[1] = true;

            // Left
            // If it's outside the grid
            if (y - 1 < 0) bools[2] = false;
            // If it's on the grid
            else bools[2] = true;

            // Right
            // If it's outside the grid
            if (y + 1 > grid.Columns - 1) bools[3] = false;
            // If it's on the grid
            else bools[3] = true;

            // Return array of bools with all verifications
            return bools;
        }

        /// <summary>
        /// Method to show options that player has
        /// </summary>
        public void ShowsOptions()
        {
            Console.WriteLine("\nOptions");
            Console.WriteLine("---------");
            Console.WriteLine("(W) Move NORTH  (A) Move WEST  " +
                "  (S) Move SOUTH (D) Move EAST");
            Console.WriteLine("(F) Attack NPC  (E) Pick up item " +
                "(U) Use item   (V) Drop item");
            Console.WriteLine("(I) Information (K) Save game    (Q) Quit game");
        }

        /// <summary>
        /// Method to show info interface.
        /// </summary>
        public void InfoInterface()
        {
            Console.Clear();

            // Show food info
            ShowFoodInfo();

            // Show weapon info
            ShowWeaponInfo();

            // Show trap info
            ShowTrapInfo();

            Console.ReadKey();
        }

        /// <summary>
        /// Method to show all food info.
        /// </summary>
        public void ShowFoodInfo()
        {
            List<Food> FoodList = new List<Food>();
            Console.WriteLine("Food          |    Hp Increase|      Weight|");
            Console.WriteLine("--------------------------------------------");
            for (int i = 0; i < Enum.GetNames(typeof(TypesOfTraps)).Length; i++)
            {
                FoodList.Add(new Food((TypesOfFood)(i)));
                Console.WriteLine(FoodList[i]);
            }
            Console.WriteLine("\n\n\n\n");
        }

        /// <summary>
        /// Method to show all weapon info.
        /// </summary>
        public void ShowWeaponInfo()
        {
            List<Weapon> WeaponList = new List<Weapon>();
            Console.WriteLine("Weapon        |   Attack Power|      Weight|   " +
                "Durability|");
            Console.WriteLine("-----------------------------------------------" +
                "-----------");
            for (int i = 0; i < Enum.GetNames(typeof(TypesOfTraps)).Length; i++)
            {
                WeaponList.Add(new Weapon((TypesOfWeapon)(i)));
                Console.WriteLine(WeaponList[i]);
            }
            Console.WriteLine("\n\n\n\n");
        }

        /// <summary>
        /// Method to show all trap info.
        /// </summary>
        public void ShowTrapInfo()
        {
            List<Trap> TrapList = new List<Trap>();
            Console.WriteLine("Trap          |      MaxDamage|");
            Console.WriteLine("-------------------------------");
            for (int i = 0; i < Enum.GetNames(typeof(TypesOfTraps)).Length; i++)
            {
                TrapList.Add(new Trap((TypesOfTraps)(i)));
                Console.WriteLine(TrapList[i]);
            }
        }

        /// <summary>
        /// Method to show interface to choose enemy screen.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        /// <param name="count">Counter to be able to set error treatment 
        /// on Fight() method</param>
        /// <returns>Returns a bool to make sure player can only fight on 
        /// certain situations.</returns>
        public bool ChooseEnemyScreen(GridManager grid, Player p, out int count)
        {
            // Initialise counter at 0
            count = 0;

            // Cycle through current tile
            foreach (IGameObject go in grid.GameGrid[p.PlayerPos.X, p.PlayerPos.Y])
            {
                // If gmae object in tile is NPC, increment counter
                if (go is NPC) count++;
            }
            Console.Clear();

            // If count is 0
            if (count == 0)
            {
                // Write error message and leave method
                Console.WriteLine("* There are no NPCs here. You cannot attack *");
                Console.ReadKey();
                return false;
            }

            // If player has no weapon equipped
            if (p.Equipped == null)
            {
                // WWrite error message and leave method
                Console.WriteLine("* You don't have a weapon selected *");
                Console.WriteLine("* You cannot attack *");
                Console.ReadKey();
                return false;
            }
            // If player has a weapon equipped
            else
            {
                // Initialise counter at 1
                count = 1;

                Console.WriteLine($"\nSelect Enemy to Attack");
                Console.WriteLine("---------------");
                Console.WriteLine("0. Go back");

                // Cycle through all game objects in current tile
                foreach (IGameObject go in
                    grid.GameGrid[p.PlayerPos.X, p.PlayerPos.Y])
                {
                    // If game object is a NPC
                    if (go is NPC)
                    {
                        Console.WriteLine($"{count}. {(go as NPC).NpcType}, " +
                            $"HP = {(go as NPC).HP:f1}, AP = {(go as NPC).AP:f1}");

                        // Increment counter
                        count++;
                    }
                }

                // Leave Method returnning true
                return true;
            }
        }

        /// <summary>
        /// Method to show interface to choose item to pick up.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        /// <param name="count">Counter to be able to set error treatment 
        /// on PickUpItem() method</param>
        /// <returns>Returns a bool to make sure player can only pick up items on 
        /// certain situations.</returns>
        public bool PickUpScreen(GridManager grid, Player p, out int count)
        {
            // Initialise counter at 0
            count = 0;

            // Cycle through current tile
            foreach (IGameObject go in grid.GameGrid[p.PlayerPos.X, p.PlayerPos.Y])
            {
                // If game object is an Item, increment counter
                if (go is Item) count++;

                // If game object is a Map, increment counter
                if (go is Map) count++;
            }
            Console.Clear();

            // If counter is 0
            if (count == 0)
            {
                // Write error message and leave method
                Console.WriteLine("* There is nothing to pick up *");
                Console.ReadKey();
                return false;
            }
            // If counter is not 0
            else
            {
                // Initialise counter at 1
                count = 1;

                Console.WriteLine($"Select Item to pick up");
                Console.WriteLine("---------------");
                Console.WriteLine("0. Go back");

                // Cycle through all game objects in current tile
                for (int i = 0; i < grid.ObjectsPerTile; i++)
                {
                    // Define game object currently analysed game object
                    IGameObject go = grid.GameGrid[p.PlayerPos.X, p.PlayerPos.Y][i];

                    // If game object is Food 
                    if (go is Food)
                    {
                        Console.WriteLine($"{count}. Food " +
                            $"({(go as Food).FoodType}) ");
                        count++;
                    }
                    // If game object is Weapon
                    else if (go is Weapon)
                    {
                        Console.WriteLine($"{count}. Weapon " +
                            $"({(go as Weapon).WeaponType}) ");
                        count++;
                    }
                    // If game object is Map
                    else if (go is Map)
                    {
                        Console.WriteLine($"{count}. Map");
                        count++;
                    }
                }

                // Leave Method returnning true
                return true;
            }
        }

        /// <summary>
        /// Method to show interface to choose item to drop.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        /// <param name="count">Counter to be able to set error treatment 
        /// on DropItem() method</param>
        /// <returns>Returns a bool to make sure player can only drop items on 
        /// certain situations.</returns>
        public bool DropItemsScreen(GridManager grid, Player p, out int count)
        {
            // Initialise counter at 0
            count = 0;

            // Cycle through player inventory
            foreach (IGameObject go in p.Inventory)
            {
                // If game object is an Item, increment counter
                if (go is Item) count++;
            }
            Console.Clear();

            // If counter is 0
            if (count == 0)
            {
                // Write error message and leave method
                Console.WriteLine("* There is nothing to drop *");
                Console.ReadKey();
                return false;
            }
            // If counter is not 0
            else
            {
                // Initialise counter at 1
                count = 1;

                Console.WriteLine($"\nSelect Item to drop");
                Console.WriteLine("---------------");
                Console.WriteLine("0. Go back");

                // Cycle through all items in player inventory
                for (int i = 0; i < p.Inventory.Count; i++)
                {
                    // Define game object currently analysed game object
                    IGameObject go = p.Inventory[i];

                    // If game object is Food 
                    if (go is Food)
                    {
                        Console.WriteLine($"{count}. Food " +
                            $"({(go as Food).FoodType}) ");
                        count++;
                    }
                    // If game object is Weapon
                    else if (go is Weapon)
                    {
                        Console.WriteLine($"{count}. Weapon " +
                            $"({(go as Weapon).WeaponType}) ");
                        count++;
                    }
                }

                // Leave Method returnning true
                return true;
            }
        }

        /// <summary>
        /// Method to show interface to choose item to use.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        /// <param name="count">Counter to be able to set error treatment 
        /// on UseItem() method</param>
        /// <returns>Returns a bool to make sure player can only use items on 
        /// certain situations.</returns>
        public bool UseItemScreen(Player p, out int count)
        {
            // Initialise counter at 0
            count = 0;

            // Cycle through player inventory
            foreach (IGameObject go in p.Inventory)
            {
                // If game object is an Item, increment counter
                if (go is Item) count++;
            }
            Console.Clear();

            // If counter is 0
            if (count == 0)
            {
                // Write error message and leave method
                Console.WriteLine("* There is nothing to use *");
                Console.ReadKey();
                return false;
            }
            // If counter is not 0
            else
            {
                // Initialise counter at 1
                count = 1;

                Console.WriteLine($"\nSelect Item to use");
                Console.WriteLine("---------------");
                Console.WriteLine("0. Go back");

                // Cycle through all items in player inventory
                for (int i = 0; i < p.Inventory.Count; i++)
                {
                    // Define game object currently analysed game object
                    IGameObject go = p.Inventory[i];

                    // If game object is Food 
                    if (go is Food)
                    {
                        Console.WriteLine($"{count}. Food " +
                            $"({(go as Food).FoodType}) ");
                        count++;
                    }
                    // If game object is Weapon
                    else if (go is Weapon)
                    {
                        Console.WriteLine($"{count}. Weapon " +
                            $"({(go as Weapon).WeaponType}) ");
                        count++;
                    }
                }

                // Leave Method returnning true
                return true;
            }
        }

        /// <summary>
        /// Method to show interface when player dies.
        /// </summary>
        public void ShowDead()
        {
            Console.Clear();
            Console.WriteLine("* WASTED *");
        }

        /// <summary>
        /// Method to show Interface when player is able to top the 10 best scores.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        public void AddNewHighscoreInterface(GridManager grid)
        {
            Console.WriteLine("* However, you're 1 of the 10 best players! *");
            Console.WriteLine($"* Score: {grid.Level} *");
            Console.WriteLine("* Please Write your name in less than 15 characters *");
        }

        /// <summary>
        /// Method to show Main Menu Interface.
        /// </summary>
        public void MainMenuInterface()
        {
            Console.Clear();
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("3. High Scores");
            Console.WriteLine("4. Credits");
            Console.WriteLine("5. Exit");
        }

        /// <summary>
        /// Method to show High score table.
        /// </summary>
        public void HighScoreTable()
        {
            HighScoreManager hsm = new HighScoreManager();

            Console.Clear();

            // Print a formatted table with the 10 best scores.
            Console.WriteLine(hsm);

            Console.WriteLine("\n* Press any key to go back... *\n");
            Console.ReadKey();
        }

        /// <summary>
        /// Method to show Game Credits.
        /// </summary>
        public void Credits()
        {
            Console.Clear();
            Console.WriteLine("* Trabalho Realizado Por *");
            Console.WriteLine();
            Console.WriteLine("* Diogo Maia *");
            Console.WriteLine("* Ianis Arquissandas *");
            Console.WriteLine("* Nuno Carriço *\n");
            Console.WriteLine();
            Console.WriteLine("* Press any key to go back... *\n");
            Console.ReadKey();
        }

        /// <summary>
        /// Method to show error message when there is no save file to load.
        /// </summary>
        public void ErrorLoading()
        {
            Console.Clear();
            Console.WriteLine("* There is no file to Load *\n");
            Console.WriteLine("* The game will exit... *\n");
        }

        /// <summary>
        /// Method that shows when player has saved the game.
        /// </summary>
        public void SaveComplete()
        {
            Console.Clear();
            Console.WriteLine("* Game Saved *\n");
            Console.WriteLine("* Press any key to go back... *\n");
            Console.ReadKey();
        }
    }
}
