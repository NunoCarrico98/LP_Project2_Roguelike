using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Roguelike
{
    /// <summary>
    /// Class that manages the gameLoop
    /// </summary>
    [Serializable()]
    public class GameManager : ISerializable
    {
        private Player player;
        private GridManager grid;
        private Renderer render;

        public GameManager()
        {
            player = new Player(this);
            grid = grid = new GridManager();
            render = new Renderer();
        }

        public void IsLoadOrNew(bool[] startGame)
        {
            if (startGame[0])
            {
                // Set initial Positions
                grid.SetInitialPositions(player);
                GameLoop();
            }
            else if (startGame[1])
            {
                if (File.Exists("GameData.dat"))
                {
                    LoadGame();
                    GameLoop();
                }
                else render.ErrorLoading();
            }
        }

        /// <summary>
        /// Method that Loops through all the game actions
        /// </summary>
        public void GameLoop()
        {
            // Variable to keep the game loop going
            bool endGame = false;

            // GameLoop
            while (!endGame)
            {
                // Render the game grid
                render.RenderBoard(grid, player);
                // Ask for player input
                player.PlayerController(grid);
                // Update the game
                grid.Update(player);

                //Call method to make sure player is not dead
                player.Die(grid);
            }

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Level", grid);
            info.AddValue("Player", player);
        }

        public GameManager(SerializationInfo info, StreamingContext context)
        {
            grid = (GridManager)info.GetValue("Level", typeof(GridManager));
            player = (Player)info.GetValue("Player", typeof(Player));
        }

        public void SaveGame()
        {
            Stream stream = File.Open("GameData.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            grid.GameGrid[player.PlayerPos.X, player.PlayerPos.Y][0] = null;

            bf.Serialize(stream, grid);
            bf.Serialize(stream, player);
            stream.Close();
        }

        public void LoadGame()
        {
            Stream stream = File.Open("GameData.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            grid = (GridManager)bf.Deserialize(stream);
            player = (Player)bf.Deserialize(stream);
            stream.Close();

            grid.GameGrid[player.PlayerPos.X, player.PlayerPos.Y][0] = player;
        }
    }
}
