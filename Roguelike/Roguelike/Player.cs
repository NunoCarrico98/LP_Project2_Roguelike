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
                    playerPos.X--;
                    break;
                case "s":
                    playerPos.X++;
                    break;
                case "a":
                    playerPos.Y--;
                    break;
                case "d":
                    playerPos.Y++;
                    break;
            }
        }
    }
}
