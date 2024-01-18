using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Factories;

public class UserFactories(AddressRepository addressRepository, ProfileRepository profileRepository, RoleRepository roleRepository, UserRepository userRepository, VerificationRepository verificationRepository)
{
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;

    public DisplayUserDto CreateFullUser(ProfileEntity profil)
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
}
