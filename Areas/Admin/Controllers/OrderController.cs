using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Orders")]
    public class OrderController : Controller
    {
        [Authentication]
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
