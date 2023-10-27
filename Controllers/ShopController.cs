using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL_QuanLyBanDienThoai.Models.ViewModel;

namespace BTL_QuanLyBanDienThoai.Controllers
{
    public class ShopController : Controller
    {
        private readonly QLBanDienThoaiContext _context;

        public ShopController(QLBanDienThoaiContext context)
        {
            _context = context;
        }

        [Route("shops")]
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();

            var allProducts = _context.Products
                .Include(p => p.Category)
                .ToList();

            var viewModel = new ShopViewModel
            {
                Product = allProducts,
                Category = categories,
            };

            return View(viewModel);
        }

    }
}
