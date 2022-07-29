using Models.Famework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaoStore.Code;
using TaoStore.Models;

namespace TaoStore.Controllers
{
    public class MomoPayController : Controller
    {
        // GET: MomoPay
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
    }
}