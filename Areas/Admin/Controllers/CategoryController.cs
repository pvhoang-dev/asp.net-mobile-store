using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Categories")]
    public class CategoryController : Controller
    {
        private readonly QLBanDienThoaiContext db;
        readonly IBufferedFileUploadService _bufferedFileUploadService;

        public CategoryController(QLBanDienThoaiContext _db, IBufferedFileUploadService bufferedFileUploadService)
        {
            db = _db;
            _bufferedFileUploadService = bufferedFileUploadService;
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
        public async Task<IActionResult> Create(Category category, IFormFile avatarFile)
        {
            if(ModelState.IsValid)
            {
                if (avatarFile != null)
                    try
                    {
                        if (await _bufferedFileUploadService.UploadFile(avatarFile))
                        {
                            Debug.WriteLine("File Upload Successful");
                            ViewBag.Message = "File Upload Successful";
                        }
                        else
                        {
                            Debug.WriteLine("File Upload Failed");
                            ViewBag.Message = "File Upload Failed";
                        }
                    }
                    catch
                    {
                        Debug.WriteLine("File Upload Failed");
                        //Log ex
                        ViewBag.Message = "File Upload Failed";
                    }

                if (category.Image != null)              
                    category.Image = Path.Combine("UploadedFiles", category.Image);
                
                    
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        [Route("Edit/{id}")]
        public IActionResult Edit()
        {
            return View();
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                db.Categories.Update(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
    }

}