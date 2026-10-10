using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models;

public partial class ProductName
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [InverseProperty("Name")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
