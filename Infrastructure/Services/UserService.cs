using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService(AddressRepository addressRepository, ProfileRepository profileRepository, RoleRepository roleRepository, UserRepository userRepository, VerificationRepository verificationRepository, UserFactories userFactories)
{
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;

    private readonly UserFactories _userFactories = userFactories;
    
    public bool CreateUser(UserRegDto user)
    {
        try
        {
            UserEntity userEntity = _userFactories.CreateUserEntity(user.FirstName, user.LastName);
            VerificationEntity verificationEntity = _userFactories.CreateVerificationEntity(user.Password, user.Email, userEntity.Id);
            AddressEntity addressEntity = _userFactories.CreateOrGetAddressEntity(user.City, user.Street, user.PostalCode);
            RoleEntity roleEntity = _userFactories.GetOrCreateRole(user.FirstName);
            ProfileEntity profileEntity = new ProfileEntity
            {
                UserId = userEntity.Id,
                RoleId = roleEntity.Id,
                AddressId = addressEntity.Id
            };
            _profileRepository.Create(profileEntity);
            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
    public IEnumerable<DisplayUserDto> GetAll()
    {
        var profileList = _profileRepository.GetAll();
        var userList = new List<DisplayUserDto>();

        foreach (var item in profileList)
        {
            var user = _userFactories.CompileUserDto(item);
            //var user = new DisplayUserDto();
            //user.FirstName = item.FirstName;
            //user.LastName = item.LastName;
            ////user.Street = item.Profile.Address.Street;
            ////user.City = item.Profile.Address.City;
            ////user.PostalCode = item.Profile.Address.PostalCode;
            ////user.RoleName = item.Profile.Role.RoleName;
            ////user.Email = item.Verification.Email;

            userList.Add(user);
        }

        return userList;
    }
}
