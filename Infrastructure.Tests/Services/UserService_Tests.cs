using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Services;

public class UserService_Tests
{
    private readonly DataContext _context =
        new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void CreateUser_ShouldTakeUserRegDtoAndSaveAllRelevantInformationInRightTables_ThenReturnTrue()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);

        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);
        var userService = new UserService(profileRepository, userFactories);

        var user = new UserRegDto { FirstName = "test", LastName = "testsson", Street = "test", City = "test", PostalCode = "test", Email = "test", Password = "test" };

        // Act
        var result = userService.CreateUser(user);

        // Assert
        Assert.True(result);
    }
    [Fact]
    public void CreateUser_ShouldReturnFalse_IfDtoIsIncomplete()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);

        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);
        var userService = new UserService(profileRepository, userFactories);

        var user = new UserRegDto();

        // Act
        var result = userService.CreateUser(user);

        // Assert
        Assert.False(result);
    }
    [Fact]
    public void GetAll_ShouldGetAllProfilesFromDatabaseConvertToDisplayUserDto_ThenReturnIEnumerableOfDisplayUserDto()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);

        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);
        var userService = new UserService(profileRepository, userFactories);

        var user = new UserRegDto { FirstName = "test", LastName = "testsson", Street = "test", City = "test", PostalCode = "test", Email = "test", Password = "test" };

        // Act
        var result = userService.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<DisplayUserDto>>(result);
    }
}
