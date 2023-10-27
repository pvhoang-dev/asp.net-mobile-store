using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BTL_QuanLyBanDienThoai.Services.Interfaces;
using BTL_QuanLyBanDienThoai.Models;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using static BTL_QuanLyBanDienThoai.Models.Order;
using Microsoft.CodeAnalysis;

namespace BTL_QuanLyBanDienThoai.Controllers
{
    public class OrderController : Controller
    {
        private readonly QLBanDienThoaiContext db;

        public OrderController(QLBanDienThoaiContext _db)
        {
            db = _db;
        }


        [Route("/checkout")]
        public IActionResult Index()
        {
            string cartString = HttpContext.Session.GetString("cart");

            List<CartItemViewModel> cartList = null;

            if (cartString != null)
            {
                cartList = JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartString);
            }

            string addressString = HttpContext.Session.GetString("address");

            List<AddressViewModel> addressList = null;

            if (addressString != null)
            {
                addressList = JsonConvert.DeserializeObject<List<AddressViewModel>>(addressString);
            }

            var cartViewModel = new CartViewModel
            {
                CartList = cartList,
                Addresses = addressList
            };
            return View(cartViewModel);
        }

        [Route("checkout/create")]
        [HttpPost]
        public IActionResult Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = 1;
                var productIds = Request.Form["product_id[]"];
                var productVariantIds = Request.Form["product_variant_id[]"];
                var quantities = Request.Form["quantity[]"];
                var prices = Request.Form["price[]"];
                
                var order = new Order
                {
                    UserId = userId,
                    City = Request.Form["city_id"],
                    District = Request.Form["district_id"],
                    Ward = Request.Form["ward_id"],
                    Address = Request.Form["address"],
                    Phone = Request.Form["phone_number"],
                    Note = Request.Form["note"],
                };

                db.Orders.Add(order);
                db.SaveChanges();

                float total = 0;

                for (int i = 0; i < productIds.Count; i++)
                {
                    var productId = productIds[i];
                    var productVariantId = productVariantIds[i];
                    var quantity = quantities[i];
                    var price = prices[i];

                    // Kiểm tra giá trị trước khi sử dụng
                    if (!string.IsNullOrEmpty(productId) && !string.IsNullOrEmpty(productVariantId))
                    {
                        // Chuyển đổi các giá trị thành kiểu dữ liệu cần thiết
                        var parsedProductId = int.Parse(productId);
                        var parsedProductVariantId = int.Parse(productVariantId);
                        var parsedQuantity = int.Parse(quantity);
                        var parsedPrice = int.Parse(price);

                        total += parsedQuantity * (float)parsedPrice;

                        // Tạo và thêm OrderItem vào cơ sở dữ liệu
                        var orderItem = new OrderItem
                        {
                            OrderId = (int)order.Id,
                            ProductId = parsedProductId,
                            ProductVariantId = parsedProductVariantId,
                            Quantity = parsedQuantity,
                            Price = parsedPrice
                        };

                        
                        db.OrderItems.Add(orderItem);
                    }
                }
                
                order.Amount = total;
                db.Orders.Update(order);
                db.SaveChanges();

                HttpContext.Session.Remove("cart");
                return RedirectToAction("Index", "Cart");
            }
            return View();
          
        }

        private void CreateOrderItem(int id, List<OrderItem> orderItems)
        {
            foreach (var orderItem in orderItems)
            {
                var item = new OrderItem
                {
                    OrderId = id,
                    ProductId = orderItem.ProductId,
                    ProductVariantId = orderItem.ProductVariantId,
                    Quantity = orderItem.Quantity,
                    Price = orderItem.Price
                };

                db.OrderItems.Add(item);
            }

            db.SaveChanges();
        }
    }
}
