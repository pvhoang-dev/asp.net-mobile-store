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

                int attrVal_1 = int.Parse(Request.Form["attribute_1_value"]);
                int attrVal_2 = int.Parse(Request.Form["attribute_2_value"]);

                var existingVartiant1 = db.ProductAttributeValues.FirstOrDefault(
                        a => a.ProductId == id &&
                        a.AttributeValueId == attrVal_1
                    );

                var existingVartiant2 = db.ProductAttributeValues.FirstOrDefault(
                        a => a.ProductId == id &&
                        a.AttributeValueId == attrVal_2
                    );

                if (existingVartiant1 != null && existingVartiant2 != null)
                {
                    db.ProductVariants.Remove(pV);
                    db.SaveChanges();

                    ViewBag.Message = "Product variant with these attributes existed";
                    ViewBag.Text = "warning";

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
            ViewBag.Message = "Error";
            ViewBag.Text = "warning";
            return View();
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            ProductVariant proVariant = db.ProductVariants.Find(id);

            ProductVariantViewModel productVariantViewModel = new ProductVariantViewModel();
            productVariantViewModel.Product = db.Products.Find(proVariant.ProductId);
            productVariantViewModel.Name = proVariant.Name;
            productVariantViewModel.Price = proVariant.Price;
            productVariantViewModel.Quantity = proVariant.Quantity;

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
        public IActionResult Edit(ProductVariantViewModel productVariantViewModel,int id)
        {
            ProductVariant proVariant = db.ProductVariants.Find(id);
             
            proVariant.Name = productVariantViewModel.Name;
            proVariant.Price = productVariantViewModel.Price;
            proVariant.Quantity = productVariantViewModel.Quantity;

            db.Update(proVariant);
            db.SaveChanges();

            return RedirectToAction("Edit", "Product", new { id = proVariant.ProductId });
        }
    }
}
