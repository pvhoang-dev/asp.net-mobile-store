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

            var HomeViewModel = new HomeViewModel
            {
                Categories = categories,
                Products = products
            };

            return View(HomeViewModel);
        }
        public ActionResult GetProducts()
        {
            var products = db.Products.ToList(); // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            return Json(products, JsonRequestBehavior.AllowGet);
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