using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using System.IO;
using System.Web.UI;
using System.Text;
using NReco.PdfGenerator;
using Microsoft.AspNet.Identity;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.parser;

namespace SEA_Application.Controllers
{

    [Authorize(Roles = "Student,Principal")]
    public class Student_DashboardController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        private string StudentID;
        public int SessionID = Convert.ToInt32(SessionIDStaticController.GlobalSessionID);

        public Student_DashboardController()
        {

            StudentID = Convert.ToString(System.Web.HttpContext.Current.Session["StudentID"]);
        }

        // GET: Student_Dashboard
        public ActionResult Dashboard()
        {

            //return RedirectToAction("Index", "StudentCourses");
            return View("BlankPage");
        }


        public ViewResult View_Project()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            

            return View();
        }

        public ActionResult AllSubjectsOfStudent()
        {
            try
            {
                var userID = User.Identity.GetUserId();
                var UserRole = db.GetUserRoleById(userID).FirstOrDefault();
                int ClassID = db.AspNetClasses.Where(x => x.SessionID == SessionID).FirstOrDefault().Id;

                var AllSubjectsOfStudent = from Subject in db.GenericSubjects
                                           join StudentSubject in db.Student_GenericSubjects on Subject.Id equals StudentSubject.GenericSubjectId
                                           where StudentSubject.StudentId == userID
                                           select new
                                           {
                                               Subject.Id,
                                               Subject.SubjectName,

                                           };


                return Json(AllSubjectsOfStudent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Quiz_student_check()
        {
            var student_id = db.AspNetStudents.Where(x => x.StudentID == StudentID).Select(x => x.Id).FirstOrDefault();

            var student_performed_quizes = db.Student_Quiz_Scoring.Where(x => x.StudentId == student_id).Select(x => x.AspnetQuiz.Id).Distinct().ToList();
            var all_quizes = db.AspnetQuizs.Select(x => x.Id).ToList();

            var Unperformed_quizes = all_quizes.Except(student_performed_quizes).ToList();

            return Json(Unperformed_quizes, JsonRequestBehavior.AllowGet);
        }


        public class option
        {
            public int id;
            public string name;
        }

       public class question
        {
            public int id;
            public string name;
            public string type;
            public List<option> options;
        }

        public ViewResult Quizes()
        {
            var today = DateTime.Today.Date;
            var subjects_list = db.AspNetStudent_Subject.Where(x => x.StudentID == StudentID).Select(x => x.SubjectID).ToList();
            //var quizes = db.AspnetQuizs.Where(x => x.Start_Date == today && today <= x.Due_Date && x.Quiz_Topic_Questions.Select(x=> x.AspnetQuestion.AspnetLesson.AspnetSubjectTopic.SubjectId).Contains(subjects_list))).ToList();
            var quizes = db.AspnetQuizs.Where(x => x.Start_Date == today && today <= x.Due_Date ).ToList();
            ViewBag.quizes = quizes;
            return View();
        }

        //Question of a Quiz;
        public ActionResult GetQuestions(int Id)
        {
            var questionList_MCQS = new List<question>();
            var questionList_Fill = new List<question>();
            var Quiz_Questions = db.Quiz_Topic_Questions.Where(x => x.QuizId == Id && x.AspnetQuestion.Type == "MCQ").ToList();

            foreach (var item in Quiz_Questions)
            {
                var q = new question();
                q.id = item.AspnetQuestion.Id;
                q.name = item.AspnetQuestion.Name;
                q.type = item.AspnetQuestion.Type;
                q.options = new List<option>();
                var op = db.AspnetOptions.Where(x => x.QuestionId == q.id).ToList();
                foreach (var item1 in op)
                {
                    var op1 = new option();
                    op1.id = item1.Id;
                    op1.name = item1.Name;
                    q.options.Add(op1);
                }
                questionList_MCQS.Add(q);
            }

            var Quiz_Questions_Fill = db.Quiz_Topic_Questions.Where(x => x.QuizId == Id && x.AspnetQuestion.Type == "Fill").ToList();

            foreach (var item in Quiz_Questions_Fill)
            {
                var q = new question();
                q.id = item.AspnetQuestion.Id;
                q.name = item.AspnetQuestion.Name;
                q.type = item.AspnetQuestion.Type;

                questionList_Fill.Add(q);
            }

            ViewBag.questionList_MCQS = questionList_MCQS;
            ViewBag.questionList_Fill = questionList_Fill;
            ViewBag.QuizId = Id;
            return View();
        }

        public ActionResult StartQuiz_Student(int QuizId)
        {
            try
            {
                var questions = db.Quiz_Topic_Questions.Where(x => x.QuizId == QuizId).Select(x => x.QuestionId).ToList();
                var student_id = db.AspNetStudents.Where(x => x.StudentID == StudentID).Select(x => x.Id).FirstOrDefault();
                foreach (var item in questions)
                {
                    var quiz_student = new Student_Quiz_Scoring();
                    quiz_student.QuizId = QuizId;
                    quiz_student.QuestionId = item;
                    quiz_student.StudentId = student_id;
                    db.Student_Quiz_Scoring.Add(quiz_student);
                }
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                var logs = new AspNetLog();
                logs.UserID = StudentID;
                logs.Operation = "Error while starting the Quiz ->" + ex;
                logs.Time = DateTime.Now;
                db.AspNetLogs.Add(logs);
                db.SaveChanges();
                return Json("Something went Wrong", JsonRequestBehavior.AllowGet);
            }
            
        }
        public class Quiz
        {
            public List<int> OptionId;
            public int QuizId;
            public List<int> QuestionId;
        }
        public ActionResult submit_question(string Question,string  Answer,int QuizID)
        {
            var student_id = db.AspNetStudents.Where(x => x.StudentID == StudentID).Select(x => x.Id).FirstOrDefault();
            int score = 0;
            string[] selectedQuestions = Question.Split(',');
            string[] selectedAnswers = Answer.Split(',');
         
            int i = 0;
            foreach (var item in selectedQuestions)
            {
              var record = db.Student_Quiz_Scoring.Where(x => x.QuizId == QuizID && x.QuestionId.ToString() == item && x.StudentId == student_id).FirstOrDefault();
              var ans = db.AspnetQuestions.Where(x => x.Id.ToString() == item).Select(x => x.AnswerId).FirstOrDefault();
                if(selectedAnswers[i] == ans.ToString())
                {
                    record.Score = "true";
                    score++;
                }
                else
                    record.Score = "false";
                i++;
               db.SaveChanges();
            }
            
            return Content(score.ToString());
        }


        public ViewResult PerformanceReports()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            return View();
        }

        public ActionResult Student_Dairy()
        {
            return RedirectToAction("Index", "AspNetHomeworks");
        }

        public ActionResult PerformanceReport_Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetStudentPerformanceReport aspNetStudentPerformanceReport = db.AspNetStudentPerformanceReports.Where(x => x.Id == id && x.StudentID == StudentID).FirstOrDefault();
            if (aspNetStudentPerformanceReport == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudentPerformanceReport);
        }

        public ActionResult Project_Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetStudent_Project aspNetStudentProject = db.AspNetStudent_Project.Where(x => x.ProjectID == id && x.StudentID == StudentID).FirstOrDefault();
            if (aspNetStudentProject == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudentProject);
        }
        public ViewResult View_Assessment()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            return View();
        }


        public ActionResult Assessment_Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetStudent_Assessment aspNetStudentAssessment = db.AspNetStudent_Assessment.Where(x => x.AssessmentID == id && x.StudentID == StudentID).FirstOrDefault();
            if (aspNetStudentAssessment == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudentAssessment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Student_Assessment_Submission(AspNetStudent_Assessment stu_assessment)
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            AspNetStudent_Assessment result = (from p in db.AspNetStudent_Assessment
                                               where p.Id == id
                                               select p).SingleOrDefault();

            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["document"];
                if (ModelState.IsValid)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/AssessmentSubmission"), fileName);
                        file.SaveAs(path);
                        result.SubmittedFileName = fileName;
                    }
                }


                result.SubmissionStatus = true;
                result.SubmissionDate = DateTime.Now;

                db.SaveChanges();
                dbTransaction.Commit();
            }
            catch (Exception) { dbTransaction.Dispose(); }
            return RedirectToAction("Assessment_Detail", new { id = result.AssessmentID });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Student_Project_Submission(AspNetStudent_Project stu_project)
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            AspNetStudent_Project student_project = (from p in db.AspNetStudent_Project
                                                     where p.Id == id
                                                     select p).SingleOrDefault();

            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["document"];
                if (ModelState.IsValid)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/ProjectsSubmission"), fileName);
                        file.SaveAs(path);
                        student_project.SubmittedFileName = fileName;
                    }
                }


                student_project.SubmissionStatus = true;
                student_project.SubmissionDate = DateTime.Now;

                db.SaveChanges();
                dbTransaction.Commit();
            }
            catch (Exception) { dbTransaction.Dispose(); }
            return RedirectToAction("Project_Detail", new { id = student_project.ProjectID });

        }


        public ViewResult Student_Marks()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            return View("_Student_Marks");
        }

        public ViewResult Student_Attendance()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            return View("_Student_Attendance");
        }


        // GET: Assignment/Details/5
        public ActionResult AnnouncementDetail(int? id)
        {

            var announcementID = db.AspNetAnnouncement_Subject.Where(s => s.Id == id).Select(s => s.AnnouncementID).SingleOrDefault();
            AspNetStudent_Announcement result = db.AspNetStudent_Announcement.Where(x => x.StudentID == StudentID && x.AnnouncementID == announcementID).Select(x => x).FirstOrDefault();

            result.Seen = true;
            db.SaveChanges();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var Announcement = db.AspNetAnnouncement_Subject.Find(id);

            if (Announcement == null)
            {
                return HttpNotFound();
            }
            return View(Announcement);
        }

        public ActionResult CalendarNotification()
        {
            var id = User.Identity.GetUserId();
            var checkdate = DateTime.Now;
            var date = TimeZoneInfo.ConvertTime(DateTime.UtcNow.ToUniversalTime(), TimeZoneInfo.Local);
            var name = "";

            name = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();

            var day = date.DayOfWeek;
            var dd = date.Day;
            var mm = date.Month;
            var yy = date.Year;
            string[] array = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            var Date = day + ", " + dd + " " + array[mm - 1] + " " + yy;
            var result = new { checkdate, Date, name };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public class event1
        {
            public int Id { set; get; }
            public string Name { set; get; }
            public string StartDate { set; get; }
            public string EndDate { set; get; }
            public string StartTime { set; get; }
            public string EndTime { set; get; }
            public string Color { set; get; }
            public string Url { set; get; }

        }
        public JsonResult GetEvents()
        {
            using (SEA_DatabaseEntities dc = new SEA_DatabaseEntities())
            {
                var id = User.Identity.GetUserId();
                var events = dc.Events.Where(x => x.UserId == id || x.IsPublic == true).Select(x => new { x.Description, x.End, x.EventID, x.IsFullDay, x.Subject, x.ThemeColor, x.Start, x.IsPublic }).ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            e.UserId = User.Identity.GetUserId();
            var status = false;
            using (SEA_DatabaseEntities dc = new SEA_DatabaseEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Events.Add(e);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (SEA_DatabaseEntities dc = new SEA_DatabaseEntities())
            {
                var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ViewResult Student_Announcement()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            return View("_Student_Announcement");
        }



        [HttpGet]
        public JsonResult StudentAnnouncement()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var ann2 = (from t1 in db.AspNetAnnouncement_Subject
                        join t3 in db.AspNetStudent_Subject on t1.SubjectID equals t3.SubjectID
                        join t4 in db.AspNetUsers on t3.StudentID equals t4.Id
                        join t2 in db.AspNetStudent_Announcement on t1.AnnouncementID equals t2.AnnouncementID

                        where t2.StudentID == StudentID && t2.Seen == false && t4.Id == StudentID
                        select new { t1.AspNetAnnouncement.Title, t1.Id }).ToList();

            ViewBag.Subjects = ann2;
            return Json(ann2, JsonRequestBehavior.AllowGet);

        }

        public FileResult downloadProjectFile(int id)
        {
            AspNetProject Project = db.AspNetProjects.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/Content/StudentProjects/"), Project.FileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Project.FileName);

        }

        public FileResult downloadStudentSubmittedFile(int id)
        {
            AspNetStudent_Project Student_Project = db.AspNetStudent_Project.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/App_Data/ProjectsSubmission/"), Student_Project.SubmittedFileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Student_Project.SubmittedFileName);

        }

        [HttpGet]
        public JsonResult MarksBySubject(int subjectID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var marks = (from student_assessment in db.AspNetStudent_Assessment
                         where student_assessment.AspNetAssessment.AspNetSubject_Catalog.AspNetSubject.Id == subjectID && student_assessment.StudentID == StudentID
                         group new { student_assessment.AspNetAssessment.Title, student_assessment.AspNetAssessment.DueDate, student_assessment.AspNetAssessment.TotalMarks, student_assessment.MarksGot } by student_assessment.AspNetAssessment.AspNetSubject_Catalog.AspNetCatalog.CatalogName into g
                         select new { Key = g.Key, Marks = g.ToList() }).ToList();

            return Json(marks, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AttendanceBySubject(int subjectID)
        {
            var attendances = (from attendance in db.AspNetStudent_Attendance
                               where attendance.StudentID == StudentID && attendance.AspNetAttendance.SubjectID == subjectID
                               select new { attendance.Reason, attendance.Status, attendance.AspNetAttendance.Date }).ToList();
            return Json(attendances, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AnnouncementBySubject(int subjectID)
        {
            var announcements = (from announcement in db.AspNetAnnouncement_Subject
                                 where announcement.SubjectID == subjectID
                                 select new { announcement.AspNetAnnouncement.Title, announcement.AspNetSubject.SubjectName, announcement.Id }).ToList();
            return Json(announcements, JsonRequestBehavior.AllowGet);
        }


        public ActionResult StudentAssessment()
        {
            try
            {
                var session = db.AspNetSessions.OrderByDescending(x => x.Id).First();
                ViewBag.TermId = new SelectList(db.AspNetTerms.Where(x => x.SessionID == session.Id), "Id", "TermName");      // ).ToList();
            }
            catch
            {
                ViewBag.Error = "No session or term has started yet";
            }

            return View();
        }

        public JsonResult Assessment(int termId)
        {
            string studentId = User.Identity.GetUserId();

            bool Submited_flag;

            try
            {
                var check = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termId && x.StudentID == StudentID && x.Status == "Principle_submit").Select(x => x.Id).ToList();
                if (check.Count == 0)
                    Submited_flag = false;
                else
                    Submited_flag = true;
            }
            catch (Exception ex)
            {
                Submited_flag = false;
            }

            Student_data Assessment_DATA = new Student_data();
            Assessment_DATA.assessment_data = new List<subject>();

            if (Submited_flag)
            {
                var STAID = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termId && x.StudentID == StudentID).Select(x => x.Id).ToList();
                Assessment_DATA.PrincipleComment = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termId && x.StudentID == StudentID).Select(x => x.PrincipalComments).FirstOrDefault();


                var catageory = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.AspNetStudent_Term_Assessment.TermID == termId && x.AspNetStudent_Term_Assessment.StudentID == StudentID).Select(x => x.Catageory).Distinct().ToList();

                foreach (var item in STAID)
                {
                    subject Subject = new subject();
                    Subject.SubjectName = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.STAID == item).Select(x => x.AspNetStudent_Term_Assessment.AspNetSubject.SubjectName).First();
                    Subject.TeacherComment = db.AspNetStudent_Term_Assessment.Where(x => x.Id == item).Select(x => x.TeacherComments).FirstOrDefault();
                    Subject.CatageoryList = new List<Catageory>();

                    foreach (var Catag in catageory)
                    {
                        Catageory Catageory = new Catageory();
                        Catageory.catageoryName = Catag;
                        Catageory.QuestionList = new List<Questions>();

                        var AQID = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.STAID == item && x.Catageory == Catag).ToList();

                        if (AQID.Count != 0)
                        {
                            foreach (var aqid in AQID)
                            {
                                Questions question = new Questions();
                                question.Question = aqid.Question;
                                question.Answer = aqid.Answer;
                                Catageory.QuestionList.Add(question);
                            }
                            Subject.CatageoryList.Add(Catageory);
                        }
                    }

                    Assessment_DATA.assessment_data.Add(Subject);
                }

            }

            return Json(Assessment_DATA, JsonRequestBehavior.AllowGet);


        }

        public class Student_data
        {
            public List<subject> assessment_data { get; set; }
            public string PrincipleComment { set; get; }
        }
        public class subject
        {
            public string SubjectName { set; get; }
            public string TeacherComment { set; get; }
            public List<Catageory> CatageoryList { set; get; }
        }

        public class Catageory
        {
            public string catageoryName { get; set; }
            public List<Questions> QuestionList { get; set; }
        }

        public class Questions
        {
            public string Question { get; set; }
            public string Answer { get; set; }
        }
    }
}