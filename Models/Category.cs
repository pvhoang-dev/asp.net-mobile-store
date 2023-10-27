using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace BTL_QuanLyBanDienThoai.Models;

public partial class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter the category name.")]
    [MaxLength(50)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Please enter the category image.")]
    public string? Image { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
