using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BTL_QuanLyBanDienThoai.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly QLBanDienThoaiContext _db;

        public MenuViewComponent(QLBanDienThoaiContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _db.Categories.ToList();
            string cartString = HttpContext.Session.GetString("cart");

            List<CartItemViewModel> cartList = null;

            if (cartString != null)
            {
                cartList = JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartString);
            }

            var menuViewModel = new BTL_QuanLyBanDienThoai.Models.ViewModel.MenuViewModel
            {
                Cart = cartList ?? new List<CartItemViewModel>(),
                Categories = categories
            };

            return View(menuViewModel);
        }
    }
}
