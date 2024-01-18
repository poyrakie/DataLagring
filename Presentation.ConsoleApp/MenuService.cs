using Infrastructure.Dtos;
using Infrastructure.Services;

namespace Presentation.ConsoleApp;

public class MenuService(UserService userService)
{
    private readonly UserService _userService = userService;
    public void MainMenu()
    {
        //UserRegDto user = new UserRegDto();
        //Console.Clear();

        //Console.WriteLine("Let's create a new user!");
        //Console.Write("Please enter a first name!");
        //user.FirstName = Console.ReadLine()!;
        //Console.Clear();

        //Console.Write("Please enter a last name!");
        //user.LastName = Console.ReadLine()!;
        //Console.Clear();

        //Console.Write("Please enter a City!");
        //user.City = Console.ReadLine()!;
        //Console.Clear();

        //Console.Write("Please enter a Street!");
        //user.Street = Console.ReadLine()!;
        //Console.Clear();

        //Console.Write("Please enter a Postalcode!");
        //user.PostalCode = Console.ReadLine()!;
        //Console.Clear();

        //Console.Write("Please enter an Email!");
        //user.Email = Console.ReadLine()!;
        //Console.Clear();

        //Console.Write("Please enter a Password!");
        //user.Password = Console.ReadLine()!;
        //Console.Clear();

        //var result = _userService.CreateUser(user);

        //if (result)
        //{
        //    Console.WriteLine("The user was registered successfully!");
        //}
        //else
        //{
        //    Console.WriteLine("Something went wrong!");
        //}
        //Console.ReadKey();


        var userList = _userService.GetAll();
        Console.Clear();

        Console.WriteLine("DISPLAYING ALL USERS: ");
        foreach (var item in userList)
        {
            Console.WriteLine(item.FirstName);
            Console.WriteLine(item.LastName);
            Console.WriteLine(item.Email);
            Console.WriteLine(item.City);
            Console.WriteLine(item.Street);
            Console.WriteLine(item.PostalCode);
        }
        Console.ReadKey();
    }
}
