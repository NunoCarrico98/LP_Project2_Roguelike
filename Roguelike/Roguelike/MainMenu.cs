﻿using System;

namespace Roguelike
{
    public class MainMenu
    {
        Renderer render = new Renderer();

        public bool GetMenuOption()
        {
            string input = "";
            int inputInt = 0;
            bool startGame = false;

            while (((inputInt < 1) || (inputInt > 4)) || !startGame)
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
                    // High Scores
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
