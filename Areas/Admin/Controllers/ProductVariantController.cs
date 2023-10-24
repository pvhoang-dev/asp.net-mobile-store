using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using BTL_QuanLyBanDienThoai.Utils;
using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;



namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product-Variants")]
    public class ProductVariantController : Controller
    {
        Slug slug = new Slug();
        private readonly QLBanDienThoaiContext db;

        public ProductVariantController(QLBanDienThoaiContext _db)
        {
            db = _db;
        }
        [Authentication]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Create/{id}")]
        public IActionResult Create(int? id)
        {
            List<AttributeWithValueViewModel> attributeWithValueViewModel =
                            (from attr in db.Attrs
                             join attrValue in db.AttributeValues
                             on attr.Id equals attrValue.AttributeId
                             select new AttributeWithValueViewModel
                             {
                                 AttributeName = attr.Name,
                                 AttributeNameId = attr.Id,
                                 AttributeValue = attrValue.Name,
                                 AttributeValueId = attrValue.Id
                             }).ToList();

            Dictionary<string, Dictionary<string, int>> resultDictionary = new Dictionary<string, Dictionary<string, int>>();

            foreach (var item in attributeWithValueViewModel)
            {
                string key = item.AttributeName; // Giá trị cột đầu tiên làm khóa
                string subKey = item.AttributeValue; // Giá trị cột thứ ba
                int subValue = item.AttributeValueId; // Giá trị cột thứ hai

                if (!resultDictionary.ContainsKey(key))
                {
                    resultDictionary[key] = new Dictionary<string, int>();
                }

                resultDictionary[key][subKey] = subValue;
            }

            ProductVariantViewModel productVariantViewModel = new ProductVariantViewModel
            {
                Product = db.Products.Find(id),
                attributeWithValueViewModel = attributeWithValueViewModel,
                resultDict = resultDictionary
            };
            return View(productVariantViewModel);
        }

        [Route("Create/{id}")]
        [HttpPost]
        public IActionResult Create(ProductVariantViewModel productVariantViewModel, int? id)
        {
            if (ModelState.IsValid)
            {
                ProductVariant pV = new ProductVariant
                {
                    Name = productVariantViewModel.Name,
                    ProductId = id,
                    Quantity = productVariantViewModel.Quantity,
                    Price = productVariantViewModel.Price,
                    Slug = slug.Create(productVariantViewModel.Name),
                };
                db.ProductVariants.Add(pV);
                
                db.SaveChanges();

                db.ProductAttributeValues.Add(new ProductAttributeValue
                {
                    ProductId = id,
                    ProductVariantId = pV.Id,
                    AttributeValueId = int.Parse(Request.Form["attribute_1_value"])
                });

                db.ProductAttributeValues.Add(new ProductAttributeValue
                {
                    ProductId = id,
                    ProductVariantId = pV.Id,
                    AttributeValueId = int.Parse(Request.Form["attribute_2_value"])
                });

                db.SaveChanges();

                return RedirectToAction("Index", "Product");
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
