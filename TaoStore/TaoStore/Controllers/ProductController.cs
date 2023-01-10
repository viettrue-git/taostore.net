using Models;
using Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int pg=1)
        {
            const int pageSize = 8;
            var products = new ProductDao().ListProductNew();
            if (pg < 1)
                pg = 1;
            int resCount = products.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
           // skip la bo qua so phan tu trc do va lay ra cac phan tu con lai
           // trang 2 se bo qua so phan tu o trang so 1
            var model = products.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.isShow = true;
            return View(model);
        }
        public ActionResult ProductDetail(int id)
        {
            var product = new ProductDao().FindProductById(id);
            this.ViewBag.list = new ProductDao().FetureCat(id);
            //this.ViewBag.listSize= 
            return View(product);
        }
        public ActionResult Header()
        {
            this.ViewBag.listCat = new CategoryDao().GetAll();
            this.ViewBag.listBrand = new BrandDao().GetAll();
            return PartialView();
        }
        public ActionResult ProductByCat(int id,int pg=1)
        {
            const int pageSize = 8;
            var products = new ProductDao().ProductByCat(id);
            int resCount = products.Count();
            if (resCount == 0)
            {
                ViewBag.Mes = "We are so sorry! There are currently no matching products.Here are some other options!";
            }
            if (pg<1)
            {
                pg = 1;
            }
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var model = products.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.idCat = id;
            return View(model);
        }
        public ActionResult ProductByBrand(int id, int pg = 1)
        {
            const int pageSize = 8;
            var products = new ProductDao().ProductByBrand(id);
            this.ViewBag.idBrand = id;
            int resCount = products.Count();
            if (resCount == 0)
            {
                ViewBag.Mes = "We are so sorry! There are currently no matching products.Here are some other options!";
            }
            if (pg < 1)
            {
                pg = 1;
            }
            var pager = new Pager(resCount, pg, pageSize); // trang hien tai
            int resSkip = (pg - 1) * pageSize; // so san pham truoc do
            var model = products.Skip(resSkip).Take(pager.PageSize).ToList(); // so san pham trang hien tai
            this.ViewBag.Pager = pager;
            return View(model);
        }
        public ActionResult ProductByDiscount(int discount, int pg=1)
        {
            const int pageSize = 8;
            var products = new ProductDao().ProductByDiscount(discount);
            int resCount = products.Count();
            if (resCount == 0)
            {
                ViewBag.Mes = "We are so sorry! There are currently no matching products.Here are some other options!";
            }
            if (pg < 1)
            {
                pg = 1;
            }
            var pager = new Pager(resCount, pg, pageSize); // trang hien tai
            int resSkip = (pg - 1) * pageSize; // so san pham truoc do
            var model = products.Skip(resSkip).Take(pager.PageSize).ToList(); // so san pham trang hien tai
            this.ViewBag.Pager = pager;
            this.ViewBag.Discount = discount;
            return View(model);
        }
    }
}