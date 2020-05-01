using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers.TermAssessmentControllers
{
    public class AspNetAssessment_Questions_CategoryController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetAssessment_Questions_Category
        public ActionResult Index()
        {
            return View(db.AspNetAssessment_Questions_Category.ToList());
        }

        // GET: AspNetAssessment_Questions_Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment_Questions_Category aspNetAssessment_Questions_Category = db.AspNetAssessment_Questions_Category.Find(id);
            if (aspNetAssessment_Questions_Category == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAssessment_Questions_Category);
        }

        // GET: AspNetAssessment_Questions_Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetAssessment_Questions_Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryName")] AspNetAssessment_Questions_Category aspNetAssessment_Questions_Category)
        {
            if (ModelState.IsValid)
            {
                db.AspNetAssessment_Questions_Category.Add(aspNetAssessment_Questions_Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetAssessment_Questions_Category);
        }

        // GET: AspNetAssessment_Questions_Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment_Questions_Category aspNetAssessment_Questions_Category = db.AspNetAssessment_Questions_Category.Find(id);
            if (aspNetAssessment_Questions_Category == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAssessment_Questions_Category);
        }

        // POST: AspNetAssessment_Questions_Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryName")] AspNetAssessment_Questions_Category aspNetAssessment_Questions_Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetAssessment_Questions_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetAssessment_Questions_Category);
        }

        // GET: AspNetAssessment_Questions_Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment_Questions_Category aspNetAssessment_Questions_Category = db.AspNetAssessment_Questions_Category.Find(id);
            if (aspNetAssessment_Questions_Category == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAssessment_Questions_Category);
        }

        // POST: AspNetAssessment_Questions_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetAssessment_Questions_Category aspNetAssessment_Questions_Category = db.AspNetAssessment_Questions_Category.Find(id);
            db.AspNetAssessment_Questions_Category.Remove(aspNetAssessment_Questions_Category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCnfm(int id)
        {
            AspNetAssessment_Questions_Category aspNetAssessment_Questions_Category = db.AspNetAssessment_Questions_Category.Find(id);
            try
            {
                db.AspNetAssessment_Questions_Category.Remove(aspNetAssessment_Questions_Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "It can't be deleted";
                return View("Details", aspNetAssessment_Questions_Category);
            }
          
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
