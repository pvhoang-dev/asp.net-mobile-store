using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using BTL_QuanLyBanDienThoai.Utils;
using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;
using Newtonsoft.Json;
using X.PagedList;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authorization]
    [Area("Admin")]
    [Route("Admin/Product-Variants")]
    public class ProductVariantController : Controller
    {
        Slug slug = new Slug();
        private readonly QLBanDienThoaiContext db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductVariantController(
            QLBanDienThoaiContext _db,
            IWebHostEnvironment webHostEnvironment)
        {
            db = _db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int page = 1)
        {
            List<ProductVariantViewModel> productVariants = (from productVariant in db.ProductVariants
                                                             join product in db.Products
                                                             on productVariant.ProductId equals product.Id
                                                             orderby product.Id
                                                             select new ProductVariantViewModel
                                                             {
                                                                 Id = productVariant.Id,
                                                                 Name = productVariant.Name,
                                                                 Price = productVariant.Price,
                                                                 Quantity = productVariant.Quantity,
                                                                 Slug = productVariant.Slug,
                                                                 Product = product
                                                             }).ToList();
            int pageSize = 8;

            IPagedList<ProductVariantViewModel> pagedList = productVariants.ToPagedList(page, pageSize);

            return View(pagedList);
        }

        [Route("Create/{id}")]
        public IActionResult Create(int? id)
        {
            List<ProductAttributeValue> pAVs = db.ProductAttributeValues.Where(p => p.ProductId == id).Take(2).ToList();
            List<AttributeWithValueViewModel> attributeWithValueViewModel = new List<AttributeWithValueViewModel>();
            if (pAVs.Count != 0)
            {
                int attrVal1 = (int)pAVs[0].AttributeValueId;
                int attrVal2 = (int)pAVs[1].AttributeValueId;

                var av1 = db.AttributeValues.Find(attrVal1);
                var av2 = db.AttributeValues.Find(attrVal2);

                attributeWithValueViewModel = (from attr in db.Attrs
                                               join attrValue in db.AttributeValues
                                               on attr.Id equals attrValue.AttributeId
                                               where attr.Id == av1.AttributeId || attr.Id == av2.AttributeId
                                               select new AttributeWithValueViewModel
                                               {
                                                   AttributeName = attr.Name,
                                                   AttributeNameId = attr.Id,
                                                   AttributeValue = attrValue.Name,
                                                   AttributeValueId = attrValue.Id
                                               }).ToList();
            }
            else
            {
                attributeWithValueViewModel = (from attr in db.Attrs
                                               join attrValue in db.AttributeValues
                                               on attr.Id equals attrValue.AttributeId
                                               select new AttributeWithValueViewModel
                                               {
                                                   AttributeName = attr.Name,
                                                   AttributeNameId = attr.Id,
                                                   AttributeValue = attrValue.Name,
                                                   AttributeValueId = attrValue.Id
                                               }).ToList();
            }

            Dictionary<string, Dictionary<string, int>> resultDictionary = new Dictionary<string, Dictionary<string, int>>();

            foreach (var item in attributeWithValueViewModel)
            {
                string key = item.AttributeName;
                string subKey = item.AttributeValue;
                int subValue = item.AttributeValueId;

                if (!resultDictionary.ContainsKey(key))
                {
                    resultDictionary[key] = new Dictionary<string, int>();
                }

                resultDictionary[key][subKey] = subValue;
            }

            ProductVariantViewModel productVariantViewModel = new ProductVariantViewModel
            {
                Product = db.Products.Find(id),
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

                int attrVal_1 = int.Parse(Request.Form["attribute_1_value"]);
                int attrVal_2 = int.Parse(Request.Form["attribute_2_value"]);

                var existingVartiant1 = db.ProductAttributeValues.Where(
                        a => a.ProductId == id &&
                        a.AttributeValueId == attrVal_1
                    ).ToList();

                var existingVartiant2 = db.ProductAttributeValues.Where(
                        a => a.ProductId == id &&
                        a.AttributeValueId == attrVal_2
                    ).ToList();

                bool checkExist = existingVartiant1.Any(ev1 => existingVartiant2.Any(ev2 => ev1.ProductVariantId == ev2.ProductVariantId));

                if (checkExist)
                {
                    db.ProductVariants.Remove(pV);
                    db.SaveChanges();

                    productVariantViewModel.exist = id;

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
                        string key = item.AttributeName;
                        string subKey = item.AttributeValue;
                        int subValue = item.AttributeValueId;

                        if (!resultDictionary.ContainsKey(key))
                        {
                            resultDictionary[key] = new Dictionary<string, int>();
                        }

                        resultDictionary[key][subKey] = subValue;
                    }

                    productVariantViewModel.resultDict = resultDictionary;
                    productVariantViewModel.Product = db.Products.Find(id);

                    return View(productVariantViewModel);
                }
                else
                {
                    Product product = db.Products.Find(id);

                    product.Quantity = product.Quantity + pV.Quantity;

                    product.Status = 1;

                    db.Update(product);

                    db.ProductAttributeValues.Add(new ProductAttributeValue
                    {
                        ProductId = id,
                        ProductVariantId = pV.Id,
                        AttributeValueId = attrVal_1
                    });

                    db.ProductAttributeValues.Add(new ProductAttributeValue
                    {
                        ProductId = id,
                        ProductVariantId = pV.Id,
                        AttributeValueId = attrVal_2
                    });

                    db.SaveChanges();

                    return RedirectToAction("Edit", "Product", new { id = id });
                }
            }

            return View();
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            ProductVariant proVariant = db.ProductVariants.Find(id);

            ProductVariantViewModel productVariantViewModel = new ProductVariantViewModel
            {
                Product = db.Products.Find(proVariant.ProductId),
                Name = proVariant.Name,
                Price = proVariant.Price,
                Quantity = proVariant.Quantity,
            };

            List<ProductAttributeValue> productAttributeValues =
                            (from pav in db.ProductAttributeValues
                             where pav.ProductVariantId == id
                             select new ProductAttributeValue
                             {
                                 AttributeValueId = pav.AttributeValueId,
                             }).ToList();

            int a1 = productAttributeValues[0].AttributeValueId.Value;
            int a2 = productAttributeValues[1].AttributeValueId.Value;

            AttributeValue attributeValue1 = db.AttributeValues.Find(a1);
            AttributeValue attributeValue2 = db.AttributeValues.Find(a2);

            productVariantViewModel.attrVal1 = attributeValue1.Name;
            productVariantViewModel.attrVal2 = attributeValue2.Name;

            Attr attr1 = db.Attrs.Find(attributeValue1.AttributeId);
            Attr attr2 = db.Attrs.Find(attributeValue2.AttributeId);

            productVariantViewModel.attr1 = attr1.Name;
            productVariantViewModel.attr2 = attr2.Name;

            return View(productVariantViewModel);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(ProductVariantViewModel productVariantViewModel, int id)
        {
            ProductVariant proVariant = db.ProductVariants.Find(id);

            int amountPro = (int)proVariant.Quantity;

            Product product = db.Products.Find(proVariant.ProductId);

            product.Quantity = product.Quantity - amountPro + productVariantViewModel.Quantity;

            proVariant.Name = productVariantViewModel.Name;
            proVariant.Price = productVariantViewModel.Price;
            proVariant.Quantity = productVariantViewModel.Quantity;
            proVariant.Slug = slug.Create(productVariantViewModel.Name);

            db.Update(product);

            db.SaveChanges();

            return RedirectToAction("Edit", "ProductVariant", new { id = id });
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var productVariant = db.ProductVariants.FirstOrDefault(x => x.Id == id);

            if (productVariant != null)
            {
                try
                {

                    var itemsToRemove = db.ProductAttributeValues.Where(pav => pav.ProductVariantId == id).ToList();

                    Product product = db.Products.Find(productVariant.ProductId);

                    product.Quantity = (product.Quantity - productVariant.Quantity) >= 0 ? (product.Quantity - productVariant.Quantity) : 0;

                    if(product.Quantity == 0)
                    {
                        product.Status = 0;
                    }

                    db.Update(product);

                    db.ProductAttributeValues.RemoveRange(itemsToRemove);

                    db.ProductVariants.Remove(productVariant);

                    db.SaveChanges();

                    return Json(new { success = true, message = "done" });
                }
                catch (Exception ex)
                {
                    return BadRequest(JsonConvert.SerializeObject(
                        new
                        {
                            error = "Can not delete"
                        }
                    ));
                }
            }

            return BadRequest(JsonConvert.SerializeObject(
                new
                {
                    error = "Can not delete this product variant"
                }
            ));
        }

        [HttpPost]
        [Route("Search")]
        public IActionResult Search(string query)
        {
            var searchResults = db.ProductVariants
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
