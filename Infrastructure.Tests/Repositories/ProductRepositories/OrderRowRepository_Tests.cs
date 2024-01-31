using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class OrderRowRepository_Tests
{
    private readonly DbFirstDataContext _context =
        new(new DbContextOptionsBuilder<DbFirstDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    private void GetAll_ShouldGetAllOrderRowEntities_ThenReturnList()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var orderRowRepository = new OrderRowRepository(_context);

        var manufacturerEntity = new Manufacturer { Name = "Test" };
        var categoryEntity = new Category { Name = "Test" };
        var orderEntity = new Order { UserId = 1 };

        orderEntity = orderRepository.Create(orderEntity);
        manufacturerEntity = manufacturerRepository.Create(manufacturerEntity);
        categoryEntity = categoryRepository.Create(categoryEntity);

        var productEntity = new Product { Title = "Test", Description = "Test", Price = 1, CategoryId = categoryEntity.Id, ManufacturerId = manufacturerEntity.Id };
        productEntity = productRepository.Create(productEntity);

        var orderRowEntity = new OrderRow { ProductId = productEntity.Id, OrderId = orderEntity.Id };
        orderRowRepository.Create(orderRowEntity);


        // Act
        var result = orderRowRepository.GetAll();


        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<OrderRow>>(result);
    }
    [Fact]
    public void GetAll_ShouldReturnEmptyList_WhenNoOrderRowsInDatabase()
    {
        // Arrange
        var orderRowRepository = new OrderRowRepository(_context);


        // Act
        var result = orderRowRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
