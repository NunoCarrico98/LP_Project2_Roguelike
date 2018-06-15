using System;
using System.Text;

namespace Roguelike
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            GameManager game = new GameManager();
            MainMenu mainMenu = new MainMenu();

            bool[] startGame = new bool[2];

            mainMenu.GetMenuOption(ref startGame);

            if (startGame[0] || startGame[1])
            {
                game.GameLoop(startGame);
            }
        }
    }
}
