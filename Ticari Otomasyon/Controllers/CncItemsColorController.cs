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
    public class CncItemsColorController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();

        // GET: CncItemsColor
        public ActionResult Index()
        {
            return View(db.ItemsColors.ToList());
        }

        // GET: CncItemsColor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemsColor itemsColor = db.ItemsColors.Find(id);
            if (itemsColor == null)
            {
                return HttpNotFound();
            }
            return View(itemsColor);
        }

        // GET: CncItemsColor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CncItemsColor/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ItemsID,ColorName")] ItemsColor itemsColor)
        {
            if (ModelState.IsValid)
            {
                db.ItemsColors.Add(itemsColor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(itemsColor);
        }

        // GET: CncItemsColor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemsColor itemsColor = db.ItemsColors.Find(id);
            if (itemsColor == null)
            {
                return HttpNotFound();
            }
            return View(itemsColor);
        }

        // POST: CncItemsColor/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ItemsID,ColorName")] ItemsColor itemsColor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemsColor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemsColor);
        }

        // GET: CncItemsColor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemsColor itemsColor = db.ItemsColors.Find(id);
            if (itemsColor == null)
            {
                return HttpNotFound();
            }
            return View(itemsColor);
        }

        // POST: CncItemsColor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemsColor itemsColor = db.ItemsColors.Find(id);
            db.ItemsColors.Remove(itemsColor);
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
