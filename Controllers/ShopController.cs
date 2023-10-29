using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Runtime.CompilerServices;

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
            List<Product> allProducts;

            var searchProduct = HttpContext.Request.Query["search-product"];

            var categories = _context.Categories.ToList();
            var products = _context.Products.Include(p => p.Category);

            if (!string.IsNullOrEmpty(searchProduct))
            {
                allProducts = products
                    .Where(p => p.Status == 1)
                    .Where(p => p.Name.Contains(searchProduct))
                    .ToList();
            }
            else
            {
                allProducts = products.Where(p => p.Status == 1).ToList();
            }

            var viewModel = new ShopViewModel
            {
                Product = allProducts,
                Category = categories,
            };

            return View(viewModel);
        }

        [Route("products/ajax-search-price")]
        [HttpGet]
        public ActionResult AjaxSearch(double priceMin, double priceMax)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                try
                {
                    var data = _context.Products
                    .Where(p => p.Status == 1)
                    .Where(p => p.Price >= priceMin && p.Price <= priceMax)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.ImageDefault,
                        p.Price,
                        p.Price2,
                        p.CategoryId
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
    }
}
