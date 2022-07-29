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
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index(int? page)
        {
            int pagenumber = (page ?? 1);
            int pageSize = 4;
            var iplProduct = new ProductModel();
            var model = iplProduct.GetAllProducts(pagenumber, pageSize);
            return View(model);
        }
        // Add photo 
        public ActionResult CreatePhotos()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            OnlineShopDBContext context = new OnlineShopDBContext();
            ViewBag.CategoryId = new SelectList(context.Categories.ToList().OrderBy(n => n.CategoryId), "CategoryId", "CategoryName");
            ViewBag.BrandId = new SelectList(context.Brands.ToList().OrderBy(n => n.BrandId), "BrandId", "BrandName");
           // ViewBag.SizeId = new SelectList(context.Sizes.ToList().OrderBy(n => n.SizeId), "SizeId", "size1"+"-"+"size2");
            ViewBag.SizeId  = context.Sizes.Select(f => new SelectListItem()
            {
                Text = f.size1+"-"+f.size2,
                Value = f.SizeId.ToString()
            }).ToList();
            return View();
        }
        //[Bind(Include = "ProductName,ProductPrice,DisCount,Note,Status,ImageFile1,ImageFile2,ImageFile3,BrandId,CategoryId,size1,size2")] Product product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FormCollection form,List<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                string productName = form["ProductName"].ToString();
                string note = form["Note"].ToString();
                int price = Int32.Parse(form["ProductPrice"].ToString());
                int discount = Int32.Parse(form["DisCount"].ToString());
                int brandId = Int32.Parse(form["BrandId"].ToString());
                int categoryId = Int32.Parse(form["CategoryId"].ToString());
                int sizeId = Int32.Parse(form["SizeId"].ToString());
                bool status = Convert.ToBoolean(form["Status"].ToString());
                int quantity = Int32.Parse(form["Quantity"].ToString());
                // validate 
                // input images
                var path = "";
                Photo photo = new Photo();
                List<string> arr = new List<string>();
                int i = 0;
                foreach(var item in files)
                {
                    if(item!=null && item.ContentLength > 0)
                    {
                        if(Path.GetExtension(item.FileName).ToLower()==".jpg"|| Path.GetExtension(item.FileName).ToLower() == ".png" || Path.GetExtension(item.FileName).ToLower() == ".jpeg" || Path.GetExtension(item.FileName).ToLower() == ".gif")
                        {
                            string name = Path.GetFileNameWithoutExtension(item.FileName);
                            string ex = Path.GetExtension(item.FileName);
                            name = name +DateTime.Now.Millisecond.ToString() + ex;
                            arr.Add("~/Asset/Image/Product/" + name);
                            path = Path.Combine(Server.MapPath("~/Asset/Image/Product"),name);
                            item.SaveAs(path);
                            i++;
                        }
                    }
                }
                for(int j=0; j<arr.Count;j++)
                {
                    if (arr[j] == "" || arr[j] == null)
                    {
                        arr.Add("~/Asset/Image/Product/not-avaiable.jpg");
                    }
                }
                photo.Photo1 = arr[0];
                photo.Photo2 = arr[1];
                photo.Photo3 = arr[2];
                OnlineShopDBContext context = new OnlineShopDBContext();
                context.Photos.Add(photo);
                context.SaveChanges();
                Photo photonew = context.Photos.OrderByDescending(x => x.PhotoId).FirstOrDefault();
                var photoId = photonew.PhotoId;
                // get photo_id of photo_updatenew
                Product product = new Product();
                // validate 
                product.ProductName = productName;
                product.ProductPrice = price;
                product.Discount = discount;
                product.Status = status;
                product.SizeId = sizeId;
                product.PhotoId =photoId;
                product.Note = note;
                product.CategoryId = categoryId;
                product.BrandId = brandId;
                product.Quantity = quantity;
                context.Products.Add(product);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                OnlineShopDBContext context = new OnlineShopDBContext();
                ViewBag.CategoryId = new SelectList(context.Categories.ToList().OrderBy(n => n.CategoryId), "CategoryId", "CategoryName");
                ViewBag.BrandId = new SelectList(context.Brands.ToList().OrderBy(n => n.BrandId), "BrandId", "BrandName");
                ViewBag.SizeId = context.Sizes.Select(f => new SelectListItem()
                {
                    Text = f.size1 + "-" + f.size2,
                    Value = f.SizeId.ToString()
                }).ToList();
                ViewBag.Product = false;
                ViewBag.Mes = "Add new product fail!!";
                return View();
            }
        }
        
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ProductModel productModel = new ProductModel();
            var model = productModel.Find(id);
            if (model == null)
            {
                return View();
            }
            else
            {
                OnlineShopDBContext context = new OnlineShopDBContext();
                ViewBag.CategoryId = new SelectList(context.Categories.ToList().OrderBy(n => n.CategoryId), "CategoryId", "CategoryName");
                ViewBag.BrandId = new SelectList(context.Brands.ToList().OrderBy(n => n.BrandId), "BrandId", "BrandName");
                // ViewBag.SizeId = new SelectList(context.Sizes.ToList().OrderBy(n => n.SizeId), "SizeId", "size1"+"-"+"size2");
                ViewBag.SizeId = context.Sizes.Select(f => new SelectListItem()
                {
                    Text = f.size1 + "-" + f.size2,
                    Value = f.SizeId.ToString()
                }).ToList();
                return View(model);

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(FormCollection form, List<HttpPostedFileBase> files)
        {
            OnlineShopDBContext context = new OnlineShopDBContext();
            int idProduct = Int32.Parse(form["ProductId"].ToString());
            var product = context.Products.Find(idProduct);
            if (ModelState.IsValid)
            {
                string productName = form["ProductName"].ToString();
                string note = form["Note"].ToString();
                int price = Int32.Parse(form["ProductPrice"].ToString());
                int discount = Int32.Parse(form["DisCount"].ToString());
                int brandId = Int32.Parse(form["BrandId"].ToString());
                int categoryId = Int32.Parse(form["CategoryId"].ToString());
                int sizeId = Int32.Parse(form["SizeId"].ToString());
                bool status = Convert.ToBoolean(form["Status"].ToString());
                int quantity = Int32.Parse(form["Quantity"].ToString());
                int photoId = Int32.Parse(form["PhotoId"].ToString());

                // validate 
                // input images
                var path = "";
                Photo photo = context.Photos.Find(photoId);
                List<string> arr = new List<string>();
                int i = 0;
                foreach (var item in files)
                {
                    if (item != null && item.ContentLength > 0)
                    {
                        if (Path.GetExtension(item.FileName).ToLower() == ".jpg" || Path.GetExtension(item.FileName).ToLower() == ".png" || Path.GetExtension(item.FileName).ToLower() == ".jpeg" || Path.GetExtension(item.FileName).ToLower() == ".gif")
                        {
                            string name = Path.GetFileNameWithoutExtension(item.FileName);
                            string ex = Path.GetExtension(item.FileName);
                            name = name + DateTime.Now.Millisecond.ToString() + ex;
                            arr.Add("~/Asset/Image/Product/" + name);
                            path = Path.Combine(Server.MapPath("~/Asset/Image/Product"), name);
                            item.SaveAs(path);
                            i++;
                        }
                    }
                }
                for (int j = 0; j < arr.Count; j++)
                {
                    if (arr[j] == "" || arr[j] == null)
                    {
                        arr.Add("~/Asset/Image/Product/not-avaiable.jpg");
                    }
                }
                if (i == 1)
                {
                    photo.Photo1 = arr[0];
                }
                if (i == 2)
                {
                    photo.Photo1 = arr[0];
                    photo.Photo2 = arr[1];
                }
                if (i >= 3)
                {
                    photo.Photo1 = arr[0];
                    photo.Photo2 = arr[1];
                    photo.Photo3 = arr[2];
                }
                //context.SaveChanges();
                // get photo_id of photo_updatenew
                // validate 
                product.ProductName = productName;
                product.ProductPrice = price;
                product.Discount = discount;
                product.Status = status;
                product.SizeId = sizeId;
                product.PhotoId = photoId;
                product.Note = note;
                product.CategoryId = categoryId;
                product.BrandId = brandId;
                product.Quantity = quantity;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.CategoryId = new SelectList(context.Categories.ToList().OrderBy(n => n.CategoryId), "CategoryId", "CategoryName");
                ViewBag.BrandId = new SelectList(context.Brands.ToList().OrderBy(n => n.BrandId), "BrandId", "BrandName");
                // ViewBag.SizeId = new SelectList(context.Sizes.ToList().OrderBy(n => n.SizeId), "SizeId", "size1"+"-"+"size2");
                ViewBag.SizeId = context.Sizes.Select(f => new SelectListItem()
                {
                    Text = f.size1 + "-" + f.size2,
                    Value = f.SizeId.ToString()
                }).ToList();
                return View(product);

            }
        }
        public ActionResult Delete(int id)
        {
            new ProductModel().Delete(id);
            return Json(new { messanger = "Successfully delete", productId = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult Status(int id)
        {
            ProductModel product = new ProductModel();
            var result = product.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}