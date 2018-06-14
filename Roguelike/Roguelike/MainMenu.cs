using System;

namespace Roguelike
{
    public class MainMenu
    {
        private Renderer render = new Renderer();

        public bool GetMenuOption()
        {
            string input = "";
            int inputInt = 0;
            bool startGame = false;

            while ((input != "1" && input != "2" && input != "3" && 
                input != "4") || !startGame)
            {
                render.MainMenuInterface();
                Console.Write("Option: ");
                input = Console.ReadLine();
                inputInt = Convert.ToInt32(input);
                SetMenuOption(input, ref startGame);
            }

            return startGame;
        }

        public void SetMenuOption(string input, ref bool startGame)
        {
            switch (input)
            {
                case "1":
                    startGame = true;
                    break;
                case "2":
                    render.HighScoreTable();
                    break;
                case "3":
                    render.Credits();
                    break;
                case "4":
                    Environment.Exit(1);
                    break;
            }
        }
    }
}
