using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Controllers
{
    public class PaymentController : Controller
    {
        // GET: PayPal
        public ActionResult Index()
        {
            return View();
        }
    }
}