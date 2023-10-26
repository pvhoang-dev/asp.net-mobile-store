using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BTL_QuanLyBanDienThoai.Models.Authentication
{
    public class Authentication: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.HttpContext.Session.GetString("Role") != "1")
            {
                context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"Controller", "Home"},
                            {"Action", "Index" }
                        }
                    );
            }
        }
    }
}
