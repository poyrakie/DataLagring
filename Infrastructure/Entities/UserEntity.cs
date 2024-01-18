using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class UserEntity
{
    [Key]
    public int Id { get; set; }
    [StringLength(50)]
    public string? FirstName { get; set; }
    [StringLength(50)]
    public string? LastName { get; set;}
    public VerificationEntity Verification { get; set; } = null!;
    public ProfileEntity Profile { get; set; } = null!;
}
