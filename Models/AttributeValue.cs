using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTL_QuanLyBanDienThoai.Models;

public partial class AttributeValue
{
    [Key]
    public int Id { get; set; }


    [Required(ErrorMessage = "Please enter the attribute value name.")]
    public int? AttributeId { get; set; }

    [Required(ErrorMessage = "Please enter the attribute value name.")]
    [MaxLength(50)]
    public string? Name { get; set; }

    public virtual Attr? Attr { get; set; }

    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
}
