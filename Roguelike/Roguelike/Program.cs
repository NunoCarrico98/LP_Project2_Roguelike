namespace Roguelike
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager game = new GameManager();
            MainMenu mainMenu = new MainMenu();

            if (mainMenu.GetMenuOption())
            {
                game.GameLoop();
            }
        }
    }
}
