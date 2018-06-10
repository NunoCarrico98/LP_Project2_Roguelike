using System;

namespace Roguelike
{
    public class Player : IGameObject
    {
        public float Health { get; set; }

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

        public void GetInput()
        {
            string input = "";

            Console.Write("Where to move? ");
            input = Console.ReadLine();

            switch (input.ToLower())
            {
                case "w":

                    break;
                case "s":

                    break;
                case "a":

                    break;
                case "d":

                    break;
            }
        }
    }
}
