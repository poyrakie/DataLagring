using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class AddressEntity
{
    [Key] 
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Street { get; set; } = null!;
    [Required]
    [StringLength(50)]
    public string City { get; set; } = null!;
    [Required]
    [StringLength(10)]
    public string PostalCode { get; set; } = null!;

    public virtual ICollection<ProfileEntity> Profiles { get; set; } = new List<ProfileEntity>();
}
