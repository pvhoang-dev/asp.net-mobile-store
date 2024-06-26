﻿using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class Product
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public double? Price { get; set; }

    public double? Price2 { get; set; }

    public int? Quantity { get; set; }

    public string? Description { get; set; }

    public string? ImageDefault { get; set; }

    public int? Status { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
