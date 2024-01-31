using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class ManufacturerRepository_Tests
{
    private readonly DbFirstDataContext _context =
        new(new DbContextOptionsBuilder<DbFirstDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void Update_ShouldUpdateManufacturerEntityInDatabase_ThenReturnUpdatedEntity()
    {
        // Arrange
        var manufacturerRepository = new ManufacturerRepository(_context);
        var manufacturerEntity = new Manufacturer { Name = "Test" };
        manufacturerEntity = manufacturerRepository.Create(manufacturerEntity);


        // Act
        manufacturerEntity.Name = "Updatedtest";
        manufacturerEntity = manufacturerRepository.Update(x => x.Id == manufacturerEntity.Id, manufacturerEntity);


        // Assert
        Assert.NotEqual("Test", manufacturerEntity.Name);

    }
    [Fact]
    public void Update_ShouldReturnNull_WhenNoManufacturerEntityExists()
    {
        // Arrange
        var manufacturerRepository = new ManufacturerRepository(_context);
        var manufacturerEntity = new Manufacturer { Name = "Test" };


        // Act
        var result = manufacturerRepository.Update(x => x.Id == manufacturerEntity.Id, manufacturerEntity);


        // Assert
        Assert.Null(result);

    }
}
