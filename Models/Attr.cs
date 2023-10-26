using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class Attr
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter attribute name.")]
    [MaxLength(50)]
    public string? Name { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();
}
