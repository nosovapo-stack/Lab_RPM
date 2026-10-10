using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models;

[Table("PickupPoint")]
public partial class PickupPoint
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? Adress { get; set; }

    [InverseProperty("PickupPoint")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
