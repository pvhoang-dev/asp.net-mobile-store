using static BTL_QuanLyBanDienThoai.Controllers.CartController;

namespace BTL_QuanLyBanDienThoai.Models.ViewModel
{
	public class CartItemViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double? Price { get; set; }
		public int Quantity { get; set; }
        public CartItemAttributesViewModel Attributes { get; set; }
        public int ProductId { get;  set; }
    }

    public class CartItem
    {
        public int Key { get; set; }
        public int Value { get; set; }
    }

    public class AddressViewModel
    {
        public string Id { get; set; }
        public string Street { get; set; } 
        public string City { get; set; } 
        public string District { get; set; }
        public string Ward { get; set; }
    }
    public class CartItemAttributesViewModel
	{
		public string Image { get; set; }
		public int ProductId { get; set; }
	}

	public class CartViewModel
	{
		public List<CartItemViewModel> CartList { get; set; }
		public List<AddressViewModel> Addresses { get; set; }

        public int? checkLogin { get; set; }
    }
}
