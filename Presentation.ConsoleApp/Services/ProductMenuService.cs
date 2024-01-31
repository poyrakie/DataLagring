using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using Infrastructure.Services;
using System.Security;

namespace Presentation.ConsoleApp.Services;

public class ProductMenuService(ProductService productService, VerificationRepository verificationRepository, ProfileRepository profileRepository, ProductRepository productRepository, ProductFactories productFactories, OrderRowRepository orderRowRepository)
{
    private readonly ProductFactories _productFactories = productFactories;
    private readonly ProductService _productService = productService;

    private readonly ProductRepository _productRepository = productRepository;
    private readonly OrderRowRepository _orderRowRepository = orderRowRepository;

    private readonly VerificationRepository _verificationRepository = verificationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;

    public void ProductMainMenu(MainMenuService mainMenuService)
    {
        string[] menuOptions =
            {
                "Shop menu.",
                "Admin menu.",
                "Return to main menu."
            };
        while (true)
        {
            Console.Clear();
            MenuHeader("Product Menu");
            Console.WriteLine("Welcome, what would you like to do?");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine($"{i + 1}: \t\t{menuOptions[i]}");
            }
            var answer = Console.ReadKey().Key;
            switch (answer)
            {
                case ConsoleKey.D1:
                    ShowShopMenu(mainMenuService);
                    break;
                case ConsoleKey.D2:
                    AdminProductMenu(mainMenuService);
                    break;
                case ConsoleKey.D3:
                    mainMenuService.MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid input detected, press any key to try again!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public void ShowShopMenu(MainMenuService mainMenuService)
    {
        MenuHeader("Shopping menu");
        Console.WriteLine("Please enter your email to start shopping");
        string email = Console.ReadLine()!;
        if (email == null)
        {
            Console.WriteLine("You must enter an email.");
            Console.WriteLine("Returning to product menu");
            Console.ReadKey();
        }
        else
        {

            if (_verificationRepository.Exists(x => x.Email == email))
            {
                Console.Clear();
                MenuHeader("ShoppingMenu");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. See all your previous orders.");
                Console.WriteLine("2. Create a new order.");
                Console.WriteLine("3. Return to product menu.");
                string answer = Console.ReadLine()!;
                switch (answer)
                {
                    case "1":
                        ShowPreviousOrdersMenu(mainMenuService, email);
                        break;
                    case "2":
                        ShowNewOrderMenu(mainMenuService, email);
                        break;
                    case "3":
                        ProductMainMenu(mainMenuService);
                        break;
                    default:
                        Console.WriteLine("Invalid input detected. Returning to product menu.");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.WriteLine("User not found. Returning to product menu.");
                Console.ReadKey();
            }
        }
    }

    public void ShowPreviousOrdersMenu(MainMenuService mainMenuService, string email)
    {
        VerificationEntity verificationEntity = _verificationRepository.GetOne(x => x.Email == email);
        IEnumerable<Order> orders = _productFactories.GetAllOrders(verificationEntity.UserId);
        Console.Clear();
        if (orders.Any())
        {
            Console.WriteLine("Please enter the number of the order you would like to inspect, or 0 to return to product menu.");
            foreach (var (item, index) in orders.Select((o, i) => (o, i)))
            {
                Console.Write($"{index + 1}, ");
            }
            string answer = Console.ReadLine()!;
            if (int.TryParse(answer, out int result))
            {
                if (result == 0)
                {
                    Console.WriteLine("Returning to product menu");
                    Console.ReadKey();
                    ProductMainMenu(mainMenuService);
                }
                else
                {
                    Order order = orders.ElementAt(result - 1);
                    IEnumerable<OrderRow> orderRows = _productFactories.GetAllOrderRows(order.Id);
                    var productList = _productService.GetAllProducts();
                    decimal price = 0;
                    foreach (var row in orderRows)
                    {
                        ProductDto product = productList.FirstOrDefault(x => x.Id == row.ProductId)!;

                        Console.WriteLine("------------------");
                        Console.WriteLine($"{row.Amount} {product.Title}");
                        Console.WriteLine($"Category: {product.CategoryName}");
                        Console.WriteLine($"Manufacturer: {product.ManufacturerName}");
                        Console.WriteLine($"Price: {product.Price}");
                        Console.WriteLine("------------------");
                        price += product.Price*row.Amount;
                    }
                    Console.WriteLine($"Total price: {price}");
                    Console.WriteLine("Press any key to return to product menu");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Invalid input detected. Returning to product menu.");
                Console.ReadKey();
                ProductMainMenu(mainMenuService);
            }
        }
        else
        {
            Console.WriteLine("This user does not seem to have any previous order. Would you like to create a new order instead? (y/n)");
            var answer = Console.ReadKey().Key;
            if (answer == ConsoleKey.Y)
            {
                ShowNewOrderMenu(mainMenuService, email);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Returning to product menu");
                ProductMainMenu(mainMenuService);
            }
        }
    }

    public void ShowNewOrderMenu(MainMenuService mainMenuService, string email)
    {
        VerificationEntity verificationEntity = _verificationRepository.GetOne(x => x.Email == email);
        Order order = _productFactories.CreateOrderEntity(verificationEntity.UserId);
        while (true)
        {
            var productList = _productService.GetAllProducts();

            Console.Clear();
            MenuHeader("All products");
            foreach (var (item, index) in productList.Select((p, i) => (p, i)))
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Product {index + 1}:");
                Console.WriteLine($"Title:\t\t{item.Title} ");
                Console.WriteLine($"Category:\t{item.CategoryName}");
                Console.WriteLine($"Manufacturer:\t{item.ManufacturerName}");

                Console.WriteLine("----------------------------");
            }
            Console.Write("Enter the number of the product you would like to add to your shoppingcart, or 0 to return to product menu: ");
            string answer = Console.ReadLine()!;
            if (int.TryParse(answer, out int result))
            {
                if (result == 0)
                {
                    Console.WriteLine("Returning to product menu");
                    Console.ReadKey();
                    ProductMainMenu(mainMenuService);
                }
                else
                {
                    result--;
                    if (result >= 1 && result <= productList.Count())
                    {
                        ProductDto product = productList.ElementAt(result - 1);
                        if (product != null && !_orderRowRepository.Exists(x => x.OrderId == order.Id && x.ProductId == product.Id))
                        {
                            OrderRow row = new OrderRow
                            {
                                ProductId = product.Id,
                                OrderId = order.Id,
                                Amount = 1
                            };
                            row = _orderRowRepository.Create(row);
                            if (row != null)
                            {
                                Console.WriteLine($"{product.Title} added to your order. Would you like to add more products?(y/n)");

                            }
                        }
                        else if (product != null && _orderRowRepository.Exists(x => x.OrderId == order.Id && x.ProductId == product.Id))
                        {
                            OrderRow row = _orderRowRepository.GetOne(x => x.OrderId == order.Id && x.ProductId == product.Id);
                            row.Amount++;
                            row = _orderRowRepository.Update(x => x.OrderId == row.OrderId && x.ProductId == row.ProductId, row);
                            Console.WriteLine($"Another {product.Title} added to your order. You now have a total of {row.Amount} in your order");
                            Console.WriteLine("Would you like to add more products?(y/n)");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Product not found. Would you like to try again?(y/n)");
                    }
                    var exit = Console.ReadKey().Key;
                    if (exit != ConsoleKey.Y)
                    {
                        ProductMainMenu(mainMenuService);
                    }
                }
            }

        }
    }

    public void AdminProductMenu(MainMenuService mainMenuService)
    {
        Console.Clear();
        Console.WriteLine("Please enter your email to comfirm you're an admin!");
        string email = Console.ReadLine()!;
        if (!string.IsNullOrEmpty(email))
        {
            VerificationEntity verificationEntity = _verificationRepository.GetOne(x => x.Email == email);
            ProfileEntity profileEntity = _profileRepository.GetOne(x => x.UserId == verificationEntity.UserId);
            if (profileEntity != null && profileEntity.RoleId == 1)
            {
                while (true)
                {
                    Console.Clear();
                    MenuHeader("Admin menu");
                    Console.WriteLine("\tWhat would you like to do?");
                    Console.WriteLine("1.\tAdd a product");
                    Console.WriteLine("2.\tRemove a product");
                    Console.WriteLine("3.\tReturn to product menu");
                    var answer = Console.ReadKey().Key;
                    switch (answer)
                    {
                        case ConsoleKey.D1:
                            AddProductMenu(mainMenuService);
                            break;
                        case ConsoleKey.D2:
                            RemoveProductMenu(mainMenuService);
                            break;
                        case ConsoleKey.D3:
                            ProductMainMenu(mainMenuService);
                            break;
                        default:
                            Console.WriteLine("Invalid input detected. Please try again!");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Admin privilege not found");
            }
        }
        else { Console.WriteLine("Invalid input"); }
        Console.ReadKey();
    }

    public void AddProductMenu(MainMenuService mainMenuService)
    {
        string[] menuStrings =
                    {
                    "Title",
                    "Category",
                    "Manufacturer",
                    "Description",
                    "Price"
                    };
        ProductDto product = new ProductDto();
        Console.Clear();
        for (int i = 0; i < menuStrings.Length; i++)
        {
            switch (menuStrings[i])
            {
                case "Title":
                    SetValidInput(val => product.Title = val, menuStrings[0]);
                    break;
                case "Category":
                    SetValidInput(val => product.CategoryName = val, menuStrings[1]);
                    break;
                case "Manufacturer":
                    SetValidInput(val => product.ManufacturerName = val, menuStrings[2]);
                    break;
                case "Description":
                    SetValidInput(val => product.Description = val, menuStrings[3]);
                    break;
                case "Price":
                    SetValidPriceInput(val => product.Price = val, menuStrings[4]);
                    break;
            }
        }
        if (_productService.CreateProduct(product))
        {
            Console.WriteLine("Product successfully saved");
            Console.WriteLine("Would you like to add another product? (y/n)");
            var answer = Console.ReadKey().Key;
            switch (answer)
            {
                case ConsoleKey.Y:
                    AdminProductMenu(mainMenuService);
                    break;
                default:
                    ProductMainMenu(mainMenuService);
                    break;
            }
        }
    }

    public void RemoveProductMenu(MainMenuService mainMenuService)
    {
        var productList = _productService.GetAllProducts();

        Console.Clear();
        MenuHeader("All products");
        foreach (var (item, index) in productList.Select((p, i) => (p, i)))
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine($"Product {index + 1}:");
            Console.WriteLine($"Title:\t\t{item.Title} ");
            Console.WriteLine($"Category:\t{item.CategoryName}");
            Console.WriteLine($"Manufacturer:\t{item.ManufacturerName}");

            Console.WriteLine("----------------------------");
        }
        Console.Write("Enter the number of the product you would like to remove, or 0 to return to admin menu: ");
        string answer = Console.ReadLine()!;
        if (int.TryParse(answer, out int result))
        {
            if (result == 0)
            {
                Console.WriteLine("Returning to admin menu");
                Console.ReadKey();
            }
            else
            {

                ProductDto product = productList.ElementAt(result - 1);
                if (product != null && _productRepository.Delete(x => x.Id == product.Id))
                {
                    Console.WriteLine("Product removed successfully.");
                    Console.ReadKey();
                }
            }
        }
    }

    private void SetValidInput(Action<string> setProperty, string property)
    {
        while (true)
        {
            Console.Write($"{property}: ");
            var input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                input = char.ToUpper(input[0]) + input.Substring(1);
                setProperty(input);
                break;
            }
            else
            {
                Console.WriteLine($"{property} may not be empty");
            }
        }
    }

    private void SetValidPriceInput(Action<Decimal> setProperty, string property)
    {
        while (true)
        {
            Console.Write($"{property}: ");
            var input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                decimal.TryParse(input, out decimal result);
                setProperty(result);
                break;
            }
            else
            {
                Console.WriteLine($"{property} may not be empty");
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
