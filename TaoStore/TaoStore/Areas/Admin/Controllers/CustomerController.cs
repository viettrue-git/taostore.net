using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Admin/Customer
        public ActionResult Index(int? page)
        {
            int pagenumber = (page ?? 1);
            int pageSize = 4;
            CustomerModel brand = new CustomerModel();
            var model = brand.AllCustomers(pagenumber, pageSize);
            return View(model);
        }
    }
}