using Microsoft.AspNetCore.Mvc;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Orders")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Show")]
        public IActionResult Show()
        {
            return View();
        }
    }
}
