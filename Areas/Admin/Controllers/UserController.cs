using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using BTL_QuanLyBanDienThoai.Models.Authentication;
using BTL_QuanLyBanDienThoai.Models.ViewModel;
using BTL_QuanLyBanDienThoai.Utils;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Users")]
    public class UserController : Controller
    {
        private readonly QLBanDienThoaiContext db;
        Password password = new Password();
        public UserController(QLBanDienThoaiContext _db)
        {
            db = _db;
        }
        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Role") != "1")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid && (HttpContext.Session.GetString("Role") == null))
            {
                var checkUser = db.Users.Where(x => x.Email.Equals(user.Email)).FirstOrDefault();

                if(checkUser.Role == 2)
                {
                    return View();
                }

                if(checkUser == null)
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
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        [Authorization]
        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Role");
            return View();
        }

        [Authorization]
        [Route("")]
        public IActionResult Index(int page = 1)
        {
            List<User> users = db.Users.ToList();

            int pageSize = 8;

            IPagedList<User> pagedList = users.ToPagedList(page, pageSize);

            return View(pagedList);
        }

        [Authorization]
        [Route("Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorization]
        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(User user, int id)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                var existUser = db.Users.Find(id);
                if (existUser == null)
                {
                    ModelState.AddModelError("Email", "Email has already exist.");
                    return View();
                }
                var checkDuplicateEmail = db.Users.Where(x => x.Email.Equals(existUser.Email)).FirstOrDefault();
                if (checkDuplicateEmail == null)
                {
                    ModelState.AddModelError("Email", "Email has already exist.");
                    return View();
                }
                if(existUser.Password != user.Password)
                {

                }

                if (user.Password != null)
                {
                    existUser.Password = password.HashPassword(user.Password);
                }

                db.Users.Update(existUser);
                db.SaveChanges();

                return RedirectToAction("Index", "User");
            }
            return View();
        }


        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [Route("Create")]
        [HttpPost]
        public IActionResult Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var existUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existUser != null)
                {
                    ModelState.AddModelError("Email", "Email has already exist.");
                    return View(user);
                }

                if(user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError("Password", "Password not equal confirm password.");
                    return View(user);
                }
                db.Users.Add(new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = int.Parse(Request.Form["role"]),
                    Password = password.HashPassword(user.Password),
                });
                db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            return View(user);
        }

        [Authorization]
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dbUser = db.Users.FirstOrDefault(x => x.Id == id);
            if (dbUser != null)
            {
                try
                {
                    db.Users.Remove(dbUser);

                    var ordersToBeUpdated = db.Orders.Where(o => o.UserId == id);

                    foreach (var order in ordersToBeUpdated)
                    {
                        order.UserId = null; 
                    }
                    db.SaveChanges();

                    return Json(new { success = true, message = "done" });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return BadRequest(JsonConvert.SerializeObject(
                        new
                        {
                            error = "Can not delete."
                        }
                    ));
                }
            }
            return BadRequest(JsonConvert.SerializeObject(
                new
                {
                    error = "Can not delete this user."
                }
            ));
        }

        [Authorization]
        [Route("Logout")]
        [HttpPost]
        public IActionResult Logout(User user)
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Role");
            return RedirectToAction("Login", "Users");
        }

    }
}