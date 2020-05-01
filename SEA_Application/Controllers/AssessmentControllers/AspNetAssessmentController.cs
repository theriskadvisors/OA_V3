using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.EnterpriseServices;

namespace SEA_Application.Controllers.AssessmentControllers
{
    public class AspNetAssessmentController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetAssessment
        public ActionResult Index()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id }, "Id", "CatalogName");

            return View();
        }


        public ViewResult Assessment_Submission()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id }, "Id", "CatalogName");

            return View();
        }

        public ViewResult Assessment_Marks()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id }, "Id", "CatalogName");

            return View();
        }
        

        // GET: AspNetAssessment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment aspNetAssessment = db.AspNetAssessments.Find(id);
            if (aspNetAssessment == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAssessment);
        }

        // GET: AspNetAssessment/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id },"Id","CatalogName");
            return View();
        }


        // POST: AspNetAssessment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AspNetAssessment aspNetAssessment)
        {
            HttpPostedFileBase file = Request.Files["attachment"];
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Assessments"), fileName);
                    file.SaveAs(path);
                    aspNetAssessment.FileName = fileName;
                }
                else
                {
                    aspNetAssessment.FileName = "-/-";
                }
                db.AspNetAssessments.Add(aspNetAssessment);
                db.SaveChanges();

                int AssessmentID = db.AspNetAssessments.Max(x => x.Id);
                int subjectID = Convert.ToInt32(db.AspNetSubject_Catalog.Where(x => x.Id == aspNetAssessment.Subject_CatalogID).Select(x => x.SubjectID).FirstOrDefault());
                List<string> StudentIDs = db.AspNetStudent_Subject.Where(s => s.SubjectID == subjectID).Select(s => s.StudentID).ToList();
                foreach (var item in StudentIDs)
                {
                    AspNetStudent_Assessment student_assessment = new AspNetStudent_Assessment();
                    student_assessment.StudentID = item;
                    student_assessment.AssessmentID = AssessmentID;
                    student_assessment.MarksGot = -1;
                    student_assessment.SubmissionStatus = false;
                    student_assessment.SubmittedFileName = "-/-";
                    db.AspNetStudent_Assessment.Add(student_assessment);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id }, "Id", "CatalogName");
            return View(aspNetAssessment);
        }

        // GET: AspNetAssessment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment aspNetAssessment = db.AspNetAssessments.Find(id);
            if (aspNetAssessment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id }, "Id", "CatalogName");
            return View(aspNetAssessment);
        }

        // POST: AspNetAssessment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject_CatalogID,Title,Description,PublishDate,DueDate,TotalMarks,Weightage,FileName,AcceptSubmission")] AspNetAssessment aspNetAssessment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetAssessment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id }, "Id", "CatalogName");
            return View(aspNetAssessment);
        }

        // GET: AspNetAssessment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment aspNetAssessment = db.AspNetAssessments.Find(id);
            if (aspNetAssessment == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAssessment);
        }

        // POST: AspNetAssessment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetAssessment aspNetAssessment = db.AspNetAssessments.Find(id);
            db.AspNetAssessments.Remove(aspNetAssessment);
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

        public JsonResult CatalogBySubject(int SubjectID)
        {
            var Catalog = (from catalog in db.AspNetCatalogs
                           join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                           where subject_catalog.SubjectID == SubjectID
                           select new { catalog.CatalogName, subject_catalog.Id }).ToList();
            return Json(Catalog, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AssessmentBySubject(int CatalogID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var assessments = (from assessment in db.AspNetAssessments
                               where assessment.Subject_CatalogID == CatalogID
                               select assessment).ToList();
            return Json(assessments, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AssessmentByType(int CatalogID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var assessments = (from assessment in db.AspNetAssessments
                               where assessment.Subject_CatalogID == CatalogID && assessment.AcceptSubmission==true
                               select assessment).ToList();
            return Json(assessments, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public JsonResult SubmissionByAssessment(int AssessmentID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var assessments = (from assessmentsubmission in db.AspNetStudent_Assessment
                               where assessmentsubmission.AssessmentID == AssessmentID
                               select new { assessmentsubmission, assessmentsubmission.AspNetUser.Name,assessmentsubmission.AspNetAssessment.AcceptSubmission }).ToList();

            return Json(assessments, JsonRequestBehavior.AllowGet);
        }

        public class Marks
        {
            public int Id { get; set; }
            public int GotMark { get; set; }

        }


        [HttpPost]
        public void SaveAssessmentMarks(List<Marks> marks)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            int errorItem = 0;
            try
            {
                foreach (var item in marks)
                {
                    errorItem = item.Id;
                    AspNetStudent_Assessment student_assessment = new AspNetStudent_Assessment();
                    student_assessment.Id = item.Id;
                    student_assessment.MarksGot = item.GotMark;
                    var check = db.AspNetStudent_Assessment.Any(x => x.Id == student_assessment.Id);
                    if (check)
                    {
                        AspNetStudent_Assessment student_assess = (from x in db.AspNetStudent_Assessment
                                                                   where x.Id == student_assessment.Id
                                                                 select x).First();
                        student_assess.MarksGot = student_assessment.MarksGot;
                    }
                    db.SaveChanges();
                }
                dbContextTransaction.Commit();

            }
            catch (Exception)
            {
                dbContextTransaction.Dispose();


            }

        }

        public FileResult downloadAssessmentFile(int id)
        {
            AspNetAssessment Assessment = db.AspNetAssessments.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/App_Data/Assessments/"), Assessment.FileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Assessment.FileName);

        }

        public FileResult downloadStudentSubmittedFile(int id)
        {
            AspNetStudent_Assessment Student_Assessment = db.AspNetStudent_Assessment.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/App_Data/AssessmentSubmission/"), Student_Assessment.SubmittedFileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Student_Assessment.SubmittedFileName);

        }

    }
}
