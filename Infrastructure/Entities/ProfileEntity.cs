using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class ProfileEntity
{
    [Key, ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [ForeignKey(nameof(Role))]
    public int RoleId { get; set; }
    [ForeignKey(nameof(Address))]
    public int AddressId { get; set; }
    public UserEntity User { get; set; } = null!;
    public RoleEntity Role { get; set; } = null!;
    public AddressEntity Address { get; set; } = null!;
}
