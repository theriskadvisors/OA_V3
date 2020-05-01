using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace SEA_Application.Controllers
{
    public class AspNetRolesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetRoles
        public ActionResult Index()
        {
            return View(db.AspNetRoles.ToList());
        }

      
        public ActionResult Attendance()
       {
            return View();
        }
        public ActionResult Time_Setting(TimeSpan absent,TimeSpan late)
        {
          AspNetTime_Setting ts=  db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();           
                ts.AbsentTime = absent;          
                ts.LateTime = late;                       
                db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ab_Time_Setting(TimeSpan absent)
        {
            AspNetTime_Setting ts = db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();
            ts.AbsentTime = absent;
          
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult Lt_Time_Setting(TimeSpan late)
        {
            AspNetTime_Setting ts = db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();
            
            ts.LateTime = late;
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_Time()
        {
            var result=db.AspNetTime_Setting.FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Auto_Attendance(string StdId)
        {        
            
            var d = DateTime.Now;
            var currentdate=d.Date;
            var classname = db.AspNetStudents.Where(x => x.AspNetUser.UserName == StdId).Select(x => x.AspNetClass.ClassName).FirstOrDefault();
            var result = db.AspNetStudent_AutoAttendance.Where(x => x.Roll_Number == StdId && x.Date ==currentdate ).Select(x => x).FirstOrDefault();          
            var datetime = DateTime.Now;
            var timein = datetime.TimeOfDay;
            var timeout = datetime.TimeOfDay;
            if (result==null)
            {
                AspNetStudent_AutoAttendance attendance = new AspNetStudent_AutoAttendance();
                attendance.Roll_Number = StdId;
                attendance.Date = currentdate;
                attendance.TimeIn = timein;
                attendance.Class = classname;
                db.AspNetStudent_AutoAttendance.Add(attendance);
                db.SaveChanges();
            }
            else
            {
              AspNetStudent_AutoAttendance tout=  db.AspNetStudent_AutoAttendance.Where(x => x.Roll_Number == StdId && x.Date ==currentdate).FirstOrDefault();
                tout.TimeOut = timeout;
                db.SaveChanges();
            }

            return View("Attendance");
        }
     
        // GET: AspNetRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // GET: AspNetRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,VirtualId")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                db.AspNetRoles.Add(aspNetRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetRole);
        }

        // GET: AspNetRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,VirtualId")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetRole);
        }

        // GET: AspNetRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            db.AspNetRoles.Remove(aspNetRole);
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
