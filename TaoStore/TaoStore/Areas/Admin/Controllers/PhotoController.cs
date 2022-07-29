using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Areas.Admin.Controllers
{
    public class PhotoController : Controller
    {
        // GET: Admin/Photo
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost] 
        public ActionResult Add(FormCollection form)
        {
            string photo1 = form["ImageFile"].ToString();
            var i = 1;
            return View();

        }
    }
}