using Models;
using Models.Famework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaoStore.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        // GET: Admin/Brand
        /// <summary>
        /// get all brand
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int ? page)
        {
            int pagenumber = (page ?? 1);
            int pageSize = 4;
            BrandModel brand = new BrandModel();
            var model = brand.AllBrands(pagenumber, pageSize);
            return View(model);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "BrandId,BrandName,Note,Status,ImageFile")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                BrandModel brandModel = new BrandModel();
                OnlineShopDBContext context = new OnlineShopDBContext();
                if (brandModel.FindBrandByName(brand.BrandName))
                {
                    ViewBag.Brand = false;
                    ViewBag.Mes = "Tên thương hiệu đã tồn tại!";
                    return View(brand);
                }
                if (brand.ImageFile == null)
                {
                    ViewBag.Brand = false;
                    ViewBag.Mes = "Vui lòng chọn file!";
                    return View(brand);
                }
                string fileName = Path.GetFileNameWithoutExtension(brand.ImageFile.FileName);
                string extention = Path.GetExtension(brand.ImageFile.FileName);
                fileName = fileName + extention;
                brand.BrandLogo = "~/Asset/Image/Brand/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Asset/Image/Brand/"), fileName);
                brand.ImageFile.SaveAs(fileName);
                context.Brands.Add(brand);
                context.SaveChanges();
                ViewBag.Category = true;
                ViewBag.Mes = "Add successfully!!!";
                return RedirectToAction("Index");
            }
            ViewBag.Brand = false;
            ViewBag.Mes = "Thêm mới thất bại";
            return View(brand);
        }
        /// <summary>
        /// view edit model brand
        /// </summary>
        /// <param name="id"></param>
        /// <returns>view model</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Brand brand = new BrandModel().FindBrandById(id);
            if (brand == null)
            {
                return View();
            }
            else return View(brand);
        }
        [HttpPost]//đẩy dữ liệu và sever
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BrandId,BrandName,Note,Status,ImageFile,BrandLogo")] Brand brand)
        {

            if (ModelState.IsValid)
            {
                OnlineShopDBContext context = new OnlineShopDBContext();
                Brand model = context.Brands.Find(brand.BrandId);
                // nếu chọn ảnh thì sẽ chọn lại đường dẫn
                if (brand.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(brand.ImageFile.FileName);
                    string extention = Path.GetExtension(brand.ImageFile.FileName);
                    fileName = fileName + extention;
                    brand.BrandLogo = "~/Asset/Image/Category/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Asset/Image/Category/"), fileName);
                    brand.ImageFile.SaveAs(fileName);
                }
                // nếu ko chọn ảnh thì giá trị vẫn được giữ nguyên
                model.BrandName = brand.BrandName;
                model.Note = brand.Note;
                model.BrandLogo = brand.BrandLogo;
                model.ImageFile = brand.ImageFile;
                model.Status = brand.Status;
                context.SaveChanges();
                ViewBag.Brand = true;
                ViewBag.Mes = "Edit successfully!!!";
                return RedirectToAction("Index");
            }
            ViewBag.Category = false;
            ViewBag.Mes = "Update mới thất bại";
            return View(brand);
        }
        /// <summary>
        /// remove brand
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        public ActionResult Delete(int id)
        {
            new BrandModel().Delete(id);
            return Json(new { messanger = "Successfully delete", brandID = id }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// change status of brand
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        [HttpPut]
        public JsonResult Status(int id)
        {
            BrandModel brandModel = new BrandModel();
            bool result = brandModel.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

    }
}