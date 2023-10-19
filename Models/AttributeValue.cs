using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class AttributeValue
{
    public int Id { get; set; }

    public int? AttributeId { get; set; }

    public string? Name { get; set; }

    public virtual Attr? Attr { get; set; }

    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
}
