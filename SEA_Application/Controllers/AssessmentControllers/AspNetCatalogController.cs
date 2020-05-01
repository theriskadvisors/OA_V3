using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers.AssessmentControllers
{
    public class AspNetCatalogController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetCatalog
        public ActionResult Index()
        {
            return View(db.AspNetCatalogs.ToList());
        }

        // GET: AspNetCatalog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCatalog aspNetCatalog = db.AspNetCatalogs.Find(id);
            if (aspNetCatalog == null)
            {
                return HttpNotFound();
            }
            return View(aspNetCatalog);
        }

        // GET: AspNetCatalog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetCatalog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CatalogName")] AspNetCatalog aspNetCatalog)
        {
            if (ModelState.IsValid)
            {
                db.AspNetCatalogs.Add(aspNetCatalog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetCatalog);
        }

        // GET: AspNetCatalog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCatalog aspNetCatalog = db.AspNetCatalogs.Find(id);
            if (aspNetCatalog == null)
            {
                return HttpNotFound();
            }
            return View(aspNetCatalog);
        }

        // POST: AspNetCatalog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CatalogName")] AspNetCatalog aspNetCatalog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetCatalog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetCatalog);
        }

        // GET: AspNetCatalog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCatalog aspNetCatalog = db.AspNetCatalogs.Find(id);
            if (aspNetCatalog == null)
            {
                return HttpNotFound();
            }
            return View(aspNetCatalog);
        }

        // POST: AspNetCatalog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetCatalog aspNetCatalog = db.AspNetCatalogs.Find(id);
            db.AspNetCatalogs.Remove(aspNetCatalog);
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
