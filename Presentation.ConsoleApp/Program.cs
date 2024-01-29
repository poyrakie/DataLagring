using Infrastructure.Contexts;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.ConsoleApp.Services;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Programmering\EC\Databas\SubmissionTask\Infrastructure\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30"));
    services.AddDbContext<DbFirstDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Programmering\EC\Databas\SubmissionTask\Infrastructure\Data\dbfirst_local_database.mdf;Integrated Security=True;Connect Timeout=30"));

    services.AddScoped<AddressRepository>();
    services.AddScoped<UserRepository>();
    services.AddScoped<RoleRepository>();
    services.AddScoped<ProfileRepository>();
    services.AddScoped<VerificationRepository>();

    services.AddScoped<CategoryRepository>();
    services.AddScoped<ManufacturerRepository>();
    services.AddScoped<OrderRepository>();
    services.AddScoped<OrderRowRepository>();
    services.AddScoped<ProductRepository>();

    services.AddScoped<ProductFactories>();
    services.AddScoped<UserFactories>();

    services.AddScoped<UserService>();
    services.AddScoped<ProductService>();

    services.AddScoped<MainMenuService>();
    services.AddScoped<UserMenuService>();
    services.AddScoped<ProductMenuService>();


}).Build();

builder.Start();

var menuService = builder.Services.GetRequiredService<MainMenuService>();

menuService.MainMenu();