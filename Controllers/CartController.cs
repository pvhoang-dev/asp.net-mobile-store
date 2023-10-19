using Microsoft.AspNetCore.Mvc;

namespace BTL_QuanLyBanDienThoai.Controllers
{
	public class CartController : Controller
	{
		[Route("home/carts")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
