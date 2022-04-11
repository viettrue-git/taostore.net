using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaoStore.Models;

namespace TaoStore.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        private OnlineShopDBContext context = new OnlineShopDBContext();
        // GET: Cart
        public ActionResult Index()
        {
            Acount acount = (Acount)Session["loginSession"];
            if (acount == null)
            {
                List<CartItem> giohang = Session["giohang"] as List<CartItem>;
                if (giohang !=null)
                {
                    int total = giohang.Select(x => x.Total).Sum();
                    ViewBag.Total = total;
                }

                return View(giohang);
            }
            else
            {
                OnlineShopDBContext context = new OnlineShopDBContext();
                var customer = context.Customers.Where(x => x.Email == acount.Email).FirstOrDefault();
                var cart = context.Carts.Where(x => x.CustomerId == customer.CustomerId && x.StatusOrder == 0).FirstOrDefault();
                // check cart null 
                if (cart == null)
                {
                    return View();
                }
                var idCart = cart.CartId;
                List<CartDetail> cartDetails = context.CartDetails.Where(x => x.CartId == idCart && x.StatusCart == 0).ToList();
                List<CartItem> giohang = new List<CartItem>();
                foreach (var item in cartDetails)
                {
                     
                    CartItem cartItem = new CartItem
                    {
                        product = item.Product,
                        Size = item.Size,
                        Quantity = (int)item.Quantity
                    };
                    giohang.Add(cartItem);
                }
                if (giohang.Count>0)
                {
                    int total = giohang.Select(x => x.Total).Sum();
                    ViewBag.Total = total;
                }
                Session["giohang"] = giohang;
                return View(giohang);
            }
        }
        [HttpPost]
        public ActionResult AddCart(int productId, FormCollection form)
        {
            var quantity = Int32.Parse(form["quantity"].ToString());
            var size = Int32.Parse(form["size"].ToString());
            if (quantity == 0)
            {
                ViewBag.MesCart = "The number of product must more than 1";
            }
            if (size == 0)
            {
                ViewBag.MesCart = "Pleas choose size of product";
            }
            Acount acount = (Acount)Session["loginSession"];
            if (acount == null)
            {
                // check request
                // chưa login 
                if (Session["giohang"] == null) // Nếu giỏ hàng chưa được khởi tạo
                {
                    Session["giohang"] = new List<CartItem>();  // Khởi tạo Session["giohang"] là 1 List<CartItem>
                }

                List<CartItem> giohang = Session["giohang"] as List<CartItem>;  // Gán qua biến giohang dễ code

                // Kiểm tra xem sản phẩm khách đang chọn đã có trong giỏ hàng chưa

                if (giohang.FirstOrDefault(m => m.product.ProductId == productId) == null) // ko co sp nay trong gio hang
                {
                    Product sp = context.Products.Find(productId);  // tim sp theo sanPhamID

                    CartItem newItem = new CartItem()
                    {
                        product = sp,
                        Quantity = quantity,
                        Size = size

                    }; // Tạo ra 1 CartItem mới

                    giohang.Add(newItem);  // Thêm CartItem vào giỏ 
                }
                else
                {
                    // Size san pham da co thi ta tang so luong
                    List<CartItem> list = giohang.Where(m => m.product.ProductId == productId).ToList();
                    foreach (var item in list)
                    {
                        if (item.Size == size)
                        {
                            item.Quantity += quantity;
                            context.SaveChanges();
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                    }
                    // size cua san pham chua co trong gio thi them moi san pham
                    Product sp = context.Products.Find(productId);  // tim sp theo sanPhamID
                    CartItem newItem = new CartItem()
                    {
                        product = sp,
                        Quantity = quantity,
                        Size = size

                    };
                    giohang.Add(newItem);
                    // Tạo ra 1 CartItem mới

                    // Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
                }
            }
            else
            {
                OnlineShopDBContext context = new OnlineShopDBContext();
                var customer = context.Customers.Where(x => x.Email == acount.Email).FirstOrDefault();
                var cart = context.Carts.Where(x => x.CustomerId == customer.CustomerId && x.StatusOrder == 0).FirstOrDefault();
                var product = context.Products.Find(productId);
                // check cart null
                if (cart == null)
                {
                    Cart newCart = new Cart()
                    {
                        CustomerId = customer.CustomerId
                    };
                    context.Carts.Add(newCart);
                    context.SaveChanges();
                    CartDetail cartDetail = new CartDetail()
                    {
                        CartId = newCart.CartId,
                        ProductId = productId,
                        ProductPrice = product.ProductPrice,
                        Size = size,
                        Quantity = quantity,
                        Discount = product.Discount,
                        StatusCart = 0
                    };
                    context.CartDetails.Add(cartDetail);
                    context.SaveChanges();
                }
                else
                {
                    List<CartDetail> cartDetails = context.CartDetails.Where(x => x.CartId == cart.CartId && x.StatusCart == 0).ToList();
                    foreach(var item in cartDetails)
                    {
                        if(item.ProductId==productId&&item.Size==size)
                        {
                            item.Quantity += quantity;
                            context.SaveChanges();
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                    }
                    CartDetail cartDetail = new CartDetail()
                    {
                        CartId = cart.CartId,
                        ProductId = productId,
                        Quantity = quantity,
                        Size = size,
                        Discount = product.Discount,
                        ProductPrice = product.ProductPrice,
                        StatusCart = 0
                    };
                    context.CartDetails.Add(cartDetail);
                    context.SaveChanges();
                }
            }

            // Action này sẽ chuyển hướng về trang chi tiết sp khi khách hàng đặt vào giỏ thành công. Bạn có thể chuyển về chính trang khách hàng vừa đứng bằng lệnh return Redirect(Request.UrlReferrer.ToString()); nếu muốn.
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult EditCart(int productId,int quantity,int size)
        {
            //quantity="$('#slm').val()",
            //edit in session
            if (quantity < 1 || quantity > 100||size==0)
            {
                ViewBag.Flag = false;
                ViewBag.MesCart = "Quantity or size of product not valiable!";
                return RedirectToAction("Index");
            }
            if (Session["loginSession"] == null)
            {
                List<CartItem> giohang = Session["giohang"] as List<CartItem>;
                var cartItem = giohang.Where(x => x.product.ProductId == productId && x.Size == size).FirstOrDefault();
                if (cartItem != null)
                {
                        cartItem.Quantity = quantity;
                }
            }
            else
            {
                Acount acount = (Acount)Session["loginSession"];
                //var customer = context.Customers.Where(x => x.Email == acount.Email).FirstOrDefault();
                //var cart = context.Carts.Where(x => x.CustomerId == customer.CustomerId).FirstOrDefault();
                //List<CartDetail> carts = context.CartDetails.Where(x => x.CartId == cart.CartId && x.StatusCart == 0).ToList();
                List<CartDetail> cartDetails = context.CartDetails.Where(x => x.Cart.Customer.Email == acount.Email&&x.StatusCart==0).ToList();
                foreach(var item in cartDetails)
                {
                    if (item.ProductId == productId && item.Size == size)
                    {
                        item.Quantity = quantity;
                        context.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Delete(int id,int size)
        {
            if (Session["loginSession"] == null)
            {
                List<CartItem> giohang = Session["giohang"] as List<CartItem>;
                var cartItem = giohang.Where(x => x.product.ProductId == id && x.Size == size).FirstOrDefault();
                if (cartItem != null)
                {
                    giohang.Remove(cartItem);
                }
                Session["giohang"] = giohang;
            }
            else
            {
                Acount acount = (Acount)Session["loginSession"];
                //List<CartDetail> cartDetails = context.CartDetails.Where(x => x.Cart.Customer.Email == acount.Email && x.StatusCart == 0).ToList();
                var customer = context.Customers.Where(x => x.Email == acount.Email).FirstOrDefault();
                    var cart = context.Carts.Where(x => x.CustomerId == customer.CustomerId).FirstOrDefault();
                List<CartDetail> cartDetails = context.CartDetails.Where(x => x.CartId == cart.CartId).ToList();
                foreach (var item in cartDetails)
                {
                    if (item.ProductId == id && item.Size == size)
                    {
                        context.CartDetails.Remove(item);
                        context.SaveChanges();
                        break;
                    }
                }
            }
            return Json(new
            {
                status = true
            });
        }
    }
}