using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using BTL_QuanLyBanDienThoai.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/Products")]
    public class ProductController : Controller
    {
        Slug slug = new Slug();
        private readonly QLBanDienThoaiContext db;

        public ProductController(QLBanDienThoaiContext _db)
        {
            db = _db;
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
                                                           CategoryName = category.Name
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
        public IActionResult Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(new Product
                {
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Quantity = productViewModel.Quantity,
                    Slug = slug.Create(productViewModel.Name),
                    CategoryId = int.Parse(Request.Form["category_id"]),
                    Status = 1,
                    Description = productViewModel.Description,
                    ImageDefault = "adc",
                });

                db.SaveChanges();

                return RedirectToAction("Index", "Product");
            }
            return View();
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
                pro.Status = productViewModel.Status;
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
    }
}
