using Facebook;
using Microsoft.Owin.Security;
using Models;
using Models.Famework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaoStore.Code;
using TaoStore.Models;

namespace TaoStore.Controllers
{
    public class LoginController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FaceBookCallBack");
                return uriBuilder.Uri;
            }
        }
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// login acount 
        /// </summary>
        /// <param name="model">Loginmodel</param>
        /// <returns>session is acount_email or return error</returns>
        /// admin - pass 123
        [HttpPost]
        [ValidateAntiForgeryToken] //chong viec request lien tuc
        public ActionResult Index(LoginModel model)
        {
            var result = new AcountModel().Login(model.UserName,Encode.EncodePassword(model.PassWord));
            if (result && ModelState.IsValid)
            {
                // SessionHelper.SetSession(new UserSession() { UserName = model.UserName });
                FormsAuthentication.SetAuthCookie(model.UserName, model.Remember);
                var acountModel = new AcountModel();
                var acount = acountModel.GetAcount(model.UserName);
                Session.Add("loginSession", acount);
                if (acount.AcountRole == 0||acount.AcountRole==null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Admin/HomeAdmin");
                }
            }
            else
            {
                ModelState.AddModelError("", "Email or PassWord no corect");
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["giohang"] = null;
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Acount acount)
        {
            if (ModelState.IsValid)
            {
                AcountModel acountModel = new AcountModel();
                if (acountModel.GetAcount(acount.Email) != null)
                {
                    ViewBag.MesAcount = "Email already exists, please changer orther email!";
                    return View(acount);
                }
                else
                {
                    try
                    {
                        string pass = Encode.EncodePassword(acount.PassWord);
                        OnlineShopDBContext context = new OnlineShopDBContext();
                        acount.PassWord = pass;
                        acount.F_Password = pass;
                        context.Acounts.Add(acount);
                        context.SaveChanges();
                        Customer customer = new Customer()
                        {
                            Name = acount.AcountName,
                            PhoneNumber = acount.Phone,
                            Email = acount.Email
                        };
                        context.Customers.Add(customer);
                        context.SaveChanges();
                        Session.Add("loginSession",acount);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return View(acount);
        }
        public ActionResult LoginFaceBook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FBAppId"],
                client_secret = ConfigurationManager.AppSettings["FBSecret"],
                redirect_uri= RedirectUri.AbsoluteUri,
                response_type="code",
                scope="email"
            }) ;
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FaceBookCallBack(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FBAppId"],
                client_secret = ConfigurationManager.AppSettings["FBSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code=code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string name = me.first_name + " " + me.last_name;
                string email = me.email;
                Customer customer = new Customer();
                customer.Name = name;
                customer.Email = email;
                customer.FullName= me.first_name +" "+ me.middle_name+ " " + me.last_name;
                CustomerModel customerModel = new CustomerModel();
                var resultInsert = customerModel.InsertWithFaceBook(customer);
                if(resultInsert>0) // đã đăng nhập trước đó rồi
                {
                    AcountModel model = new AcountModel();
                    Acount acount = model.GetAcount(email);
                    if (acount == null)
                    {
                        Acount acountNew = new Acount();
                        acountNew.AcountName = name;
                        acountNew.AcountRole = 0;
                        acountNew.Email = email;
                        model = new AcountModel();
                        model.Insert(acountNew);
                        Session.Add("loginSession", acountNew);
                    }
                    else
                    {
                        Session.Add("loginSession", acount);
                    }
                }
                else
                {
                    Acount acount = new Acount();
                    acount.AcountName = name;
                    acount.AcountRole = 0;
                    acount.Email = email;
                    AcountModel model = new AcountModel();
                    model.Insert(acount);
                    Session.Add("loginSession", acount);
                }

            }
            return Redirect("/");
        }
    }
}