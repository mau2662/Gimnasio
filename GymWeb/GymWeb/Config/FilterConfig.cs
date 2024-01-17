using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GymWeb.Config
{
    public class FilterConfig : ActionFilterAttribute
    {

        /// <summary>
        /// Saca al usuario del sistema si se ha vencido su informacion de usuario o si no se ha logueado.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("userInfo") == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "Controller", "Login" },
                        { "Action", "Login" }
                    });
            }

            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("userInfo") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "Controller", "Login" },
                        { "Action", "Login" }
                    });
            }
            base.OnActionExecuting(context);
        }

    }
}
