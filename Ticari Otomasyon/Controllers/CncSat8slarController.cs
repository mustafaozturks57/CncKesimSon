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
    public class CncSatıslarController : Controller
    {
      
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();

        // GET: CncSatıslar
        public ActionResult Index()
        {
            return View(db.Satıslar.ToList());
        }

        // GET: CncSatıslar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satıslar satıslar = db.Satıslar.Find(id);
            if (satıslar == null)
            {
                return HttpNotFound();
            }
            return View(satıslar);
        }

        // GET: CncSatıslar/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CncSatıslar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Alınanlar,TARIH,MIKTAR,ADET,MUSTERI_ID,SATIS_NO,TOPLAM,TUR,ODEME_TUTARI,BIRIM,TUR_ID,ACIKLAMA,KULLANICI_ID,ODEME_ZAMANI,ODEME_HABER,URUN_ID,ALIS_FIYATI,ISKONTO,KDV,FATURA")] Satıslar satıslar)
        {
            if (ModelState.IsValid)
            {
                db.Satıslar.Add(satıslar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(satıslar);
        }

        // GET: CncSatıslar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satıslar satıslar = db.Satıslar.Find(id);
            if (satıslar == null)
            {
                return HttpNotFound();
            }
            return View(satıslar);
        }

        // POST: CncSatıslar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Alınanlar,TARIH,MIKTAR,ADET,MUSTERI_ID,SATIS_NO,TOPLAM,TUR,ODEME_TUTARI,BIRIM,TUR_ID,ACIKLAMA,KULLANICI_ID,ODEME_ZAMANI,ODEME_HABER,URUN_ID,ALIS_FIYATI,ISKONTO,KDV,FATURA")] Satıslar satıslar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(satıslar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(satıslar);
        }

        // GET: CncSatıslar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satıslar satıslar = db.Satıslar.Find(id);
            if (satıslar == null)
            {
                return HttpNotFound();
            }
            return View(satıslar);
        }

        // POST: CncSatıslar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Satıslar satıslar = db.Satıslar.Find(id);
            db.Satıslar.Remove(satıslar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
