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
    public class AspNetTermController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetTerm
        public ActionResult Index()
        {
            var aspNetTerms = db.AspNetTerms.Include(a => a.AspNetSession);
            return View(aspNetTerms.ToList());
        }

        public ActionResult Indexs()
        {
            var aspNetTerms = db.AspNetTerms.Include(a => a.AspNetSession);
            ViewBag.Error = "Term Started successfully";
            return View("Index", aspNetTerms.ToList());
        }

        // GET: AspNetTerm/Details/5
        public ActionResult Details(int id)
        {

            //var parent = db.AspNetParents.Where(x => x.AspNetUser.UserName == UserName).Select(x => x).FirstOrDefault();
            //AspNetUser aspNetUser = db.AspNetUsers.Where(x => x.UserName == UserName).Select(x => x).FirstOrDefault();


            AspNetTerm aspNetTerm = db.AspNetTerms.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            if (aspNetTerm == null)
            {
                return HttpNotFound();
            }
            ViewBag.SessionID = new SelectList(db.AspNetSessions, "Id", "SessionName", aspNetTerm.SessionID);
            return View(aspNetTerm);
        }

        // GET: AspNetTerm/Create
        public ActionResult Create()
        {
            var session = new SelectList(db.AspNetSessions, "Id", "SessionName");
            if (session.Count() == 0)
            {
                ViewBag.Error = "Please add a session first";
            }
            else
                ViewBag.SessionID = session;

            return View();
        }

        // POST: AspNetTerm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TermName,SessionID,TermStartDate,TermEndDate,Status")] AspNetTerm aspNetTerm)
        {

            var TransactionObj = db.Database.BeginTransaction();
            try
            {


                if (ModelState.IsValid)
                {
                    var terms = db.AspNetTerms.Where(p => p.SessionID == aspNetTerm.SessionID).Count();
                    aspNetTerm.TermNo = terms + 1;

                    db.AspNetTerms.Add(aspNetTerm);
                    db.SaveChanges();
                }
                TransactionObj.Commit();
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var UserNameLog = User.Identity.Name;
                AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                string UserIDLog = a.Id;

                var logMessage = "New Term Added, TermName: " + aspNetTerm.TermName + ", TermStartDate: " + aspNetTerm.TermStartDate + ", TermEndDate: " + aspNetTerm.TermEndDate + ", Status: " + aspNetTerm.Status;

                var LogControllerObj = new AspNetLogsController();
                LogControllerObj.CreateLogSave(logMessage, UserIDLog);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                return RedirectToAction("Indexs");
            }
            catch
            {
                TransactionObj.Dispose();
            }

            ViewBag.SessionID = new SelectList(db.AspNetSessions, "Id", "SessionName", aspNetTerm.SessionID);
            return RedirectToAction("Index");
        }

        // GET: AspNetTerm/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTerm aspNetTerm = db.AspNetTerms.Find(id);
            if (aspNetTerm == null)
            {
                return HttpNotFound();
            }
            ViewBag.SessionID = new SelectList(db.AspNetSessions, "Id", "SessionName", aspNetTerm.SessionID);
            return View(aspNetTerm);
        }

        // POST: AspNetTerm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TermName,SessionID,TermStartDate,TermEndDate,Status")] AspNetTerm aspNetTerm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetTerm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SessionID = new SelectList(db.AspNetSessions, "Id", "SessionName", aspNetTerm.SessionID);
            return View(aspNetTerm);
        }

        // GET: AspNetTerm/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTerm aspNetTerm = db.AspNetTerms.Find(id);
            if (aspNetTerm == null)
            {
                return HttpNotFound();
            }
            return View(aspNetTerm);
        }

        // POST: AspNetTerm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetTerm aspNetTerm = db.AspNetTerms.Find(id);
            db.AspNetTerms.Remove(aspNetTerm);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteTerm(int id)
        {
            var term = db.AspNetTerms.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            try
            {
                db.AspNetTerms.Remove(term);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                ViewBag.SessionID = new SelectList(db.AspNetSessions, "Id", "SessionName", term.SessionID);
                ViewBag.Error = "This term can't be deleted. It has relation with Assessment";
                return View("Details", term);
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