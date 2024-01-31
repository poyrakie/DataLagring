namespace Presentation.ConsoleApp.Services;

public class MainMenuService(UserMenuService userMenuService, ProductMenuService productMenuService)
{
    private readonly UserMenuService _userMenuService = userMenuService;
    private readonly ProductMenuService _productMenuService = productMenuService;

    public void MainMenu()
    {
        string[] menuOptions =
    {
                "Naviagate to user menu.",
                "Naviagate to product menu.",
                "Exit program."
            };
        while (true)
        {
            Console.Clear();
            MenuHeader("User Menu");
            Console.WriteLine("Welcome, what would you like to do?");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine($"{i + 1}: \t\t{menuOptions[i]}");
            }
            var answer = Console.ReadKey().Key;
            switch (answer)
            {
                case ConsoleKey.D1:
                    _userMenuService.UserMainMenu(this);
                    break;
                case ConsoleKey.D2:
                    _productMenuService.ProductMainMenu(this);
                    break;
                case ConsoleKey.D3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input detected, press any key to try again!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void MenuHeader(string menu)
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine($"\t{menu}");
        Console.WriteLine("----------------------------\n\n");
    }
}
