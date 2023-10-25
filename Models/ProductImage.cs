using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Path { get; set; }

    public virtual Product? Product { get; set; }
}
