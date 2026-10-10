using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? FullName { get; set; }

    [StringLength(255)]
    public string? Login { get; set; }

    [StringLength(255)]
    public string? Password { get; set; }

    public int? Role { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("Role")]
    [InverseProperty("Users")]
    public virtual Role? RoleNavigation { get; set; }
}
