using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class RoleRepository_Tests
{
    private readonly DataContext _context = 
        new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public void Create_ShouldCreateRoleEntityThen_ReturnRoleEntityWithId()
    {
        // Arrange
        var roleRepository = new RoleRepository(_context);
        var roleEntity = new RoleEntity { RoleName = "Test" };

        // Act
        var result = roleRepository.Create(roleEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.IsType<RoleEntity>(result);
    }

    [Fact]
    public void Create_ShouldNotCreateRoleEntity_IfRoleNameAlreadyExists_ThenReturnNull()
    {
        // Arrange
        var roleRepository = new RoleRepository(_context);
        var roleEntity = new RoleEntity { RoleName = "Test" };

        // Act
        roleRepository.Create(roleEntity);
        var result = roleRepository.Create(roleEntity);

        // Assert
        Assert.Null(result);
    }
}
