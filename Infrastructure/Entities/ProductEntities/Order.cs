using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.ProductEntities;

public partial class Order
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderRow> OrderRows { get; set; } = new List<OrderRow>();
}
