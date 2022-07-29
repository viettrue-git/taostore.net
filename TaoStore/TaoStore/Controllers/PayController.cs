using Models.Famework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaoStore.Code;
using TaoStore.Models;

namespace TaoStore.Controllers
{
    public class PayController : Controller
    {
        // GET: Pay
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                OnlineShopDBContext context = new OnlineShopDBContext();
                string name = form["name"].ToString();
                string email = form["email"].ToString();
                string adress = form["adress"].ToString();
                string phone = form["phone"].ToString();
                string note = form["note"].ToString();
                int total=0;
                var check = new CheckPay().checkInfor(name, email, phone, adress);
                if (check==false)
                {
                    ViewBag.Flag = false;
                    ViewBag.MesPay = "Please check your informaition again!";
                    return View();
                }
                // check and add customer
                // add order and orderdetail
                // send email
                Acount acount = (Acount)Session["loginSession"];
                List<CartItem> cartItems = Session["giohang"] as List<CartItem>;
                if (cartItems.Count <= 0)
                {
                    ViewBag.Flag = false;
                    ViewBag.MesPay = "Your order not success!";
                    return View();
                }
                Order order = new Order();
                Customer customer = new Customer();
                if (acount == null)
                {
                    // neu khach hang da ton tai thi khong them moi con neu chua thi add new customer
                    customer = context.Customers.Where(x => x.Email == email).FirstOrDefault();
                    if (customer == null)
                    {
                        Customer newCustomer = new Customer()
                        {
                            Email=email,
                            PhoneNumber=Int32.Parse(phone),
                            Adress=adress,
                            Name=name
                        };
                        context.Customers.Add(newCustomer);
                        context.SaveChanges();
                        customer = newCustomer;
                    }
                }
                else
                {
                    customer = context.Customers.Where(x => x.Email == email).FirstOrDefault();
                }
                    order.AddressOrder = adress;
                    order.CustomerId = customer.CustomerId;
                    order.Note = note;
                    order.OrderDate = DateTime.Now;
                    order.StatusOrder = 0;
                    context.Orders.Add(order);
                    context.SaveChanges();
                    foreach (var item in cartItems)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.Size = item.Size;
                        orderDetail.Quantity = item.Quantity;
                        orderDetail.ProductId = item.product.ProductId;
                        orderDetail.Discount = item.product.Discount;
                        orderDetail.ProductPrice = item.product.ProductPrice;
                        orderDetail.OrderId = order.OrderId;
                        total = (item.product.ProductPrice.GetValueOrDefault(0) * item.Quantity);
                        context.OrderDetails.Add(orderDetail);
                        context.SaveChanges();
                    }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/Email.cshtml"));
                ViewBag.Total = total;
                content = content.Replace("{{CustomerName}}", name);
                content = content.Replace("{{Phone}}", phone);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", adress);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(email, "New order by TaoStore", content);
                new MailHelper().SendMail(toEmail, "New order by TaoStore", content);
                Session["giohang"] = null;
                if (acount != null)
                {
                    var cart = context.Carts.Where(x => x.CustomerId == customer.CustomerId).FirstOrDefault();
                    List<CartDetail> list = context.CartDetails.Where(x => x.CartId == cart.CartId).ToList();
                    foreach (var i in list)
                    {
                        context.CartDetails.Remove(i);
                        context.SaveChanges();
                    }
                    context.Carts.Remove(cart);
                    context.SaveChanges();
                    Session["giohang"] = null;
                }
            }
            return RedirectToAction("PaySuccess");
        }
        public ActionResult PaySuccess()
        {
            return View();
        }
    }
}