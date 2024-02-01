using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Factories;

public class UserFactories(AddressRepository addressRepository, RoleRepository roleRepository, UserRepository userRepository, VerificationRepository verificationRepository, ProfileRepository profileRepository)
{
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;

    /// <summary>
    /// Takes two strings to save a userEntity to the database, then returns the entity if created or null if unsuccessfull
    /// </summary>
    /// <param name="firstName">firstname of the userentity</param>
    /// <param name="lastName">lastname of the userentity</param>
    /// <returns>userentity if successfull, null if unsuccessfull</returns>
    public UserEntity CreateUserEntity(string firstName,  string lastName)
    {
        try
        {
            UserEntity userEntity = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName
            };
            userEntity = _userRepository.Create(userEntity);
            return userEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Takes two strings and one int to save a verificationentity to the database,
    /// checks if email is already registered before saving. Returns null if already exists
    /// </summary>
    /// <param name="password">password string for verificationentity</param>
    /// <param name="email">email string for verificationentity</param>
    /// <param name="userId">userid int for verificationentity</param>
    /// <returns>verificationentity if saved, null if unsuccessfull or email already exists</returns>
    public VerificationEntity CreateVerificationEntity(string password, string email, int userId)
    {
        try
        {
            if(!_verificationRepository.Exists(x => x.Email == email))
            {
                VerificationEntity verificationEntity = new VerificationEntity
                {
                    Password = password,
                    Email = email,
                    UserId = userId
                };
                verificationEntity = _verificationRepository.Create(verificationEntity);
                return verificationEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Takes three strings for an addressentity,
    /// checks if the address already exists in the database.
    /// If it exists, returns existing values from database, else saves and return new entry.
    /// </summary>
    /// <param name="city">string city for addressentity</param>
    /// <param name="street">string street for addressentity</param>
    /// <param name="postalCode">string postalcode for addressentity</param>
    /// <returns>Existing address if it is previously registered, else saves and returns new entity</returns>
    public AddressEntity CreateOrGetAddressEntity(string city, string street, string postalCode)
    {
        try
        {
            AddressEntity addressEntity = new AddressEntity
            {
                City = city,
                Street = street,
                PostalCode = postalCode
            };
            if(_addressRepository.Exists(x => x.City == city && x.Street == street && x.PostalCode == postalCode))
            {
                addressEntity = _addressRepository.GetOne(x => x.City == city && x.Street == street && x.PostalCode == postalCode);
            }
            else
            {
                addressEntity = _addressRepository.Create(addressEntity);
            }
            return addressEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Takes one string of firstname, create or get a rolentity for the user depending on the name of the user
    /// </summary>
    /// <param name="firstName">string firstname of the user of wich to apply the roleentity</param>
    /// <returns>returns created or fetched roleentity if successfull, else null</returns>
    public RoleEntity GetOrCreateRole(string firstName)
    {
        try
        {
            if(firstName == "Hans" || firstName == "Johan" || firstName == "Joakim" || firstName == "Tommy")
            {
                RoleEntity roleEntity = CreateRoleEntity("Admin");
                return roleEntity;
            }
            else
            {
                RoleEntity roleEntity = CreateRoleEntity("User");
                return roleEntity;
            }
            
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }


    /// <summary>
    /// Takes profileentity with all relevant id's, returns full userDto for display purposes
    /// </summary>
    /// <param name="profil">profileentity of user to display</param>
    /// <returns>userDto from the profile input if successfull, else null</returns>
    public DisplayUserDto CompileUserDto(ProfileEntity profil)
    {
        try
        {
            var userEntity = _userRepository.GetOne(x => x.Id == profil.UserId);
            var addressEntity = _addressRepository.GetOne(x => x.Id == profil.AddressId);
            var verificationEntity = _verificationRepository.GetOne(x => x.UserId == profil.UserId);
            var roleEntity = _roleRepository.GetOne(x => x.Id == profil.RoleId);
            var user = new DisplayUserDto
            {
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Street = addressEntity.Street,
                City = addressEntity.City,
                PostalCode = addressEntity.PostalCode,
                Email = verificationEntity.Email,
                RoleName = roleEntity.RoleName
            };
            return user;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Takes string of rolename of roleentity that wishes to be created. Checks if entity already exists, else creates it.
    /// </summary>
    /// <param name="roleName">string rolename of roleentity that should be checked/created</param>
    /// <returns>returns fetched/created roleentity if successfull, else null</returns>
    public RoleEntity CreateRoleEntity(string roleName)
    {
        try
        {
            if(_roleRepository.Exists(x => x.RoleName == roleName))
            {
                RoleEntity roleEntity = _roleRepository.GetOne(x => x.RoleName == roleName);
                return roleEntity;
            }
            else
            {
                RoleEntity roleEntity = new RoleEntity
                {
                    RoleName = roleName
                };
                roleEntity = _roleRepository.Create(roleEntity);
                return roleEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public ProfileEntity CreateProfileEntity(int userId, int roleId, int addressId)
    {
        try
        {
            if(_userRepository.Exists(x => x.Id == userId) && _roleRepository.Exists(x => x.Id ==  roleId) && _addressRepository.Exists(x => x.Id == addressId))
            {
                ProfileEntity profileEntity = new ProfileEntity
                {
                    UserId = userId,
                    RoleId = roleId,
                    AddressId = addressId
                };
                profileEntity = _profileRepository.Create(profileEntity);
                return profileEntity;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
