using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService(ProfileRepository profileRepository, UserFactories userFactories)
{
    private readonly ProfileRepository _profileRepository = profileRepository;

    private readonly UserFactories _userFactories = userFactories;
    
    public bool CreateUser(UserRegDto user)
    {
        try
        {
            UserEntity userEntity = _userFactories.CreateUserEntity(user.FirstName, user.LastName);
            VerificationEntity verificationEntity = _userFactories.CreateVerificationEntity(user.Password, user.Email, userEntity.Id);
            AddressEntity addressEntity = _userFactories.CreateOrGetAddressEntity(user.City, user.Street, user.PostalCode);
            RoleEntity roleEntity = _userFactories.GetOrCreateRole(user.FirstName);
            ProfileEntity profileEntity = _userFactories.CreateProfileEntity(userEntity.Id, addressEntity.Id, roleEntity.Id);
            if (profileEntity != null)
            {
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    // Skyddar lösenord genom att göra om det till en DTO som inte tar med password
    public IEnumerable<DisplayUserDto> GetAll()
    {
        var profileList = _profileRepository.GetAll();
        var userList = new List<DisplayUserDto>();

        foreach (var item in profileList)
        {
            var user = _userFactories.CompileUserDto(item);
            userList.Add(user);
        }

        return userList;
    }
}