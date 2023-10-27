using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Utils;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using Newtonsoft.Json;
using System.Diagnostics;
using BTL_QuanLyBanDienThoai.Models.Authentication;


namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authentication]
    [Area("admin")]
    [Route("admin/attribute-values")]
    public class AttributeValueController : Controller
    {
        Slug slug = new Slug();
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
                                                                         orderby attr.Name
                                                                         select new AttributeValueViewModel
                                                                         {
                                                                             Id = attrValue.Id,
                                                                             Name = attr.Name,
                                                                             Value = attrValue.Name
                                                                         }).ToList();
            return View(attributeAndAttributeValues);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            AttributeValueViewModel attrValue = new AttributeValueViewModel
            {
                attrs = db.Attrs.ToList(),
            };
            return View(attrValue);
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(AttributeValueViewModel attributeValue)
        {
            if (ModelState.IsValid)
            {
                int attrId = int.Parse(Request.Form["attribute_id"]);

                string attrValue = attributeValue.Name;

                var existingAttrValue = db.AttributeValues.FirstOrDefault(
                    a => a.AttributeId == attrId &&
                    a.Name == attrValue
                );

                if (existingAttrValue == null)
                {

                    db.AttributeValues.Add(new AttributeValue
                    {
                        AttributeId = attrId,
                        Name = attrValue,
                    });

                    db.SaveChanges();

                    return RedirectToAction("Index", "AttributeValue");

                }
                else
                {
                    ViewBag.Message = "Attribute value '" + attrValue + "' existed";
                    attributeValue.attrs = db.Attrs.ToList();
                    attributeValue.attrId = attrId;
                    return View(attributeValue);
                }
            }

            attributeValue.attrs = db.Attrs.ToList();
            return View(attributeValue);
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            AttributeValue attrValue = db.AttributeValues.Find(id);

            if (attrValue == null)
            {
                return NotFound();
            }

            AttributeValueViewModel attributeValue = new AttributeValueViewModel
            {
                attrs = db.Attrs.ToList(),
                attrId = attrValue.AttributeId,
                Name = attrValue.Name,
            };

            return View(attributeValue);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(AttributeValueViewModel attributeValue, int id)
        {
            if (ModelState.IsValid)
            {
                var attrVal = db.AttributeValues.Find(id);
                int attrId = int.Parse(Request.Form["attribute_id"]);
                attrVal.AttributeId = attrId;
                attrVal.Name = attributeValue.Name;
                db.AttributeValues.Update(attrVal);
                db.SaveChanges();
                ViewBag.Message = "Edit Attribute Value Successful";
                ViewBag.Text = "success";
                attributeValue.attrId = attrId;
            }
            else
            {
                ViewBag.Message = "Edit Attribute Value Failing";
                ViewBag.Text = "warning";
            }

            attributeValue.attrs = db.Attrs.ToList();

            return View(attributeValue);
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dbAttrVal = db.AttributeValues.FirstOrDefault(x => x.Id == id);

            if (dbAttrVal != null)
            {
                try
                {
                    var existingPAV = db.ProductAttributeValues.FirstOrDefault(a => a.AttributeValueId == id);

                    if (existingPAV != null)
                    {
                        return Json(new { success = false, message = "You need to delete product variants in this attribute first !!!" });
                    }

                    db.AttributeValues.Remove(dbAttrVal);
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
