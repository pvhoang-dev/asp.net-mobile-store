using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Models.ViewModel;


namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/attribute-values")]
    public class AttributeValueController : Controller
    {
        private readonly QLBanDienThoaiContext db;

        public AttributeValueController(QLBanDienThoaiContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            List<AttributeValueViewModel> attributeAndAttributeValues = (from attr in db.Attrs
                                              join attrValue in db.AttributeValues
                                              on attr.Id equals attrValue.AttributeId
                                              select new AttributeValueViewModel
                                              {
                                                  Id = attr.Id,
                                                  Name = attr.Name,
                                                  Value = attrValue.Name
                                              }).ToList();
            return View(attributeAndAttributeValues);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            AttributeValueViewModel attrs = new AttributeValueViewModel
            {
                attrs = db.Attrs.ToList(),
            };
            return View(attrs);
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(AttributeValueViewModel attributeValue)
        {
            if(ModelState.IsValid)
            {

                db.AttributeValues.Add(new AttributeValue
                {
                    Name= attributeValue.Name,
                    AttributeId = int.Parse(Request.Form["attribute_id"]),
                }) ;

                db.SaveChanges();
                return RedirectToAction("Index", "AttributeValue");
            }
            
            return View();
        }

        [Route("Edit")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            return View();
        }
        

    }
}
