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
    public class CncSpecialCodesController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();

        // GET: CncSpecialCodes
        public ActionResult Index()
        {
            return View(db.SpecialCodes.ToList());
        }

        // GET: CncSpecialCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialCode specialCode = db.SpecialCodes.Find(id);
            if (specialCode == null)
            {
                return HttpNotFound();
            }
            return View(specialCode);
        }

        // GET: CncSpecialCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CncSpecialCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] SpecialCode specialCode)
        {
            if (ModelState.IsValid)
            {
                db.SpecialCodes.Add(specialCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialCode);
        }

        // GET: CncSpecialCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialCode specialCode = db.SpecialCodes.Find(id);
            if (specialCode == null)
            {
                return HttpNotFound();
            }
            return View(specialCode);
        }

        // POST: CncSpecialCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] SpecialCode specialCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialCode);
        }

        // GET: CncSpecialCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialCode specialCode = db.SpecialCodes.Find(id);
            if (specialCode == null)
            {
                return HttpNotFound();
            }
            return View(specialCode);
        }

        // POST: CncSpecialCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SpecialCode specialCode = db.SpecialCodes.Find(id);
            db.SpecialCodes.Remove(specialCode);
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
