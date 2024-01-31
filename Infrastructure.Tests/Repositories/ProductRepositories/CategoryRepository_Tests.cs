using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class CategoryRepository_Tests
{
    private readonly DbFirstDataContext _context =
        new(new DbContextOptionsBuilder<DbFirstDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public void Exist_ShouldReturnTrue_IfEntityExist()
    {
        {
            // Arrange
            var categoryRepository = new CategoryRepository(_context);
            var categoryEntity = new Category { Name = "Test" };
            categoryEntity = categoryRepository.Create(categoryEntity);


            // Act
            var result = categoryRepository.Exists(x => x.Id == categoryEntity.Id);

            // Assert
            Assert.True(result);
        }
    }
    [Fact]
    public void Exist_ShouldReturnFalse_IfEntityDoesNotExist()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);


        // Act
        var result = categoryRepository.Exists(x => x.Id == 1);

        // Assert
        Assert.False(result);
    }
    [Fact]
    public void Delete_ShouldDeleteEntity_ThenReturnTrue()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryEntity = new Category { Name = "Test" };
        categoryEntity = categoryRepository.Create(categoryEntity);


        // Act
        var result = categoryRepository.Delete(x => x.Id == categoryEntity.Id);

        // Assert
        Assert.True(result);
    }
    [Fact]
    public void Delete_ShouldNotDeleteWhenEntityDoesNotExist_ThenReturnFalse()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_context);


        // Act
        var result = categoryRepository.Delete(x => x.Id == 1);

        // Assert
        Assert.False(result);
    }
}
