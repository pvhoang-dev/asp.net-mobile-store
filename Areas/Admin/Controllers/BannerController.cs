using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Services.Interfaces;
using BTL_QuanLyBanDienThoai.Models;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    //[Authentication]
    [Area("Admin")]
    [Route("Admin/Banners")]
    public class BannerController : Controller
    {
        private readonly QLBanDienThoaiContext db;
        readonly IBufferedFileUploadService _bufferedFileUploadService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BannerController(QLBanDienThoaiContext _db,
            IBufferedFileUploadService bufferedFileUploadService,
            IWebHostEnvironment webHostEnvironment
            )
        {
            db = _db;
            _bufferedFileUploadService = bufferedFileUploadService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Banner> banners = db.Banners.ToList();
            return View(banners);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(Banner banner, IFormFile cateImg)
        {
            if(ModelState.IsValid)
            {
                if (cateImg != null)
                {
                    await _bufferedFileUploadService.UploadFile(cateImg, "banners");
                }
                if(banner.Image != null)
                {
                    banner.Image = Path.Combine("UploadedFiles\\banners", banner.Image);
                }
                db.Banners.Add(banner);
                db.SaveChanges();
                return RedirectToAction("Index", "Banner");
            }

            return View();
        }

        [Route("Edit")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
