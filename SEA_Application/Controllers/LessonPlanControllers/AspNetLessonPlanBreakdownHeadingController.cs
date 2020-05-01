using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers.LessonPlanControllers
{
    public class AspNetLessonPlanBreakdownHeadingController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetLessonPlanBreakdownHeading
        public ActionResult Index()
        {
            return View(db.AspNetLessonPlanBreakdownHeadings.ToList());
        }

        // GET: AspNetLessonPlanBreakdownHeading/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlanBreakdownHeading aspNetLessonPlanBreakdownHeading = db.AspNetLessonPlanBreakdownHeadings.Find(id);
            if (aspNetLessonPlanBreakdownHeading == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLessonPlanBreakdownHeading);
        }

        // GET: AspNetLessonPlanBreakdownHeading/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetLessonPlanBreakdownHeading/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BreakDownHeadingName")] AspNetLessonPlanBreakdownHeading aspNetLessonPlanBreakdownHeading)
        {
            if (ModelState.IsValid)
            {
                db.AspNetLessonPlanBreakdownHeadings.Add(aspNetLessonPlanBreakdownHeading);
                db.SaveChanges();
                return RedirectToAction("Index" , "AspNetLessonPlanBreakdownHeading");
            }

            return View(aspNetLessonPlanBreakdownHeading);
        }

        // GET: AspNetLessonPlanBreakdownHeading/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlanBreakdownHeading aspNetLessonPlanBreakdownHeading = db.AspNetLessonPlanBreakdownHeadings.Find(id);
            if (aspNetLessonPlanBreakdownHeading == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLessonPlanBreakdownHeading);
        }

        // POST: AspNetLessonPlanBreakdownHeading/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BreakDownHeadingName")] AspNetLessonPlanBreakdownHeading aspNetLessonPlanBreakdownHeading)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetLessonPlanBreakdownHeading).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetLessonPlanBreakdownHeading);
        }

        // GET: AspNetLessonPlanBreakdownHeading/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlanBreakdownHeading aspNetLessonPlanBreakdownHeading = db.AspNetLessonPlanBreakdownHeadings.Find(id);
            if (aspNetLessonPlanBreakdownHeading == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLessonPlanBreakdownHeading);
        }

        // POST: AspNetLessonPlanBreakdownHeading/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetLessonPlanBreakdownHeading aspNetLessonPlanBreakdownHeading = db.AspNetLessonPlanBreakdownHeadings.Find(id);
            db.AspNetLessonPlanBreakdownHeadings.Remove(aspNetLessonPlanBreakdownHeading);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCnfm(int id)
        {
            AspNetLessonPlanBreakdownHeading aspNetLessonPlanBreakdownHeading = db.AspNetLessonPlanBreakdownHeadings.Find(id);
            try
            {
                db.AspNetLessonPlanBreakdownHeadings.Remove(aspNetLessonPlanBreakdownHeading);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "It can't be deleted";
                return View("Details", aspNetLessonPlanBreakdownHeading);
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
