using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authorization]
    [Area("admin")]
    [Route("admin")]
    public class HomeAdminController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
