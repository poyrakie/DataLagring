using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.ProductEntities;

[Index("Name", Name = "UQ__Manufact__737584F67B2FD870", IsUnique = true)]
public partial class Manufacturer
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("Manufacturer")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
