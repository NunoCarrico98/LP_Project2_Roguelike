namespace Roguelike
{
    /// <summary>
    /// Class that manages the gameLoop
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// Method that Loops through all the game actions
        /// </summary>
        public void GameLoop()
        {
            // Variables to Initialise game
            Player player = new Player();
            GridManager grid = new GridManager();
            Renderer render = new Renderer();

            // variable to end game
            bool endGame = false;

            // Set initial Positions
            grid.SetInitialPositions(player);

            // GameLoop
            while (!endGame)
            {
                // Render the game grid
                render.RenderBoard(grid, player);
                // Ask for player input
                player.PlayerController(grid);
                // Update the game
                grid.Update(player);

                //Call method to make sure payer is not dead
                player.Die(grid);
            }
        }
    }
}
