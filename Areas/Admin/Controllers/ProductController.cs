using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using BTL_QuanLyBanDienThoai.Services.Interfaces;
using BTL_QuanLyBanDienThoai.Utils;
using BTL_QuanLyBanDienThoai.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authentication]
    [Area("admin")]
    [Route("admin/Products")]
    public class ProductController : Controller
    {
        Slug slug = new Slug();
        private readonly QLBanDienThoaiContext db;
        readonly IBufferedFileUploadService _bufferedFileUploadService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
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
            List<ProductViewModel> productViewModel = (from product in db.Products
                                                       join category in db.Categories
                                                       on product.CategoryId equals category.Id
                                                       select new ProductViewModel
                                                       {
                                                           Id = product.Id,
                                                           Name = product.Name,
                                                           Price = product.Price,
                                                           Quantity = product.Quantity,
                                                           Slug = product.Slug,
                                                           Status = product.Status,
                                                           CategoryName = category.Name,
                                                           ImageDefault = product.ImageDefault,
                                                       }).ToList();
            return View(productViewModel);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ProductViewModel productViewModel = new ProductViewModel
            {
                Categories = db.Categories.ToList()
            };

            return View(productViewModel);
        }


        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.Photo != null)
                    await _bufferedFileUploadService.UploadFile(productViewModel.Photo, "products");

                if (productViewModel.ImageDefault != null)
                    productViewModel.ImageDefault = Path.Combine("UploadedFiles\\products", productViewModel.ImageDefault);

                db.Products.Add(new Product
                {
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Quantity = productViewModel.Quantity,
                    Slug = slug.Create(productViewModel.Name),
                    CategoryId = int.Parse(Request.Form["category_id"]),
                    Status = 1,
                    Description = productViewModel.Description,
                    ImageDefault = productViewModel.ImageDefault,
                });

                db.SaveChanges();

                return RedirectToAction("Index", "Product");
            }

            productViewModel.Categories = db.Categories.ToList();
            return View(productViewModel);
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            var pro = db.Products.Find(id);
            ProductViewModel productViewModel = new ProductViewModel
            {
                Categories = db.Categories.ToList(),
                Id = id,
                Name = pro.Name,
                Price = pro.Price,
                Quantity = pro.Quantity,
                Description = pro.Description,
                CategoryId = pro.CategoryId,
            };

            return View(productViewModel);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel, int? id)
        {
            if (ModelState.IsValid)
            {
                productViewModel.Slug = slug.Create(productViewModel.Name);

                var pro = db.Products.Find(id);

                pro.Name = productViewModel.Name;
                pro.Slug = productViewModel.Slug;
                pro.Description = productViewModel.Description;
                pro.Quantity = productViewModel.Quantity;
                pro.Price = productViewModel.Price;
                pro.CategoryId = int.Parse(Request.Form["category_id"]);
                db.Products.Update(pro);

                db.SaveChanges();

                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dbPro = db.Products.FirstOrDefault(x => x.Id == id);

            if (dbPro != null)
            {
                db.Products.Remove(dbPro);
                db.SaveChanges();
                try
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, dbPro.ImageDefault);

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

        [HttpPost]
        [Route("UpdateStatus")]
        public IActionResult UpdateStatus(int object_id, int status)
        {
            var product = db.Products.Find(object_id);

            if (product != null)
            {
                if (status == 1)
                    status = 0;
                else
                    status = 1;
                product.Status = status;
                db.Products.Update(product);
                db.SaveChanges();

                return Json(new { status = status });
            }

            return Json(new { message = "Error" });
        }
    }
}