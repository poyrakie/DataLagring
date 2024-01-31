using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class OrderRepository_Tests
{
    private readonly DbFirstDataContext _context =
        new(new DbContextOptionsBuilder<DbFirstDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void GetOne_ShouldGetOneOrderEntityFromDatabase_ThenReturnEntity()
    {
        // Arrange
        var orderRepository = new OrderRepository(_context);
        var orderEntity = new Order { UserId = 1 };
        orderEntity = orderRepository.Create(orderEntity);


        // Act
        var result = orderRepository.GetOne(x => x.Id == orderEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Order>(result);
        Assert.Equal(orderEntity.Id, result.Id);
        Assert.Equal(orderEntity.UserId, result.UserId);
    }
    [Fact]
    public void GetOne_ShouldNotGetOrderEntity_IfOrderEntityDoesNotExist()
    {
        // Arrange
        var orderRepository = new OrderRepository(_context);


        // Act
        var result = orderRepository.GetOne(x => x.Id == 1);

        // Assert
        Assert.Null(result);
    }
}
