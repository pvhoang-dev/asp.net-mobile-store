using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class OrderItem
{
    [Key]
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
