using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Models;

public partial class ProductVariant
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public double? Price { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
}
