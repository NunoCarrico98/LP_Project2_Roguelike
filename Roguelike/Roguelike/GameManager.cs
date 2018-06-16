using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Roguelike
{
    /// <summary>
    /// Class that manages the game.
    /// </summary>
    [Serializable()]
    public class GameManager : ISerializable
    {
        /// <summary>
        /// Instance that defines the player.
        /// </summary>
        private Player player;
        /// <summary>
        /// Instance that defines the game board.
        /// </summary>
        private GridManager grid;
        /// <summary>
        /// Instance that defines a renderer.
        /// </summary>
        private Renderer render;

        /// <summary>
        /// Constructor that initialises the player, game board and renderer.
        /// </summary>
        public GameManager()
        {
            //Initialise player
            player = new Player(this);
            //Initialise game board
            grid = grid = new GridManager();
            //Initialise renderer
            render = new Renderer();
        }

        /// <summary>
        /// Method that verifies if it's a new game or a saved game.
        /// </summary>
        /// <param name="startGame">Array that contains 2 bools that define 
        /// if it's a save or load.</param>
        public void IsLoadOrNew(bool[] startGame)
        {
            // If it's a new game
            if (startGame[0])
            {
                // Set initial Positions
                grid.SetInitialPositions(player);
                // Call Gameloop
                GameLoop();
            }
            // If it's a saved game
            else if (startGame[1])
            {
                // If save file exists
                if (File.Exists("GameData.dat"))
                {
                    // Load Game
                    LoadGame();
                    // Call Gameloop
                    GameLoop();
                }
                // If file does not exist, send error message and leave game
                else render.ErrorLoading();
            }
        }

        /// <summary>
        /// Method that Loops through all the game actions.
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

        /// <summary>
        /// Method that gets object data to be saved.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Store data to be serialized
            info.AddValue("Level", grid);
            info.AddValue("Player", player);
        }

        /// <summary>
        /// Alternative Constructor to initialise grid and player as 
        /// they are in the save file.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public GameManager(SerializationInfo info, StreamingContext context)
        {
            // Get all the data through deserialization
            grid = (GridManager)info.GetValue("Level", typeof(GridManager));
            player = (Player)info.GetValue("Player", typeof(Player));
        }

        /// <summary>
        /// Method to save the game.
        /// </summary>
        public void SaveGame()
        {
            // Create and Initialise stream and binary formatter
            Stream stream = File.Open("GameData.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            // Remove player from grid
            grid.GameGrid[player.PlayerPos.X, player.PlayerPos.Y][0] = null;

            // Serialize level
            bf.Serialize(stream, grid);
            // Serialize player
            bf.Serialize(stream, player);
            // Close stream
            stream.Close();
        }

        /// <summary>
        /// Method to load the save file.
        /// </summary>
        public void LoadGame()
        {
            Stream stream = File.Open("GameData.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            // Deserialize level
            grid = (GridManager)bf.Deserialize(stream);
            // Deserialize player
            player = (Player)bf.Deserialize(stream);
            // Close stream
            stream.Close();

            grid.GameGrid[player.PlayerPos.X, player.PlayerPos.Y][0] = player;
        }
    }
}
