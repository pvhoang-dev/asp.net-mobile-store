using Slugify;

namespace BTL_QuanLyBanDienThoai.Utils
{
    public class Slug
    {
         public string Create(string name)
        {
            SlugHelper helper = new SlugHelper();
            string slug = helper.GenerateSlug(name);
            return slug;
        }
    }
}
