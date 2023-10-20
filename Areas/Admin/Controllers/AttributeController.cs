using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{


    [Area("admin")]
    [Route("admin/attributes")]
    public class AttributeController : Controller
    {
        private readonly QLBanDienThoaiContext db;

        public AttributeController(QLBanDienThoaiContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index()
        {
            List<Attr> attrs = db.Attrs.ToList();
            return View(attrs);
            // return View();
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(Attr attr)
        {
            if (ModelState.IsValid)
            {
                this.db.Attrs.Add(attr);
                this.db.SaveChanges();
                return RedirectToAction("Index", "Attribute");
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
            Attr attr = db.Attrs.Find(id);
            if (attr == null)
            {
                return NotFound();
            }
            return View(attr);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(Attr attr)
        {
            if (ModelState.IsValid)
            {
                db.Attrs.Update(attr);
                db.SaveChanges();
                return RedirectToAction("Index", "Attribute");

            }
            return View();
        }

        [Route("Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Attr attr = this.db.Attrs.Find(id);
            if (attr == null)
            {
                return NotFound();
            }
            return View(attr);
        }

        [Route("Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Attr? attr = db.Attrs.Find(id);
            if (attr == null)
            {
                return NotFound();
            }

            db.Attrs.Remove(attr);
            db.SaveChanges();
            return RedirectToAction("Index", "Attribute");
        }

        
    }
}
