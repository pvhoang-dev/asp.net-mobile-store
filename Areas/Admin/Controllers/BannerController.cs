using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{   
    [Area("Admin")]
    [Route("Admin/Banners")]
    public class BannerController : Controller
    {
        [Authentication]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Edit")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
