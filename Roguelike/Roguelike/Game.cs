using System;

namespace Roguelike
{
    public class Game
    {
        public void GameLoop()
        {
            Player player = new Player();

            bool endGame = false;

            while (!endGame)
            {
                player.Die();
                player.Health--;
                Console.ReadKey();
            }
        }
    }
}
