using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using System.Linq.Expressions;

namespace Presentation.ConsoleApp;

public class MenuService(AddressRepository addressRepository, ProfileRepository profileRepository, RoleRepository roleRepository, UserRepository userRepository, VerificationRepository verificationRepository, UserFactories userFactories, UserService userService)
{
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;

    private readonly UserFactories _userFactories = userFactories;

    private readonly UserService _userService = userService;

    public void MainMenu()
    {
        string[] menuOptions = 
            {
                "Show all users.",
                "Add a user.",
                "Exit program."
            };
        while (true)
        {
            Console.Clear();
            MenuHeader("Main Menu");
            Console.WriteLine("Welcome, what would you like to do?");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine($"{i+1}: \t\t{menuOptions[i]}");
            }
            var answer = Console.ReadKey().Key;
            switch (answer)
            {
                case ConsoleKey.D1:
                    ShowAllMenu();
                    break;
                case ConsoleKey.D2:
                    ShowAddMenu();
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
    public void ShowAllMenu()
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
                Console.WriteLine($"Email:\t\t<{item.Email}>");
                Console.WriteLine($"City:\t\t{item.City}");
                Console.WriteLine($"Street:\t\t{item.Street}");
                Console.WriteLine($"Postalcode:\t{item.PostalCode}");
                Console.WriteLine("----------------------------");
                i++;
            }
            Console.Write("Enter the number of the user you would like to inspect, or 0 to return to main menu: ");
            string answer = Console.ReadLine()!;
            if (int.TryParse(answer, out result))
            {
                if (result == 0)
                {
                    Console.WriteLine("Returning to main menu");
                    Console.ReadKey();
                    MainMenu();
                }
                else
                {
                    
                    DisplayUserDto user = userList.ElementAt(result -1);
                    VerificationEntity e = _verificationRepository.GetOne(x => x.Email == user.Email);
                    ProfileEntity entity = _profileRepository.GetOne(x => x.UserId == e.UserId);
                    if (entity != null)
                    {
                        ShowEditAndDeleteMenu(entity);
                    }
                }
            }
            
        }
    }
    public void ShowAddMenu()
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
            for(int i = 0; i < menuStrings.Length; i++)
            {
                Console.Write($"{menuStrings[i]}: ");
                string answer = Console.ReadLine()!;
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
                        if(!_verificationRepository.Exists(x => x.Email == answer))
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
                                    ShowAddMenu();
                                    break;
                                default:
                                    Console.WriteLine("Returning to main menu");
                                    MainMenu();
                                    break;
                            }
                            break;
                        }
                    case "Password":
                        user.Password = answer;
                        break;
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
                        ShowAddMenu();
                        break;
                    default:
                        MainMenu();
                        break;
                }
            }
        }
    }
    public void ShowEditAndDeleteMenu(ProfileEntity entity) 
    {
        while (true)
        {
            //DisplayUserDto user = _userFactories.CompileUserDto(entity);
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
            Console.Write("Choose a value to update (9 for delete, and 0 to return to main menu)");
            string answer = Console.ReadLine();
            int.TryParse(answer, out int result);
            switch(result)
            {
                case 1:
                    Console.Write("Please enter a new first name: ");
                    answer = Console.ReadLine()!;
                    if (answer != string.Empty)
                    {
                        UserEntity userEntity = _userRepository.GetOne(x => x.Id == entity.UserId);
                        userEntity.FirstName = answer;
                        userEntity = _userRepository.Update((x => x.Id == userEntity.Id),userEntity);
                        if (userEntity != null)
                        {
                            Console.Clear();
                            Console.WriteLine("Update successfull!");
                            Console.ReadKey();
                        }
                    }
                    break;
                case 2:
                    Console.Write("Please enter a new last name: ");
                    answer = Console.ReadLine()!;
                    if (answer != string.Empty)
                    {
                        UserEntity userEntity = _userRepository.GetOne(x => x.Id == entity.UserId);
                        userEntity.LastName = answer;
                        userEntity = _userRepository.Update((x => x.Id == userEntity.Id), userEntity);
                        if (userEntity != null)
                        {
                            Console.Clear();
                            Console.WriteLine("Update successfull!");
                            Console.ReadKey();
                        }

                    }
                    break;
                case 3:
                    Console.Write("Please enter a new email: ");
                    answer = Console.ReadLine()!;
                    if (answer != string.Empty)
                    {
                        VerificationEntity verificationEntity = _verificationRepository.GetOne(x => x.UserId == entity.UserId);
                        verificationEntity.Email = answer;
                        verificationEntity = _verificationRepository.Update((x => x.UserId == verificationEntity.UserId), verificationEntity);
                        if (verificationEntity != null)
                        {
                            Console.Clear();
                            Console.WriteLine("Update successfull!");
                            Console.ReadKey();
                        }
                    }
                    break;
                case 4:
                    UpdateAddress(entity);
                    //Console.Write("Please enter a new City: ");
                    //answer = Console.ReadLine()!;
                    //if (answer != string.Empty)
                    //{
                    //    AddressEntity addressEntity = _addressRepository.GetOne(x => x.Id == entity.Address.Id);
                    //    addressEntity.City = answer;
                    //    addressEntity = _addressRepository.Update((x => x.Id == addressEntity.Id), addressEntity);
                    //    if (addressEntity != null)
                    //    {
                    //        Console.Clear();
                    //        Console.WriteLine("Update successfull!");
                    //        Console.ReadKey();
                    //    }
                    //}
                    break;
                //case 5:
                //    Console.Write("Please enter a new Street: ");
                //    answer = Console.ReadLine()!;
                //    if (answer != string.Empty)
                //    {
                //        AddressEntity addressEntity = _addressRepository.GetOne(x => x.Id == entity.Address.Id);
                //        addressEntity.Street = answer;
                //        addressEntity = _addressRepository.Update((x => x.Id == addressEntity.Id), addressEntity);
                //        if (addressEntity != null)
                //        {
                //            Console.Clear();
                //            Console.WriteLine("Update successfull!");
                //            Console.ReadKey();
                //        }
                //    }
                //    break;
                //case 6:
                //    Console.Write("Please enter a new postalcode: ");
                //    answer = Console.ReadLine()!;
                //    if (answer != string.Empty)
                //    {
                //        AddressEntity addressEntity = _addressRepository.GetOne(x => x.Id == entity.Address.Id);
                //        addressEntity.PostalCode = answer;
                //        addressEntity = _addressRepository.Update((x => x.Id == addressEntity.Id), addressEntity);
                //        if (addressEntity != null)
                //        {
                //            Console.Clear();
                //            Console.WriteLine("Update successfull!");
                //            Console.ReadKey();
                //        }
                //    }
                //    break;

                case 9:
                    Console.Write("Are you sure you want to delete? (y/n)");
                    var answerKey = Console.ReadKey().Key;
                    if (answerKey == ConsoleKey.Y)
                    {
                        if (_verificationRepository.Delete(x => x.UserId == entity.UserId))
                        {
                            if (_profileRepository.Delete(x => x.UserId == entity.UserId))
                            {
                                if (_userRepository.Delete(x => x.Id == entity.UserId))
                                {
                                    Console.Clear();
                                    Console.WriteLine("User deleted successfully!");
                                    Console.WriteLine("Returning to main menu");
                                    Console.ReadKey();
                                    MainMenu();
                                }
                            }
                        }
                    }
                    break;
                case 0:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid input detected. Try again");
                    break;
            }
        }
    }
    private void UpdateAddress(ProfileEntity entity)
    {
        Console.Clear();
        Console.Write("Please enter a new city: ");
        string city = Console.ReadLine()!;
        if(city != null)
        {
            Console.Clear();
            Console.Write("Please enter a new street: ");
            string street = Console.ReadLine()!;
            if(street != null)
            {
                Console.Clear();
                Console.Write("Please enter a new postalcode: ");
                string postalCode = Console.ReadLine()!;
                if(postalCode != null)
                {
                    AddressEntity addressEntity = _userFactories.CreateOrGetAddressEntity(city, street, postalCode);
                    entity.AddressId = addressEntity.Id;
                    entity = _profileRepository.Update(x => x.UserId == entity.UserId, entity);
                    if(entity.AddressId == addressEntity.Id)
                    {
                        Console.WriteLine("Updated successfully");
                    }

                }
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
