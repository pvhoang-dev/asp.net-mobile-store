using System.ComponentModel.DataAnnotations;

namespace BTL_QuanLyBanDienThoai.Models.ViewModel
{
    public class AttributeValueViewModel
    {
        public List<Attr>? attrs { get; set; }

        // [Required(ErrorMessage = "Please enter the attribute")]
        public int? attrId { get; set; }
        public string? Value { get; set; }

        [Required(ErrorMessage = "Please enter the attribute value name.")]
        [MaxLength(50)]
        public string? Name { get; set; }
        public int? Id { get; set; }
    }
}
