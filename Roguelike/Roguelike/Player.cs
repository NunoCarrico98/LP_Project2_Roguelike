using System;
using System.Collections.Generic;

namespace Roguelike
{
    public class Player : IGameObject
    {
        public double Health { get; set; }
        public string Input { get; private set; } = "";
        public float MaxWeight { get; private set; }
        public float Weight { get; set; }
        public double Damage { get; set; }
        public Position PlayerPos { get; set; }
        public List<Item> Inventory { get; set; } = new List<Item>();
        public Weapon Equipped { get; set; }

        private HighScoreManager hsm;
        private Renderer render;
        private Random rnd = new Random();

        public Player()
        {
            Health = 100;
            hsm = new HighScoreManager();
            render = new Renderer();
            MaxWeight = 20;
            Equipped = null;
        }

        public void Die(GridManager grid)
        {
            if (Health <= 0)
            {
                render.RenderBoard(grid, this);
                Console.WriteLine("You Died. :(");
                AddNewHighScore(grid);
                Environment.Exit(1);
            }
        }

        public void PlayerController(GridManager grid)
        {
            do
            {
                Console.Write("\n> ");
                Input = Console.ReadLine();
                int count = 0;

                switch (Input.ToLower())
                {
                    case "w":
                        PlayerPos =
                            grid.RestrictToMap(PlayerPos.X - 1, PlayerPos.Y);
                        break;
                    case "s":
                        PlayerPos =
                            grid.RestrictToMap(PlayerPos.X + 1, PlayerPos.Y);
                        break;
                    case "a":
                        PlayerPos =
                            grid.RestrictToMap(PlayerPos.X, PlayerPos.Y - 1);
                        break;
                    case "d":
                        PlayerPos =
                            grid.RestrictToMap(PlayerPos.X, PlayerPos.Y + 1);
                        break;
                    case "i":
                        Console.Clear();
                        render.InfoInterface();
                        Console.ReadKey();
                        render.RenderBoard(grid, this);
                        break;
                    case "q":
                        MakeSureQuit(grid);
                        render.RenderBoard(grid, this);
                        break;
                    case "e":
                        if (render.PickUpScreen(grid, this, out count))
                            PickUpItems(grid, count);
                        break;
                    case "v":
                        if (render.DropItemsScreen(grid, this, out count))
                            DropItems(grid, count);
                        break;
                    case "u":
                        if (render.UseItemScreen(this, out count))
                            UseItems(count);
                        break;
                    case "f":
                        if (render.ChooseEnemyScreen(grid, this, out count))
                            Fight(grid, count);
                        break;
                }
            } while (Input == "i" || Input == "q");
        }

        public void Fight(GridManager grid, int count)
        {
            bool attacked = false;
            string choice = "";
            int i = 0;
            do
            {
                Console.Write("\n> ");
                choice = Console.ReadLine();
                if (choice != "") i = Convert.ToInt32(choice);
            } while (i < 0 || i > count - 1 || choice == "");
            if (i == 0)
            {
                return;
            }
            else
            {
                do
                {
                    IGameObject go = grid.gameGrid[PlayerPos.X, PlayerPos.Y][i];
                    if (go is NPC)
                    {
                        if ((go as NPC).NpcType == StateOfNpc.Neutral)
                            (go as NPC).NpcType = StateOfNpc.Enemy;
                        Damage = rnd.Next(0, Equipped.AttackPower);
                        (go as NPC).HP -= Damage;
                        if (rnd.NextDouble() < 1 - Equipped.Durability)
                        {
                            Equipped = null;
                            Weight -= Equipped.Weight;
                        }
                        (go as NPC).Die(grid);
                        attacked = true;
                    }
                    else
                    {
                        i++;
                    }
                } while (!attacked);
            }
        }

