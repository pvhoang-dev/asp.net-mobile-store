using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using BTL_QuanLyBanDienThoai.Models.Authentication;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authentication]
    [Area("Admin")]
    [Route("Admin/Categories")]
    public class CategoryController : Controller
    {
        private readonly QLBanDienThoaiContext db;
        readonly IBufferedFileUploadService _bufferedFileUploadService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(
            QLBanDienThoaiContext _db,
            IBufferedFileUploadService bufferedFileUploadService,
            IWebHostEnvironment webHostEnvironment)
        {
            db = _db;
            _bufferedFileUploadService = bufferedFileUploadService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Category> categories = db.Categories.ToList();
            return View(categories);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(Category category, IFormFile cateImg)
        {
            if (ModelState.IsValid)
            {
                if (cateImg != null)
                    await _bufferedFileUploadService.UploadFile(cateImg, "categories");
                
                if (category.Image != null)
                    category.Image = Path.Combine("UploadedFiles\\categories", category.Image);

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Category");
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
            Category cate = db.Categories.Find(id);
            if (cate == null)
            {
                return NotFound();
            }
            return View(cate);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Category category, IFormFile cateImg, int? id)
        {
            Category cate = db.Categories.Find(category.Id);

            if (ModelState.IsValid)
            {
                if (cateImg != null)
                    try
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, cate.Image);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        if (category.Image != null)
                            category.Image = Path.Combine("UploadedFiles\\categories", category.Image);

                        cate.Image = category.Image;

                        if (await _bufferedFileUploadService.UploadFile(cateImg, "categories"))
                        {
                            ViewBag.Message = "Edit Category Successful";
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
                    ViewBag.Message = "Edit Category Successful";
                    ViewBag.Text = "success";

                    cate.Name = category.Name;
                    db.Categories.Update(cate);
                    db.SaveChanges();
            }

            

           

            return View(cate);
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dbCat = db.Categories.FirstOrDefault(x => x.Id == id);

            if (dbCat != null)
            { 
                try
                {
                    db.Categories.Remove(dbCat);
                    db.SaveChanges();

                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, dbCat.Image);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    return Json(new { success = true, message = "done" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "You need to delete products in this category first !!!" });
                }
            }

            return Json(new { success = false, message = "That category does not exist !!!" });
        }
    }

}