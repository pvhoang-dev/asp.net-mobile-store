using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class HomeAdminController : Controller
    {
        [Authentication]
        public IActionResult Index()
        {
            return View();
        }
    }
}
