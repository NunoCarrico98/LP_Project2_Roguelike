using System;
using System.Collections.Generic;

namespace Roguelike
{
    public class Player : IGameObject
    {
        public float Health { get; set; }
        public string Input { get; private set; } = "";
        public Position PlayerPos { get; set; }
        public float MaxWeight { get; private set; }
        public List<Item> Inventory { get; set; } = new List<Item>();
        public float Weight { get; set; }
        public Weapon Equipped { get; set; }

        private HighScoreManager hsm;
        private Renderer render;

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

                switch (Input.ToLower())
                {
                    case "w":
                        if (PlayerPos.X == 0)
                        {
                            PlayerPos.X = 0;
                            break;
                        }
                        else
                        {
                            PlayerPos.X--;
                            break;
                        }
                    case "s":
                        if (PlayerPos.X == grid.Rows - 1)
                        {
                            PlayerPos.X = 7;
                            break;
                        }
                        else
                        {
                            PlayerPos.X++;
                            break;
                        }
                    case "a":

                        if (PlayerPos.Y == 0)
                        {
                            PlayerPos.Y = 0;
                            break;
                        }
                        else
                        {
                            PlayerPos.Y--;
                            break;
                        }
                    case "d":

                        if (PlayerPos.Y == grid.Columns - 1)
                        {
                            PlayerPos.Y = 7;
                            break;
                        }
                        else
                        {
                            PlayerPos.Y++;
                            break;
                        }
                    case "i":
                        Console.Clear();
                        render.InfoInterface();
                        Console.ReadKey();
                        render.RenderBoard(grid, this);
                        break;
                    case "q":
                        MakeSureQuit(grid);
                        break;
                    case "e":
                        Console.Clear();
                        if(render.PickUpScreen(grid, this))
                            grid.PickUpItems(this);
                        break;
                    case "v":
                        Console.Clear();
                        if(render.DropItemsScreen(grid, this))
                            grid.DropItems(this);
                        break;
                    case "u":
                        Console.Clear();
                        if(render.UseItemScreen(this))
                            grid.UseItems(this);
                        break;
                }
            } while (Input == "I" || Input == "Q" || Input == "E" || Input == "U" || Input == "V");

        }

        public void MakeSureQuit(GridManager grid)
        {
            Console.WriteLine("Are you sure you want to quit? (y/n)");
            string input = "";
            input = Console.ReadLine();
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
                name = Console.ReadLine();
                hsm.AddScore(name, grid.Level);
                hsm.Save();
            }
        }
    }
}
