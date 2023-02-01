using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using Ticari_Otomasyon.Models;
using Ticari_Otomasyon.Models.CncModel;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
 
    public class RaporlarController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();
        // GET: Raporlar
        public ActionResult Index()
        {
            return View();
            
        }

        public ActionResult SiparisPdf(string id)
        {

            if (id==null)
            {
                return View();
            }
            else
            {

           

            OrficheUpdateDto orficheUpdateDto = new OrficheUpdateDto();

            var a = db.Orfiches.Where(x => x.FisNo == id).First();
            orficheUpdateDto.Orfiche = a;

                var Customers  = db.Customers.Where(x => x.id == a.MusteriId).First();

                ViewBag.Custormer   =  Customers.Fırma_Adı;


                var b = db.Orflines.Where(x => x.OrficheNo == id);
            orficheUpdateDto.Orflines = b.ToList();


            return View(orficheUpdateDto);
            }

        }


        public ActionResult getPdf(int id )
        {
            var report = new ActionAsPdf("Index", new {id= id })
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4

            };

            return report;
        }



       

    }
}