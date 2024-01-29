using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Presentation.ConsoleApp.Services;

public class UserMenuService(AddressRepository addressRepository, ProfileRepository profileRepository, UserRepository userRepository, VerificationRepository verificationRepository, UserFactories userFactories, UserService userService)
{
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;

    private readonly UserFactories _userFactories = userFactories;

    private readonly UserService _userService = userService;

    public void UserMainMenu(MainMenuService mainMenuService)
    {
        string[] menuOptions =
            {
                "Show all users.",
                "Add a user.",
                "Return to main menu."
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
                    ShowAllMenu(mainMenuService);
                    break;
                case ConsoleKey.D2:
                    ShowAddMenu(mainMenuService);
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
    public void ShowAllMenu(MainMenuService mainMenuService)
    {
        while (true)
        {
            int i = 0;
            int result;
            var userList = _userService.GetAll();
            Console.Clear();
            MenuHeader("All users");
            foreach (var (item, index) in userList.Select((u, i) => (u, i)))
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine($"User {index + 1}:");
                Console.WriteLine($"First name:\t{item.FirstName}");
                Console.WriteLine($"Last name:\t{item.LastName} ");
                Console.WriteLine("----------------------------");
                i++;
            }
            Console.Write("Enter the number of the user you would like to inspect, or 0 to return to user menu: ");
            string answer = Console.ReadLine()!;
            if (int.TryParse(answer, out result))
            {
                if (result == 0)
                {
                    Console.WriteLine("Returning to user menu");
                    Console.ReadKey();
                    UserMainMenu(mainMenuService);
                }
                else
                {

                    DisplayUserDto user = userList.ElementAt(result - 1);
                    VerificationEntity e = _verificationRepository.GetOne(x => x.Email == user.Email);
                    ProfileEntity entity = _profileRepository.GetOne(x => x.UserId == e.UserId);
                    if (entity != null)
                    {
                        ShowEditAndDeleteMenu(entity, mainMenuService);
                    }
                }
            }

        }
    }
    public void ShowAddMenu(MainMenuService mainMenuService)
    {
        while (true)
        {
            string[] menuStrings =
                {
                    "First name",
                    "Last name",
                    "Street",
                    "City",
                    "Postalcode",
                    "Email",
                    "Password"
                };
            UserRegDto user = new UserRegDto();
            Console.Clear();
            for (int i = 0; i < menuStrings.Length; i++)
            {
                Console.Write($"{menuStrings[i]}: ");
                string answer = Console.ReadLine()!;
                if (answer != string.Empty)
                {
                    switch (menuStrings[i])
                    {
                        case "First name":
                            user.FirstName = answer;
                            break;
                        case "Last name":
                            user.LastName = answer;
                            break;
                        case "Street":
                            user.Street = answer;
                            break;
                        case "City":
                            user.City = answer;
                            break;
                        case "Postalcode":
                            user.PostalCode = answer;
                            break;
                        case "Email":
                            if (!_verificationRepository.Exists(x => x.Email == answer))
                            {
                                user.Email = answer;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("That email already exists. Would you like to try again? (y/n)");
                                var tryAgainAnswer = Console.ReadKey().Key;
                                switch (tryAgainAnswer)
                                {
                                    case ConsoleKey.Y:
                                        ShowAddMenu(mainMenuService);
                                        break;
                                    default:
                                        Console.WriteLine("Returning to user menu");
                                        UserMainMenu(mainMenuService);
                                        break;
                                }
                                break;
                            }
                        case "Password":
                            user.Password = answer;
                            break;
                    }
                }
            }
            if (_userService.CreateUser(user))
            {
                Console.WriteLine("User successfully saved");
                Console.WriteLine("Would you like to add another user? (y/n)");
                var answer = Console.ReadKey().Key;
                switch (answer)
                {
                    case ConsoleKey.Y:
                        ShowAddMenu(mainMenuService);
                        break;
                    default:
                        UserMainMenu(mainMenuService);
                        break;
                }
            }
        }
    }
    public void ShowEditAndDeleteMenu(ProfileEntity entity, MainMenuService mainMenuService)
    {
        string[] propertyName =
        {
            "First name",
            "Last name",
            "Email",
            "City",
            "Street",
            "Postalcode",
        };
        while (true)
        {
            Console.Clear();
            Console.WriteLine("----------------------------");
            Console.WriteLine("(value)");
            Console.WriteLine($"(1)First name:\t\t{entity.User.FirstName}");
            Console.WriteLine($"(2)Last name:\t\t{entity.User.LastName} ");
            Console.WriteLine($"(3)Email:\t\t<{entity.User.Verification.Email}>");
            Console.WriteLine("(4)Address:");
            Console.WriteLine($"\tCity:\t\t{entity.Address.City}");
            Console.WriteLine($"\tStreet:\t\t{entity.Address.Street}");
            Console.WriteLine($"\tPostalcode:\t\t{entity.Address.PostalCode}");
            Console.WriteLine("----------------------------\n\n");
            Console.Write("Choose a value to update (9 for delete, and 0 to return to user menu)");
            string answer = Console.ReadLine()!;
            int.TryParse(answer, out int result);
            switch (result)
            {
                case 1:
                    UserEntity userEntityFirstName = _userRepository.GetOne(x => x.Id == entity.UserId);
                    SetValidInput(() => Console.ReadLine()?.Trim()!, val => userEntityFirstName.FirstName = val, propertyName[0]);
                    break;
                case 2:
                    UserEntity userEntityLastName = _userRepository.GetOne(x => x.Id == entity.UserId);
                    SetValidInput(() => Console.ReadLine()?.Trim()!, val => userEntityLastName.LastName = val, propertyName[1]);
                    break;
                case 3:
                    VerificationEntity verificationEntity = _verificationRepository.GetOne(x => x.UserId == entity.UserId);
                    SetValidEmail(() => Console.ReadLine()?.Trim()!, val => verificationEntity.Email = val, propertyName[2]);
                    break;
                case 4:
                    UpdateAddress(entity, propertyName[3], propertyName[4], propertyName[5]);
                    break;
                case 9:
                    Console.Write("Are you sure you want to delete? (y/n)");
                    var answerKey = Console.ReadKey().Key;
                    if (answerKey == ConsoleKey.Y)
                    {
                        if (_verificationRepository.Delete(x => x.UserId == entity.UserId) && _profileRepository.Delete(x => x.UserId == entity.UserId) && _userRepository.Delete(x => x.Id == entity.UserId))
                        {
                            Console.Clear();
                            Console.WriteLine("User deleted successfully!");
                            Console.WriteLine("Returning to user menu");
                            Console.ReadKey();
                            UserMainMenu(mainMenuService);
                        }
                    }
                    break;
                case 0:
                    UserMainMenu(mainMenuService);
                    break;
                default:
                    Console.WriteLine("Invalid input detected. Try again");
                    break;
            }
        }
    }
    private void UpdateAddress(ProfileEntity entity, string propertyNameX, string propertyNameY, string propertyNameZ)
    {
        AddressEntity addressEntity = _addressRepository.GetOne(x => x.Id == entity.AddressId);
        SetValidInput(() => Console.ReadLine()?.Trim()!, val => addressEntity.City = val, propertyNameX);
        SetValidInput(() => Console.ReadLine()?.Trim()!, val => addressEntity.Street = val, propertyNameY);
        SetValidInput(() => Console.ReadLine()?.Trim()!, val => addressEntity.PostalCode = val, propertyNameZ);
        addressEntity = _userFactories.CreateOrGetAddressEntity(addressEntity.City, addressEntity.Street, addressEntity.PostalCode);
        entity.AddressId = addressEntity.Id;
        entity = _profileRepository.Update(x => x.UserId == entity.UserId, entity);
        if (entity.AddressId == addressEntity.Id)
        {
            Console.WriteLine("Updated successfully");
        }
    }
    private void SetValidInput(Func<string> getProperty, Action<string> setProperty, string property)
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
    private void SetValidEmail(Func<string> getProperty, Action<string> setProperty, string property)
    {
        while (true)
        {
            Console.Write($"{property}: ");
            var input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && !_verificationRepository.Exists(x => x.Email == input))
            {
                input = char.ToUpper(input[0]) + input.Substring(1);
                setProperty(input);
                break;
            }
            else if (_verificationRepository.Exists(x => x.Email == input))
            {
                Console.WriteLine($"{input} is already registered. {property} must be unique!");
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
