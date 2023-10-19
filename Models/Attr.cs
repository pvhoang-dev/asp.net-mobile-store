using System;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class Attr
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();
}
