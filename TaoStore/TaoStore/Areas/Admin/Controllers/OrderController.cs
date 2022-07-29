using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            int pagenumber = (page ?? 1);
            int pageSize = 4;
            var iplOrder = new OrderModel();
            var model = iplOrder.ListOrder(pagenumber, pageSize);
            return View(model);
        }
        [HttpPut]
        public JsonResult ChangeStatus(int id)
        {
            OrderModel orderModel = new OrderModel();
            int result=orderModel.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public ActionResult OrderWait(int? page)
        {
            int pagenumber = (page ?? 1);
            int pageSize = 4;
            var iplOrder = new OrderModel();
            var model = iplOrder.ListOrderWait(pagenumber, pageSize);
            return View(model);
        }
        public ActionResult OrderLoad(int? page)
        {
            int pagenumber = (page ?? 1);
            int pageSize = 4;
            var iplOrder = new OrderModel();
            var model = iplOrder.ListOrderLoad(pagenumber, pageSize);
            return View(model);
        }
        public ActionResult OrderSuccess(int? page)
        {
            int pagenumber = (page ?? 1);
            int pageSize = 4;
            var iplOrder = new OrderModel();
            var model = iplOrder.ListOrderSuccess(pagenumber, pageSize);
            return View(model);
        }
    }
}