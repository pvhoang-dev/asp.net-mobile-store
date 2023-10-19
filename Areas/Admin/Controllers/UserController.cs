using Microsoft.AspNetCore.Mvc;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    public class UserController : Controller
    {
        [Route("List")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Logout")]
        public IActionResult Logout()
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
