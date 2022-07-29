using Models.Famework;
using Newtonsoft.Json.Linq;
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
    public class PaymentController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Payment(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                string name = form["name"].ToString();
                string email = form["email"].ToString();
                string adress = form["adress"].ToString();
                string phone = form["phone"].ToString();
                string note = form["note"].ToString();
                Customer customer = new Customer();
                customer.Name = name;
                customer.Adress = adress;
                customer.Email = email;
                customer.PhoneNumber = Int32.Parse(phone);
                Session["customer"] = customer;
                //request params need to request to MoMo system
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMOCSPO20220415";
                string accessKey = "hpysmJXhvhJgE4OG";
                string serectkey = "2UvSuNak227QVRw9tieiDpBeRfTqAZrl";
                string orderInfo = "Thông tin đơn hàng";
                string returnUrl = "https://localhost:44347/Payment/ConfirmPaymentClient";
                string notifyurl = "https://momo.vn/";
                //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình 
                List<CartItem> giohang = Session["giohang"] as List<CartItem>;
                int total = giohang.Sum(x => x.Total);
                if (total < 1000)
                {
                    return Redirect("ConfirmPaymentClient");
                }
                string amount = giohang.Sum(x => x.Total).ToString();
                string orderid = DateTime.Now.Ticks.ToString();
                string requestId = DateTime.Now.Ticks.ToString();
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                JObject jmessage = JObject.Parse(responseFromMomo);

                return Redirect(jmessage.GetValue("payUrl").ToString());
            }
                return View();
        }
        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public ActionResult ConfirmPaymentClient()
        {
            //hiển thị thông báo cho người dùng
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity moMoSecurity = new MoMoSecurity();

            string serectkey = "2UvSuNak227QVRw9tieiDpBeRfTqAZrl";

            string signature = moMoSecurity.signSHA256(param, serectkey);

            //  string signatureRequest = Request["signature"].ToString();

            if (signature != Request["signature"].ToString())
            {
                ViewBag.Messger = "Please,you must to check payment!";
                return View();
            }
            if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.Messger = "Payment fail!";
                return View();
            }
            else
            {
                ViewBag.Messger = "Good!";
            }
            return View();
        }

        [HttpPost]
        public JsonResult SavePayment()
        {
            //cập nhật dữ liệu vào db
            // check chữ kí điện tử 
            string param = "";
            param = "partnerCode=" +
                Request["partnerCode"] + "&accessKey=" +
                Request["accessKey"] + "&requestId=" +
                Request["requestId"] + "&amount=" +
                Request["amount"] + "&orderId=" +
                Request["orderId"] + "&orderInfo=" +
                Request["orderInfo"] + "&returnUrl=" +
                Request["returnUrl"] + "&notifyUrl=" +
                Request["notifyUrl"] + "&extraData=" +
                Request["extraData"];
            param = Server.UrlDecode(param);
            MoMoSecurity moMoSecurity = new MoMoSecurity();

            string serectkey = "2UvSuNak227QVRw9tieiDpBeRfTqAZrl";

            string signature = moMoSecurity.signSHA256(param, serectkey);
            if (signature != Request["signature"].ToString())
            {
                return Json(new
                {
                    status = false
                });
            }
            else
            {
                OnlineShopDBContext context = new OnlineShopDBContext();
                // insert database
                Acount acount = (Acount)Session["loginSession"];
                List<CartItem> cartItems = Session["giohang"] as List<CartItem>;
                Order order = new Order();
                Customer customer = new Customer();
                string email = Request["email"];
                string phone = Request["phone"];
                string name = Request["name"];
                string adress = Request["address"];
                string note = Request["note"];
                int total = 1000;
                if (acount == null)
                {
                    // neu khach hang da ton tai thi khong them moi con neu chua thi add new customer
                    customer = context.Customers.Where(x => x.Email == email).FirstOrDefault();
                    if (customer == null)
                    {
                        Customer newCustomer = new Customer()
                        {
                            Email = email,
                            PhoneNumber = Int32.Parse(phone),
                            Adress = adress,
                            Name = name
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
                }
                return Json(new
                {
                    status = true
                });
            }


        }
        //set session for infor of customer 
        public bool setCustomer(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();
                string name = form["name"].ToString();
                string email = form["email"].ToString();
                string adress = form["adress"].ToString();
                string phone = form["phone"].ToString();
                string note = form["note"].ToString();
                customer.Name = name;
                customer.Adress = adress;
                customer.Email = email;
                customer.PhoneNumber = Int32.Parse(phone);
                Session["customer"] = customer;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}