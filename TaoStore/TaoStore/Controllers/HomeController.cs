using Models.Dao;
using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// GET PRODUCTS MOST NEW
        /// </summary>
        /// <returns>view with list products</returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            List<Product> model = new ProductDao().ListProductNew();
            return View(model);
        }
        /// <summary>
        /// get product best seller 
        /// </summary>
        /// <returns></returns>
        public ActionResult BestSale()
        {
            List<Product> list = new ProductDao().ListProductBestSell();
            return PartialView(list);
        }

        public ActionResult ParialMenu()
        {
            this.ViewBag.listCat = new CategoryDao().GetAll();
            this.ViewBag.listBrand = new BrandDao().GetAll();
            return PartialView();
        }
    }
}
