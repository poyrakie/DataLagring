using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class ProfileRepository_Tests
{
    private readonly DataContext _context =
        new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void Exists_ShouldScanDatabaseForEntity_ThenReturnTrueWhenFound()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);

        var addressEntity = new AddressEntity { Street = "TestStreet", City = "TestCity", PostalCode = "TestPC" };
        var roleEntity = new RoleEntity { RoleName = "Test" };
        var userEntity = new UserEntity { FirstName = "Test", LastName = "Testsson" };

        addressEntity = addressRepository.Create(addressEntity);
        roleEntity = roleRepository.Create(roleEntity);
        userEntity = userRepository.Create(userEntity);

        var profileEntity = new ProfileEntity { UserId = userEntity.Id, RoleId = roleEntity.Id, AddressId = addressEntity.Id };
        profileEntity = profileRepository.Create(profileEntity);


        // Act
        var result = profileRepository.Exists(x => x.UserId == profileEntity.UserId);


        // Assert
        Assert.True(result );

    }
    [Fact]
    public void Exists_ShouldScanDatabaseForEntity_ThenReturnFalseWhenNotFound()
    {
        // Arrange
        var profileRepository = new ProfileRepository(_context);

        var profileEntity = new ProfileEntity { UserId = 1 };


        // Act
        var result = profileRepository.Exists(x => x.UserId == profileEntity.UserId);


        // Assert
        Assert.False(result);

    }
}
