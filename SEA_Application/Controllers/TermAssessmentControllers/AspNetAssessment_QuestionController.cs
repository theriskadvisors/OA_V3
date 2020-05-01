using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;


namespace SEA_Application.Controllers.TermAssessmentControllers
{
    public class AspNetAssessment_QuestionController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetAssessment_Question
        public ActionResult Index()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
           
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            var aspNetAssessment_Question = db.AspNetAssessment_Question.Include(a => a.AspNetAssessment_Questions_Category).Include(a => a.AspNetSubject);
            return View(aspNetAssessment_Question.ToList());
        }

        // GET: AspNetAssessment_Question/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment_Question aspNetAssessment_Question = db.AspNetAssessment_Question.Find(id);
            if (aspNetAssessment_Question == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName",aspNetAssessment_Question.AspNetSubject.ClassID);
            ViewBag.QuestionCategory = new SelectList(db.AspNetAssessment_Questions_Category, "Id", "CategoryName",aspNetAssessment_Question.QuestionCategory);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName",aspNetAssessment_Question.SubjectID);
            return View(aspNetAssessment_Question);
        }

        // GET: AspNetAssessment_Question/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.QuestionCategory = new SelectList(db.AspNetAssessment_Questions_Category, "Id", "CategoryName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            return View();
        }

        // POST: AspNetAssessment_Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Question,SubjectID,QuestionCategory")] AspNetAssessment_Question aspNetAssessment_Question)
        {
            if (ModelState.IsValid)
            {
                db.AspNetAssessment_Question.Add(aspNetAssessment_Question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.QuestionCategory = new SelectList(db.AspNetAssessment_Questions_Category, "Id", "CategoryName", aspNetAssessment_Question.QuestionCategory);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetAssessment_Question.SubjectID);
            return View(aspNetAssessment_Question);
        }

        public JsonResult AllQuestiones()
        {
            var AllQuestiones = db.AspNetAssessment_Question.Select(x => new { x.Id, x.Question, x.AspNetAssessment_Questions_Category.CategoryName, x.AspNetSubject.SubjectName, x.AspNetSubject.AspNetClass.ClassName });
            return Json(AllQuestiones, JsonRequestBehavior.AllowGet);
        }

        // GET: AspNetAssessment_Question/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment_Question aspNetAssessment_Question = db.AspNetAssessment_Question.Find(id);
            if (aspNetAssessment_Question == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetAssessment_Question.AspNetSubject.ClassID);
            ViewBag.QuestionCategory = new SelectList(db.AspNetAssessment_Questions_Category, "Id", "CategoryName", aspNetAssessment_Question.QuestionCategory);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetAssessment_Question.SubjectID);
            return View(aspNetAssessment_Question);
        }

        // POST: AspNetAssessment_Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Question,SubjectID,QuestionCategory")] AspNetAssessment_Question aspNetAssessment_Question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetAssessment_Question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.QuestionCategory = new SelectList(db.AspNetAssessment_Questions_Category, "Id", "CategoryName", aspNetAssessment_Question.QuestionCategory);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetAssessment_Question.SubjectID);
            return View(aspNetAssessment_Question);
        }

        // GET: AspNetAssessment_Question/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAssessment_Question aspNetAssessment_Question = db.AspNetAssessment_Question.Find(id);
            if (aspNetAssessment_Question == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAssessment_Question);
        }

        // POST: AspNetAssessment_Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetAssessment_Question aspNetAssessment_Question = db.AspNetAssessment_Question.Find(id);
            db.AspNetAssessment_Question.Remove(aspNetAssessment_Question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCnfm(int id)
        {
            AspNetAssessment_Question aspNetAssessment_Question = db.AspNetAssessment_Question.Find(id);
            try
            {
                db.AspNetAssessment_Question.Remove(aspNetAssessment_Question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "It can't be deleted";
                ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetAssessment_Question.SubjectID);
                ViewBag.QuestionCategory = new SelectList(db.AspNetAssessment_Questions_Category, "Id", "CategoryName", aspNetAssessment_Question.QuestionCategory);
                return View("Details", aspNetAssessment_Question);
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

        public JsonResult SubjectsQuestionesByClass(int id)
        {
            var sub = db.AspNetSubjects.Where(r => r.ClassID == id).Select(x=> new { x.Id , x.SubjectName }).OrderByDescending(r => r.Id).ToList();
            var SubjectQuestion = new SubjectQuestion();

            SubjectQuestion.Subjects = new List<Subjects>();

            foreach (var item in sub)
            {
                var subjects = new Subjects();
                subjects.Id = item.Id;
                subjects.SubjectName = item.SubjectName;
                SubjectQuestion.Subjects.Add(subjects);
            }

            var questiones = db.AspNetAssessment_Question.Where(x => x.AspNetSubject.ClassID == id).Select(x => new { x.Id, x.Question, x.AspNetAssessment_Questions_Category.CategoryName, x.AspNetSubject.SubjectName, x.AspNetSubject.AspNetClass.ClassName }).ToList();

            SubjectQuestion.Questiones = new List<Questiones>();

            foreach (var item in questiones)
            {
                var Question = new Questiones();
                Question.Id = item.Id;
                Question.QuestionCatageory = item.CategoryName;
                Question.Subjectname = item.SubjectName;
                Question.ClassName = item.ClassName;
                Question.Question = item.Question;
                SubjectQuestion.Questiones.Add(Question);
            }
            return Json(SubjectQuestion, JsonRequestBehavior.AllowGet);
        }

        public class SubjectQuestion
        {
            public List<Subjects> Subjects {set; get;}
            public List<Questiones> Questiones {set; get;}
        }

        public class Subjects {
            public int Id { set; get; }
            public string SubjectName { set; get; }
        }

        public class Questiones
        {
            public int Id { set; get; }
            public string Subjectname { set; get; }
            public string QuestionCatageory { set; get; }
            public string ClassName { set; get; }
            public string Question { set; get; }
        }

        public JsonResult SubjectsByClass(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<AspNetSubject> sub = db.AspNetSubjects.Where(r => r.ClassID == id).OrderByDescending(r => r.Id).ToList();
            return Json(sub, JsonRequestBehavior.AllowGet);

        }

        
            public JsonResult QuestionBySubject(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var questions = (from question in db.AspNetAssessment_Question
                             where question.SubjectID == id
                             select new { question.Id, question.Question, question.AspNetSubject.AspNetClass.ClassName , question.AspNetSubject.SubjectName, question.AspNetAssessment_Questions_Category.CategoryName }).ToList();
            return Json(questions, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Excel_Data(HttpPostedFileBase excelfile)
        {
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                
                if (excelfile == null || excelfile.ContentLength == 0)
                {
                    TempData["Error"] = "Please select an excel file";
                    return RedirectToAction("Create", "AspNetAssessment_Question");
                }
                else if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {

                    HttpPostedFileBase file = excelfile;   // Request.Files["excelfile"];

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        ApplicationDbContext context = new ApplicationDbContext();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            AspNetAssessment_Question Question = new AspNetAssessment_Question();

                            string ClassName = workSheet.Cells[rowIterator, 1].Text.ToString().ToUpper();
                            string SubjectName = workSheet.Cells[rowIterator, 2].Text.ToString().ToUpper();
                            string Catageory = workSheet.Cells[rowIterator, 3].Text.ToString().ToUpper();

                            int ClassID = db.AspNetClasses.Where(x => x.ClassName.ToUpper() == ClassName).Select(x => x.Id).FirstOrDefault(); ;
                            int subjectID = db.AspNetSubjects.Where(x => x.ClassID == ClassID && x.SubjectName.ToUpper() == SubjectName).Select(x => x.Id).FirstOrDefault();
                            int catageoryID = db.AspNetAssessment_Questions_Category.Where(x => x.CategoryName.ToUpper() == Catageory).Select(x => x.Id).FirstOrDefault();

                            Question.SubjectID = subjectID;
                            Question.QuestionCategory = catageoryID;
                            Question.Question = workSheet.Cells[rowIterator, 4].Text.ToString();
                            db.AspNetAssessment_Question.Add(Question);

                        }
                        db.SaveChanges();
                    }
                    dbTransaction.Commit();
                    return RedirectToAction("Index", "AspNetAssessment_Question");
                }
                else
                {
                    TempData["Error"] = "File type is incorrect";
                    return RedirectToAction("Create", "AspNetAssessment_Question");
                }

            }
            catch
            {
                dbTransaction.Dispose();
                TempData["Error"] = "Incorrect data in file";
                return RedirectToAction("Create", "AspNetAssessment_Question");
            }
        }
    }
}
