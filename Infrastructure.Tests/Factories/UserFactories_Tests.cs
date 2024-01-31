using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Factories;

public class UserFactories_Tests
{
    private readonly DataContext _context =
        new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    [Fact]
    public void CreateUserEntity_ShouldTakeTwoStringsThenSaveToDatabase_ThenReturnEntityWithId()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);


        // Act
        var result = userFactories.CreateUserEntity("test", "testsson");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserEntity>(result);
    }
    [Fact]
    public void CreateVerificationEntity_ShouldTakeTwoStringsAndOneIntThenSaveToDatabase_ThenReturnEntityWithId()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);


        // Act
        var result = userFactories.CreateVerificationEntity("test123", "test@domain.com", 1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<VerificationEntity>(result);
    }
    [Fact]
    public void CreateOrGetAddressEntity_ShouldTakeThreeStringsThenSaveOrGetFromDatabase_ThenReturnEntityWithId()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);


        // Act
        var result = userFactories.CreateOrGetAddressEntity("test", "test", "test");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AddressEntity>(result);
    }
    [Fact]
    public void GetOrCreateRole_ShouldTakeOneStringOfUserFirstNameThenCreateOrGetRoleEntity_ThenReturnRoleEntity()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);


        // Act
        var result = userFactories.GetOrCreateRole("Hans");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RoleEntity>(result);
        Assert.Equal("Admin", result.RoleName);
    }
    [Fact]
    public void CompileUserDto_ShouldTakeProfileEntity_ThenReturnUserDto()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        
        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);
        
        var userEntity = new UserEntity { FirstName = "Test", LastName = "Testsson" };
        var addressEntity = new AddressEntity { Street = "Test", City = "Test", PostalCode = "Test" };
        var roleEntity = new RoleEntity { RoleName = "Test" };

        addressEntity = addressRepository.Create(addressEntity);
        roleEntity = roleRepository.Create(roleEntity);
        userEntity = userRepository.Create(userEntity);
        var verificationEntity = new VerificationEntity { UserId = userEntity.Id, Email = "Test", Password = "Test" };
        verificationRepository.Create(verificationEntity);

        var profileEntity = new ProfileEntity { UserId = userEntity.Id, RoleId = roleEntity.Id, AddressId = addressEntity.Id };



        // Act
        var result = userFactories.CompileUserDto(profileEntity);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DisplayUserDto>(result);
    }
    [Fact]
    public void CompileUserDto_ShouldTakeProfileEntity_ThenReturnUserDtoWithValueNull()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);
        var profileEntity = new ProfileEntity();


        // Act
        var result = userFactories.CompileUserDto(profileEntity);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public void CreateRoleEntity_ShouldTakeOneStringThenCheckIfThatNameExistsIfNot_SaveRoleEntityAndReturnUpdatedEntityWithId()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);



        // Act
        var result = userFactories.CreateRoleEntity("Test");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RoleEntity>(result);
        Assert.Equal("Test", result.RoleName);
    }
    [Fact]
    public void CreateProfileEntity_ShouldTakeThreeIntThenCreateAndSaveProfileEntity_ThenReturnEntity()
    {
        // Arrange
        var addressRepository = new AddressRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var userRepository = new UserRepository(_context);
        var verificationRepository = new VerificationRepository(_context);
        var profileRepository = new ProfileRepository(_context);

        var userFactories = new UserFactories(addressRepository, roleRepository, userRepository, verificationRepository, profileRepository);

        var userEntity = new UserEntity { FirstName = "Test", LastName = "Testsson" };
        var addressEntity = new AddressEntity { Street = "Test", City = "Test", PostalCode = "Test" };
        var roleEntity = new RoleEntity { RoleName = "Test" };

        addressEntity = addressRepository.Create(addressEntity);
        roleEntity = roleRepository.Create(roleEntity);
        userEntity = userRepository.Create(userEntity);


        // Act
        var result = userFactories.CreateProfileEntity(userEntity.Id, roleEntity.Id, addressEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProfileEntity>(result);
    }
}
