using BTL_QuanLyBanDienThoai.Models;

 namespace BTL_QuanLyBanDienThoai.Models.ViewModel
{
    internal class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public int? attr1 { get; set; }
        public int? attr2 { get; set; }
        public Attribute? Attribute { get; set; }
        public AttributeValue AttributeValue { get; set; }
        public List<ProductImage>? productImages { get; set; }
        public Dictionary<int, Dictionary<int, Dictionary<string, string>>> ProductVariants { get; set; }
        public List<Product> CategoryProducts { get; set; }
        public string?[] AttrNames { get; internal set; }
    }
}