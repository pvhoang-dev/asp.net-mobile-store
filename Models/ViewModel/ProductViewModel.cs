using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BTL_QuanLyBanDienThoai.Models.ViewModel
 
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Please enter product name.")]
        public string? Name { get; set; }

        public string? Slug { get; set; }

        [Required(ErrorMessage = "Please enter price.")]
        public double? Price { get; set; }
        [Required(ErrorMessage = "Please enter price 2.")]
        public double? Price2 { get; set; }
        public int? Quantity { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public string? ImageDefault { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public string? CategoryName { get; set; }
        [Required(ErrorMessage = "Please enter category.")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Please enter product image.")]
        public IFormFile? Photo { get; set; }
        public List<IFormFile>? ListPhotos { get; set; }
        public List<Category>? Categories { get; set; }
        public List<ProductVariantViewModel>? ProductVariants { get; set; }
    }
}
