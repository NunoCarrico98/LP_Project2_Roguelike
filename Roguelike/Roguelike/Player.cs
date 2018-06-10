using System;

namespace Roguelike
{
    public class Player : IGameObject
    {
        public float Health { get; set; }
        public GameObjects Type { get; }

        public int playerX = 0;
        public int playerY = 0;
        public Player playerType;

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

            Console.Write("Where to move? ");
            input = Console.ReadLine();

            switch (input.ToLower())
            {
                case "w":
                    playerY += 1;
                    grid.SetPlayerPosition(playerX, playerY, playerType);
                    break;
                case "s":
                    playerY -= 1;
                    break;
                case "a":
                    playerX -= 1;
                    break;
                case "d":
                    playerX += 1;
                    break;
            }
        }
    }
}
