using System;

namespace Roguelike
{
    public class Player : IGameObject
    {
        public float Health { get; set; }
        public GameObjects Type { get; }
        public Position playerPos { get; set; }

        public Player()
        {
            Health = 100;
            Type = GameObjects.Player;
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
                    if (playerPos.X < 1)
                    {
                        playerPos.X = 0;
                        break;
                    }
                    else {
                        playerPos.X--;
                        break;
                    }
                case "s":
                    if (playerPos.X > 6)
                    {
                        playerPos.X = 7;
                        break;
                    }
                    else
                    {
                        playerPos.X++;
                        break;
                    }
                case "a":

                    if (playerPos.Y < 1)
                    {
                        playerPos.Y = 0;
                        break;
                    }
                    else
                    {
                        playerPos.Y--;
                        break;
                    }
                case "d":

                    if (playerPos.Y > 6)
                    {
                        playerPos.Y = 7;
                        break;
                    }
                    else
                    {
                        playerPos.Y++;
                        break;
                    }
            }
        }
    }
}