        public void PickUpItems(GridManager grid, int count)
        {
            bool picked = false;
            string choice = "";
            int i = 0;
            do
            {
                Console.Write("\n> ");
                choice = Console.ReadLine();
                if (choice != "") i = Convert.ToInt32(choice);
            } while (i < 0 || i > count - 1 || choice == "");
            if (i == 0)
            {
                Health++;
                return;
            }
            else
            {
                do
                {
                    IGameObject go = grid.gameGrid[PlayerPos.X, PlayerPos.Y][i];
                    if (go is Item)
                    {
                        if ((Weight + (go as Item).Weight) > MaxWeight)
                        {
                            Console.WriteLine("You can't carry anymore.");
                            Console.ReadLine();
                            return;
                        }
                        else
                        {
                            Inventory.Add(go as Item);
                            Weight += (go as Item).Weight;
                            grid.gameGrid[PlayerPos.X, PlayerPos.Y].RemoveAt(i);
                            grid.gameGrid[PlayerPos.X, PlayerPos.Y].Add(null);
                            picked = true;
                        }
                    }
                    else if (go is Map)
                    {
                        grid.gameGrid[PlayerPos.X, PlayerPos.Y].Remove(grid.Map);
                        grid.gameGrid[PlayerPos.X, PlayerPos.Y].Add(null);
                        foreach (GameTile gt in grid.gameGrid) gt.Explored = true;
                        picked = true;
                    }
                    else
                    {
                        i++;
                    }
                } while (!picked);
            }
        }

        public void DropItems(GridManager grid, int count)
        {
            string choice = "";
            int i = 0;
            do
            {
                Console.Write("\n> ");
                choice = Console.ReadLine();
                if (choice != "") i = Convert.ToInt32(choice);
            } while (i < 0 || i > count - 1 || choice == "");

            if (i == 0)
            {
                Health++;
                return;
            }
            else
            {
                IGameObject go = Inventory[i - 1];
                if (go is Item)
                {
                    grid.gameGrid[PlayerPos.X, PlayerPos.Y].AddObject(go);
                    Inventory.RemoveAt(i - 1);
                    Weight -= (go as Item).Weight;
                }
            }
        }

        public void UseItems(int count)
        {
            string choice = "";
            int i = 0;
            do
            {
                Console.Write("\n> ");
                choice = Console.ReadLine();
                if (choice != "") i = Convert.ToInt32(choice);
            } while (i < 0 || i > count - 1 || choice == "");

            if (i == 0)
            {
                Health++;
                return;
            }
            else
            {
                IGameObject go = Inventory[i - 1];
                if (go is Food)
                {
                    Inventory.RemoveAt(i - 1);
                    Weight -= (go as Item).Weight;
                    if ((Health + (go as Food).HPIncrease) > 100)
                    {
                        Health = 100;
                    }
                    else
                    {
                        Health += (go as Food).HPIncrease;
                    }
                }
                else if (go is Weapon)
                {
                    Inventory.RemoveAt(i - 1);
                    if (Equipped == null)
                    {
                        Equipped = (go as Weapon);
                    }
                    else
                    {
                        Inventory.Add(Equipped as Weapon);
                        Equipped = (go as Weapon);
                    }
                }
            }
        }

        public void MakeSureQuit(GridManager grid)
        {
            Console.Clear();
            Console.WriteLine("Are you sure you want to quit? (y/n)");
            string input = "";
            do
            {
                Console.Write("\n> ");
                input = Console.ReadLine();
            } while (input.ToLower() != "n" && input.ToLower() != "y");
            switch (input.ToLower())
            {
                case "y":
                    AddNewHighScore(grid);
                    Environment.Exit(1);
                    break;
                case "n":
                    break;
            }
        }

        public void AddNewHighScore(GridManager grid)
        {
            if (hsm.Highscores[hsm.Highscores.Count - 1].Item2 < grid.Level)
            {
                string name = "";
                render.AddNewHighscoreInterface(grid);
                do {
                    Console.Write("\n> ");
                    name = Console.ReadLine();
                } while (name.Length > 15);
                hsm.AddScore(name, grid.Level);
                hsm.Save();
            }
        }
    }
}
