using Microsoft.AspNetCore.Mvc;
using BTL_QuanLyBanDienThoai.Models;
using BTL_QuanLyBanDienThoai.Data;
namespace BTL_QuanLyBanDienThoai.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly QLBanDienThoaiContext _db;

        public CategoryListViewComponent(QLBanDienThoaiContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _db.Categories.ToList();

            return View(categories);
        }
    }
}
