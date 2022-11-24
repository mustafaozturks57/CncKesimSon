using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Ticari_Otomasyon.Models;
using Ticari_Otomasyon.Models.Classes;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
      
        public ActionResult Index()
        {
            return View();
        }

       
        

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(Admin p)
            
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                var query = context.Database.SqlQuery<Admin>("SELECT Id Id,Username Username,Password Password,Control Control FROM TB_AUTOMATION_Admins").FirstOrDefault(x => x.Username == p.Username && x.Password == p.Password);
                if (query != null )
                {
                    FormsAuthentication.SetAuthCookie(query.Password, false);
                    Session["KullaniciAdi"] = query.Username.ToString();
                    Session["Year"] = p.Year.ToString();
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return RedirectToAction("Index", "Login");

                }
            }
           

        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
