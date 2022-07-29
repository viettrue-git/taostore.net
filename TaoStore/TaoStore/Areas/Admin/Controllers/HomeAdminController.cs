using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Areas.Admin.Controllers
{
    [Authorize()]
    [AllowAnonymous]
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChartYear()
        {
            return View();
        }
        // sản phẩm bán theo năm
        public JsonResult ListDataByYear(int year)
        {
            // input year 
            //out put nameproduct and total
            OnlineShopDBContext context = new OnlineShopDBContext();
            var query = from pro in context.Products
                        join ord in context.OrderDetails on pro.ProductId equals ord.ProductId
                        join od in context.Orders on ord.OrderId equals od.OrderId
                        where od.OrderDate.Value.Year == year
                        select new
                        {
                            name = pro.ProductName,
                            num = ord.Quantity,
                            year = od.OrderDate
                        } into gr
                        group gr by new { gr.year.Value.Year, gr.name } into res
                        select new
                        {
                            productname = res.Key.name,
                            quantity = res.Sum(x => x.num)
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        ///<summary>doanh thu và sản phẩm bán theo năm</summary>
        ///<paragram>year</paragram>
        ///<return>json</return>
        public JsonResult ListData(int year)
        {
            OnlineShopDBContext context = new OnlineShopDBContext();
            var query = from or in context.Orders
                        join ord in context.OrderDetails on or.OrderId equals ord.OrderId
                        select new
                        {
                            time = or.OrderDate,
                            quatity = ord.Quantity,
                            price = ord.ProductPrice
                        } into gr
                        group gr by new { gr.time.Value.Year, gr.time.Value.Month} into res
                        select new
                        {
                            year = res.Key.Year,
                            month = res.Key.Month,
                            countproduct = res.Sum(x => x.quatity),
                            total = res.Sum(x => x.quatity * x.price)
                        };
            var result = from ord1 in context.OrderDetails
                         join ord2 in context.OrderDetails on ord1.OrderId equals ord2.OrderId
                         group ord1 by ord1.OrderId into ord
                         select new
                         {
                             quatity = ord.Sum(x => x.Quantity),
                             money = ord.Sum(x => x.Quantity * x.ProductPrice),
                             id = ord.Key
                         } into ordl
                         join od in context.Orders on ordl.id equals od.OrderId
                         where od.OrderDate.Value.Year==year
                         select new
                         {
                             quatity = ordl.quatity,
                             money = ordl.money,
                             time = od.OrderDate
                         } into res 
                         group res by new { res.time.Value.Year, res.time.Value.Month } into resul
                         select new
                         {
                             year = resul.Key.Year,
                             month = resul.Key.Month,
                             countproduct = resul.Sum(x=>x.quatity),
                             total = resul.Sum(x => x.money)
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}