using System;
using System.Text;

namespace Roguelike
{
    /// <summary>
    /// Class that defines the Program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main Method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Change Output encoding
            Console.OutputEncoding = Encoding.UTF8;

            // Create and Initialise a GameManager instance
            GameManager game = new GameManager();

            // Create and Initialise a MainMenu instance
            MainMenu mainMenu = new MainMenu();

            // Create and Initialise an array of bools
            // startGame[0] = start new game
            // startGame[1] = load saved game
            bool[] startGame = new bool[2];

            // Ask for user input in main menu
            mainMenu.GetMenuOption(ref startGame);

            // If player chooses to start or load game
            if (startGame[0] || startGame[1])
            {
                // Call method to decide if it's a new game or a load
                game.IsLoadOrNew(startGame);
            }
        }
    }
}
