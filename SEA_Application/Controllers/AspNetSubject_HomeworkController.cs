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
    public class AspNetSubject_HomeworkController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetSubject_Homework
        public ActionResult Index(int HomeWorkId)
        {
            ViewBag.HomeworkID =db.AspNetHomeworks.Where(x => x.Id == HomeWorkId).FirstOrDefault().PrincipalApproved_status;

            var aspNetSubject_Homework = db.AspNetSubject_Homework.Where(x=>x.HomeworkID== HomeWorkId).Include(a => a.AspNetSubject);
            var aspNetSomework = db.AspNetSubject_Homework.Where(x => x.HomeworkID == HomeWorkId).FirstOrDefault();
            return View(aspNetSubject_Homework.ToList());
        }

        public ActionResult ApproveDiary(int HomeWorkId)
        {
            var stid = db.AspNetStudent_HomeWork.Where(p => p.HomeworkID == HomeWorkId).FirstOrDefault().StudentID;
            var clasid = db.AspNetStudents.Where(p => p.StudentID == stid).FirstOrDefault().ClassID;

            var gh = db.AspNetStudents.Where(p => p.ClassID == clasid).ToList();
            List<string> ParentList = new List<string>();

            foreach(var g in gh)
            {
                var parent = db.AspNetParent_Child.Where(p => p.ChildID == g.StudentID).FirstOrDefault();
                if (parent != null)
                {
                    ParentList.Add(parent.ParentID);
                }
                else
                {
                    var mee = "This student id don't have parent " + g.StudentID + " NGS Portal";
                    Utility ob = new Utility();
                    ob.messagetosupport(mee);
                }
            }


            Utility obtj = new Utility();
            AspNetMessage oob = new AspNetMessage();

            oob.Message = "Dear Parents, Please check the assigned homework to your child on portal. IPC Aziz Avenue Campus";
            obtj.SendSMS(oob, ParentList);


            //var obje = db.AspNetHomeworks.Where(x => x.Id == HomeWorkId).FirstOrDefault();
            //obje.PrincipalApproved_status = false;

            //db.Entry(obje).State = EntityState.Modified;
            //db.SaveChanges();

            //var ty = db.AspNetHomeworks.Where(x => x.Id == HomeWorkId).FirstOrDefault().PrincipalApproved_status;

            var obj = db.AspNetHomeworks.Where(x => x.Id == HomeWorkId).FirstOrDefault();
            obj.PrincipalApproved_status = "Approved";

            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();

            var tyu = db.AspNetHomeworks.Where(x => x.Id == HomeWorkId).FirstOrDefault().PrincipalApproved_status;

            var result1 = new { status = "success", Value = tyu };
            return Json(result1, JsonRequestBehavior.AllowGet);

        }
        public ActionResult RejectDiary(string HomeWorkId)
        {
            //var obje = db.AspNetHomeworks.Where(x => x.Id == HomeWorkId).FirstOrDefault();
            //obje.PrincipalApproved_status = false;

            //db.Entry(obje).State = EntityState.Modified;
            //db.SaveChanges();

            //var ty = db.AspNetHomeworks.Where(x => x.Id == HomeWorkId).FirstOrDefault().PrincipalApproved_status;
            int neww = Convert.ToInt32(HomeWorkId);
            var obj = db.AspNetHomeworks.Where(x => x.Id == neww).FirstOrDefault();
            obj.PrincipalApproved_status = "Rejected";

            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();

            var tyu = db.AspNetHomeworks.Where(x => x.Id == neww).FirstOrDefault().PrincipalApproved_status;

            var result1 = new { status = "success", Value = tyu };
            return Json(result1, JsonRequestBehavior.AllowGet);

        }


        // GET: AspNetSubject_Homework/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSubject_Homework aspNetSubject_Homework = db.AspNetSubject_Homework.Find(id);
            if (aspNetSubject_Homework == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSubject_Homework);
        }

        // GET: AspNetSubject_Homework/Create
        public ActionResult Create()
        {
            
            ViewBag.HomeworkID = new SelectList(db.AspNetHomeworks, "Id", "Id");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            return View();
        }

        // POST: AspNetSubject_Homework/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubjectID,HomeworkDetail,HomeworkID")] AspNetSubject_Homework aspNetSubject_Homework)
        {
            if (ModelState.IsValid)
            {
                db.AspNetSubject_Homework.Add(aspNetSubject_Homework);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomeworkID = new SelectList(db.AspNetHomeworks, "Id", "Id", aspNetSubject_Homework.HomeworkID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetSubject_Homework.SubjectID);
            return View(aspNetSubject_Homework);
        }

        // GET: AspNetSubject_Homework/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSubject_Homework aspNetSubject_Homework = db.AspNetSubject_Homework.Find(id);
            if (aspNetSubject_Homework == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeworkID = new SelectList(db.AspNetHomeworks, "Id", "Id", aspNetSubject_Homework.HomeworkID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetSubject_Homework.SubjectID);
            return View(aspNetSubject_Homework);
        }

        // POST: AspNetSubject_Homework/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubjectID,HomeworkDetail,HomeworkID")] AspNetSubject_Homework aspNetSubject_Homework)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetSubject_Homework).State = EntityState.Modified;
                db.SaveChanges();
                var obj = db.AspNetHomeworks.Find(aspNetSubject_Homework.HomeworkID);
                obj.PrincipalApproved_status = "Created";
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { HomeWorkId = aspNetSubject_Homework.HomeworkID });
            }
            ViewBag.HomeworkID = new SelectList(db.AspNetHomeworks, "Id", "Id", aspNetSubject_Homework.HomeworkID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetSubject_Homework.SubjectID);
            return View(aspNetSubject_Homework);
        }

        // GET: AspNetSubject_Homework/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSubject_Homework aspNetSubject_Homework = db.AspNetSubject_Homework.Find(id);
            if (aspNetSubject_Homework == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSubject_Homework);
        }

        // POST: AspNetSubject_Homework/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetSubject_Homework aspNetSubject_Homework = db.AspNetSubject_Homework.Find(id);
            db.AspNetSubject_Homework.Remove(aspNetSubject_Homework);
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
