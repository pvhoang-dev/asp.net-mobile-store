using Microsoft.AspNetCore.Mvc;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class HomeAdminController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
