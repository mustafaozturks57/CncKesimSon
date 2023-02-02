using System;
using System.Collections.Generic;
using System.Linq;
 
using System.Net;
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
       

        public ActionResult SiparisPdf(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

 
        public ActionResult SiparisPdfFabrika(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {



                OrficheUpdateDto orficheUpdateDto = new OrficheUpdateDto();

                var a = db.Orfiches.Where(x => x.FisNo == id).First();
                orficheUpdateDto.Orfiche = a;

                var Customers = db.Customers.Where(x => x.id == a.MusteriId).First();

                ViewBag.Custormer = Customers.Fırma_Adı;


                var b = db.Orflines.Where(x => x.OrficheNo == id);
                orficheUpdateDto.Orflines = b.ToList();


                return View(orficheUpdateDto);

            }

        }

        public ActionResult KargoPdf(string id, string sayı)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {



                OrficheUpdateDto orficheUpdateDto = new OrficheUpdateDto();

                var a = db.Orfiches.Where(x => x.FisNo == id).First();
                orficheUpdateDto.Orfiche = a;

                var Customers = db.Customers.Where(x => x.id == a.MusteriId).First();
                

                ViewBag.Custormer = Customers.Fırma_Adı;
                ViewBag.Sayı = Convert.ToInt32 (sayı);


                var b = db.Orflines.Where(x => x.OrficheNo == id);
                orficheUpdateDto.Orflines = b.ToList();


                return View(orficheUpdateDto);

            }
        }

        public ActionResult getPdf(int? id, string fisno    )
        {
            if (id==1 )//Müsteri
            {
                var report = new ActionAsPdf("SiparisPdf", new { id = fisno })
                {



                };

                return report;
            }
            else if (id==2)//fabrika
            {
                var report = new ActionAsPdf("SiparisPdfFabrika", new { id = fisno })
                {



                };

                return report;

            }
            else if (id==3)
            {
                var report = new ActionAsPdf("KargoPdf", new { id = fisno })
                {



                };

                return report;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
            

 

        public ActionResult getPdfKargo(string sipno,string sayı)
        {
            var report = new ActionAsPdf("KargoPdf", new {id= sipno, sayı = sayı })
            {
                

            };

            return report;
        }

 


       

    }
}