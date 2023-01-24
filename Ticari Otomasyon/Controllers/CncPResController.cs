using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Models.Classes.Views;
using Ticari_Otomasyon.Models.CncModel;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class CncPResController : Controller
    {
      
        // GET: CncPRes
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();
        public async Task<ActionResult> Index()
        {
            string paketegonderılcek = "select * from SıparısAppPoolView where Kesim_fl=1 and Palet_fl=1";
            var paketler = db.Database.SqlQuery<SiparişQliste>(paketegonderılcek).ToList();
            ViewBag.paketler = paketler;

            var Views = await db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where Kesim_fl=1 and Palet_fl=1 and Pres_fl =0 ").ToListAsync();

            return View(Views);
        }


        public ActionResult PresGönder(int paletno)
        {
             
            var find = db.SıparısAppPool.Where(x => x.PaletNo == paletno).ToList();

            if (find.Count == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {


                string query = "update SıparısAppPool set Pres_fl=1 where PaletNo=" + paletno + "  and Kesim_fl=1 and Palet_fl=1 and and Onay_fl=1";

                db.Database.ExecuteSqlCommand(query);

                string alert = "Sipariş Press Yapılmıştır Sipariş Numarası  : " + paletno;
                return RedirectToAction("Index");
            }


 
        }
    }
}