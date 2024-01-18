using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[Index(nameof(RoleName), IsUnique = true)]
public class RoleEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string RoleName { get; set; } = null!;
    public virtual ICollection<ProfileEntity> Profiles { get; set; } = new List<ProfileEntity>();
}
