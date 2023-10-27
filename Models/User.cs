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

    [Required(ErrorMessage ="Please enter your email.")]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is not in the correct format.")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    [StringLength( 255,MinimumLength = 4, ErrorMessage = "Password must have 4 characters")]
    public string? Password { get; set; }
    public int? Role { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
