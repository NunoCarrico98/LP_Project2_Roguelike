using System;

namespace Roguelike
{
    public class Game
    {
        public void GameLoop()
        {
            Player player = new Player();
            GridManager grid = new GridManager();
            Renderer render = new Renderer();

            bool endGame = false;

            grid.SetInitialPlayerAndExitPosition(player);

            while (!endGame)
            {
                render.RenderBoard(grid);


                player.Die();
                player.Health--;
                player.GetInput(grid);
                Console.ReadKey();
            }
        }
    }
}
