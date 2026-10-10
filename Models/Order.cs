using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models;

public partial class Order
{
    [Key]
    public int Id { get; set; }

    public double? Number { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeliveryDate { get; set; }

    public int? PickupPointId { get; set; }

    public int? UserId { get; set; }

    public double? Code { get; set; }

    public int? StatusId { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderArticle> OrderArticles { get; set; } = new List<OrderArticle>();

    [ForeignKey("PickupPointId")]
    [InverseProperty("Orders")]
    public virtual PickupPoint? PickupPoint { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Orders")]
    public virtual Status? Status { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User? User { get; set; }
}
