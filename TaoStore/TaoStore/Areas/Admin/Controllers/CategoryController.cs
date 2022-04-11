using Models;
using Models.Famework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace TaoStore.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index(int ? page)
        {
            int pagenumber = (page??1);
            int pageSize = 4;
            var iplCategory = new CategoryModel();
            var model = iplCategory.ListAllCategories(pagenumber,pageSize);
            return View(model);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]//đẩy dữ liệu và sever
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include="CategoryName,Note,Status,ImageFile")] Category category)
        {
           
            if (ModelState.IsValid)
            {
                OnlineShopDBContext context = new OnlineShopDBContext();
                Category model = context.Categories.Where(x => x.CategoryName == category.CategoryName).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.Category = false;
                    ViewBag.Mes = "Tên loại sản phẩm đã tồn tại";
                    return View(category);
                }
                if (category.ImageFile == null)
                {
                    ViewBag.Category = false;
                    ViewBag.Mes += "Vui long chon logo";
                    return View(category);
                }
                string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                string extention = Path.GetExtension(category.ImageFile.FileName);
                fileName = fileName + extention;
                category.CategoryLogo = "~/Asset/Image/Category/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Asset/Image/Category/"), fileName);
                category.ImageFile.SaveAs(fileName);
                context.Categories.Add(category);
                context.SaveChanges();
                //db.tDanhMucSP.Add(sanpham);
                //db.SaveChanges();
                ViewBag.Category = true;
                ViewBag.Mes = "Add successfully!!!";
                return RedirectToAction("Index");
            }
            ViewBag.Category =false;
            ViewBag.Mes = "Thêm mới thất bại";
            return View(category);
        }
        public ActionResult EditCategory(int id)
        {
            CategoryModel categoryModel = new CategoryModel();
            var model = categoryModel.Find(id);
            if (model == null)
            {
                return View();
            }
            else
            return View(model);
        }
        [HttpPost]//đẩy dữ liệu và sever
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "CategoryId,CategoryName,Note,Status,ImageFile,CategoryLogo")] Category category)
        {

            if (ModelState.IsValid)
            {
                var logo = category.CategoryLogo;
                OnlineShopDBContext context = new OnlineShopDBContext();
                Category model = context.Categories.Find(category.CategoryId);
                // nếu chọn ảnh thì sẽ chọn lại đường dẫn
                if (category.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                    string extention = Path.GetExtension(category.ImageFile.FileName);
                    fileName = fileName + extention;
                    category.CategoryLogo = "~/Asset/Image/Category/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Asset/Image/Category/"), fileName);
                    category.ImageFile.SaveAs(fileName);
                }
                // nếu ko chọn ảnh thì giá trị được hiện tại là null
                model.CategoryName = category.CategoryName;
                model.Note = category.Note;
                model.CategoryLogo = category.CategoryLogo;
                model.ImageFile = category.ImageFile;
                model.Status = category.Status;
                context.SaveChanges();
                ViewBag.Category = true;
                ViewBag.Mes = "Edit successfully!!!";
                return RedirectToAction("Index");
            }
            ViewBag.Category = false;
            ViewBag.Mes = "Update mới thất bại";
            return View(category);
        }
        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Delete(int id)
        {
            new CategoryModel().Delete(id);
            return Json(new { messanger = "Successfully delete",categoryID=id }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// change status then click btn status
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JosonResult true=kích hoạt and false=khóa</returns>
        [HttpPut]
        public JsonResult Status(int id)
        {
            CategoryModel category = new CategoryModel();
            var result = category.ChangeStastus(id);
            return Json(new
            {
                status = result
            });
        }
        

    }
}