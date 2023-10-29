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
using BTL_QuanLyBanDienThoai.Models.Authorization;

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

            if (HttpContext.Session.GetString("Id") == null)
            {
                ViewBag.Text = "You need to log in before checking out";
            }

            return View(cartViewModel);
        }

        [Authorization]
        [Route("checkout/create")]
        [HttpPost]
        public IActionResult Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("Id");
                var productIds = Request.Form["product_id[]"];
                var productVariantIds = Request.Form["product_variant_id[]"];
                var quantities = Request.Form["quantity[]"];
                var prices = Request.Form["price[]"];
                
                var order = new Order
                {
                    UserId = int.Parse(userId),
                    City = Request.Form["city_id"],
                    District = Request.Form["district_id"],
                    Ward = Request.Form["ward_id"],
                    Address = Request.Form["address"],
                    Phone = Request.Form["phone_number"],
                    Note = Request.Form["note"],
                    CreateAt = DateTime.Now,
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

                        var product = db.Products.Find(parsedProductId);

                        var productVariant = db.ProductVariants.Find(parsedProductVariantId);

                        productVariant.Quantity = (productVariant.Quantity - parsedQuantity) >= 0 ? (productVariant.Quantity - parsedQuantity) : 0;

                        product.Quantity = (product.Quantity - parsedQuantity) >= 0 ? (product.Quantity - parsedQuantity) : 0;

                        db.Products.Update(product);

                        db.ProductVariants.Update(productVariant);

                        db.OrderItems.Add(orderItem);

                        db.SaveChanges();
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
