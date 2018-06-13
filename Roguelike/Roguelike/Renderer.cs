﻿using System;
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
            }
            else
            {
                gameSymbol = "~";
            }
            return gameSymbol;
        }

        public void ShowGameInterface(GridManager grid, Player p)
        {
            Console.WriteLine(p.Health);
            Console.WriteLine(p.Weight);
            ShowMessages(p.Input);
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

        public void ShowTileObjects(GridManager grid, Player p)
        {
            Position pos1 = grid.Verify(p.PlayerPos.X - 1, p.PlayerPos.Y);
            Position pos2 = grid.Verify(p.PlayerPos.X + 1, p.PlayerPos.Y);
            Position pos3 = grid.Verify(p.PlayerPos.X, p.PlayerPos.Y - 1);
            Position pos4 = grid.Verify(p.PlayerPos.X, p.PlayerPos.Y + 1);

            Console.WriteLine("\nWhat do I see?");
            Console.WriteLine("----------------");

            Console.Write("* NORTH: ");
            ObjectsInTile(grid, pos1);
            Console.WriteLine();

            Console.Write("* EAST: ");
            ObjectsInTile(grid, pos4);
            Console.WriteLine();

            Console.Write("* WEST: ");
            ObjectsInTile(grid, pos3);
            Console.WriteLine();

            Console.Write("* SOUTH: ");
            ObjectsInTile(grid, pos2);
            Console.WriteLine();
        }

        public void ObjectsInTile(GridManager grid, Position pos)
        {
            if (grid.gameGrid[pos.X, pos.Y].Contains(grid.Exit))
                Console.Write("Exit");
            foreach (IGameObject go in grid.gameGrid[pos.X, pos.Y])
            {
                if (go is Trap) Console.Write($"Trap " +
                    $"({(TypesOfTraps)(go as Trap).TrapType})");
                if (go is Map) Console.Write("Map");
            }
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
                } else if (go is Map)
                {
                    Console.WriteLine($"{count}. Map");
                    count++;
                }
            }
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
