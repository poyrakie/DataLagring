using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.ProductEntities;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public int ManufacturerId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("ManufacturerId")]
    [InverseProperty("Products")]
    public virtual Manufacturer Manufacturer { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderRow> OrderRows { get; set; } = new List<OrderRow>();
}
