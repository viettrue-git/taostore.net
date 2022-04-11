using Models;
using Models.Famework;
using System;
using System.Collections.Generic;
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
                    return RedirectToAction("Index", "Admin/Home");
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
                        Session.Add("loginSession", acount);
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
    }
}