namespace BTL_QuanLyBanDienThoai.Models.ViewModel
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public string? Slug { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public string? ImageDefault { get; set; }
        public string? CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile? Photo { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
