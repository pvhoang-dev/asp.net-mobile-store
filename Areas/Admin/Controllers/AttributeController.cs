using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Utils;
using BTL_QuanLyBanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using BTL_QuanLyBanDienThoai.Models.Authentication;
using X.PagedList;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authorization]
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

        public IActionResult Index(int page = 1)
        {
            List<Attr> attrs = db.Attrs.ToList();
            
            int pageSize = 5;

            IPagedList<Attr> pagedList = attrs.ToPagedList(page, pageSize);

            return View(pagedList);
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
                    return new JsonResult(new
                    {
                        status = "success"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        status = "exist",
                        error = "Attribute '" + attr.Name + "' existed"
                    });
                }
            }
            else
            {
                var errors = ModelState.Values
                        .SelectMany(value => value.Errors)
                        .Select(error => error.ErrorMessage);

                return new JsonResult(new
                {
                    status = "error",
                    errors
                });
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
                ViewBag.Message = "Edit Attribute Successful";
                ViewBag.Text = "success";
            }
            return View(attr);
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dbAttr = db.Attrs.FirstOrDefault(x => x.Id == id);

            if (dbAttr != null)
            {                
                try
                {
                    var existingChildAttrValue = db.AttributeValues.FirstOrDefault(a => a.AttributeId == id);

                    if(existingChildAttrValue != null)
                    {
                        return Json(new { success = false, message = "You need to delete attribute values in this attribute first !!!" });
                    }

                    db.Attrs.Remove(dbAttr);
                    db.SaveChanges();

                    return Json(new { success = true, message = "done" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "You need to delete attribute values in this attribute first !!!" });
                }
            }

            return Json(new { success = false, message = "That attribute does not exist !!!" });
        }
    }
}
