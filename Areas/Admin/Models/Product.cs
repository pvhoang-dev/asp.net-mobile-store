using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Models;

public partial class Product
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public double? Price1 { get; set; }

    public double? Price2 { get; set; }

    public string? Description { get; set; }

    public string? ImageDefault { get; set; }

    public int? Status { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
