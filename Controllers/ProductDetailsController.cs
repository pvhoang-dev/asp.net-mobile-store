using Microsoft.AspNetCore.Mvc;

namespace BTL_QuanLyBanDienThoai.Controllers
{
    [Route("home/products/detail/{id}")]
    public class ProductDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
