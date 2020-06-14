using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using System.Globalization;

namespace SEA_Application.Controllers
{
    public class subjects_mapping
    {
        public int oldId;
        public int newId;
        public string name;
        public int teacherId;
    }
    public class AspNetSessionController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: AspNetSession
        public ActionResult Index()
        {
            return View(db.AspNetSessions.ToList());
        }

        public ActionResult Indexs()
        {
            ViewBag.Error = "Session Started successfully";
            return View("Index", db.AspNetSessions.ToList());
        }

        // GET: AspNetSession/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSession aspNetSession = db.AspNetSessions.Find(id);
            if (aspNetSession == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSession);
        }

        // GET: AspNetSession/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Password()
        {
            return View(db.ruffdatas.ToList());
        }

        // POST: AspNetSession/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SessionName,SessionStartDate,SessionEndDate,Status,Total_Fee")] AspNetSession aspNetSession)
        {

            var TransactionObj = db.Database.BeginTransaction();

            if (ModelState.IsValid)
            {

                aspNetSession.SessionEndDate = Convert.ToDateTime(Request.Form["SessionEndDate"]);
                aspNetSession.SessionStartDate = Convert.ToDateTime(Request.Form["SessionStartDate"]);
                aspNetSession.Total_Fee = aspNetSession.Total_Fee;
                db.AspNetSessions.Add(aspNetSession);
                db.SaveChanges();
            }


            TransactionObj.Commit();
            ////////////////////////////////////////////////////////Term Add/////////////////////////////////////////////////////////////////
            //int length = 3;
            //for (int i = 0; i < length; i++)
            //{
            //    AspNetTerm aspnetTerm = new AspNetTerm();
            //    aspnetTerm.SessionID = aspNetSession.Id;
            //    aspnetTerm.TermName = "Term " + (i + 1);
            //    aspnetTerm.TermStartDate = DateTime.Now;
            //    aspnetTerm.TermEndDate = DateTime.Now;
            //    aspnetTerm.Status = "InActive";
            //    db.AspNetTerms.Add(aspnetTerm);
            //    db.SaveChanges();
            //}

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var UserNameLog = User.Identity.Name;
            AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
            string UserIDLog = a.Id;
            var logMessage = "New Session Added, SessionName: " + aspNetSession.SessionName + ", SessionStartDate: " + aspNetSession.SessionStartDate + ", SessionEndDate: " + aspNetSession.SessionEndDate + ", Status: " + aspNetSession.Status;

            var LogControllerObj = new AspNetLogsController();
            LogControllerObj.CreateLogSave(logMessage, UserIDLog);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            AspNetClass Class = new AspNetClass();

            var CurrentSession = db.AspNetSessions.OrderByDescending(x => x.Id).FirstOrDefault();

            Class.ClassName = CurrentSession.SessionName;

            Class.Class = CurrentSession.SessionName;
            Class.SessionID = CurrentSession.Id;
            db.AspNetClasses.Add(Class);
            db.SaveChanges();

            //Create Subjects to New Session

            var AllSubjectsofTestSession = db.AspNetSubjects.Where(x => x.ClassID == 1).ToList();

            List<subjects_mapping> list = new List<subjects_mapping>();

            foreach (AspNetSubject Subject in AllSubjectsofTestSession)
            {
                subjects_mapping sub = new subjects_mapping();
                sub.oldId = Subject.Id;
                sub.name = Subject.SubjectName;

                AspNetSubject NewSubjectForSession = new AspNetSubject();
               
                NewSubjectForSession = Subject;

                NewSubjectForSession.ClassID = Class.Id;
                NewSubjectForSession.TeacherID = null;

                db.AspNetSubjects.Add(NewSubjectForSession);
                db.SaveChanges();

                sub.newId = NewSubjectForSession.Id;

                list.Add(sub);
            }


          var AllTeacherSubjects = db.AspNetTeacherSubjects.Where(x => x.AspNetSubject.ClassID == 1).ToList();

            foreach (var item in AllTeacherSubjects)
            {
                foreach (var item1 in list)
                {
                    if(item1.oldId == item.SubjectID)
                    {
                        AspNetTeacherSubject TS = new AspNetTeacherSubject();
                        TS.SubjectID = item1.newId;
                        TS.TeacherID = item.TeacherID;
                        db.AspNetTeacherSubjects.Add(TS);
                        break;
                    }
                    
                }
            }

            db.SaveChanges();


            var UserIDs = (from teacher in db.AspNetUsers.Where(x => x.Status != "False")
                           join t2 in db.AspNetUsers_Session
                           on teacher.Id equals t2.UserID
                           join t3 in db.AspNetEmployees
                           on teacher.Id equals t3.UserId
                           where teacher.AspNetRoles.Select(y => y.Name).Contains("Teacher") && t2.AspNetSession.AspNetClasses.Any(x => x.Id == 1) /*&& db.AspNetChapters.Any(x=>x.Id==id)*/
                           select new
                           {
                               TeacherId = teacher.Id,
                               EmployeeId = t3.Id,

                                
                           }).ToList();

                foreach (var Id in UserIDs)
                {
                string IdString = Id.TeacherId.ToString();

                AspNetUsers_Session UserSession = new AspNetUsers_Session();

                UserSession.SessionID = aspNetSession.Id;
                UserSession.UserID = IdString;

                db.AspNetUsers_Session.Add(UserSession);
                db.SaveChanges();
                
                  }


            foreach( var Id in UserIDs)
            {
                
                Aspnet_Employee_Session EmployeeSession = new Aspnet_Employee_Session();

                
                EmployeeSession.Emp_Id = Id.EmployeeId;
                EmployeeSession.Session_Id = aspNetSession.Id;

               db.Aspnet_Employee_Session.Add(EmployeeSession);
                db.SaveChanges();

            }

            return RedirectToAction("Indexs");

        }

        // GET: AspNetSession/Edit/5

        public ActionResult CheckStatus(string status)
        {
            string status1 = "error";
            if (status == "Active")
            {
                if (db.AspNetSessions.Where(x => x.Status == "Active").Count() == 0)
                {
                    status1 = "success";
                }

            }
            return Content(status1);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSession aspNetSession = db.AspNetSessions.Find(id);
            if (aspNetSession == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSession);
        }

        // POST: AspNetSession/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SessionName,SessionStartDate,SessionEndDate,Status,Total_Fee")] AspNetSession aspNetSession)
        {
            if (ModelState.IsValid)
            {

                db.Entry(aspNetSession).State = EntityState.Modified;
                db.SaveChanges();

                //Change Class Name of Session Name

                var ClassToModify  = db.AspNetClasses.Where(x => x.SessionID == aspNetSession.Id).FirstOrDefault();
                ClassToModify.ClassName = aspNetSession.SessionName;
                ClassToModify.Class = aspNetSession.SessionName;

                db.SaveChanges();

                return RedirectToAction("Index");

            }
            return View(aspNetSession);
        }

        // GET: AspNetSession/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSession aspNetSession = db.AspNetSessions.Find(id);
            if (aspNetSession == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSession);
        }
        public ActionResult DeleteSession(int id)
        {
            var session = db.AspNetSessions.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            try
            {
                db.AspNetSessions.Remove(session);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                ViewBag.Error = "This session has terms in it. It can't be deleted";
                return View("Details", session);
            }
        }

        // POST: AspNetSession/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetSession aspNetSession = db.AspNetSessions.Find(id);
            db.AspNetSessions.Remove(aspNetSession);
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