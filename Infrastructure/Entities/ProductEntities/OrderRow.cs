using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.ProductEntities;

[PrimaryKey("OrderId", "ProductId")]
public partial class OrderRow
{
    [Key]
    public int OrderId { get; set; }

    [Key]
    public int ProductId { get; set; }

    public int Amount { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderRows")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderRows")]
    public virtual Product Product { get; set; } = null!;
}
