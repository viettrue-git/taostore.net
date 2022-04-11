using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        protected override void OnActionExecuting(ActionExecutingContext actionExecuting)
        {
            var session = (Acount)Session["loginSession"];
            if (session == null)
            {
                actionExecuting.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                {  controller="Login",action="Index"
                }));
            }
            base.OnActionExecuting(actionExecuting);
        }
    }
}          