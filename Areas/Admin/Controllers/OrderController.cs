﻿using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models.Authentication;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using X.PagedList;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Authorization]
    [Area("Admin")]
    [Route("Admin/Orders")]
    public class OrderController : Controller
    {
        private readonly QLBanDienThoaiContext db;

        public OrderController(QLBanDienThoaiContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index(int page = 1)
        {
            var orders = db.Orders.OrderByDescending(o => o.CreateAt);

            int pageSize = 8;

            IPagedList<Order> pagedList = orders.ToPagedList(page, pageSize);

            return View(pagedList);
        }

        [Route("Show/{id}")]
        public IActionResult Show(int id)
        {
            Order order = db.Orders.Find(id);

            OrderShowViewModel orderShowViewModel = new OrderShowViewModel
            {
                UserId = order.UserId,

                FirstName = order.FirstName,

                LastName = order.LastName,

                Phone = order.Phone,

                City = order.City,

                District = order.District,

                Ward = order.Ward,

                Address = order.Address,

                Amount = order.Amount,

                CreateAt = order.CreateAt,

                Note = order.Note,

                orderItemList = (from oi in db.OrderItems
                                 join p in db.ProductVariants on oi.ProductVariantId equals p.Id
                                 where oi.OrderId == id
                                 select new OrderItemViewModel
                                 {
                                     orderItem = oi,
                                     productVariant = p
                                 }).ToList()
        };

            return View(orderShowViewModel);
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var order = db.Orders.FirstOrDefault(x => x.Id == id);

            if (order != null)
            {
                try
                {
                    var oiToRemove = db.OrderItems.Where(od => od.OrderId == id).ToList();

                    if (oiToRemove != null)
                    {
                        db.OrderItems.RemoveRange(oiToRemove);
                    }

                    db.Orders.Remove(order);

                    db.SaveChanges();

                    return Json(new { success = true, message = "done" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Deleted failing !!!" });
                }
            }

            return Json(new { success = false, message = "That order does not exist !!!" });
        }
    }
}
