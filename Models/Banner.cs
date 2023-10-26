using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class Banner
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage ="Please enter banner name.")]
    [MaxLength(50)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Please enter banner image.")]
    public string? Image { get; set; }

    [Required(ErrorMessage = "Please enter banner title.")]
    [MaxLength(50)]
    public string? Title { get; set; }
}
