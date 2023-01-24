using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Preview;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Models.Classes.Views;
using Ticari_Otomasyon.Models.CncModel;
using Ticari_Otomasyon.ReportService;
using DevExpress.XtraReports.Web.ReportDesigner.Native;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class CncKesimController : Controller
    {
        // GET: CncKesim

        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();
        public async Task<ActionResult> Index()
        {



            var A = db.Database.SqlQuery<Toplam>("EXEC GetToplam ").ToList();
            ViewBag.SiparislerToplam = A;

            var Views = await db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where kesim_fl=0 and Onay_fl=1 ").ToListAsync();

            var ViewsCombo = await db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where kesim_fl=0 and Onay_fl=1  ").ToListAsync();
            ViewBag.Siparisler = ViewsCombo;

            return View(Views);
        }


        public ActionResult KesWorks(string id)
        {
            string query = "update SıparısAppPool set Kesim_fl=1 where SipID='" + id + "'";

            db.Database.ExecuteSqlCommand(query);

            return RedirectToAction("Index");
        }

        public ActionResult KesWorksIptal(string id)
        {
            string query = "update SıparısAppPool set Kesim_fl=0 where SipID='" + id + "'";

            db.Database.ExecuteSqlCommand(query);

            return RedirectToAction("Index");
        }

        public ActionResult BenimTestim()
        {
            BarkodDesigner report = new BarkodDesigner();
 


                return View();
            }



        }



    }
