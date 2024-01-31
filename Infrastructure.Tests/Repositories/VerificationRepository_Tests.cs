using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class VerificationRepository_Tests
{
    private readonly DataContext _context =
        new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void Update_ShouldUpdateVerificationEntityInDatabase_ThenReturnUpdatedEntity()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var userEntity = new UserEntity { FirstName = "Test", LastName = "Testsson" };
        userEntity = userRepository.Create(userEntity);
        var verificationEntity = new VerificationEntity { UserId = userEntity.Id, Email = "Test@domain.com", Password = "Test" };
        verificationEntity = verificationRepository.Create(verificationEntity);


        // Act
        verificationEntity.Email = "Updatedtest@domain.com";
        verificationEntity = verificationRepository.Update(x => x.UserId == verificationEntity.UserId, verificationEntity);


        // Assert
        Assert.NotEqual("Test@domain.com", verificationEntity.Email);
        Assert.IsType<VerificationEntity>(verificationEntity);

    }
    [Fact]
    public void Update_ShouldReturnNull_WhenNoVerificationEntityExists()
    {
        // Arrange
        var verificationRepository = new VerificationRepository(_context);
        var verificationEntity = new VerificationEntity { UserId = 1, Email = "Test@domain.com", Password = "Test" };


        // Act
        var result = verificationRepository.Update(x => x.UserId == 1, verificationEntity);


        // Assert
        Assert.Null(result);

    }
}
