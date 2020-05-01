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
    public class AspNetLessonPlanBreakdownController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetLessonPlanBreakdown
        public ActionResult Index()
        {
            var aspNetLessonPlanBreakdowns = db.AspNetLessonPlanBreakdowns.Include(a => a.AspNetLessonPlan).Include(a => a.AspNetLessonPlanBreakdownHeading);
            return View(aspNetLessonPlanBreakdowns.ToList());
        }

        // GET: AspNetLessonPlanBreakdown/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlanBreakdown aspNetLessonPlanBreakdown = db.AspNetLessonPlanBreakdowns.Find(id);
            if (aspNetLessonPlanBreakdown == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLessonPlanBreakdown);
        }

        // GET: AspNetLessonPlanBreakdown/Create
        public ActionResult Create()
        {
            ViewBag.LessonPlanID = new SelectList(db.AspNetLessonPlans, "Id", "Id");
            ViewBag.BreakDownHeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName");
            return View();
        }

        // POST: AspNetLessonPlanBreakdown/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LessonPlanID,BreakDownHeadingID,Description,Minutes,Resources,Priority")] AspNetLessonPlanBreakdown aspNetLessonPlanBreakdown)
        {
            if (ModelState.IsValid)
            {
                db.AspNetLessonPlanBreakdowns.Add(aspNetLessonPlanBreakdown);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonPlanID = new SelectList(db.AspNetLessonPlans, "Id", "Id", aspNetLessonPlanBreakdown.LessonPlanID);
            ViewBag.BreakDownHeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName", aspNetLessonPlanBreakdown.BreakDownHeadingID);
            return View(aspNetLessonPlanBreakdown);
        }

        // GET: AspNetLessonPlanBreakdown/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlanBreakdown aspNetLessonPlanBreakdown = db.AspNetLessonPlanBreakdowns.Find(id);
            if (aspNetLessonPlanBreakdown == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonPlanID = new SelectList(db.AspNetLessonPlans, "Id", "Id", aspNetLessonPlanBreakdown.LessonPlanID);
            ViewBag.BreakDownHeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName", aspNetLessonPlanBreakdown.BreakDownHeadingID);

            aspNetLessonPlanBreakdown.Description = WebUtility.HtmlDecode(aspNetLessonPlanBreakdown.Description);
            return View(aspNetLessonPlanBreakdown);
        }

        // POST: AspNetLessonPlanBreakdown/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AspNetLessonPlanBreakdown aspNetLessonPlanBreakdown)
        {
            if (ModelState.IsValid)
            {
                aspNetLessonPlanBreakdown.Description= WebUtility.HtmlEncode(aspNetLessonPlanBreakdown.Description);
                db.Entry(aspNetLessonPlanBreakdown).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("View_LessonPlan","AspNetLessonPlan");
            }
            ViewBag.LessonPlanID = new SelectList(db.AspNetLessonPlans, "Id", "Id", aspNetLessonPlanBreakdown.LessonPlanID);
            ViewBag.BreakDownHeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName", aspNetLessonPlanBreakdown.BreakDownHeadingID);

            aspNetLessonPlanBreakdown.Description = WebUtility.HtmlDecode(aspNetLessonPlanBreakdown.Description);
            return View(aspNetLessonPlanBreakdown);
        }

        public class BreakDownStructure
        {
            public int Id { get; set; }
            public int HeadingID { get; set; }
            public string Description { get; set; }
            public int Minutes { get; set; }
            public string Resources { get; set; }
            public int LessonPlanID { get; set; }
        }


        public ActionResult BreakdownEdit(BreakDownStructure LessonPlan)
        {
            AspNetLessonPlanBreakdown breakdownobj = db.AspNetLessonPlanBreakdowns.FirstOrDefault(x => x.Id == LessonPlan.Id);
            if (ModelState.IsValid)
            {
                breakdownobj.LessonPlanID = LessonPlan.LessonPlanID;
                breakdownobj.BreakDownHeadingID = LessonPlan.HeadingID;
                breakdownobj.Minutes = LessonPlan.Minutes;
                breakdownobj.Resources = LessonPlan.Resources;
                breakdownobj.Description = WebUtility.HtmlEncode(LessonPlan.Description).ToString();
                db.SaveChanges();
                return RedirectToAction("View_LessonPlan", "AspNetLessonPlan");
            }
            ViewBag.LessonPlanID = new SelectList(db.AspNetLessonPlans, "Id", "Id", LessonPlan.LessonPlanID);
            ViewBag.BreakDownHeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName", LessonPlan.HeadingID);
            return View(LessonPlan);
        }

        [HttpGet]
        public JsonResult LessonPlanByBreakdown(int id)
        {
            var breakdownObj = db.AspNetLessonPlanBreakdowns.FirstOrDefault(x => x.Id == id);
            var lessonPlanID = breakdownObj.LessonPlanID;

            return Json(lessonPlanID, JsonRequestBehavior.AllowGet);

        }

        // GET: AspNetLessonPlanBreakdown/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlanBreakdown aspNetLessonPlanBreakdown = db.AspNetLessonPlanBreakdowns.Find(id);
            if (aspNetLessonPlanBreakdown == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLessonPlanBreakdown);
        }

        // POST: AspNetLessonPlanBreakdown/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetLessonPlanBreakdown aspNetLessonPlanBreakdown = db.AspNetLessonPlanBreakdowns.Find(id);
            db.AspNetLessonPlanBreakdowns.Remove(aspNetLessonPlanBreakdown);
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
