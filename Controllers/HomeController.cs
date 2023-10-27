using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Services.Interfaces;
using BTL_QuanLyBanDienThoai.Models.ViewModel;

namespace BTL_QuanLyBanDienThoai.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

		private readonly QLBanDienThoaiContext db;

        public HomeController(QLBanDienThoaiContext _db, ILogger<HomeController> logger)
        {
			db = _db;
			_logger = logger;
        }

        public IActionResult Index()
        {
            var categories = db.Categories.ToList();
            var products = db.Products.ToList();
            var banners = db.Banners.ToList();

            var HomeViewModel = new HomeViewModel
            {
                Categories = categories,
                Products = products,
                Banners = banners
            };

            return View(HomeViewModel);
        }
        public ActionResult GetProducts(int? page)
        {
            int pageSize = 4;

            // Lấy giá trị trang nếu tồn tại
            int currentPage = Math.Max(page ?? 1,1);

            // Giả sử bạn đã có DbContext và DbSet cho sản phẩm
            var query = db.Products; 

            var products = query
                .Where(p => p.Status == 1)
                .OrderBy(p => p.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new
            {
                Success = true,
                Data = products
            };

            return Json(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}