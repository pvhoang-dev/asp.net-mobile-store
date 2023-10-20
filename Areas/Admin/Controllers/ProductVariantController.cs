using Microsoft.AspNetCore.Mvc;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product-Variants")]
    public class ProductVariantController : Controller
    {
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
