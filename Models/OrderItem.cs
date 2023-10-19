using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? ProductVariantId { get; set; }

    public int? Quantity { get; set; }

    public int? Price { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? OrderNavigation { get; set; }

    public virtual ProductVariant? ProductVariant { get; set; }
}
