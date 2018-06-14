using System;
using System.Text;

namespace Roguelike
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            GameManager game = new GameManager();
            MainMenu mainMenu = new MainMenu();

            if (mainMenu.GetMenuOption())
            {
                game.GameLoop();
            }
        }
    }
}
