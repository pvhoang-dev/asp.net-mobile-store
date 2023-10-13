using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class ProductAttributeValue
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? ProductVariantId { get; set; }

    public int? AttributeValueId { get; set; }

    public virtual AttributeValue? AttributeValue { get; set; }

    public virtual ProductVariant? ProductVariant { get; set; }
}
