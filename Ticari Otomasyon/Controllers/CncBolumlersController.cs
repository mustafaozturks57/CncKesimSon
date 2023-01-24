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
    public class CncBolumlersController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();

        // GET: CncBolumlers
        public ActionResult Index()
        {
            return View(db.Bolumlers.ToList());
        }

        // GET: CncBolumlers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bolumler bolumler = db.Bolumlers.Find(id);
            if (bolumler == null)
            {
                return HttpNotFound();
            }
            return View(bolumler);
        }

        // GET: CncBolumlers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CncBolumlers/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,BolumName")] Bolumler bolumler)
        {
            if (ModelState.IsValid)
            {
                db.Bolumlers.Add(bolumler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bolumler);
        }

        // GET: CncBolumlers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bolumler bolumler = db.Bolumlers.Find(id);
            if (bolumler == null)
            {
                return HttpNotFound();
            }
            return View(bolumler);
        }

        // POST: CncBolumlers/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,BolumName")] Bolumler bolumler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bolumler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bolumler);
        }

        // GET: CncBolumlers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bolumler bolumler = db.Bolumlers.Find(id);
            if (bolumler == null)
            {
                return HttpNotFound();
            }
            return View(bolumler);
        }

        // POST: CncBolumlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bolumler bolumler = db.Bolumlers.Find(id);
            db.Bolumlers.Remove(bolumler);
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
