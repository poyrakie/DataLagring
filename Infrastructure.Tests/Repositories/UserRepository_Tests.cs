using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class UserRepository_Tests
{
    private readonly DataContext _context =
        new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void GetOne_ShouldGetOneUserEntityFromDatabase_ThenReturnEntity()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var userEntity = new UserEntity { FirstName = "Test", LastName = "Testsson" };
        userEntity = userRepository.Create(userEntity);


        // Act
        var result = userRepository.GetOne(x => x.Id == userEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userEntity.FirstName, result.FirstName);
        Assert.Equal(userEntity.LastName, result.LastName);
        Assert.Equal(userEntity.Id, result.Id);
        Assert.IsType<UserEntity>(result);
    }
    [Fact]
    public void GetOne_ShouldNotGetUserEntity_IfUserEntityDoesNotExist()
    {
        // Arrange
        var userRepository = new UserRepository(_context);


        // Act
        var result = userRepository.GetOne(x => x.Id == 1);

        // Assert
        Assert.Null(result);
    }
}
