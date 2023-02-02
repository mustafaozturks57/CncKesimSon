using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Models.Classes.Views;
using Ticari_Otomasyon.Models.CncModel;

using DevExpress.XtraReports.UserDesigner;
using System.Net;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class CncPaketController : Controller
    {
        // GET: CncPaket
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();
        public async Task<ActionResult> Index()
        {
            var Views = await db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where kesim_fl=1 and palet_fl=1 and pres_fl=1 and PaletNo>0  and Onay_fl=1 ").ToListAsync();


            var Viewss = db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where  paket_fl=1 and  PaketSayı>0    and Onay_fl=1 ").ToList();

            ViewBag.Yazdır = Viewss;


            return View(Views);
        }


        public ActionResult PaketYazdır(string sipno,int pakeysayı)
        {




            var find = db.SıparısAppPool.Where(x => x.SipID == sipno).ToList();

            if (find.Count == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {


                string query = "update SıparısAppPool set Paket_fl=1,PaketSayı="+pakeysayı+"  where SipID=" + sipno;

                db.Database.ExecuteSqlCommand(query);

                GetExcelDosyaYazdırma(sipno,pakeysayı);

                string alert = "Sipariş Kesim İptal Edilmiştir , Sipariş Numarası  : " + sipno;
                return RedirectToAction("Index");

               


            }


        }

        public ActionResult GetExcelDosyaYazdırma(string sipno,int paketsayı)
        {

            if (sipno!=null && paketsayı>0 )
            {
                return RedirectToAction("getPdf", "Raporlar", new { id = 3, fisno = sipno });
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
    
        }
    }
}