using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Models.Classes.Views;
using Ticari_Otomasyon.Models.CncModel;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class CncOnayHavuzController : Controller
    {
        // GET: CncOnayHavuz
    
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();
        public ActionResult Index()
        {


            var lıst = db.Database.SqlQuery<SiparisModulsOnayHavuz>(" select * from SiparisModulsOnayHavuz").ToList();
            return View(lıst);
        }
        public ActionResult Onay(string id)
        {

            string query = "update SıparısAppPool set  Onay_fl=1 where SipID='"+id+"'";
            db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Index");
        }

        public ActionResult Teslim(string id)
        {

            string query = "update SıparısAppPool set  Teslim_fl=1 where SipID='" + id + "'";
            db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Index");
        }

        public ActionResult Onayİptal(string id)
        {

            string query = "update SıparısAppPool set  Onay_fl=0 where SipID='" + id + "'";
            db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Index");
        }
        public ActionResult Teslimİptal(string id)
        {

            string query = "update SıparısAppPool set  Teslim_fl=0 where SipID='" + id + "'";
            db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Index");
        }


    }
}