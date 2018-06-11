using System;

namespace Roguelike
{
    public class GameManager
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
                player.GetInput(grid);
                grid.Update(player);

                player.Die();
            }
        }
    }
}
