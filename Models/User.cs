using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [Required(ErrorMessage ="Please enter your name.")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must have 8 characters")]
    public string? Password { get; set; }

    public int? Role { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
