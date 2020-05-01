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
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Text;

namespace SEA_Application.Controllers
{
    public class AspNetProjectController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);
        private string TeacherID;

        public AspNetProjectController()
        {

            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }
        public ActionResult GetSubjectsByClass(string CT)
        {
            var UserId = User.Identity.GetUserId();

            var SubjectofCurrentSessionTeacher = from subject in db.GenericSubjects
                                                 join TeacherSubject in db.Teacher_GenericSubjects on subject.Id equals TeacherSubject.SubjectId
                                                 join employee in db.AspNetEmployees on TeacherSubject.TeacherId equals employee.Id
                                                 where employee.UserId == UserId &&  subject.SubjectType == CT
                                                 select new
                                                 {
                                                     subject.Id,
                                                     subject.SubjectName,
                                                 };

           //   var SubjectsByClass = db.GenericSubjects.Where(x =>x.SubjectType== CT).ToList().Select(x => new { x.Id,x.SubjectName});

            string status = Newtonsoft.Json.JsonConvert.SerializeObject(SubjectofCurrentSessionTeacher);
    
            // return Json(SubjectsByClass, JsonRequestBehavior.AllowGet);
               return Content(status);

        }
      
             public ActionResult GetLession(int TopID)
        {
            var TopicList = db.AspnetLessons.Where(x => x.TopicId == TopID ).ToList().Select(x => new { x.Id, x.Name });

            string status = Newtonsoft.Json.JsonConvert.SerializeObject(TopicList);

            // return Json(SubjectsByClass, JsonRequestBehavior.AllowGet);
            return Content(status);

        }
        public ActionResult GetTopic(int SubID)
        {
            var TopicList = db.AspnetSubjectTopics.Where(x => x.SubjectId == SubID ).ToList().Select(x => new { x.Id, x.Name });

            string status = Newtonsoft.Json.JsonConvert.SerializeObject(TopicList);

            // return Json(SubjectsByClass, JsonRequestBehavior.AllowGet);
            return Content(status);

        }
        // GET: AspNetProject
        public ActionResult Index()
        {


            if (User.IsInRole("Teacher"))
            {

                // ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.AspNetSession.Id == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
                ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            }

            else
            {
                ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x=> x.SessionID == SessionID), "Id", "ClassName");
            }
            
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            return View();
        }

        public ViewResult Project_Submission()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x=> x.SessionID == SessionID), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            
            return View();
        }

        // GET: AspNetProject/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetProject aspNetProject = db.AspNetProjects.Find(id);
            if (aspNetProject == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            return View(aspNetProject);
        }

        // GET: AspNetProject/Create
        public ActionResult Create()
        {
            //ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");

            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            return View();
        }

        // POST: AspNetProject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( AspNetProject aspNetProject)
        {
            HttpPostedFileBase file = Request.Files["attachment"];
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    //var path = Path.Combine(Server.MapPath("/App_Data/Projects/"), fileName);
                    
                    file.SaveAs(Server.MapPath("~/Content/StudentProjects/") + fileName);

                   // file.SaveAs(path);
                    aspNetProject.FileName = fileName;
                }
                else
                {
                    aspNetProject.FileName = "-/-";
                }
                db.AspNetProjects.Add(aspNetProject);
                db.SaveChanges();

                int ProjectID = db.AspNetProjects.Max(x => x.Id);
                 List<string> StudentIDs = db.AspNetStudent_Subject.Where(s => s.SubjectID == aspNetProject.SubjectID).Select(s => s.StudentID).ToList();
                foreach (var item in StudentIDs)
                {
                    AspNetStudent_Project student_project = new AspNetStudent_Project();
                    student_project.StudentID = item;
                    student_project.ProjectID = ProjectID;
                    student_project.SubmissionStatus = false;
                    student_project.SubmittedFileName = "-/-";
                    db.AspNetStudent_Project.Add(student_project);

                    db.SaveChanges();
                }
           /////////////////////////////////////////////////NOTIFICATION/////////////////////////////////////

                var NotificationObj = new AspNetNotification();
                NotificationObj.Description =aspNetProject.Description;
                NotificationObj.Subject =aspNetProject.Title;
                NotificationObj.SenderID = User.Identity.GetUserId();
                NotificationObj.Time = DateTime.Now;
                NotificationObj.Url = "/AspNetProject/Details/" + aspNetProject.Id;
                db.AspNetNotifications.Add(NotificationObj);
                db.SaveChanges();

                var NotificationID = db.AspNetNotifications.Max(x => x.Id);
                var students = db.AspNetStudent_Project.Where(sp => sp.ProjectID == aspNetProject.Id).Select(x => x.StudentID).ToList();

                var users = new List<String>();

                foreach (var item in students)
                {
                    var parentID = db.AspNetParent_Child.Where(x => x.ChildID == item).Select(x => x.ParentID).FirstOrDefault();
                    users.Add(parentID);
                }

                var allusers = users.Union(students);

                foreach (var receiver in allusers)
                {
                    var notificationRecieve = new AspNetNotification_User();
                    notificationRecieve.NotificationID = NotificationID;
                    notificationRecieve.UserID = Convert.ToString(receiver);
                    notificationRecieve.Seen = false;
                    db.AspNetNotification_User.Add(notificationRecieve);
                    db.SaveChanges();
                }

                ///////////////////////////////////////Email/////////////////////////////////////////

                //var subject = db.AspNetSubjects.Where(x => x.Id == aspNetProject.SubjectID).Select(x => x.SubjectName).FirstOrDefault();
                //var StudentEmail = db.AspNetStudent_Project.Where(sp => sp.ProjectID == aspNetProject.Id).Select(x => x.StudentID).ToList();
                //var StudentRoll = db.AspNetStudent_Project.Where(sp => sp.ProjectID == aspNetProject.Id).Select(x => x.AspNetUser.UserName).ToList();
                //string[] studentRollList = new string[StudentRoll.Count];
                //int c = 0;
                //foreach (var item in StudentRoll)
                //{
                //    studentRollList[c] = item;
                //    c++;
                //}
                //var StudentName = db.AspNetStudent_Project.Where(sp => sp.ProjectID == aspNetProject.Id).Select(x => x.AspNetUser.Name).ToList();

                //string[] studentNamelist = new string[StudentName.Count];
                //int i = 0;
                //foreach (var item in StudentName)
                //{
                //    studentNamelist[i] = item;
                //    i++;
                //}

                //var Users = new List<String>();
                //foreach (var item in StudentEmail)
                //{
                //    Users.Add(db.AspNetParent_Child.Where(x => x.ChildID == item).Select(x => x.ParentID).FirstOrDefault());
                //}

                ////Message start
                ////var classe = db.AspNetClasses.Where(p => p.Id == aspNetHomework.ClassId).FirstOrDefault();
                //Utility obj = new Utility();
                //obj.SMSToOffitialsp("Dear Principal, Project has been assigned. IPC NGS Preschool, Aziz Avenue, Lahore.");
                //obj.SMSToOffitialsa("Dear Admin, Project has been assigned. IPC NGS Preschool, Aziz Avenue, Lahore.");
                //AspNetMessage oob = new AspNetMessage();
                //oob.Message = "Dear Parents, The thematic project has been assigned to your child on portal. IPC NGS Preschool, Aziz Avenue, Lahore.";// Title : " + aspNetProject.Title + ", For discription login to Portal please  -
                //obj.SendSMS(oob, Users);
                ////Message end


                //List<string> EmailList = new List<string>();
                //foreach (var sender in Users)
                //{
                //    EmailList.Add(db.AspNetUsers.Where(x => x.Id == sender).Select(x => x.Email).FirstOrDefault());
                //}
                //var j = 0;
                //foreach (var toEmail in EmailList)
                //{
                //    try
                //    {
                //        NotificationObj.Subject = studentNamelist[j] + "(" + studentRollList[j] + ") " + " Subject:" + subject + " Title:" + aspNetProject.Title;
                //        j++;
                //        string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                //        string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                //        SmtpClient client = new SmtpClient("smtpout.secureserver.net", 25);
                //        client.EnableSsl = false;
                //        client.Timeout = 100000;
                //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //        client.UseDefaultCredentials = false;
                //        client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                //        MailMessage mailMessage = new MailMessage(senderEmail, toEmail, NotificationObj.Subject, NotificationObj.Description);
                //        mailMessage.CC.Add(new MailAddress(senderEmail));
                //        mailMessage.IsBodyHtml = true;
                //        mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                //        client.Send(mailMessage);
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}

                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x=> x.SessionID == SessionID), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName", aspNetProject.SubjectID);
            return View(aspNetProject);
        }

        // GET: AspNetProject/Edit/5`
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetProject aspNetProject = db.AspNetProjects.Find(id);
            if (aspNetProject == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetProject.SubjectID);
            return View(aspNetProject);
        }

        // POST: AspNetProject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,PublishDate,DueDate,FileName,AcceptSubmission,SubjectID")] AspNetProject aspNetProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetProject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetProject.SubjectID);
            return View(aspNetProject);
        }

        // GET: AspNetProject/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetProject aspNetProject = db.AspNetProjects.Find(id);
            if (aspNetProject == null)
            {
                return HttpNotFound();
            }
            return View(aspNetProject);
        }

        // POST: AspNetProject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetProject aspNetProject = db.AspNetProjects.Find(id);
            db.AspNetProjects.Remove(aspNetProject);
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

        public JsonResult ProjectBySubject(int subjectID)
        {
          var StudentId =  User.Identity.GetUserId();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            if (User.IsInRole("Teacher"))
            {
                var projects = (from project in db.AspNetProjects
                                join t4 in db.AspNetSubjects on project.SubjectID equals t4.Id
                                where project.SubjectID == subjectID && t4.AspNetClass.SessionID == SessionID 
                                select project).ToList();
                return Json(projects, JsonRequestBehavior.AllowGet);
            }

           else
          {

            var projects = (from project in db.AspNetProjects
                            join t4 in db.AspNetSubjects on project.SubjectID equals t4.Id
                            join studentproject in db.AspNetStudent_Project  on project.Id equals studentproject.ProjectID
                                where project.SubjectID == subjectID && t4.AspNetClass.SessionID == SessionID  && studentproject.StudentID == StudentId
                            select project).ToList();

                return Json(projects, JsonRequestBehavior.AllowGet);
            }

        
        }

        public FileResult downloadProjectFile(int id)
        {
            AspNetProject Project = db.AspNetProjects.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/App_Data/Projects/"), Project.FileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Project.FileName);

        }

        public FileResult downloadStudentSubmittedFile(int id)
        {
            AspNetStudent_Project Student_Project = db.AspNetStudent_Project.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/App_Data/ProjectsSubmission/"), Student_Project.SubmittedFileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Student_Project.SubmittedFileName);

        }

        public JsonResult ProjectBySubjectAcceptSubmission(int subjectID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var projects = (from project in db.AspNetProjects
                               where project.SubjectID == subjectID && project.AcceptSubmission == true
                               select project).ToList();
            return Json(projects, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SubmissionByProject(int ProjectID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var projects = (from projectsubmission in db.AspNetStudent_Project
                               where projectsubmission.ProjectID == ProjectID && projectsubmission.AspNetUser.Status != "False"
                            select new { projectsubmission, projectsubmission.AspNetUser.Name, projectsubmission.AspNetProject.AcceptSubmission }).ToList();

            return Json(projects, JsonRequestBehavior.AllowGet);
        }
    }
}
