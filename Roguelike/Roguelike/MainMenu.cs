using System;

namespace Roguelike
{
    /// <summary>
    /// Class that defines the MainMenu.
    /// </summary>
    public class MainMenu
    {
        /// <summary>
        /// Instance to define and initialise a renderer.
        /// </summary>
        private Renderer render = new Renderer();

        /// <summary>
        /// Method that asks user for an input in main menu.
        /// </summary>
        /// <param name="startGame">Array that be changed according to user 
        /// input.</param>
        public void GetMenuOption(ref bool[] startGame)
        {
            // Create and Initialise variable to hold input
            string input = "";

            // Do cycle while input is not correct or 
            // both start and load game are false
            while ((input != "1" && input != "2" && input != "3" &&
                input != "4") || (!startGame[0] && !startGame[1]))
            {
                // Render Menu Interface
                render.MainMenuInterface();
                Console.Write("> ");
                // Save user input to variable input
                input = Console.ReadLine();
                // Define what program does with input
                SetMenuOption(input, ref startGame);
            }
        }

        /// <summary>
        /// Method that defines what to do after user input in main menu.
        /// </summary>
        /// <param name="input">Variable that contanis user input</param>
        /// <param name="startGame">Array that be changed according to user 
        /// input.</param>
        public void SetMenuOption(string input, ref bool[] startGame)
        {
            // Do something according to user input
            switch (input)
            {
                // If it's 1
                case "1":
                    // Start a new game
                    startGame[0] = true;
                    break;
                // If it's 2
                case "2":
                    // Load a saved game
                    startGame[1] = true;
                    break;
                // If it's 3
                case "3":
                    // Render highscore table
                    render.HighScoreTable();
                    break;
                // If it's 4
                case "4":
                    // Render game credits
                    render.Credits();
                    break;
                // If it's 5
                case "5":
                    // Quit game
                    Console.SetCursorPosition(38, 28);
                    Environment.Exit(1);
                    break;
            }
        }
    }
}
