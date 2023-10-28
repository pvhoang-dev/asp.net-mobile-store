using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Services.Interfaces;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var categories = db.Categories.Take(3).ToList();
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
            int currentPage = Math.Max(page ?? 1, 1);

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

        [Route("products/ajax-search")]
        [HttpGet]
        public ActionResult AjaxSearch(string searchValue)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                try
                {
                    var data = db.Products
                    .Where(p => p.Name.Contains(searchValue) || p.Description.Contains(searchValue))
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.ImageDefault,
                        p.Price,
                        p.Price2
                    })
                    .ToList();

                    return Json(new { success = true, data = data });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = ex.Message });
                }
            }

            return View();
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