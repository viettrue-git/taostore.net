using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Areas.Admin.Controllers
{
   // [Authorize]
  //  [AllowAnonymous]
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}