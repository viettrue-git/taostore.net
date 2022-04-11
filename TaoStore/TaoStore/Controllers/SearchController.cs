using Models;
using Models.Dao;
using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResultSearch(FormCollection form,int pg=1)
        {
            string key = form["txtTimKiem"].ToString();
            ViewBag.Key = key;
            var list = new ProductDao().SearchProduct(key);
            if (list.Count() <= 0)
            {
                ViewBag.Mes = "Not found any product!";
                const int pageSize = 8;
                var products = new ProductDao().ListProductNew();
                if (pg < 1)
                    pg = 1;
                int resCount = products.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var model = products.Skip(resSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                return View(model);
            }
            else
            {
                ViewBag.Mes = "Some result!";
                const int pageSize = 8;
                if (pg < 1)
                    pg = 1;
                int resCount = list.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var model = list.Skip(resSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult ResultSearch(string key,int pg=1)
        {
            ViewBag.Key = key;
            var list = new ProductDao().SearchProduct(key);
            if (list.Count() <= 0)
            {
                ViewBag.Mes = "Not found any product!";
                const int pageSize = 8;
                var products = new ProductDao().ListProductNew();
                if (pg < 1)
                    pg = 1;
                int resCount = products.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var model = products.Skip(resSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                return View(model);
            }
            else
            {
                ViewBag.Mes = "Some result!";
                const int pageSize = 8;
                if (pg < 1)
                    pg = 1;
                int resCount = list.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var model = list.Skip(resSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                return View(model);
            }
        }
    }
}