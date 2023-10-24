namespace BTL_QuanLyBanDienThoai.Models.ViewModel
{
    public class ProductVariantViewModel
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Slug { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }
 
        public Product? Product { get; set; }
        public string? attr1 { get; set; }
        public string? attr2 { get; set; }
        public string? attrVal1 { get; set; }
        public string? attrVal2 { get; set; }
        public List<AttributeWithValueViewModel>? attributeWithValueViewModel { get; set; }
        public Dictionary<string, Dictionary<string, int>>? resultDict { get; set; }
    }
}
