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
using OfficeOpenXml;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Globalization;

namespace SEA_Application.Controllers
{
    [Authorize(Roles = "Teacher,Principal")]
    public class Teacher_DashboardController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        private string TeacherID;
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        public Teacher_DashboardController()
        {

            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }
        public Teacher_DashboardController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Teacher_Dashboard

        private static string GetOrdinalSuffix(int num)
        {
            string number = num.ToString();
            if (number.EndsWith("11")) return "th";
            if (number.EndsWith("12")) return "th";
            if (number.EndsWith("13")) return "th";
            if (number.EndsWith("1")) return "st";
            if (number.EndsWith("2")) return "nd";
            if (number.EndsWith("3")) return "rd";
            return "th";
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
            var dayInString = dd + GetOrdinalSuffix(dd);

            string[] array = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            var Date = day + ", " + dayInString + " " + array[mm - 1] + " " + yy;
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

        public ViewResult Dashboard()
        {
            var CurrentUserId = User.Identity.GetUserId();

            var allMessages = (from a in db.AspNetMessages
                               join b in db.AspNetMessage_Receiver
                               on a.Id equals b.MessageID
                               where b.ReceiverID == CurrentUserId && b.Seen == "Not Seen"
                               join c in db.AspNetUsers
                              on a.SenderID equals c.Id
                               select new { a.Message, a.Time, c.Name }).ToList();
            List<Message> messages = new List<Message>();
            foreach (var item in allMessages)
            {
                Message m = new Message();
                m.Name = item.Name;
                m.message = item.Message;
                string monthName = item.Time.Value.ToString("MMM", CultureInfo.InvariantCulture);
                m.date = monthName + " " + item.Time.Value.Day + "," + item.Time.Value.Year;
                messages.Add(m);

            }
            ViewBag.Messages = messages;
            int classid = db.AspNetClasses.Where(m => m.TeacherID == CurrentUserId).Select(a => a.Id).FirstOrDefault();
            ViewBag.allStudents = db.AspNetStudents.Where(m => m.ClassID == classid).Count();
            ViewBag.TotalMessages = db.AspNetMessage_Receiver.Where(m => m.ReceiverID == CurrentUserId && m.Seen == "Not Seen").Count();
            ViewBag.TotalNotifications = db.AspNetNotification_User.Where(m => m.UserID == CurrentUserId && m.Seen == false).Count();

            var ty = (from a in db.AspNetClasses
                      join b in db.AspNetHomeworks
                      on a.Id equals b.ClassId
                      where a.TeacherID == CurrentUserId && b.PrincipalApproved_status == "Rejected"
                      select new { a.ClassName, b.Date, b.Id }).ToList().OrderByDescending(a => a.Date);



            var Classes1 = db.AspNetTeacherSubjects.Where(x => x.AspNetEmployee.UserId == CurrentUserId).Select(x => x.AspNetSubject.SubjectName).Distinct().ToList();
            List<string> classes = new List<string>();

            foreach (var clas in Classes1)
            {
                classes.Add(clas);
            }

            //classes.Add("Not Published");
            ViewBag.AllClasses = classes;

            return View("BlankPage");

            return View("BlankPage");
        }
        public class Message
        {
            public string Name { get; set; }
            public string message { get; set; }

            public string date { get; set; }
        }
        public ViewResult Teacher_Subject()
        {
            var subjects = db.AspNetSubjects.Include(s => s.AspNetClass).Include(s => s.AspNetUser).Where(s => s.TeacherID == TeacherID && s.AspNetClass.AspNetSession.Id == SessionID);
            return View("_Teacher_Subject", subjects);
        }

        [HttpGet]
        public JsonResult StudentsByClass(int id)
        {
            string ClassHead = db.AspNetClasses.Where(x => x.Id == id).Select(x => x.TeacherID).First();
            string currentTeacher = User.Identity.GetUserId();

            // if(String.Compare( ClassHead , currentTeacher) == 0)

            var students = (from student in db.AspNetUsers
                            join student_subject in db.AspNetStudent_Subject on student.Id equals student_subject.StudentID
                            join subject in db.AspNetSubjects on student_subject.SubjectID equals subject.Id
                            where subject.ClassID == id
                            select new { student.Id, student.UserName, student.Name }).Distinct().ToList();

            return Json(students, JsonRequestBehavior.AllowGet);



        }
        public ActionResult EditStudent(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            var employee = db.AspNetStudents.Where(x => x.StudentID == id).Select(x => x).FirstOrDefault();
            ViewBag.employee = employee;
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent([Bind(Include = "Id,Email,PasswordHash,SecurityStamp,PhoneNumber,UserName,Name")] AspNetUser aspNetUser)
        {
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    IEnumerable<string> selectedsubjects = Request.Form["subjects"].Split(',');
                    AspNetStudent_Subject stu_sub_rem = new AspNetStudent_Subject();
                    do
                    {
                        stu_sub_rem = db.AspNetStudent_Subject.FirstOrDefault(x => x.StudentID == aspNetUser.Id);
                        try
                        {
                            db.AspNetStudent_Subject.Remove(stu_sub_rem);
                            var student = db.AspNetStudents.Where(x => x.StudentID == aspNetUser.Id).Select(x => x).FirstOrDefault();
                            student.Nationality = Request.Form["Nationality"];
                            student.BirthDate = Request.Form["BirthDate"];
                            student.Religion = Request.Form["Religion"];
                            student.Gender = Request.Form["Gender"];
                            student.SchoolName = Request.Form["SchoolName"];
                            db.SaveChanges();
                        }
                        catch
                        {

                        }
                    }
                    while (stu_sub_rem != null);


                    foreach (var item in selectedsubjects)
                    {
                        AspNetStudent_Subject stu_sub = new AspNetStudent_Subject();
                        stu_sub.StudentID = aspNetUser.Id;
                        stu_sub.SubjectID = Convert.ToInt32(item);
                        db.AspNetStudent_Subject.Add(stu_sub);
                        db.SaveChanges();
                    }

                    db.Entry(aspNetUser).State = EntityState.Modified;
                    db.SaveChanges();
                }
                dbTransaction.Commit();
                return RedirectToAction("StudentsIndex");
            }
            catch (Exception) { dbTransaction.Dispose(); }

            return View("StudentsIndex");
        }
        public ViewResult Teacher_Announcement()
        {
            if (User.IsInRole("Teacher"))
            {
                ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.AspNetSession.Id == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            }
            else
            {
                ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x => x.SessionID == SessionID), "Id", "ClassName");
            }
            return View("_Teacher_Announcement");
        }

        public ViewResult Attendance()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            return View("_Attendance");
        }
        public ActionResult View_Attendance()
        {

            return View();
        }
        public JsonResult AllAttendance()
        {
            var teacherId = db.AspNetEmployees.Max(x => x.Id);
            var attendance = (from emp_attendance in db.AspNetEmployee_Attendance
                              where emp_attendance.EmployeeID == teacherId
                              select new { emp_attendance.Reason, emp_attendance.Status, emp_attendance.AspNetEmployeeAttendance.Date }).ToList();

            return Json(attendance, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Topics()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            return View("_Topics");
        }

        public JsonResult Topic_status(int id)
        {
            AspNetTopic TopicStatus = db.AspNetTopics.Where(x => x.Id == id).Select(x => x).FirstOrDefault();

            if (TopicStatus.Status == false)
            {
                TopicStatus.Status = true;
            }
            else if (TopicStatus.Status == true)
            {
                TopicStatus.Status = false;
            }

            db.SaveChanges();

            return Json(TopicStatus.Status, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Add_Curriculum()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            return View("_Add_Curriculum");
        }

        public PartialViewResult Calendar_View()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            return PartialView("_Calendar_View");
        }
        public ViewResult ParentTeacherMeeting()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            return View("_ParentTeacherMeeting");
        }

        public ViewResult Subject_Assessment()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            return View("_SubjectAssessment");
        }

        public ViewResult Class_Assessment()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");

            //ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x => x.TeacherID == TeacherID), "Id", "ClassName");
            //ViewBag.ClassID = new SelectList((from c in db.AspNetClasses
            //                                  join s in db.AspNetSubjects
            //                                  on c.Id equals s.ClassID
            //                                  where c.TeacherID == TeacherID
            //                                  select new { s.ClassID, c.ClassName }).Distinct(), "Id", "ClassName");

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            ViewBag.TermID = new SelectList(db.AspNetTerms.Where(x => x.SessionID == SessionID), "Id", "TermName", "TermNo");
            //ViewBag.TermId = new SelectList(db.AspNetTerms.Where(p => p.))
            return View("_ClassAssessment");
        }

        public ViewResult LessonPlanView()
        {

            return View("_LessonPlanView");
        }
        public JsonResult StudentClass(string id)
        {


            var subID = db.AspNetStudent_Subject.FirstOrDefault(x => x.StudentID == id);
            var classidcheck = db.AspNetSubjects.FirstOrDefault(x => x.Id == subID.SubjectID);
            var ClassIDscheck = db.AspNetClasses.FirstOrDefault(x => x.Id == classidcheck.ClassID);
            var ClassIDNAMEv = ClassIDscheck.Id;

            return Json(ClassIDNAMEv, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SubjectsByClasss(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<AspNetSubject> sub = db.AspNetSubjects.Where(r => r.ClassID == id).OrderByDescending(r => r.Id).ToList();
            ViewBag.Subjects = sub;
            return Json(sub, JsonRequestBehavior.AllowGet);

        }

        public ViewResult StudentsIndex()
        {
            string s = User.Identity.GetUserId();
            ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x => x.TeacherID == s), "Id", "ClassName");
            return View();
        }

        [HttpGet]
        public JsonResult SubjectsStudentsByClass(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var StSu = new StudentAndSubjects();
            var subject = db.AspNetSubjects.Where(r => r.ClassID == id).OrderByDescending(r => r.Id).Select(x => new { x.Id, x.SubjectName, x.AspNetClass.ClassName }).ToList();
            List<Subjects> sub = new List<Subjects>();

            foreach (var item in subject)
            {
                Subjects s = new Subjects();
                s.id = item.Id;
                s.Subjectname = item.SubjectName;
                s.Classname = item.ClassName;
                sub.Add(s);
            }

            StSu.Subjects = sub;

            var students = (from student in db.AspNetStudents
                            where student.ClassID == id
                            select new { student.AspNetUser.PhoneNumber, student.AspNetUser.Email, student.AspNetUser.UserName, student.AspNetUser.Name, student.AspNetClass.ClassName }).Distinct().ToList();

            StSu.Students = new List<Students>();

            foreach (var item in students)
            {
                var student = new Students();
                student.Name = item.Name;
                student.PhoneNo = item.PhoneNumber;
                student.Email = item.Email;
                student.RollNo = item.UserName;
                student.ClassName = item.ClassName;
                StSu.Students.Add(student);
            }

            return Json(StSu, JsonRequestBehavior.AllowGet);

        }

        public class Students
        {
            public string Name { set; get; }
            public string RollNo { set; get; }
            public string Email { set; get; }
            public string PhoneNo { set; get; }
            public string ClassName { set; get; }
        }

        public class StudentAndSubjects
        {
            public List<Students> Students { set; get; }
            public List<Subjects> Subjects { set; get; }
        }

        public class Subjects
        {
            public int id { set; get; }
            public string Subjectname { set; get; }
            public string Classname { set; get; }
        }

        [HttpGet]
        public JsonResult SubjectsByStudent(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var subs = (from sub in db.AspNetStudent_Subject
                        where sub.StudentID == id
                        select new { sub.AspNetSubject.Id, sub.AspNetSubject.SubjectName, sub.AspNetSubject.ClassID }).ToList();


            return Json(subs, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult StudentsBySubjects(int id)
        {

            var students = (from student in db.AspNetStudent_Subject
                            where student.SubjectID == id && student.AspNetUser.Status != "False"
                            select new
                            {
                                student.AspNetUser.Id,
                                student.AspNetUser.Email,
                                student.AspNetUser.UserName,
                                student.AspNetUser.Name,
                                student.AspNetUser.PhoneNumber,
                                student.AspNetSubject.AspNetClass.ClassName
                            }).ToList();


            return Json(students, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CreateStudent()
        {
            //var data = db.AspNetClasses 
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStudent(RegisterViewModel model)
        {
            var dbTransaction = db.Database.BeginTransaction();

            string fullName = model.Name;
            char[] upper = fullName.ToCharArray();
            upper[0] = char.ToUpper(upper[0]);
            string upperr = new string(upper) + " ";
            string pass = upperr.Substring(0, upperr.IndexOf(" "));
            model.Email = model.UserName + "@gmail.com";
            model.Password = pass + "@1234";
            model.ConfirmPassword = model.Password;

            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                IEnumerable<string> selectedsubjects = Request.Form["subjects"].Split(',');
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = false, Name = model.Name, PhoneNumber = Request.Form["cellNo"] };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    AspNetStudent student = new AspNetStudent();
                    student.StudentID = user.Id;
                    student.SchoolName = Request.Form["SchoolName"];
                    student.BirthDate = Request.Form["BirthDate"];
                    student.Nationality = Request.Form["Nationality"];
                    student.Religion = Request.Form["Religion"];
                    student.Gender = Request.Form["Gender"];
                    student.ClassID = Convert.ToInt32(Request.Form["ClassID"]);

                    db.AspNetStudents.Add(student);
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    db.SaveChanges();
                    var errors1 = ModelState.Values.SelectMany(v => v.Errors);

                    foreach (var item in selectedsubjects)
                    {
                        AspNetStudent_Subject stu_sub = new AspNetStudent_Subject();
                        stu_sub.StudentID = user.Id;
                        stu_sub.SubjectID = Convert.ToInt32(item);
                        db.AspNetStudent_Subject.Add(stu_sub);

                        db.SaveChanges();

                    }

                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    userManager.AddToRole(user.Id, "Student");

                    dbTransaction.Commit();
                    // string Error = "Student successfully saved.";
                    return RedirectToAction("StudentsIndex");

                }
                else
                {
                    dbTransaction.Dispose();
                    // AddErrors(result);
                }
            }

            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View(model);
        }

        public ActionResult StudentDetail(string userName)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Where(x => x.UserName == userName).Select(x => x).FirstOrDefault();
            var employee = db.AspNetStudents.Where(x => x.StudentID == aspNetUser.Id).Select(x => x).FirstOrDefault();
            ViewBag.employee = employee;
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");

            ViewBag.Detail = "data";

            return View(aspNetUser);
        }
        public JsonResult GetUserName(string userName)
        {
            check Check = new check();
            Check.count = 0;
            try
            {
                var user = db.AspNetUsers.Where(x => x.UserName == userName);
                if (user.Count() > 0)
                {
                    Check.count = 1;
                    Check.by = user.Select(x => x.AspNetRoles.Select(y => y.Name).FirstOrDefault()).FirstOrDefault();

                }
                else
                {
                    Check.count = 0;
                }
            }
            catch
            {
                Check.count = 0;
            }

            return Json(Check, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Email(string Email)
        {
            int count;
            try
            {
                var user = db.AspNetUsers.Where(x => x.Email == Email);
                if (user.Count() > 0)
                {
                    count = 1;
                }
                else
                {
                    count = 0;
                }
            }
            catch
            {
                count = 0;
            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public class TODOLIST
        {
            public int HomeWorkId { get; set; }
            public string date { get; set; }
            public string Actualdate { get; set; }
            public string Classname { get; set; }

            public bool isToDay { get; set; }
        }

        public class check
        {
            public int count { get; set; }
            public string by { get; set; }
        }

        /******************************************************************************************************************
         * 
         *                                       Common Classes
         *                                       
         ******************************************************************************************************************/
        public class Marks
        {
            public int Id { get; set; }
            public int GotMark { get; set; }

        }


        public class NewCurriculums
        {

            public int WeightageValue { get; set; }
            public int CurriculumID { get; set; }
            public int SubjectID { get; set; }
        }


        public class NewLessonPlan
        {

            public int EnteredLessonPlanID { get; set; }
            public int EnteredLessonHeadingID { get; set; }
            public String EnteredDescription { get; set; }
            public String EnteredResources { get; set; }
            public int EnteredMinutes { get; set; }
            public int EnteredPriority { get; set; }

        }



        /******************************************************************************************************************
         * 
         *                                       Common Filter Functions
         *                                       
         ******************************************************************************************************************/
        [HttpGet]
        public JsonResult ClassByTeacher()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<AspNetClass> classes = db.AspNetClasses.ToList();
            ViewBag.Subjects = classes;
            return Json(classes, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult SubjectsByClass(int id)
        {

            if (User.IsInRole("Teacher"))
            {
                db.Configuration.ProxyCreationEnabled = false;

                var classTeacherID = db.AspNetClasses.Where(x => x.Id == id).Select(x => x.TeacherID).FirstOrDefault();

                if (classTeacherID == TeacherID)
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var subject = (from subjects in db.AspNetSubjects
                                   orderby subjects.Id descending
                                   where subjects.ClassID == id
                                   select new { subjects.Id, subjects.SubjectName }).ToList();

                    return Json(subject, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    //   List<AspNetSubject> sub = db.AspNetSubjects.Where(r => r.ClassID == id && r.TeacherID == TeacherID).OrderByDescending(r => r.Id).ToList();
                      List<AspNetSubject> sub = db.AspNetSubjects.Where(r => r.ClassID == id).OrderByDescending(r => r.Id).ToList();

                    return Json(sub, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var sub = db.AspNetSubjects.Where(x => x.ClassID == id).Select(x => new { x.Id, x.SubjectName }).ToList();
                return Json(sub, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpGet]
        public JsonResult StudentsBySubject(int id)
        {
            ApplicationDbContext d = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            List<string> studentsID = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Student")).Select(s => s.Id).ToList();
            var stu = (from t in db.AspNetUsers
                       join t1 in db.AspNetStudent_Subject on t.Id equals t1.StudentID
                       where studentsID.Contains(t.Id) && t1.SubjectID == id
                       select new { t.Id, t.Name, t.UserName }).ToList();
            return Json(stu, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult AnnouncementBySubject(int id)
        {
            ApplicationDbContext d = new ApplicationDbContext();

            db.Configuration.ProxyCreationEnabled = false;
            var Announcements = (from announcement_subject in db.AspNetAnnouncement_Subject
                                 where announcement_subject.SubjectID == id
                                 select new { announcement_subject.AspNetAnnouncement.Id, announcement_subject.AspNetAnnouncement.Title, announcement_subject.AspNetSubject.SubjectName }).ToList();

            return Json(Announcements, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSubjects()
        {
            var uniqueId = User.Identity.GetUserId();

            AspNetEmployee aspNetEmployee = db.AspNetEmployees.Where(x => x.UserId == uniqueId).FirstOrDefault();

            var result = from s in db.AspNetSubjects
                         join ts in db.AspNetTeacherSubjects
                         on s.Id equals ts.SubjectID
                         where (ts.TeacherID == aspNetEmployee.Id)
                         select new
                         {
                             Id = s.Id,

                             Name = s.SubjectName,
                         };



            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetChapters(int id)
        {

            var result = from s in db.AspNetSubjects
                         join ts in db.AspNetChapters
                         on s.Id equals ts.SubjectID
                         where (ts.SubjectID == id)
                         select new
                         {
                             Id = ts.Id,

                             Name = ts.ChapterName,
                             ChapterNo = ts.ChapterNo,
                             SubjectID = ts.SubjectID
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopics(int id)
        {

            var result = from s in db.AspNetChapters
                         join ts in db.AspNetTopics
                         on s.Id equals ts.ChapterID
                         where (ts.ChapterID == id)
                         select new
                         {
                             Id = ts.Id,

                             ChapterName = ts.Id,
                             Name = ts.TopicName,
                             SubjectID = ts.TopicNo
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopicPercentage(int id)
        {

          var TopicPercentage =   db.AspNetTopics.Where(x => x.Id == id).FirstOrDefault().Percentage_Completion;


            return Json( new { TopicPercentage = TopicPercentage }, JsonRequestBehavior.AllowGet);

        }



    }
}