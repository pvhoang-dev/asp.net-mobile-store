using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
				allProducts = products.Where(p => p.Name.Contains(searchProduct)).ToList();
			}
			else
			{
				allProducts = products.ToList(); 
			}

			var viewModel = new ShopViewModel
			{
				Product = allProducts,
				Category = categories,
			};

			return View(viewModel);
		}
	}
}
