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
    public class AspNetSubject_CatalogController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetSubject_Catalog
        public ActionResult Index()
        {
            var aspNetSubject_Catalog = db.AspNetSubject_Catalog.Include(a => a.AspNetCatalog).Include(a => a.AspNetSubject);
            return View(aspNetSubject_Catalog.ToList());
        }
     

        // GET: AspNetSubject_Catalog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSubject_Catalog aspNetSubject_Catalog = db.AspNetSubject_Catalog.Find(id);
            if (aspNetSubject_Catalog == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSubject_Catalog);
        }

        // GET: AspNetSubject_Catalog/Create
        public ActionResult Create()
        {
            ViewBag.CatalogID = new SelectList(db.AspNetCatalogs, "Id", "CatalogName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }

        // POST: AspNetSubject_Catalog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubjectID,CatalogID,Weightage")] AspNetSubject_Catalog aspNetSubject_Catalog)
        {
            if (ModelState.IsValid)
            {
                db.AspNetSubject_Catalog.Add(aspNetSubject_Catalog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.CatalogID = new SelectList(db.AspNetCatalogs, "Id", "CatalogName", aspNetSubject_Catalog.CatalogID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetSubject_Catalog.SubjectID);
            return View(aspNetSubject_Catalog);
        }

        // GET: AspNetSubject_Catalog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSubject_Catalog aspNetSubject_Catalog = db.AspNetSubject_Catalog.Find(id);
            if (aspNetSubject_Catalog == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatalogID = new SelectList(db.AspNetCatalogs, "Id", "CatalogName", aspNetSubject_Catalog.CatalogID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetSubject_Catalog.SubjectID);
            return View(aspNetSubject_Catalog);
        }

        // POST: AspNetSubject_Catalog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubjectID,CatalogID,Weightage")] AspNetSubject_Catalog aspNetSubject_Catalog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetSubject_Catalog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatalogID = new SelectList(db.AspNetCatalogs, "Id", "CatalogName", aspNetSubject_Catalog.CatalogID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetSubject_Catalog.SubjectID);
            return View(aspNetSubject_Catalog);
        }

        // GET: AspNetSubject_Catalog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSubject_Catalog aspNetSubject_Catalog = db.AspNetSubject_Catalog.Find(id);
            if (aspNetSubject_Catalog == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSubject_Catalog);
        }

        // POST: AspNetSubject_Catalog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetSubject_Catalog aspNetSubject_Catalog = db.AspNetSubject_Catalog.Find(id);
            db.AspNetSubject_Catalog.Remove(aspNetSubject_Catalog);
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
