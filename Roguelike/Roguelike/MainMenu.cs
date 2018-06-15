using System;

namespace Roguelike
{
    public class MainMenu
    {
        private Renderer render = new Renderer();

        public void GetMenuOption(ref bool[] startGame)
        {
            string input = "";
            int inputInt = 0;

            while ((input != "1" && input != "2" && input != "3" &&
                input != "4") || (!startGame[0] && !startGame[1]))
            {
                render.MainMenuInterface();
                Console.Write("Option: ");
                input = Console.ReadLine();
                inputInt = Convert.ToInt32(input);
                SetMenuOption(input, ref startGame);
            }
        }

        public void SetMenuOption(string input, ref bool[] startGame)
        {
            switch (input)
            {
                case "1":
                    startGame[0] = true;
                    break;
                case "2":
                    startGame[1] = true;
                    break;
                case "3":
                    render.HighScoreTable();
                    break;
                case "4":
                    render.Credits();
                    break;
                case "5":
                    Environment.Exit(1);
                    break;
            }
        }
    }
}
