using System;

namespace Roguelike
{
    public class Player : IGameObject
    {
        public float Health { get; set; }
        public Position PlayerPos { get; set; }
        private HighScoreManager hsm;
        private Renderer render;

        public Player()
        {
            Health = 100;
            hsm = new HighScoreManager();
            render = new Renderer();
        }

        public void Die(GridManager grid)
        {
            if (Health <= 0)
            {
                Console.WriteLine("You Died. :(");
                AddNewHighScore(grid);
                Environment.Exit(1);
            }
        }

        public void PlayerController(GridManager grid)
        {
            string input = "";

            Console.Write("> ");
            input = Console.ReadLine();

            switch (input.ToLower())
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
                    if (PlayerPos.X == grid.Rows)
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
                    break;
                case "q":
                    AddNewHighScore(grid);
                    Environment.Exit(1);
                    break;
                case "e":               
                    grid.PicksMap(this);
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
