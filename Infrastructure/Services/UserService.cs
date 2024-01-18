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
            UserEntity userEntity = new UserEntity
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            userEntity = _userRepository.Create(userEntity);

            AddressEntity addressEntity = new AddressEntity
            {
                City = user.City,
                PostalCode = user.PostalCode,
                Street = user.Street,
            };
            addressEntity = _addressRepository.Create(addressEntity);

            RoleEntity roleEntity = new RoleEntity
            {
                RoleName = "Admin"
            };
            roleEntity = _roleRepository.Create(roleEntity);

            VerificationEntity verificationEntity = new VerificationEntity
            {
                Email = user.Email,
                Password = user.Password,
                UserId = userEntity.Id
            };
            verificationEntity = _verificationRepository.Create(verificationEntity);

            ProfileEntity profileEntity = new ProfileEntity
            {
                UserId = userEntity.Id,
                RoleId = roleEntity.Id,
                AddressId = addressEntity.Id,
            };
            profileEntity = _profileRepository.Create(profileEntity);
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
            var user = _userFactories.CreateFullUser(item);
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
