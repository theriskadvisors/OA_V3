using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using SEA_Application.Models;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;

namespace SEA_Application.Controllers
{
    public class AspnetLessonsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspnetLessons
        public ActionResult Index()
        {
            var aspnetLessons = db.AspnetLessons.Include(a => a.AspnetSubjectTopic);
            return View(aspnetLessons.ToList());
        }

        // GET: AspnetLessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetLesson aspnetLesson = db.AspnetLessons.Find(id);
            if (aspnetLesson == null)
            {
                return HttpNotFound();
            }
            return View(aspnetLesson);
        }

        // GET: AspnetLessons/Create

        public ActionResult LoadSectionIdDDL()
        {
            var ClassList = db.AspNetSessions.ToList().Select(x => new { x.Id, x.SessionName });

            string status = Newtonsoft.Json.JsonConvert.SerializeObject(ClassList);

            // return Json(SubjectsByClass, JsonRequestBehavior.AllowGet);
            return Content(status);

        }

        public ActionResult Create()
        {
            ViewBag.TopicId = new SelectList(db.AspnetSubjectTopics, "Id", "Name");

            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }

        // POST: AspnetLessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LessonViewModel LessonViewModel)
        {

            AspnetLesson Lesson = new AspnetLesson();

            Lesson.Name = LessonViewModel.LessonName;
            Lesson.Video_Url = LessonViewModel.LessonVideoURL;
            Lesson.TopicId = LessonViewModel.TopicId;
            Lesson.DurationMinutes = LessonViewModel.LessonDuration;
            Lesson.IsActive = LessonViewModel.IsActive;
            Lesson.CreationDate = LessonViewModel.CreationDate;
            Lesson.Description = LessonViewModel.LessonDescription;
            Lesson.CreationDate = DateTime.Now;

            string EncrID = Lesson.Name + Lesson.Description + Lesson.Id;

            Lesson.EncryptedID = Encrpt.Encrypt(EncrID, true);


            var newString = Regex.Replace(Lesson.EncryptedID, @"[^0-9a-zA-Z]+", "s");

            // Lesson.EncryptedID.Replace('/', 's').Replace('-','s').Replace('+','s').Replace('%','s').Replace('&','s');

            Lesson.EncryptedID = newString;
            db.AspnetLessons.Add(Lesson);
            db.SaveChanges();



            //Lesson_Session lessonSession = new Lesson_Session();
            //lessonSession.LessonId = Lesson.Id;
            //lessonSession.SessionId = LessonViewModel.SessionId;
            //lessonSession.StartDate = LessonViewModel.StartDate;
            //lessonSession.DueDate = LessonViewModel.DueDate;
            //db.Lesson_Session.Add(lessonSession);

            //db.SaveChanges();



            HttpPostedFileBase Assignment = Request.Files["Assignment"];
            HttpPostedFileBase Attachment1 = Request.Files["Attachment1"];
            HttpPostedFileBase Attachment2 = Request.Files["Attachment2"];
            HttpPostedFileBase Attachment3 = Request.Files["Attachment3"];

            if (Assignment.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Assignment.FileName);
                Assignment.SaveAs(Server.MapPath("~/Content/StudentAssignments/") + fileName);
                AspnetStudentAssignment studentAssignment = new AspnetStudentAssignment();

                studentAssignment.FileName = fileName;

                studentAssignment.Name = LessonViewModel.AssignmentName;


                string DueDate = Convert.ToString(LessonViewModel.AssignmentDueDate);


                if (DueDate == "1/1/0001 12:00:00 AM")
                {
                    studentAssignment.DueDate = null;

                }
                else
                {

                    studentAssignment.DueDate = LessonViewModel.AssignmentDueDate;

                }


                studentAssignment.Description = LessonViewModel.AssignmentDescription;
                studentAssignment.CreationDate = DateTime.Now;
                studentAssignment.LessonId = Lesson.Id;

                db.AspnetStudentAssignments.Add(studentAssignment);
                db.SaveChanges();


            }

            if (Attachment1.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Attachment1.FileName);
                Attachment1.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName);

                AspnetStudentAttachment studentAttachment1 = new AspnetStudentAttachment();

                studentAttachment1.Name = LessonViewModel.AttachmentName1;
                studentAttachment1.Path = fileName;
                studentAttachment1.CreationDate = DateTime.Now;
                studentAttachment1.LessonId = Lesson.Id;
                db.AspnetStudentAttachments.Add(studentAttachment1);
                db.SaveChanges();


            }
            if (Attachment2.ContentLength > 0)
            {

                var fileName = Path.GetFileName(Attachment2.FileName);
                Attachment2.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName);

                AspnetStudentAttachment studentAttachment2 = new AspnetStudentAttachment();

                studentAttachment2.Name = LessonViewModel.AttachmentName2;
                studentAttachment2.Path = fileName;
                studentAttachment2.CreationDate = DateTime.Now;
                studentAttachment2.LessonId = Lesson.Id;
                db.AspnetStudentAttachments.Add(studentAttachment2);

                db.SaveChanges();

            }

            if (Attachment3.ContentLength > 0)
            {

                var fileName = Path.GetFileName(Attachment3.FileName);
                Attachment3.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName);

                AspnetStudentAttachment studentAttachment3 = new AspnetStudentAttachment();

                studentAttachment3.Name = LessonViewModel.AttachmentName3;
                studentAttachment3.Path = fileName;
                studentAttachment3.CreationDate = DateTime.Now;
                studentAttachment3.LessonId = Lesson.Id;
                db.AspnetStudentAttachments.Add(studentAttachment3);
                db.SaveChanges();

            }

            if (LessonViewModel.LinkUrl1 != null)
            {
                AspnetStudentLink link1 = new AspnetStudentLink();

                link1.URL = LessonViewModel.LinkUrl1;
                link1.CreationDate = DateTime.Now;
                link1.LessonId = Lesson.Id;
                db.AspnetStudentLinks.Add(link1);
                db.SaveChanges();
            }

            if (LessonViewModel.LinkUrl2 != null)
            {
                AspnetStudentLink link2 = new AspnetStudentLink();

                link2.URL = LessonViewModel.LinkUrl2;
                link2.CreationDate = DateTime.Now;
                link2.LessonId = Lesson.Id;
                db.AspnetStudentLinks.Add(link2);
                db.SaveChanges();
            }


            if (LessonViewModel.LinkUrl3 != null)
            {
                AspnetStudentLink link3 = new AspnetStudentLink();

                link3.URL = LessonViewModel.LinkUrl3;
                link3.CreationDate = DateTime.Now;
                link3.LessonId = Lesson.Id;
                db.AspnetStudentLinks.Add(link3);
                db.SaveChanges();
            }



            return RedirectToAction("ViewTopicsAndLessons", "AspnetSubjectTopics");

        }
        public JsonResult SessionByLesson(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var subs = (from session in db.Lesson_Session
                        where session.LessonId == id
                        select new { session.SessionId, session.LessonId }).ToList();


            return Json(subs, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LessonSessionView()
        {


            return View("LessonSessionView");
        }

        public ActionResult GetLessonSessions()
        {
            var AllLessonSessions = from lesson in db.AspnetLessons
                                    join lessonsesion in db.Lesson_Session on lesson.Id equals lessonsesion.LessonId
                                    select new
                                    {
                                        lessonsesion.Id,
                                        lesson.Name,
                                        lessonsesion.AspNetSession.SessionName,
                                        lessonsesion.StartDate,
                                        lessonsesion.DueDate

                                    };


            return Json(AllLessonSessions, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditLessonSession(int id)
        {

            ViewBag.Id = id;

            var LS = db.Lesson_Session.Where(x => x.Id == id).FirstOrDefault();

            if (LS != null)
            {


                int? TopicId = db.AspnetLessons.Where(x => x.Id == LS.LessonId).FirstOrDefault().TopicId;

                int? SubjectId = db.AspnetSubjectTopics.Where(x => x.Id == TopicId).FirstOrDefault().SubjectId;

                string SubjectType = db.GenericSubjects.Where(x => x.Id == SubjectId).FirstOrDefault().SubjectType;

                //   ViewBag.SubId = new SelectList(db.GenericSubjects.Where(x => x.SubjectType == SubjectType), "Id", "SubjectName", SubjectId);


                ViewBag.SubId = new SelectList(db.GenericSubjects.Where(x => x.SubjectType == SubjectType), "Id", "SubjectName", SubjectId);
                ViewBag.TopicId = new SelectList(db.AspnetSubjectTopics.Where(x => x.SubjectId == SubjectId), "Id", "Name", TopicId);
                ViewBag.LessonId = new SelectList(db.AspnetLessons.Where(x => x.TopicId == TopicId), "Id", "Name", LS.LessonId);

                ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName", LS.SessionId);

                ViewBag.CTId = SubjectType;



                var StartDate = Convert.ToDateTime(LS.StartDate);

                var StartDateInString = StartDate.ToString("yyyy-MM-dd");

                ViewBag.LessonStartDate = StartDateInString;

                ////Due Date
                var DueDate = Convert.ToDateTime(LS.DueDate);

                var DueDateInString = DueDate.ToString("yyyy-MM-dd");


                ViewBag.LessonDueDate = DueDateInString;



            }






            return View();
        }
        [HttpPost]
        public ActionResult EditLessonSession(int SessionId, int TopicId)
        {


            var LessonSessionId = Convert.ToInt64(Request.Form["LessonSessionId"]);

            var LessonSessionToDelete = db.Lesson_Session.Where(x => x.Id == LessonSessionId).FirstOrDefault();

            db.Lesson_Session.Remove(LessonSessionToDelete);
            db.SaveChanges();



            var SessionId1 = Request.Form["SessionId"];
            var LessonId = Request.Form["LessonId"];
            var StartDate = Request.Form["StartDate"];
            var DueDate = Request.Form["DueDate"];



            Lesson_Session ls = new Lesson_Session();

            ls.LessonId = Convert.ToInt32(LessonId);
            ls.SessionId = Convert.ToInt32(SessionId1);
            ls.StartDate = Convert.ToDateTime(StartDate);
            ls.DueDate = Convert.ToDateTime(DueDate);


            db.Lesson_Session.Add(ls);
            db.SaveChanges();

            return RedirectToAction("LessonSessionView");






        }

        public ActionResult CreateLessonSession()
        {

            ViewBag.TopicId = new SelectList(db.AspnetSubjectTopics, "Id", "Name");

            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");

            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name");



            return View();

        }
        [HttpPost]
        public ActionResult CreateLessonSession(int SessionId)
        {

            var SessionId1 = Request.Form["SessionId"];
            var LessonId = Request.Form["LessonId"];
            var StartDate = Request.Form["StartDate"];
            var DueDate = Request.Form["DueDate"];



            Lesson_Session ls = new Lesson_Session();

            ls.LessonId = Convert.ToInt32(LessonId);
            ls.SessionId = Convert.ToInt32(SessionId1);
            ls.StartDate = Convert.ToDateTime(StartDate);
            ls.DueDate = Convert.ToDateTime(DueDate);


            db.Lesson_Session.Add(ls);
            db.SaveChanges();

            return RedirectToAction("LessonSessionView");
        }



        // GET: AspnetLessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspnetLesson aspnetLesson = db.AspnetLessons.Find(id);
            if (aspnetLesson == null)
            {
                return HttpNotFound();
            }

            AspnetStudentAssignment studentAssignment = db.AspnetStudentAssignments.Where(x => x.LessonId == aspnetLesson.Id).FirstOrDefault();
            List<AspnetStudentAttachment> studentAttachments = db.AspnetStudentAttachments.Where(x => x.LessonId == aspnetLesson.Id).ToList();
            List<AspnetStudentLink> studentLinks = db.AspnetStudentLinks.Where(x => x.LessonId == aspnetLesson.Id).ToList();
            LessonViewModel lessonViewModel = new LessonViewModel();
            lessonViewModel.LessonDescription = aspnetLesson.Description;
            lessonViewModel.LessonVideoURL = aspnetLesson.Video_Url;
            lessonViewModel.LessonName = aspnetLesson.Name;
            lessonViewModel.LessonDuration = aspnetLesson.DurationMinutes;

            //    Lesson_Session LessonSession = db.Lesson_Session.Where(x => x.LessonId == id).FirstOrDefault();

            lessonViewModel.IsActive = Convert.ToBoolean(aspnetLesson.IsActive);

            //var StartDate = Convert.ToDateTime(LessonSession.StartDate);

            //var StartDateInString = StartDate.ToString("yyyy-MM-dd");

            //ViewBag.LessonStartDate = StartDateInString;

            ////Due Date
            //var DueDate = Convert.ToDateTime(LessonSession.DueDate);

            //var DueDateInString = DueDate.ToString("yyyy-MM-dd");


            //ViewBag.LessonDueDate = DueDateInString;


            int? TopicId = aspnetLesson.TopicId;

            ViewBag.LessonDuration = aspnetLesson.DurationMinutes;

            int? SubjectId = db.AspnetSubjectTopics.Where(x => x.Id == TopicId).FirstOrDefault().SubjectId;
            GenericSubject Subject = db.GenericSubjects.Where(x => x.Id == SubjectId).FirstOrDefault();



            var CourseType = Subject.SubjectType;

            lessonViewModel.Id = aspnetLesson.Id;
            if (studentAssignment != null)
            {
                lessonViewModel.AssignmentName = studentAssignment.Name;
                lessonViewModel.AssignmentDescription = studentAssignment.Description;
                DateTime Date = Convert.ToDateTime(studentAssignment.DueDate);
                string date = Date.ToString("yyyy-MM-dd");

                ViewBag.AssignmentFileName = studentAssignment.FileName;

                lessonViewModel.AssignmentDueDate = studentAssignment.DueDate;
                ViewBag.Date = date;


            }


            int count = 1;

            foreach (var link in studentLinks)
            {

                if (count == 1)
                {

                    lessonViewModel.LinkUrl1 = link.URL;

                }
                else if (count == 2)
                {

                    lessonViewModel.LinkUrl2 = link.URL;

                }
                else if (count == 3)
                {
                    lessonViewModel.LinkUrl3 = link.URL;


                }
                else
                {

                }

                count++;

            }

            count = 1;
            foreach (var attachment in studentAttachments)
            {

                if (count == 1)
                {

                    lessonViewModel.AttachmentName1 = attachment.Name;
                    ViewBag.Attachment1FileName = attachment.Path;

                }
                else if (count == 2)
                {

                    lessonViewModel.AttachmentName2 = attachment.Name;
                    ViewBag.Attachment2FileName = attachment.Path;


                }
                else if (count == 3)
                {
                    lessonViewModel.AttachmentName3 = attachment.Name;
                    ViewBag.Attachment3FileName = attachment.Path;



                }
                else
                {

                }

                count++;

            }


            //  ViewBag.SecId = new SelectList(db.AspNetClasses, "Id", "ClassName", ClassId);
            // ViewBag.SubId = new SelectList(db.GenericSubjects.Where(x => x.SubjectType == Subject.SubjectType), "Id", "SubjectName", SubjectId);

            ViewBag.SubId = new SelectList(db.GenericSubjects.Where(x => x.SubjectType == Subject.SubjectType), "Id", "SubjectName", SubjectId);
            ViewBag.TopicId = new SelectList(db.AspnetSubjectTopics.Where(x => x.SubjectId == SubjectId), "Id", "Name", aspnetLesson.TopicId);
            // ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName", LessonSession.SessionId);

            ViewBag.CTId = Subject.SubjectType;



            return View(lessonViewModel);
        }

        // POST: AspnetLessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LessonViewModel LessonViewModel)
        {


            AspnetLesson Lesson = db.AspnetLessons.Where(x => x.Id == LessonViewModel.Id).FirstOrDefault();
            Lesson.Name = LessonViewModel.LessonName;
            Lesson.Video_Url = LessonViewModel.LessonVideoURL;
            Lesson.TopicId = LessonViewModel.TopicId;
            Lesson.DurationMinutes = LessonViewModel.LessonDuration;
            Lesson.Description = LessonViewModel.LessonDescription;
            Lesson.IsActive = LessonViewModel.IsActive;

            db.SaveChanges();



            HttpPostedFileBase Assignment = Request.Files["Assignment"];
            HttpPostedFileBase Attachment1 = Request.Files["Attachment1"];
            HttpPostedFileBase Attachment2 = Request.Files["Attachment2"];
            HttpPostedFileBase Attachment3 = Request.Files["Attachment3"];

            var fileName = "";
            if (Assignment.ContentLength > 0)
            {
                fileName = Path.GetFileName(Assignment.FileName);
                Assignment.SaveAs(Server.MapPath("~/Content/StudentAssignments/") + fileName);

            }
            AspnetStudentAssignment studentAssignment = db.AspnetStudentAssignments.Where(x => x.LessonId == Lesson.Id).FirstOrDefault();

            if (studentAssignment != null)
            {

                if (fileName != "")
                {

                    studentAssignment.FileName = fileName;

                }

                studentAssignment.Name = LessonViewModel.AssignmentName;
                string DueDate = Convert.ToString(LessonViewModel.AssignmentDueDate);

                if (DueDate == "1/1/0001 12:00:00 AM")
                {
                    studentAssignment.DueDate = null;
                }
                else
                {
                    studentAssignment.DueDate = LessonViewModel.AssignmentDueDate;

                }

                studentAssignment.Description = LessonViewModel.AssignmentDescription;
                db.SaveChanges();
            }
            else
            {
                if (Assignment.ContentLength > 0)
                {

                    AspnetStudentAssignment studentAssignment1 = new AspnetStudentAssignment();

                    studentAssignment1.FileName = fileName;

                    studentAssignment1.Name = LessonViewModel.AssignmentName;


                    string DueDate = Convert.ToString(LessonViewModel.AssignmentDueDate);


                    if (DueDate == "1/1/0001 12:00:00 AM")
                    {
                        studentAssignment1.DueDate = null;

                    }
                    else
                    {

                        studentAssignment1.DueDate = LessonViewModel.AssignmentDueDate;

                    }


                    studentAssignment1.Description = LessonViewModel.AssignmentDescription;
                    studentAssignment1.CreationDate = DateTime.Now;
                    studentAssignment1.LessonId = Lesson.Id;

                    db.AspnetStudentAssignments.Add(studentAssignment1);
                    db.SaveChanges();



                }

            }


            List<AspnetStudentAttachment> studentAttachments = db.AspnetStudentAttachments.Where(x => x.LessonId == Lesson.Id).ToList();
            List<AspnetStudentLink> studentLinks = db.AspnetStudentLinks.Where(x => x.LessonId == Lesson.Id).ToList();



            //db.AspnetStudentAttachments.RemoveRange(studentAttachments);
            //db.SaveChanges();

            db.AspnetStudentLinks.RemoveRange(studentLinks);
            db.SaveChanges();

            SEA_DatabaseEntities db1 = new SEA_DatabaseEntities();

            List<AspnetStudentAttachment> studentAttachments1 = db1.AspnetStudentAttachments.Where(x => x.LessonId == Lesson.Id).ToList();

            int TotalAttachments = studentAttachments1.Count;

            if (TotalAttachments == 0)
            {
                if (Attachment1.ContentLength > 0)
                {
                    var fileName1 = Path.GetFileName(Attachment1.FileName);
                    Attachment1.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                    AspnetStudentAttachment studentAttachment1 = new AspnetStudentAttachment();

                    studentAttachment1.Name = LessonViewModel.AttachmentName1;
                    studentAttachment1.Path = fileName1;
                    studentAttachment1.CreationDate = DateTime.Now;
                    studentAttachment1.LessonId = Lesson.Id;
                    db.AspnetStudentAttachments.Add(studentAttachment1);
                    db.SaveChanges();

                }
                if (Attachment2.ContentLength > 0)
                {

                    var fileName1 = Path.GetFileName(Attachment2.FileName);
                    Attachment2.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                    AspnetStudentAttachment studentAttachment2 = new AspnetStudentAttachment();

                    studentAttachment2.Name = LessonViewModel.AttachmentName2;
                    studentAttachment2.Path = fileName1;
                    studentAttachment2.CreationDate = DateTime.Now;
                    studentAttachment2.LessonId = Lesson.Id;
                    db.AspnetStudentAttachments.Add(studentAttachment2);

                    db.SaveChanges();

                }

                if (Attachment3.ContentLength > 0)
                {

                    var fileName1 = Path.GetFileName(Attachment3.FileName);
                    Attachment3.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                    AspnetStudentAttachment studentAttachment3 = new AspnetStudentAttachment();

                    studentAttachment3.Name = LessonViewModel.AttachmentName3;
                    studentAttachment3.Path = fileName1;
                    studentAttachment3.CreationDate = DateTime.Now;
                    studentAttachment3.LessonId = Lesson.Id;
                    db.AspnetStudentAttachments.Add(studentAttachment3);
                    db.SaveChanges();

                }


            }
            else
            {

                if (TotalAttachments == 1)
                {
                    var FirstElement = studentAttachments1.ElementAt(0);
                    FirstElement.Name = LessonViewModel.AttachmentName1;

                    var FileName = FirstElement.Path;

                    if (Attachment1.ContentLength > 0)
                    {
                        var fileName1 = Path.GetFileName(Attachment1.FileName);
                        FileName = fileName1;
                        Attachment1.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                    }
                    FirstElement.Path = FileName;
                    db1.SaveChanges();

                    if (Attachment2.ContentLength > 0)
                    {

                        var fileName1 = Path.GetFileName(Attachment2.FileName);
                        Attachment2.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                        AspnetStudentAttachment studentAttachment2 = new AspnetStudentAttachment();

                        studentAttachment2.Name = LessonViewModel.AttachmentName2;
                        studentAttachment2.Path = fileName1;
                        studentAttachment2.CreationDate = DateTime.Now;
                        studentAttachment2.LessonId = Lesson.Id;
                        db.AspnetStudentAttachments.Add(studentAttachment2);

                        db.SaveChanges();

                    }

                    if (Attachment3.ContentLength > 0)
                    {

                        var fileName1 = Path.GetFileName(Attachment3.FileName);
                        Attachment3.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                        AspnetStudentAttachment studentAttachment3 = new AspnetStudentAttachment();

                        studentAttachment3.Name = LessonViewModel.AttachmentName3;
                        studentAttachment3.Path = fileName1;
                        studentAttachment3.CreationDate = DateTime.Now;
                        studentAttachment3.LessonId = Lesson.Id;
                        db.AspnetStudentAttachments.Add(studentAttachment3);
                        db.SaveChanges();

                    }

                }

                else if (TotalAttachments == 2)
                {

                    var FirstElement = studentAttachments1.ElementAt(0);
                    FirstElement.Name = LessonViewModel.AttachmentName1;

                    var FileName0 = FirstElement.Path;

                    if (Attachment1.ContentLength > 0)
                    {
                        var fileName1 = Path.GetFileName(Attachment1.FileName);
                        FileName0 = fileName1;
                        Attachment1.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                    }
                    FirstElement.Path = FileName0;
                    db1.SaveChanges();


                    var SecondElement = studentAttachments1.ElementAt(1);
                    SecondElement.Name = LessonViewModel.AttachmentName2;

                    var FileName1 = SecondElement.Path;

                    if (Attachment2.ContentLength > 0)
                    {
                        var fileName2 = Path.GetFileName(Attachment2.FileName);
                        FileName1 = fileName2;
                        Attachment2.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName2);

                    }
                    SecondElement.Path = FileName1;
                    db1.SaveChanges();



                    if (Attachment3.ContentLength > 0)
                    {

                        var fileName1 = Path.GetFileName(Attachment3.FileName);
                        Attachment3.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                        AspnetStudentAttachment studentAttachment3 = new AspnetStudentAttachment();

                        studentAttachment3.Name = LessonViewModel.AttachmentName3;
                        studentAttachment3.Path = fileName1;
                        studentAttachment3.CreationDate = DateTime.Now;
                        studentAttachment3.LessonId = Lesson.Id;
                        db.AspnetStudentAttachments.Add(studentAttachment3);
                        db.SaveChanges();

                    }

                }

                else
                {

                    var FirstElement = studentAttachments1.ElementAt(0);
                    FirstElement.Name = LessonViewModel.AttachmentName1;

                    var FileName0 = FirstElement.Path;

                    if (Attachment1.ContentLength > 0)
                    {
                        var fileName1 = Path.GetFileName(Attachment1.FileName);
                        FileName0 = fileName1;
                        Attachment1.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName1);

                    }
                    FirstElement.Path = FileName0;
                    db1.SaveChanges();


                    var SecondElement = studentAttachments1.ElementAt(1);
                    SecondElement.Name = LessonViewModel.AttachmentName2;

                    var FileName1 = SecondElement.Path;

                    if (Attachment2.ContentLength > 0)
                    {
                        var fileName2 = Path.GetFileName(Attachment2.FileName);
                        FileName1 = fileName2;
                        Attachment2.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName2);

                    }
                    SecondElement.Path = FileName1;
                    db1.SaveChanges();


                    var ThirdElement = studentAttachments1.ElementAt(2);
                    ThirdElement.Name = LessonViewModel.AttachmentName3;

                    var FileName2 = ThirdElement.Path;

                    if (Attachment3.ContentLength > 0)
                    {
                        var fileName3 = Path.GetFileName(Attachment3.FileName);
                        FileName2 = fileName3;
                        Attachment2.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName3);

                    }
                    ThirdElement.Path = FileName2;
                    db1.SaveChanges();


                }


            }



            if (LessonViewModel.LinkUrl1 != null)
            {
                AspnetStudentLink link1 = new AspnetStudentLink();

                link1.URL = LessonViewModel.LinkUrl1;
                link1.CreationDate = DateTime.Now;
                link1.LessonId = Lesson.Id;
                db.AspnetStudentLinks.Add(link1);
                db.SaveChanges();
            }

            if (LessonViewModel.LinkUrl2 != null)
            {
                AspnetStudentLink link2 = new AspnetStudentLink();

                link2.URL = LessonViewModel.LinkUrl2;
                link2.CreationDate = DateTime.Now;
                link2.LessonId = Lesson.Id;
                db.AspnetStudentLinks.Add(link2);
                db.SaveChanges();
            }


            if (LessonViewModel.LinkUrl3 != null)
            {
                AspnetStudentLink link3 = new AspnetStudentLink();

                link3.URL = LessonViewModel.LinkUrl3;
                link3.CreationDate = DateTime.Now;
                link3.LessonId = Lesson.Id;
                db.AspnetStudentLinks.Add(link3);
                db.SaveChanges();
            }


            return RedirectToAction("ViewTopicsAndLessons", "AspnetSubjectTopics");
        }

        // GET: AspnetLessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetLesson aspnetLesson = db.AspnetLessons.Find(id);
            if (aspnetLesson == null)
            {
                return HttpNotFound();
            }
            return View(aspnetLesson);
        }

        // POST: AspnetLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspnetLesson aspnetLesson = db.AspnetLessons.Find(id);
            db.AspnetLessons.Remove(aspnetLesson);
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
