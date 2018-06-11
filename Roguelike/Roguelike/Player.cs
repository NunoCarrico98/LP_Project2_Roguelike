using System;

namespace Roguelike
{
    public class Player : IGameObject
    {
        public float Health { get; set; }
        public Position PlayerPos { get; set; }

        public Player()
        {
            Health = 100;
        }

        public void Die()
        {
            if (Health <= 0)
            {
                Console.WriteLine("You Died. :(");
                Environment.Exit(1);
            }
        }

        public void GetInput(GridManager grid)
        {
            string input = "";

            Console.Write("> ");
            input = Console.ReadLine();

            switch (input.ToLower())
            {
                case "w":
                    if (PlayerPos.X < 1)
                    {
                        PlayerPos.X = 0;
                        break;
                    }
                    else {
                        PlayerPos.X--;
                        break;
                    }
                case "s":
                    if (PlayerPos.X > 6)
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

                    if (PlayerPos.Y < 1)
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

                    if (PlayerPos.Y > 6)
                    {
                        PlayerPos.Y = 7;
                        break;
                    }
                    else
                    {
                        PlayerPos.Y++;
                        break;
                    }
            }
        }
    }
}
