using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    [Authorize]
    public class AspNetLogsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        /*******************************************************************************************/
        
        public ActionResult ViewLogs()
        {
            //var aspNetLogs = db.AspNetLogs.Include(a => a.AspNetUser);
            //return View(aspNetLogs.ToList());

            if (User.IsInRole("Admin") || User.IsInRole("Supervisor")) 
            {
                ViewBag.UserID = new SelectList(db.AspNetUsers.Where(x => (x.AspNetRoles.Select(y => y.Name).Contains("Teacher") || x.AspNetRoles.Select(y => y.Name).Contains("Teacher")) && x.Status != "False"), "UserName", "UserName");
            }
            else if (User.IsInRole("Principal"))
            {
                ViewBag.UserID = new SelectList(db.AspNetUsers, "UserName", "UserName");
            }
           
            return View();
        }

        [HttpGet]
        public JsonResult GetUserLogs(string username)
        {
            try
            {
                var UserObj = db.AspNetUsers.FirstOrDefault(x => x.UserName == username);
                var HistoryLogs = (from HistoryLog in db.AspNetLogs
                                   where HistoryLog.UserID == UserObj.Id
                                   orderby HistoryLog.Id descending
                                   select new { HistoryLog.Id, HistoryLog.Operation, HistoryLog.Time, HistoryLog.AspNetUser.Name, HistoryLog.AspNetUser.Email, HistoryLog.AspNetUser.PhoneNumber }).ToList();
                //ViewBag.curriculums = curriculums;
                return Json(HistoryLogs, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("No users found", JsonRequestBehavior.AllowGet);
            }
            
        }

        /*******************************************************************************************/


        // GET: AspNetLogs
        public ActionResult Index()
        {
            var aspNetLogs = db.AspNetLogs.Include(a => a.AspNetUser);
            return View(aspNetLogs.ToList());
        }

        // GET: AspNetLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLog aspNetLog = db.AspNetLogs.Find(id);
            if (aspNetLog == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLog);
        }
      
        // GET: AspNetLogs/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Operation,Time,UserID")] AspNetLog aspNetLog)
        {
            if (ModelState.IsValid)
            {
                db.AspNetLogs.Add(aspNetLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetLog.UserID);
            return View(aspNetLog);
        }

        // GET: AspNetLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLog aspNetLog = db.AspNetLogs.Find(id);
            if (aspNetLog == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetLog.UserID);
            return View(aspNetLog);
        }

        // POST: AspNetLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Operation,Time,UserID")] AspNetLog aspNetLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetLog.UserID);
            return View(aspNetLog);
        }

        // GET: AspNetLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLog aspNetLog = db.AspNetLogs.Find(id);
            if (aspNetLog == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLog);
        }

        // POST: AspNetLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetLog aspNetLog = db.AspNetLogs.Find(id);
            db.AspNetLogs.Remove(aspNetLog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /**********************************************************************************/

        public void CreateLogSave(string LogOperation, String LogUserID)
        {
            AspNetLog LogObj = new AspNetLog();
            LogObj.Operation = LogOperation;
            LogObj.Time = DateTime.Now;
            LogObj.UserID = LogUserID;
            db.AspNetLogs.Add(LogObj);
            db.SaveChanges();
        }


        /**********************************************************************************/



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
