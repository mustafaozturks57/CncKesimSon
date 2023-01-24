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
    public class CncIsyerisController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();

        // GET: CncIsyeris
        public ActionResult Index()
        {
            return View(db.Isyeris.ToList());
        }

        // GET: CncIsyeris/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Isyeri isyeri = db.Isyeris.Find(id);
            if (isyeri == null)
            {
                return HttpNotFound();
            }
            return View(isyeri);
        }

        // GET: CncIsyeris/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CncIsyeris/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "id,IsyeriName")] Isyeri isyeri)
        {
            if (ModelState.IsValid)
            {
                db.Isyeris.Add(isyeri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(isyeri);
        }

        // GET: CncIsyeris/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Isyeri isyeri = db.Isyeris.Find(id);
            if (isyeri == null)
            {
                return HttpNotFound();
            }
            return View(isyeri);
        }

        // POST: CncIsyeris/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,IsyeriName")] Isyeri isyeri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(isyeri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(isyeri);
        }

        // GET: CncIsyeris/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Isyeri isyeri = db.Isyeris.Find(id);
            if (isyeri == null)
            {
                return HttpNotFound();
            }
            return View(isyeri);
        }

        // POST: CncIsyeris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Isyeri isyeri = db.Isyeris.Find(id);
            db.Isyeris.Remove(isyeri);
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
