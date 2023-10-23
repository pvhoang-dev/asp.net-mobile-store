using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Utils;
using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("Admin/Attributes")]
    public class AttributeController : Controller
    {
        Slug slug = new Slug();
        private readonly QLBanDienThoaiContext db;

        public AttributeController(QLBanDienThoaiContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index()
        {
            List<Attr> attrs = db.Attrs.ToList();
            return View(attrs);
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
            string code = slug.Create(attr.Name);
            var existingAttr = db.Attrs.FirstOrDefault(a => a.Code == code);

            if (existingAttr == null)
            {
                db.Attrs.Add(new Attr
                {
                    Name = attr.Name,
                    Code = code,
                });
                db.SaveChanges();

                return RedirectToAction("Index", "Attribute");
            }
            else
            {
                ViewBag.Message = "Attribute '" + attr.Name + "' existed";

                return View(attr);
            }
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
                attr.Code = slug.Create(attr.Name);
                db.Attrs.Update(attr);
                db.SaveChanges();
                return RedirectToAction("Index", "Attribute");

            }
            return View();
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dbAttr = db.Attrs.FirstOrDefault(x => x.Id == id);

            if (dbAttr != null)
            {
                db.Attrs.Remove(dbAttr);
                try
                {
                    db.SaveChanges();

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
    }
}
