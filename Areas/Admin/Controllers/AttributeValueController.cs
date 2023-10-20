using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;

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
            List<AttributeValue> attributeValues = db.AttributeValues.ToList();
            return View(attributeValues);
        }

        /*
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(AttributeValue attributeValue)
        {
            if(ModelState.IsValid)
            {
                db.AttributeValues.Add(attributeValue);
                db.SaveChanges();
                return RedirectToAction("Index", "AttributeValue");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            return View();
        }
        */
    }
}
