using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class ProductImage
{
    [Key]
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Path { get; set; }

    public virtual Product? Product { get; set; }
}
