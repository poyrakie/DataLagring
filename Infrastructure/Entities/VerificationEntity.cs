using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[Index(nameof(Email), IsUnique = true)]
public class VerificationEntity
{
    [Key, ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
}
