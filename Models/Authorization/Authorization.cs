using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BTL_QuanLyBanDienThoai.Models.Authorization
{
    public class Authorization: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.HttpContext.Session.GetString("Id") == null)
            {
                context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"Controller", "Auth"},
                            {"Action", "Login" }
                        }
                    );
            }
        }
    }
}
