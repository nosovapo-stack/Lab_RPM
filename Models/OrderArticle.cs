using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models;

[Table("OrderArticle")]
public partial class OrderArticle
{
    [Key]
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public double? Count { get; set; }

    public int? ProductId { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderArticles")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("OrderArticles")]
    public virtual Product? Product { get; set; }
}
