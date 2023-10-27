using System.Collections.Generic;
using BTL_QuanLyBanDienThoai.Models;

namespace BTL_QuanLyBanDienThoai.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Category> ?Categories { get; set; }
        public List<Product> ?Products { get; set; }
        public List<Banner> ?Banners { get; set; }
    }
}
