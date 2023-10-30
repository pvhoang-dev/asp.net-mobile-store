namespace BTL_QuanLyBanDienThoai.Models.ViewModel
{
    public class OrderShowViewModel
    {
        public int? UserId { get; set; }

        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

        public string? Address { get; set; }

        public double? Amount { get; set; }

        public DateTime? CreateAt { get; set; }

        public string? Note { get; set; }

        public List<OrderItem>? orderItemList { get; set; }
    }
}
