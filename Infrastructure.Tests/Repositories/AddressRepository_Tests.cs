using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class AddressRepository_Tests
{
    private readonly DataContext _context =
        new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public void GetAll_ShouldGetAllAddressEntitiesFromDatabase_ThenReturnListWithAllEntities()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { Street = "TestStreet", City = "TestCity", PostalCode = "TestPC" };
        addressRepository.Create(addressEntity);


        // Act
        var result = addressRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
    }
    [Fact]
    public void GetAll_ShouldReturnEmptyList_WhenNoAddressesInDatabase()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);


        // Act
        var result = addressRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public void Delete_ShouldDeleteEntity_ThenReturnTrue()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { Street = "TestStreet", City = "TestCity", PostalCode = "TestPC" };
        addressEntity = addressRepository.Create(addressEntity);


        // Act
        var result = addressRepository.Delete(x => x.Id == addressEntity.Id);

        // Assert
        Assert.True(result);
    }
    [Fact]
    public void Delete_ShouldNotDeleteWhenEntityDoesNotExist_ThenReturnFalse()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);


        // Act
        var result = addressRepository.Delete(x => x.Id == 1);

        // Assert
        Assert.False(result);
    }
}
