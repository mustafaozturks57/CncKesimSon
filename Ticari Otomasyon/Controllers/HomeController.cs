using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Models.Classes.HomeClass;
using Ticari_Otomasyon.Models.CncModel;
using Ticari_Otomasyon.Roles;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();


        public ActionResult Index()
        {

            Decimal AylıkFiyat = db.Database.SqlQuery<Decimal>("exec AnasayfaIslemler @pTranType=4").FirstOrDefault();
            ViewBag.AylıkFiyat = AylıkFiyat;

            int MüsteriSayısı = db.Database.SqlQuery<int>("exec AnasayfaIslemler @pTranType=3").FirstOrDefault();
            ViewBag.MüsteriSayısı = MüsteriSayısı;

            var KesilenSonSip = db.Database.SqlQuery<Top6Siparis>("exec AnasayfaIslemler @pTranType=5").ToList();
            ViewBag.KesilenSonSip = KesilenSonSip;

            var GünlükKesim = db.Database.SqlQuery<GünlükKesim>(" exec AnasayfaIslemler @pTranType=2").ToList();
            ViewBag.GünlükKesim = GünlükKesim;

            var günlükSatis = db.Database.SqlQuery<GünlükSatis>(" exec AnasayfaIslemler @pTranType=1").ToList();
            ViewBag.GünlükSatis = günlükSatis;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Benimle iletişime geçin.";

            return View();
        }

        public ActionResult IndexSonTest()
        {   


            return View();
        }
    }
}