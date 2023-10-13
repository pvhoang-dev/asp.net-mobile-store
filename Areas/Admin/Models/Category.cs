using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
