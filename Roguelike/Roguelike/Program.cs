namespace Roguelike
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            MainMenu mainMenu = new MainMenu();

            if (mainMenu.GetMenuOption())
            {
                game.GameLoop();
            }
        }
    }
}
