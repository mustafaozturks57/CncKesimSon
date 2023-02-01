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
    public class CncSıpDurumuController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();

        // GET: CncSıpDurumu
        public ActionResult Index()
        {
            return View(db.SıpDurumu.ToList());
        }

        // GET: CncSıpDurumu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SıpDurumu sıpDurumu = db.SıpDurumu.Find(id);
            if (sıpDurumu == null)
            {
                return HttpNotFound();
            }
            return View(sıpDurumu);
        }

        // GET: CncSıpDurumu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CncSıpDurumu/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,SiparisDurumu")] SıpDurumu sıpDurumu)
        {
            if (ModelState.IsValid)
            {
                db.SıpDurumu.Add(sıpDurumu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sıpDurumu);
        }

        // GET: CncSıpDurumu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SıpDurumu sıpDurumu = db.SıpDurumu.Find(id);
            if (sıpDurumu == null)
            {
                return HttpNotFound();
            }
            return View(sıpDurumu);
        }

        // POST: CncSıpDurumu/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,SiparisDurumu")] SıpDurumu sıpDurumu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sıpDurumu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sıpDurumu);
        }

        // GET: CncSıpDurumu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SıpDurumu sıpDurumu = db.SıpDurumu.Find(id);
            if (sıpDurumu == null)
            {
                return HttpNotFound();
            }
            return View(sıpDurumu);
        }

        // POST: CncSıpDurumu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SıpDurumu sıpDurumu = db.SıpDurumu.Find(id);
            db.SıpDurumu.Remove(sıpDurumu);
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
