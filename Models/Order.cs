using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Phone { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public double? Amount { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User? User { get; set; }
}
