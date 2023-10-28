using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Services.Interfaces;
using BTL_QuanLyBanDienThoai.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authentication]
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
        public IActionResult Index(int page = 1)
        {
            List<Banner> banners = db.Banners.ToList();

            int pageSize = 5;

            IPagedList<Banner> pagedList = banners.ToPagedList(page, pageSize);

            return View(pagedList);
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

        [Route("Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Banner banner, IFormFile cateImg, int? id)
        {
            Banner ban = db.Banners.Find(banner.Id);

            if (ModelState.IsValid)
            {
                if (cateImg != null)
                    try
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, banner.Image);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        if (banner.Image != null)
                            banner.Image = Path.Combine("UploadedFiles\\banners", banner.Image);

                        ban.Image = banner.Image;

                        if (await _bufferedFileUploadService.UploadFile(cateImg, "banners"))
                        {
                            ViewBag.Message = "Edit Banner Successful";
                            ViewBag.Text = "success";
                        }
                        else
                        {
                            ViewBag.Message = "File Upload Failed";
                            ViewBag.Text = "warning";
                        }
                    }
                    catch
                    {
                        ViewBag.Message = "File Upload Failed";
                        ViewBag.Text = "warning";
                    }
            }

            ViewBag.Message = "Edit Banner Successful";
            ViewBag.Text = "success";

            ban.Name = banner.Name;
            ban.Title = banner.Title;
            

            db.Banners.Update(ban);
            db.SaveChanges();

            return View(banner);
        }


        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id) 
        {
            var dbBanner = db.Banners.FirstOrDefault(x => x.Id == id);
            if(dbBanner != null)
            {
                db.Banners.Remove(dbBanner);
                try
                {
                    db.SaveChanges();
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, dbBanner.Image);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    return Json(new { success = true, message = "done" });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return BadRequest(JsonConvert.SerializeObject(
                        new
                        {
                            error = "Can not delete this attribute."
                        }
                    ));
                }
            }
            return BadRequest(JsonConvert.SerializeObject(
                new
                {
                    error = "Can not delete this attribute."
                }
            ));
        }
    }
}
