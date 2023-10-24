using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using BTL_QuanLyBanDienThoai.Models.Authentication;


namespace BTL_QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Users")]
    public class UserController : Controller
    {
        private readonly QLBanDienThoaiContext db;
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
            if (HttpContext.Session.GetString("Role") == null)
            {
                var checkUser = db.Users.Where(x => x.Email.Equals(user.Email) && x.Password.Equals(user.Password)).FirstOrDefault();
                if (checkUser != null)
                {
                    HttpContext.Session.SetString("Role", checkUser.Role.ToString());
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Role");
            return View();
        }

        [Route("Logout")]
        [HttpPost]
        public IActionResult Logout(User user)
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Role");
            return RedirectToAction("Login", "Users");
        }


        [Authentication]
        [Route("")]
        public IActionResult Index()
        {
            List<User> users = db.Users.ToList();
            return View(users);
        }



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


        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Update(user);
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
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = int.Parse(Request.Form["role"]),
                    Password = user.Password,

                });
                db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dbUser = db.Users.FirstOrDefault(x => x.Id == id);
            if (dbUser != null)
            {
                db.Users.Remove(dbUser);
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
