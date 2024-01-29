namespace Presentation.ConsoleApp.Services;

public class ProductMenuService
{


    //public void ProductMainMenu()
    //{
    //    string[] menuOptions =
    //        {
    //            "Show all products.",
    //            "Add a product.",
    //            "Return to main menu."
    //        };
    //    while (true)
    //    {
    //        Console.Clear();
    //        MenuHeader("Product Menu");
    //        Console.WriteLine("Welcome, what would you like to do?");
    //        for (int i = 0; i < menuOptions.Length; i++)
    //        {
    //            Console.WriteLine($"{i + 1}: \t\t{menuOptions[i]}");
    //        }
    //        var answer = Console.ReadKey().Key;
    //        switch (answer)
    //        {
    //            case ConsoleKey.D1:
    //                break;
    //            case ConsoleKey.D2:
    //                break;
    //            case ConsoleKey.D3:
    //                _mainMenuService.MainMenu();
    //                break;
    //            default:
    //                Console.WriteLine("Invalid input detected, press any key to try again!");
    //                Console.ReadKey();
    //                break;
    //        }
    //    }
    //}


    private void MenuHeader(string menu)
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine($"\t{menu}");
        Console.WriteLine("----------------------------\n\n");
    }
}
