using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace BTL_QuanLyBanDienThoai.Controllers
{
    public class CartController : Controller
    {

        private readonly QLBanDienThoaiContext db;

        public CartController(QLBanDienThoaiContext _db)
        {
            db = _db;
        }

        [Route("home/carts")]
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

        [Route("/cart/add")]
        [HttpPost]
        public ActionResult AddToCart(int id, int qty)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                List<CartItemViewModel> cartList;

                if (HttpContext.Session.TryGetValue("cart", out byte[] cartBytes))
                {
                    // Nếu giỏ hàng đã tồn tại trong phiên, lấy danh sách từ phiên
                    var cartString = Encoding.UTF8.GetString(cartBytes);

                    cartList = JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartString);

                }
                else
                {
                    // Nếu giỏ hàng chưa tồn tại trong phiên, tạo một danh sách mới
                    cartList = new List<CartItemViewModel>();
                }

                // Tìm sản phẩm và thêm vào giỏ hàng
                var productVariant = db.ProductVariants.Include("Product").FirstOrDefault(p => p.Id == id);

                if (productVariant != null)
                {
                    // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
                    var existingCartItem = cartList.FirstOrDefault(item => item.Id == productVariant.Id);
                    var productId = (int)productVariant.ProductId;
                    if (existingCartItem != null)
                    {
                        // Nếu sản phẩm đã tồn tại, cập nhật Quantity
                        existingCartItem.Quantity += qty;
                    }
                    else
                    {
                        // Nếu sản phẩm chưa tồn tại, thêm mới vào giỏ hàng
                        var cartItem = new CartItemViewModel
                        {
                            ProductId = productId,
                            Id = productVariant.Id,
                            Name = productVariant.Name,
                            Price = productVariant.Price,
                            Quantity = qty,
                            Attributes = new CartItemAttributesViewModel
                            {
                                Image = productVariant.Product.ImageDefault,
                                ProductId = productId
                            }
                        };

                            cartList.Add(cartItem);
                    }

                    // Lưu trạng thái giỏ hàng vào phiên
                    string cartString = JsonConvert.SerializeObject(cartList);
                    HttpContext.Session.SetString("cart", cartString);

                    return Json(new { message = "Item added successfully" });

                }

            }
            return View();
        }

        [Route("/cart/update")]
        [HttpPost]
        public IActionResult Update([FromBody] List<CartItem> arr)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (arr != null)
                {
                    List<CartItemViewModel> cartList;

                    // Lấy giỏ hàng từ session nếu đã tồn tại
                    if (HttpContext.Session.TryGetValue("cart", out byte[] cartBytes))
                    {
                        var cartString = Encoding.UTF8.GetString(cartBytes);
                        cartList = JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartString);
                    }
                    else
                    {
                        cartList = new List<CartItemViewModel>();
                    }

                    foreach (var item in arr)
                    {
                        int key = item.Key; // Đây là ID của sản phẩm
                        int value = item.Value; // Đây là số lượng cần cập nhật

                        // Tìm sản phẩm và thêm vào giỏ hàng
                        var productVariant = db.ProductVariants.Include("Product").FirstOrDefault(p => p.Id == key);

                        if (productVariant != null)
                        {
                            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
                            var existingCartItem = cartList.FirstOrDefault(cartItem => cartItem.Id == productVariant.Id);

                            if (existingCartItem != null)
                            {
                                // Nếu sản phẩm đã tồn tại, cập nhật Quantity
                                existingCartItem.Quantity = value;
                            }

                            // Lưu trạng thái giỏ hàng vào phiên
                            string cartString = JsonConvert.SerializeObject(cartList);
                            HttpContext.Session.SetString("cart", cartString);
                        }
                    }

                    return Json(new { message = "Cập nhật giỏ hàng thành công" });
                }
                else
                {
                    return Json(new { message = "No data received" });
                }
            }

            // Trả về một giá trị mặc định nếu không nằm trong các trường hợp trên
            return BadRequest();
        }

        [Route("/cart/update-address")]
        [HttpPost]
        public IActionResult UpdateAddress([FromBody] AddressViewModel addressData)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (addressData != null)
                {
                    // Thông tin địa chỉ trong biến addressData
                    string id = "address"; // ID cố định

                    List<AddressViewModel> addressList;

                    // Lấy thông tin địa chỉ từ session nếu đã tồn tại
                    string addressSession = HttpContext.Session.GetString("address");
                    if (addressSession != null)
                    {
                        addressList = JsonConvert.DeserializeObject<List<AddressViewModel>>(addressSession);

                        // Kiểm tra xem đã tồn tại địa chỉ với cùng ID trong danh sách chưa
                        AddressViewModel existingAddress = addressList.FirstOrDefault(a => a.Id == id);

                        if (existingAddress != null)
                        {
                            // Đã tồn tại địa chỉ với ID tương ứng, cập nhật nó
                            existingAddress.City = addressData.City;
                            existingAddress.District = addressData.District;
                            existingAddress.Ward = addressData.Ward;
                            existingAddress.Street = addressData.Street;
                        }
                        else
                        {
                            // Không tìm thấy địa chỉ với ID tương ứng, thêm mới địa chỉ
                            addressList.Add(new AddressViewModel
                            {
                                Id = id,
                                City = addressData.City,
                                District = addressData.District,
                                Ward = addressData.Ward,
                                Street = addressData.Street
                            });
                        }
                    }
                    else
                    {
                        // Nếu session address chưa tồn tại, tạo danh sách mới và thêm địa chỉ vào đó
                        addressList = new List<AddressViewModel>
                {
                    new AddressViewModel
                    {
                        Id = id,
                        City = addressData.City,
                        District = addressData.District,
                        Ward = addressData.Ward,
                        Street = addressData.Street
                    }
                };
                    }

                    // Lưu thông tin địa chỉ vào phiên
                    string updatedAddressSession = JsonConvert.SerializeObject(addressList);
                    HttpContext.Session.SetString("address", updatedAddressSession);

                    return Json(new { message = "Address updated successfully" });
                }
                else
                {
                    return Json(new { message = "No data received" });
                }
            }

            // Trả về một giá trị mặc định nếu không nằm trong các trường hợp trên
            return BadRequest();
        }



    }
}
