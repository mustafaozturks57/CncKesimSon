using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Models.CncModel;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class CncOrfichesController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();

        // GET: CncOrfiches
        public ActionResult Index()
        {
            return View(db.Orfiches.ToList());
        }

        // GET: CncOrfiches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orfiche orfiche = db.Orfiches.Find(id);
            if (orfiche == null)
            {
                return HttpNotFound();
            }
            return View(orfiche);
        }

      

        [HttpPost]
        public JsonResult Delete(string fisNo)
        {
           DeleteOrficheAndOrfline(fisNo);
            return Json("");
        }
        // GET: CncOrfiches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CncOrfiches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(OrficheDto orficheDto, OrflineDto[] orflines)
        {
            var orficheInsert = InsertOrfiche(orficheDto);
            if (orficheInsert)
            {
                var orfline = InsertOrfline(orflines, orficheDto.FisNo);


                if (orfline)
                {
                    ViewBag.Succes = "Sipariş Kaydedildi.";
                }
            }
            else
            {
                ViewBag.Error = "Sipariş Kaydedilemedi.";
            }

            return View();
        }

        // GET: CncOrfiches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orfiche orfiche = db.Orfiches.Find(id);
            if (orfiche == null)
            {
                return HttpNotFound();
            }
            return View(orfiche);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MusteriId,Tarih,BarkodNo,SiparisSekli,BID,Aciklama,TerminTarihi,BelgeNo,OzelKod,Bölüm,Departman,Isyeri,SiparisDurumu")] Orfiche orfiche)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orfiche).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orfiche);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public bool InsertOrfiche(OrficheDto orficheDto)
        {

            string sqlString = "INSERT INTO Orfiche VALUES ('" + orficheDto.MusteriId + "'," +
                "'" + orficheDto.Tarih + "'," +
                "'" + orficheDto.FisNo + "'," +
                "'" + orficheDto.BarkodNo + "'," +
                "'" + orficheDto.SiparisSekli + "'," +
                "'" + orficheDto.BID + "'," +
                "'" + orficheDto.Aciklama + "'," +
                "'" + orficheDto.TerminTarihi + "'," +
                "'" + orficheDto.BelgeNo + "'," +
                "'" + orficheDto.OzelKod + "'," +
                "'" + orficheDto.Bölüm + "'," +
                "'" + orficheDto.Departman + "'," +
                "'" + orficheDto.Isyeri + "'," +
                "'" + orficheDto.SiparisDurumu + "' )";



            var sqlQuery = db.Database.ExecuteSqlCommand(sqlString);


            return true;
        }
        public bool InsertOrfline(OrflineDto[] orflines, string orficheId)
        {

            if (orflines != null)
            {
                foreach (var line in orflines)
                {
                    if (!string.IsNullOrEmpty(line.MalzemeAdi))
                    {
                        string sqlString = "INSERT INTO Orfline VALUES ('" + orficheId + "','" + line.MalzemeKodu + "','" + line.MalzemeAdi + "','" + line.Ozellik + "','" + line.Boy + "','" + line.En + "','" + line.Adet + "','" + line.M2 + "','" + line.M2Fiyat + "','" + line.Fiyat + "')";

                        var sqlQuery = db.Database.ExecuteSqlCommand(sqlString);


                    }
                }

                return true;
            }
            return false;
        }
        public bool DeleteOrficheAndOrfline(string ficheNo)
        {


            string sqlString = "DELETE FROM Orfiche WHERE FisNo='" + ficheNo + "';DELETE FROM Orfline WHERE OrficheNo='" + ficheNo + "';";

            var sqlQuery = db.Database.ExecuteSqlCommand(sqlString);






            return true;
        }
    }
}
