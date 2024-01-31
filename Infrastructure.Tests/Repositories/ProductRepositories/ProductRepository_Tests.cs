using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class ProductRepository_Tests
{
    private readonly DbFirstDataContext _context =
        new(new DbContextOptionsBuilder<DbFirstDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void Create_ShouldSaveProductEntity_ThenReturnUpdatedEntityWithId()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufacturerRepository = new ManufacturerRepository(_context);

        var manufacturerEntity = new Manufacturer { Name = "Test" };
        var categoryEntity = new Category { Name = "Test" };

        manufacturerEntity = manufacturerRepository.Create(manufacturerEntity);
        categoryEntity = categoryRepository.Create(categoryEntity);

        var productEntity = new Product { Title = "Test", Description = "Test", Price = 1, CategoryId = categoryEntity.Id, ManufacturerId = manufacturerEntity.Id };


        // Act
        var result = productRepository.Create(productEntity);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Product>(result);
        Assert.Equal(1, result.Id);
    }
    [Fact]
    public void Create_ShouldNotCreateProductEntity_ValuesAreEmpty_ThenReturnNull()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var productEntity = new Product();

        // Act
        var result = productRepository.Create(productEntity);

        // Assert
        Assert.Null(result);
    }
}
