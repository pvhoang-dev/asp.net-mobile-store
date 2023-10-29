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
using X.PagedList;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authorization]
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

        public IActionResult Index(int page = 1)
        {
            List<ProductViewModel> productViewModel = (from product in db.Products
                                                       join category in db.Categories
                                                       on product.CategoryId equals category.Id
                                                       orderby category.Id
                                                       select new ProductViewModel
                                                       {
                                                           Id = product.Id,
                                                           Name = product.Name,
                                                           Price = product.Price,
                                                           Price2 = product.Price2,
                                                           Quantity = product.Quantity,
                                                           Slug = product.Slug,
                                                           Status = product.Status,
                                                           CategoryName = category.Name,
                                                           ImageDefault = product.ImageDefault,
                                                       }).ToList();

            int pageSize = 8;

            IPagedList<ProductViewModel> pagedList = productViewModel.ToPagedList(page, pageSize);

            return View(pagedList);
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
            ModelState.Remove("CategoryId");
            if (ModelState.IsValid)
            {
                Product pro = new Product
                {
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Price2 = productViewModel.Price2,
                    Slug = slug.Create(productViewModel.Name),
                    CategoryId = int.Parse(Request.Form["category_id"]),
                    Quantity = 0,
                    Status = 0,
                    Description = productViewModel.Description,
                };

                db.Products.Add(pro);

                db.SaveChanges();

                if (productViewModel.Photo != null)
                {
                    string path = Path.Combine("UploadedFiles\\products\\" + pro.Id + "\\default_image", productViewModel.Photo.FileName);
                    await _bufferedFileUploadService.UploadFile(productViewModel.Photo, "products\\" + pro.Id + "\\default_image");
                    pro.ImageDefault = path;
                }

                db.Products.Update(pro);
                db.SaveChanges();

                if (productViewModel.ListPhotos != null)
                {
                    foreach (var Img in productViewModel.ListPhotos)
                    {
                        await _bufferedFileUploadService.UploadFile(Img, "products\\" + pro.Id + "\\images");

                        string path = Path.Combine("UploadedFiles\\products\\" + pro.Id + "\\images", Img.FileName);

                        ProductImage proImg = new ProductImage
                        {
                            ProductId = pro.Id,
                            Path = path,
                        };

                        db.ProductImages.Add(proImg);
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Product");
            }

            productViewModel.Categories = db.Categories.ToList();
            return View(productViewModel);
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var pro = db.Products.Find(id);

            List<ProductVariantViewModel> productVariants = (from productVariant in db.ProductVariants
                                                             join product in db.Products
                                                             on productVariant.ProductId equals product.Id
                                                             where product.Id == id
                                                             select new ProductVariantViewModel
                                                             {
                                                                 Id = productVariant.Id,
                                                                 Name = productVariant.Name,
                                                                 Price = productVariant.Price,
                                                                 Quantity = productVariant.Quantity,
                                                                 Slug = productVariant.Slug
                                                             }).ToList();

            List<ProductImage> productImages = (from proImg in db.ProductImages
                                                where proImg.ProductId == id
                                                select new ProductImage
                                                {
                                                    Id = proImg.Id,
                                                    Path = proImg.Path,
                                                }).ToList();

            ProductViewModel productViewModel = new ProductViewModel
            {
                Categories = db.Categories.ToList(),
                Id = id,
                Name = pro.Name,
                Price = pro.Price,
                Price2 = pro.Price2,
                Quantity = pro.Quantity,
                Description = pro.Description,
                CategoryId = pro.CategoryId,
                ImageDefault = pro.ImageDefault,
                ProductImages = productImages,
                ProductVariants = productVariants
            };

            return View(productViewModel);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel, int? id)
        {
            ModelState.Remove("Photo");
            ModelState.Remove("CategoryId");
            if (ModelState.IsValid)
            {
                productViewModel.Slug = slug.Create(productViewModel.Name);

                var pro = db.Products.Find(id);

                pro.Name = productViewModel.Name;
                pro.Slug = productViewModel.Slug;
                pro.Description = productViewModel.Description;
                pro.Price = productViewModel.Price;
                pro.Price2 = productViewModel.Price2;
                pro.CategoryId = int.Parse(Request.Form["category_id"]);
                db.Products.Update(pro);

                db.SaveChanges();

                return RedirectToAction("Edit", "Product", new { id = id });
            }
            var proExist = db.Products.Find(id);

            List<ProductVariantViewModel> productVariants = (from productVariant in db.ProductVariants
                                                             join product in db.Products
                                                             on productVariant.ProductId equals product.Id
                                                             where product.Id == id
                                                             select new ProductVariantViewModel
                                                             {
                                                                 Id = productVariant.Id,
                                                                 Name = productVariant.Name,
                                                                 Price = productVariant.Price,
                                                                 Quantity = productVariant.Quantity,
                                                                 Slug = productVariant.Slug
                                                             }).ToList();

            List<ProductImage> productImages = (from proImg in db.ProductImages
                                                where proImg.ProductId == id
                                                select new ProductImage
                                                {
                                                    Id = proImg.Id,
                                                    Path = proImg.Path,
                                                }).ToList();

            ProductViewModel productViewModels = new ProductViewModel
            {
                Categories = db.Categories.ToList(),
                Id = id,
                Name = proExist.Name,
                Price = proExist.Price,
                Price2 = proExist.Price2,
                Quantity = proExist.Quantity,
                Description = proExist.Description,
                CategoryId = proExist.CategoryId,
                ImageDefault = proExist.ImageDefault,
                ProductImages = productImages,
                ProductVariants = productVariants
            };
            return View(productViewModels);
        }

        [Route("UpdateImage")]
        [HttpPost]
        public async Task<IActionResult> UpdateImage(IFormCollection form, int id)
        {
            Product pro = db.Products.Find(id);

            var photo = Request.Form.Files.GetFile("Photo");

            var listPhotos = Request.Form.Files.GetFiles("ListPhotos");


            if (photo != null)
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, pro.ImageDefault);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                string path = Path.Combine("UploadedFiles\\products\\" + id + "\\default_image", photo.FileName);

                await _bufferedFileUploadService.UploadFile(photo, "products\\" + id + "\\default_image");

                pro.ImageDefault = path;

                db.Products.Update(pro);

                db.SaveChanges();
            }


            if (listPhotos.Count != 0)
            {
                List<ProductImage> productImages = (from proImg in db.ProductImages
                                                    where proImg.ProductId == id
                                                    select new ProductImage
                                                    {
                                                        Id = proImg.Id,
                                                        Path = proImg.Path,
                                                    }).ToList();

                if (productImages != null)
                {
                    foreach (ProductImage image in productImages)
                    {
                        string path = Path.Combine(_webHostEnvironment.WebRootPath, image.Path);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        db.ProductImages.Remove(db.ProductImages.Find(image.Id));
                    }
                }

                foreach (var Img in listPhotos)
                {
                    await _bufferedFileUploadService.UploadFile(Img, "products\\" + id + "\\images");

                    string path = Path.Combine("UploadedFiles\\products\\" + id + "\\images", Img.FileName);

                    ProductImage proImg = new ProductImage
                    {
                        ProductId = id,
                        Path = path,
                    };

                    db.ProductImages.Add(proImg);
                }

                db.SaveChanges();
            }

            return Json(new { message = "Success" });
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dbPro = db.Products.FirstOrDefault(x => x.Id == id);

            if (dbPro != null)
            {
                try
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, dbPro.ImageDefault);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    List<ProductImage> productImages = (from proImg in db.ProductImages
                                                        where proImg.ProductId == id
                                                        select new ProductImage
                                                        {
                                                            Id = proImg.Id,
                                                            Path = proImg.Path,
                                                        }).ToList();

                    if (productImages != null)
                    {
                        foreach (ProductImage image in productImages)
                        {
                            string path = Path.Combine(_webHostEnvironment.WebRootPath, image.Path);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            db.ProductImages.Remove(db.ProductImages.Find(image.Id));
                        }
                    }

                    var pvToRemove = db.ProductVariants.Where(pav => pav.ProductId == id).ToList();

                    var pvToRemoveIds = pvToRemove.Select(pv => pv.Id).ToList();

                    var pavToRemove = db.ProductAttributeValues
                        .Where(pav => pvToRemoveIds.Contains((int)pav.ProductVariantId))
                        .ToList();

                    if (pavToRemove != null)
                    {
                        db.ProductAttributeValues.RemoveRange(pavToRemove);
                    }

                    if (pvToRemove != null)
                    {
                        db.ProductVariants.RemoveRange(pvToRemove);
                    }

                    db.Products.Remove(dbPro);

                    db.SaveChanges();

                    return Json(new { success = true, message = "done" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Can not delete this product !" });
                }
            }

            return Json(new { success = false, message = "This id product do not exist !!!" });
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

        [HttpPost]
        [Route("Search")]
        public IActionResult Search(string query)
        {
            var searchResults = db.Products
                .Where(product => product.Name.Contains(query))
                .Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                })
                .ToList();

            return Json(searchResults);
        }
    }
}