using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Utils;
using BTL_QuanLyBanDienThoai.Models.ViewModel;

namespace BTL_QuanLyBanDienThoai.Controllers
{
    public class AuthController : Controller
    {
        private readonly QLBanDienThoaiContext db;
        Password password = new Password();

        public AuthController(QLBanDienThoaiContext _db)
        {
            db = _db;
        }

        [Route("login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid && HttpContext.Session.GetString("Role") == null)
            {
                var checkUser = db.Users.Where(x => x.Email.Equals(user.Email)).FirstOrDefault();
                if (checkUser == null)
                {
                    ModelState.AddModelError("Password", "User does not exist, please re-enter.");
                    return View();
                }
                if (!password.VerifyPassword(user.Password, checkUser.Password))
                {
                    ModelState.AddModelError("Password", "User does not exist, please re-enter.");
                    return View();
                }
                if (checkUser != null)
                {
                    HttpContext.Session.SetString("Role", checkUser.Role.ToString());
                    HttpContext.Session.SetString("Name", checkUser.Name.ToString());
                    HttpContext.Session.SetString("Email", checkUser.Email.ToString());
                    HttpContext.Session.SetString("Id", checkUser.Id.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(UserViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                var checkUser = db.Users.Where(x => x.Email.Equals(userViewModel.Email)).FirstOrDefault();
                if (checkUser != null)
                {
                    ModelState.AddModelError("Email", "Email has already exist.");
                    return View(userViewModel);
                }

                if(userViewModel.Password != userViewModel.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password comfirm not equal password.");
                    return View(userViewModel);
                }

                db.Users.Add(new User
                {
                    Name = userViewModel.Name,
                    Email = userViewModel.Email,
                    Role = 2,
                    Password = password.HashPassword(userViewModel.Password),
                });
                db.SaveChanges();
                ViewBag.Message = "Create User Successful";
                ViewBag.Text = "success";
                return RedirectToAction("Login", "Auth");
            }
            return View(userViewModel);
        }

        [Route("logout")]
        [HttpPost]
        public IActionResult Logout(User user)
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("Name");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Id");

            return RedirectToAction("Login", "Auth");
        }
    }
}
