using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? Article { get; set; }

    public int? NameId { get; set; }

    [StringLength(255)]
    public string? Unit { get; set; }

    public double? Price { get; set; }

    public int? SupplierId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? CategoryId { get; set; }

    public double? Discount { get; set; }

    public double? Count { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [StringLength(255)]
    public string? ImagePath { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }

    [ForeignKey("ManufacturerId")]
    [InverseProperty("Products")]
    public virtual Manufacturer? Manufacturer { get; set; }

    [ForeignKey("NameId")]
    [InverseProperty("Products")]
    public virtual ProductName? Name { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderArticle> OrderArticles { get; set; } = new List<OrderArticle>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Supplier? Supplier { get; set; }
}
