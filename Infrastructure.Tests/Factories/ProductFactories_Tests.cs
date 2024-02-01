using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Factories;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Factories;

public class ProductFactories_Tests
{
    private readonly DbFirstDataContext _context =
        new(new DbContextOptionsBuilder<DbFirstDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void GetOrCreateCategoryEntity_ShouldTakeOneStringAndEitherGetOrCreateCategoryEntity_ThenReturnUpdatedEntityWithId()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);


        // Act
        var result = productFactories.GetOrCreateCategoryEntity("test");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Category>(result);
        Assert.Equal(1, result.Id);
    }
    [Fact]
    public void GetOrCreateManufacturerEntity_ShouldTakeOneStringAndEitherGetOrCreateManufacturerEntity_ThenReturnUpdatedEntityWithId()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);


        // Act
        var result = productFactories.GetOrCreateManufacturerEntity("test");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Manufacturer>(result);
        Assert.Equal(1, result.Id);
    }
    [Fact]
    public void CreateProductEntity_ShouldTakeTwoStringsOneDecimalTwoIntThenCreateProductEntity_ThenReturnEntity()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);

        var categoryEntity = productFactories.GetOrCreateCategoryEntity("test");
        var manufacturerEntity = productFactories.GetOrCreateManufacturerEntity("test");

        // Act
        var result = productFactories.CreateProductEntity("test", "test", 1, categoryEntity.Id, manufacturerEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Product>(result);
        Assert.Equal(1, result.Id);
    }
    [Fact]
    public void CreateProductEntity_ShouldReturnNull_IfGivenIntDoesNotExistInDatabase()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);


        // Act
        var result = productFactories.CreateProductEntity("test", "test", 1, 1, 1);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public void CreateOrderEntity_ShouldTakeOneIntAndSaveOrderEntity_ThenReturnUpdatedEntityWithId()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);


        // Act
        var result = productFactories.CreateOrderEntity(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Order>(result);
        Assert.Equal(1, result.Id);
    }
    [Fact]
    public void CreateOrderRowEntity_ShouldTakeTwoIntSaveOrderRowEntityToDatabase_ThenReturnEntity()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);

        var manufacturerEntity = productFactories.GetOrCreateManufacturerEntity("test");
        var categoryEntity = productFactories.GetOrCreateCategoryEntity("test");
        var productEntity = productFactories.CreateProductEntity("test", "test", 1, categoryEntity.Id, manufacturerEntity.Id);
        var orderEntity = productFactories.CreateOrderEntity(1);

        // Act
        var result = productFactories.CreateOrderRowEntity(orderEntity.Id, productEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OrderRow>(result);
    }
    [Fact]
    public void CreateOrderRowEntity_ShouldReturnFalse_IfGivenIdsDoesNotMatchDatabase()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);


        // Act
        var result = productFactories.CreateOrderRowEntity(1, 1);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public void CompileFullProduct_ShouldTakeProductEntity_ThenReturnProductDto()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);

        var categoryEntity = productFactories.GetOrCreateCategoryEntity("test");
        var manufacturerEntity = productFactories.GetOrCreateManufacturerEntity("test");
        var productEntity = productFactories.CreateProductEntity("test", "test", 1, categoryEntity.Id, manufacturerEntity.Id);

        // Act
        var result = productFactories.CompileFullProduct(productEntity);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProductDto>(result);
        Assert.Equal(categoryEntity.Name, result.CategoryName);
        Assert.Equal(manufacturerEntity.Name, result.ManufacturerName);
    }
    [Fact]
    public void GetAllOrders_ShouldTakeOneIntOfUserThenGetAllOrdersMatchingInDatabase_ThenReturnIEnumerableListOfOrders()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);

        var orderEntity = productFactories.CreateOrderEntity(1);


        // Act
        var result = productFactories.GetAllOrders(orderEntity.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Order>>(result);
    }
    [Fact]
    public void GetAllOrders_ShouldReturnNull_IfUserIdDoesNotMatchDatabase()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);


        // Act
        var result = productFactories.GetAllOrders(1);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public void GetAllOrderRows_ShouldTakeOneIntOfOrderThenGetAllOrderRowsMatchingInDatabase_ThenReturnIEnumerableListOfOrderRows()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);

        var orderEntity = productFactories.CreateOrderEntity(1);


        // Act
        var result = productFactories.GetAllOrderRows(orderEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<OrderRow>>(result);
    }
    [Fact]
    public void GetAllOrderRows_ShouldReturnNull_IfOrderIdDoesNotMatchDatabase()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var productRepository = new ProductRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);
        var productFactories = new ProductFactories(categoryRepository, productRepository, manufacturerRepository, orderRepository, orderRowRepository);


        // Act
        var result = productFactories.GetAllOrderRows(1);

        // Assert
        Assert.Null(result);
    }

}
