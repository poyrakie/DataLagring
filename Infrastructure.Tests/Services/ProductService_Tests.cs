using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Factories;
using Infrastructure.Repositories.ProductRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Services;

public class ProductService_Tests
{
    private readonly DbFirstDataContext _context =
        new(new DbContextOptionsBuilder<DbFirstDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void CreateProduct_ShouldTakeProductDtoAndSave_ThenReturnTrue()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);

        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);
        var productService = new ProductService(productFactories, productRepository);

        var product = new ProductDto { Title = "test", Description = "test", Price = 1, CategoryName = "Test", ManufacturerName = "test" };

        // Act
        var result = productService.CreateProduct(product);

        // Assert
        Assert.True(result);
    }
    [Fact]
    public void CreateProduct_ShouldReturnFalse_WhenProductDtoIsIncomplete()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);

        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);
        var productService = new ProductService(productFactories, productRepository);

        var product = new ProductDto();

        // Act
        var result = productService.CreateProduct(product);

        // Assert
        Assert.False(result);
    }
    [Fact]
    public void GetAllProducts_ShouldGetAllProductsAndMakeIntoList_ThenReturnIEnumerableOfTypeProductDto()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);

        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);
        var productService = new ProductService(productFactories, productRepository);

        var product = new ProductDto { Title = "test", Description = "test", Price = 1, CategoryName = "Test", ManufacturerName = "test" };

        productService.CreateProduct(product);

        // Act
        var result = productService.GetAllProducts();

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result);
    }
}
