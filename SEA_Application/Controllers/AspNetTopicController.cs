using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using OfficeOpenXml;

namespace SEA_Application.Controllers
{
    
    public class AspNetTopicController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);
        string TeacherID;


        public AspNetTopicController()
        {
            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }


        // GET: AspNetTopic
        public ActionResult Index()
        {
            var aspNetTopics = db.AspNetTopics.Include(a => a.AspNetChapter);
            return View(aspNetTopics.ToList());
        }

        // GET: AspNetTopic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTopic aspNetTopic = db.AspNetTopics.Find(id);
            if (aspNetTopic == null)
            {
                return HttpNotFound();
            }
            return View(aspNetTopic);
        }

        // GET: AspNetTopic/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.ChapterID = new SelectList(db.AspNetChapters.Where(x=> x.AspNetSubject.AspNetClass.SessionID == SessionID), "Id", "ChapterName");
            return View();
        }

        // POST: AspNetTopic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TopicNo,TopicName,StartDate,EndDate,ChapterID,Status")] AspNetTopic aspNetTopic)
        {
            var dbTransaction = db.Database.BeginTransaction();
            try
            {

                
                if (ModelState.IsValid)
                {
                    
                    db.AspNetTopics.Add(aspNetTopic);
                    db.SaveChanges();
                        
                   
                }
                dbTransaction.Commit();
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var UserNameLog = User.Identity.Name;
                AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                string UserIDLog = a.Id;
                var logMessage = "New Topic Added, SubjectID: " + aspNetTopic.ChapterID + ", Topic: " + aspNetTopic.TopicName;

                var LogControllerObj = new AspNetLogsController();
                LogControllerObj.CreateLogSave(logMessage, UserIDLog);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                ViewBag.CreateTopic = "Topic created and updated successfully";
                ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
                return View("../Teacher_Dashboard/_Topics");
            }
            catch (Exception)
            {
                dbTransaction.Dispose();
                ViewBag.ChapterID = new SelectList(db.AspNetChapters.Where(x=> x.AspNetSubject.AspNetClass.SessionID == SessionID), "Id", "ChapterName", aspNetTopic.ChapterID);
                return View(aspNetTopic);
            }
            
            ViewBag.ChapterID = new SelectList(db.AspNetChapters.Where(x => x.AspNetSubject.AspNetClass.SessionID == SessionID), "Id", "ChapterName", aspNetTopic.ChapterID);
            return View(aspNetTopic);
        }

        // GET: AspNetTopic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTopic aspNetTopic = db.AspNetTopics.Find(id);
            if (aspNetTopic == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChapterID = new SelectList(db.AspNetChapters, "Id", "ChapterName", aspNetTopic.ChapterID);
            return View(aspNetTopic);
        }

        // POST: AspNetTopic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TopicNo,TopicName,StartDate,EndDate,ChapterID,Status")] AspNetTopic aspNetTopic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetTopic).State = EntityState.Modified;
                db.SaveChanges();
                var status = db.AspNetTopics.Where(x => x.ChapterID == aspNetTopic.ChapterID).Select(x => x.Status).ToList();
                bool completed = true;
                foreach(var item in status)
                {
                    if(item==false)
                    {
                        completed = false;
                        break;
                    }
                   
                }
                AspNetChapter aspNetChapter = db.AspNetChapters.Where(x => x.Id == aspNetTopic.ChapterID).FirstOrDefault();
                if (completed)
                {
                   
                    aspNetChapter.Status = true;
                    db.SaveChanges();
                }
                else
                {
                    aspNetChapter.Status = false;
                    db.SaveChanges();
                }
                return RedirectToAction("Dashboard", "Account");
            }
            ViewBag.ChapterID = new SelectList(db.AspNetChapters, "Id", "ChapterName", aspNetTopic.ChapterID);
            return View(aspNetTopic);
        }

        // GET: AspNetTopic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTopic aspNetTopic = db.AspNetTopics.Find(id);
            if (aspNetTopic == null)
            {
                return HttpNotFound();
            }
            return View(aspNetTopic);
        }

        // POST: AspNetTopic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetTopic aspNetTopic = db.AspNetTopics.Find(id);
            db.AspNetTopics.Remove(aspNetTopic);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public JsonResult LessonBySubject(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var lessonplan = db.AspNetLessonPlans.Where(x => x.AspNetSubject.Id == id).Select(x => x).ToList();

            return Json(lessonplan, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SubjectsByClass(int id)
        {
            var sub = db.AspNetSubjects.Where(x => x.ClassID == id).Select(x=> new { x.Id , x.SubjectName}).ToList();
            return Json(sub, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult lesssonSubjectsByClasses(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            lessonplanSubjects LessonplanSub = new lessonplanSubjects();
            LessonplanSub.lessonlist = new List<Lesson>();
            LessonplanSub.Subjects = new List<Subjects>();

            if (User.IsInRole("Teacher"))
            {
                var sub = db.AspNetSubjects.Where(r => r.ClassID == id && r.TeacherID == TeacherID).Select(x => new { x.Id, x.SubjectName }).OrderByDescending(r => r.Id).ToList();

                foreach (var item in sub)
                {
                    var SUB = new Subjects();
                    SUB.Id = item.Id;
                    SUB.SubjectName = item.SubjectName;

                    LessonplanSub.Subjects.Add(SUB);
                }
            }else
            {
                var sub = db.AspNetSubjects.Where(r => r.ClassID == id).Select(x => new { x.Id, x.SubjectName }).OrderByDescending(r => r.Id).ToList();

                foreach (var item in sub)
                {
                    var SUB = new Subjects();
                    SUB.Id = item.Id; 
                    SUB.SubjectName = item.SubjectName;

                    LessonplanSub.Subjects.Add(SUB);
                }
            }

            
            var lessonplan = db.AspNetLessonPlans.Where(x => x.AspNetSubject.ClassID == id).ToList();

            

            foreach (var item in lessonplan)
            {
                var lesson = new Lesson();
                lesson.Id = item.Id;
                lesson.Date = item.Date.ToString();
                lesson.Duration = item.Duration.ToString();
                lesson.LessonPlanNo = (int)item.LessonPlanNo;
                lesson.SubjectName = db.AspNetSubjects.Where(x => x.Id == item.SubjectID).Select(x => x.SubjectName).FirstOrDefault(); 

                LessonplanSub.lessonlist.Add(lesson);
            }

            return Json(LessonplanSub, JsonRequestBehavior.AllowGet);
        }

        public class Lesson
        {
            public int LessonPlanNo { get; set; }
            public string Date { get; set; }
            public string Duration { get; set; }
            public string SubjectName { get; set; }
            public int Id { set; get; }
        }

        public class lessonplanSubjects
        {
            public List<Lesson> lessonlist { set; get; }
            public List<Subjects> Subjects { get; set; }
        }

        public class Subjects
        {
            public int Id { set; get; }
            public string SubjectName { set; get; }
        }

        [HttpGet]
        public JsonResult ChapterBySubject(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<AspNetChapter> chapter = db.AspNetChapters.Where(r => r.SubjectID == id).OrderByDescending(r => r.Id).ToList();

            return Json(chapter, JsonRequestBehavior.AllowGet);

        }
        
       
        [HttpGet]
        public JsonResult ChapterBySubjectLESSON(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<AspNetChapter> chapter = db.AspNetChapters.Where(r => r.SubjectID == id).OrderByDescending(r => r.Id).ToList();

            return Json(chapter, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult TopicByLesson(int id)
        {

            var topicList = (from lesson_topic in db.AspNetLessonPlan_Topic
                          where lesson_topic.LessonPlanID == id
                          select lesson_topic.TopicID).ToList();
            
            

            return Json(topicList, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult TopicsBySubject(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var chapters = (from chapter in db.AspNetChapters
                            where chapter.SubjectID == id
                            select new { chapter.Id, chapter.ChapterName, chapter.ChapterNo, chapter.StartDate, chapter.EndDate, chapter.Status, chapter.AspNetTopics }).ToList();
            return Json(chapters, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]
        public JsonResult TopicByChapter(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            var Topics= (from topics in db.AspNetTopics
                        where topics.ChapterID==id
                        select new {topics.Id, topics.TopicName}).ToList();
          return Json(Topics, JsonRequestBehavior.AllowGet);

        }
        

        public ActionResult CurriculumFromFile()
        {
           
                HttpPostedFileBase file = Request.Files["curriculum"];
                int SubjectID = Convert.ToInt32(Request.Form["SubjectID"]);
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    for (int rowIterator = 1; rowIterator <= noOfRow; rowIterator++)
                    {
                        var heading= workSheet.Cells[rowIterator, 1].Value.ToString();
                        if(heading=="Chapter")
                        {
                            rowIterator++;
                            AspNetChapter aspNetChapter = new AspNetChapter();
                            aspNetChapter.SubjectID = SubjectID;
                            aspNetChapter.ChapterNo = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value.ToString());
                            aspNetChapter.ChapterName = workSheet.Cells[rowIterator, 2].Value.ToString();
                            aspNetChapter.StartDate = Convert.ToDateTime(workSheet.Cells[rowIterator, 3].Value.ToString());
                            aspNetChapter.EndDate = Convert.ToDateTime(workSheet.Cells[rowIterator, 4].Value.ToString());
                            aspNetChapter.Status = Convert.ToBoolean(Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value.ToString()));
                            db.AspNetChapters.Add(aspNetChapter);
                            db.SaveChanges();
                        }
                        else if(heading=="Topic")
                        {
                            int ChapterID = db.AspNetChapters.Max(item => item.Id);
                            rowIterator++;
                            AspNetTopic aspNetTopic = new AspNetTopic();
                            aspNetTopic.ChapterID = ChapterID;
                            aspNetTopic.TopicNo = Convert.ToDouble(workSheet.Cells[rowIterator, 1].Value.ToString());
                            aspNetTopic.TopicName = workSheet.Cells[rowIterator, 2].Value.ToString();
                            aspNetTopic.StartDate = Convert.ToDateTime(workSheet.Cells[rowIterator, 3].Value.ToString());
                            aspNetTopic.EndDate = Convert.ToDateTime(workSheet.Cells[rowIterator, 4].Value.ToString());
                            aspNetTopic.Status = Convert.ToBoolean(Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value.ToString()));
                            db.AspNetTopics.Add(aspNetTopic);
                            db.SaveChanges();
                        }
                        else
                        {
                            int ChapterID = db.AspNetChapters.Max(item => item.Id);
                            AspNetTopic aspNetTopic = new AspNetTopic();
                            aspNetTopic.ChapterID = ChapterID;
                            aspNetTopic.TopicNo = Convert.ToDouble(workSheet.Cells[rowIterator, 1].Value.ToString());
                            aspNetTopic.TopicName = workSheet.Cells[rowIterator, 2].Value.ToString();
                            aspNetTopic.StartDate = Convert.ToDateTime(workSheet.Cells[rowIterator, 3].Value.ToString());
                            aspNetTopic.EndDate = Convert.ToDateTime(workSheet.Cells[rowIterator, 4].Value.ToString());
                            aspNetTopic.Status = Convert.ToBoolean(Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value.ToString()));
                            db.AspNetTopics.Add(aspNetTopic);
                            db.SaveChanges();
                        }
                       
                        }


                    }
            return RedirectToAction("Dashboard", "Teacher_Dashboard");
        }
       
        public class calendarEvents
        {
            
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string color { get; set; }
            public string url { get; set; }
        }
        [HttpGet]
        public JsonResult TopicsBySubjectCalender(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var chapters = (from chapter in db.AspNetChapters
                            where chapter.SubjectID == id
                            select new { chapter.Id, chapter.ChapterName, chapter.ChapterNo, chapter.StartDate, chapter.EndDate, chapter.Status, chapter.AspNetTopics }).ToList();
            List<calendarEvents> events = new List<calendarEvents>();
            foreach (var chapter in chapters)
            {
                calendarEvents chapterevent = new calendarEvents();
                chapterevent.title = chapter.ChapterName;
                chapterevent.start = Convert.ToDateTime(chapter.StartDate).ToString("yyyy-MM-dd hh:mm:ss");
                chapterevent.end = Convert.ToDateTime(chapter.EndDate).ToString("yyyy-MM-dd hh:mm:ss");
                chapterevent.color = "Orange";
                events.Add(chapterevent);
                foreach(var topic in chapter.AspNetTopics)
                {
                    calendarEvents topicevent = new calendarEvents();
                    topicevent.title = topic.TopicName;
                    
                    topicevent.start = Convert.ToDateTime(topic.StartDate).ToString("yyyy-MM-dd hh:mm:ss");

                    topicevent.end = Convert.ToDateTime(topic.EndDate).ToString("yyyy-MM-dd hh:mm:ss");
                    topicevent.color = "#27a0c9";
                    events.Add(topicevent);
                }
            }

            var LessonPlans = db.AspNetLessonPlans.Where(x=> x.AspNetSubject.Id == id).Select(x => x).ToList();
            foreach(var lessonplan in LessonPlans)
            {
                calendarEvents lessonplanevent = new calendarEvents();
                lessonplanevent.title = "Lesson Plan";
                lessonplanevent.start=Convert.ToDateTime(lessonplan.Date).ToString("yyyy-MM-dd hh:mm:ss");
                lessonplanevent.end = Convert.ToDateTime(lessonplan.Date).ToString("yyyy-MM-dd hh:mm:ss");
                lessonplanevent.color = "#8BC34A";
                lessonplanevent.url= "javascript:MyFunction("+lessonplan.Id+");";
                events.Add(lessonplanevent);

            }
            return Json(events, JsonRequestBehavior.AllowGet);

        }

    
    }
}
