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

namespace SEA_Application.Controllers.LessonPlanControllers
{
    public class AspNetLessonPlanController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        private string TeacherID;
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        public AspNetLessonPlanController()
        {

            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }
        // GET: AspNetLessonPlan
        public ActionResult Index()
        {
            var aspNetLessonPlans = db.AspNetLessonPlans.Include(a => a.AspNetSubject);
            var session = SessionIDStaticController.GlobalSessionID;
            return View(aspNetLessonPlans.ToList());
        }

        // GET: AspNetLessonPlan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlan aspNetLessonPlan = db.AspNetLessonPlans.Find(id);
            if (aspNetLessonPlan == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLessonPlan);
        }

        // GET: AspNetLessonPlan/Create
        public ActionResult Create()
        {
            ViewBag.HeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName");
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID ).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            return View();
        }

        public ActionResult View_LessonPlan()
        {
            if (User.IsInRole("Teacher"))
            {
                ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            }
            else
            {
                ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            }
            
            return View();
        }

        // POST: AspNetLessonPlan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Duration,SubjectID")] AspNetLessonPlan aspNetLessonPlan)
        {
            if (ModelState.IsValid)
            {
                db.AspNetLessonPlans.Add(aspNetLessonPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName");
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName", aspNetLessonPlan.SubjectID);
            return View(aspNetLessonPlan);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public JsonResult GetLessonPlanInfo(int id)
        {
            var LessonPlan = db.AspNetLessonPlans.FirstOrDefault(d => d.Id == id);
            
            lessonPlan Info = new lessonPlan();
            Info.GetTopics = new List<string>();
            Info.BreakDown = new List<BreakDowns>();
            Info.classID = Convert.ToInt32(LessonPlan.AspNetSubject.AspNetClass.Id);
            Info.subjectID = Convert.ToInt32(LessonPlan.SubjectID);
            Info.Date = Convert.ToDateTime(LessonPlan.Date);
            Info.Duration = Convert.ToInt32(LessonPlan.Duration);
            Info.lessonPlanNo = Convert.ToInt32(LessonPlan.LessonPlanNo);

            foreach (var topic in LessonPlan.AspNetLessonPlan_Topic)
            {
                Info.GetTopics.Add(topic.AspNetTopic.TopicName);
            }
            foreach (var breakdown in LessonPlan.AspNetLessonPlanBreakdowns)
            {
                BreakDowns breakDown = new BreakDowns();
                breakDown.Id = breakdown.Id;
                breakDown.HeadingName = breakdown.AspNetLessonPlanBreakdownHeading.BreakDownHeadingName;
                breakDown.Description = WebUtility.HtmlDecode(breakdown.Description);
                breakDown.Minutes = Convert.ToInt32(breakdown.Minutes);
                breakDown.Resources = breakdown.Resources;
                Info.BreakDown.Add(breakDown);
            }

            return Json(Info, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLessonPlanID(int SubjectID, DateTime Date)
        {

            var lessonplan = db.AspNetLessonPlans.FirstOrDefault(x => x.SubjectID == SubjectID && x.Date == Date);
            int id = lessonplan.Id;
            return Json(id, JsonRequestBehavior.AllowGet);
           
        }
        
        public ActionResult EditTopics(int? id)
        {
            var LessonPlanTopicobj = db.AspNetLessonPlan_Topic.FirstOrDefault(x => x.LessonPlanID == id);
            var topicObj = db.AspNetTopics.FirstOrDefault(x => x.Id == LessonPlanTopicobj.TopicID);
            var chapObj = db.AspNetChapters.FirstOrDefault(s => s.Id == topicObj.ChapterID);
            ViewBag.TopicID = new SelectList(db.AspNetTopics.Where(x => x.ChapterID == chapObj.Id), "Id", "ClassName");
            ViewBag.ChapterID = new SelectList(db.AspNetChapters.Where(s => s.SubjectID == chapObj.SubjectID), "Id", "ChapterName");
            return View();
        }

        public ActionResult EditLessonPlan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlan aspNetLessonPlan = db.AspNetLessonPlans.Find(id);
            if (aspNetLessonPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.HeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName");
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=>x.AspNetClass.Id==aspNetLessonPlan.AspNetSubject.AspNetClass.Id), "Id", "SubjectName", aspNetLessonPlan.SubjectID);
            ViewBag.LessonPlanNo = new SelectList(db.AspNetLessonPlans.Where(x => x.Id == id), "Id", "LessonPlanNo");
            ViewBag.LessonPlanIDEdit = id;


            return View(aspNetLessonPlan);
        }
        
        public void EditLessonPlanobj(lessonPlan LessonPlan)
        {
            var lessonPlanToEdit = db.AspNetLessonPlans.FirstOrDefault(s => s.Date == LessonPlan.Date && s.SubjectID == LessonPlan.subjectID);
            AspNetLessonPlan aspNetLessonPlan = db.AspNetLessonPlans.FirstOrDefault(x => x.Id == lessonPlanToEdit.Id);
            aspNetLessonPlan.LessonPlanNo = LessonPlan.lessonPlanNo;
            aspNetLessonPlan.SubjectID = LessonPlan.subjectID;
            aspNetLessonPlan.Date = LessonPlan.Date;
            aspNetLessonPlan.Duration = LessonPlan.Duration;
            //db.AspNetLessonPlans.Add(aspNetLessonPlan);
            db.SaveChanges();

            int LessonPlanID = db.AspNetLessonPlans.Max(x => x.Id);

            foreach (var topic in LessonPlan.Topics)
            {
                AspNetLessonPlan_Topic lessonPlanTopicExist = db.AspNetLessonPlan_Topic.FirstOrDefault(x => x.TopicID == topic && x.LessonPlanID == LessonPlanID);
                if (lessonPlanTopicExist == null)
                {
                    AspNetLessonPlan_Topic lessonPlanTopic = new AspNetLessonPlan_Topic();
                    lessonPlanTopic.TopicID = topic;
                    lessonPlanTopic.LessonPlanID = LessonPlanID;
                    db.AspNetLessonPlan_Topic.Add(lessonPlanTopic);
                    db.SaveChanges();
                }
            }

            foreach (var breakdown in LessonPlan.BreakDown)
            {
                AspNetLessonPlanBreakdown BreakdownCheck = db.AspNetLessonPlanBreakdowns.FirstOrDefault(x => x.BreakDownHeadingID == breakdown.HeadingID && x.LessonPlanID == LessonPlanID);
                if (BreakdownCheck == null)
                {
                    AspNetLessonPlanBreakdown aspNetLessonBreakdown = new AspNetLessonPlanBreakdown();
                    aspNetLessonBreakdown.LessonPlanID = LessonPlanID;
                    aspNetLessonBreakdown.Minutes = breakdown.Minutes;
                    aspNetLessonBreakdown.Resources = breakdown.Resources;
                    aspNetLessonBreakdown.BreakDownHeadingID = breakdown.HeadingID;
                    aspNetLessonBreakdown.Description = WebUtility.HtmlEncode(breakdown.Description).ToString();
                    db.AspNetLessonPlanBreakdowns.Add(aspNetLessonBreakdown);
                    db.SaveChanges();
                }
                else
                {
                    BreakdownCheck.Minutes = breakdown.Minutes;
                    BreakdownCheck.BreakDownHeadingID = breakdown.HeadingID;
                    BreakdownCheck.Description = breakdown.Description;
                    db.SaveChanges();
                }
            }
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlan aspNetLessonPlan = db.AspNetLessonPlans.Find(id);
            if (aspNetLessonPlan == null)
            {
                return HttpNotFound();
            }
            var lessonplan = db.AspNetSubjects.FirstOrDefault(s => s.Id == aspNetLessonPlan.SubjectID);
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName",lessonplan.ClassID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetLessonPlan.SubjectID);
            ViewBag.LessonPlanId = aspNetLessonPlan.Id;
            //ViewBag.HeadingID = new SelectList(db.AspNetLessonPlanBreakdownHeadings, "Id", "BreakDownHeadingName");
            int? no = aspNetLessonPlan.LessonPlanNo;
            ViewBag.LessonPlanNo = no;
            ViewBag.LessonPlanIDEdit = id;
            ViewBag.Date = aspNetLessonPlan.Date;

            var LessonTopicObj = db.AspNetLessonPlan_Topic.FirstOrDefault(s => s.LessonPlanID == id);
            var topicObj = db.AspNetTopics.FirstOrDefault(s => s.Id == LessonTopicObj.TopicID);
            ViewBag.ChapterID = new SelectList(db.AspNetChapters.Where(x => x.SubjectID == aspNetLessonPlan.SubjectID), "Id", "ChapterName", topicObj.ChapterID);



            return View(aspNetLessonPlan);
        }

        [HttpGet]
        public JsonResult ChapterByLessonPlan(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var LessonTopicObj = db.AspNetLessonPlan_Topic.FirstOrDefault(s => s.LessonPlanID == id);
            var topicObj = db.AspNetTopics.FirstOrDefault(s => s.Id == LessonTopicObj.TopicID);

            AspNetChapter chapterobj = db.AspNetChapters.FirstOrDefault(x => x.Id == topicObj.ChapterID);
            int? chapter = chapterobj.Id;
            return Json(chapter, JsonRequestBehavior.AllowGet);

        }

        // POST: AspNetLessonPlan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Duration,SubjectID")] AspNetLessonPlan aspNetLessonPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetLessonPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetLessonPlan.SubjectID);

            return View(aspNetLessonPlan);
        }

        // GET: AspNetLessonPlan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetLessonPlan aspNetLessonPlan = db.AspNetLessonPlans.Find(id);
            if (aspNetLessonPlan == null)
            {
                return HttpNotFound();
            }
            return View(aspNetLessonPlan);
        }

        // POST: AspNetLessonPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetLessonPlan aspNetLessonPlan = db.AspNetLessonPlans.Find(id);
            db.AspNetLessonPlans.Remove(aspNetLessonPlan);
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


        public class BreakDowns
        {
            public int Id { get; set; }
            public int HeadingID { get; set; }
            public string HeadingName { get; set; }
            public string Description { get; set; }
            public int Minutes { get; set; }
            public string Resources { get; set; }
        }


        public class lessonPlan
        {
            public int lessonPlanNo { get; set; }
            public int classID { get; set; }
            public string className { get; set; }
            public int subjectID { get; set; }
            public string subjectName { get; set; }
            public DateTime Date { get; set; }
            public int ChapterID { get; set; }
            public int[] Topics { get; set; }
            public List<string>GetTopics { get; set; }
            public int Duration { get; set; }
            public List<BreakDowns> BreakDown { get; set; }
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //public void EditLessonPlan(int lessonPlanNo, int classID, int subjectID, DateTime Date, int ChapterID, int[] TopicsList, int Duration)
        //{
        //    var lessonID = Convert.ToInt32(Request.Form["LessonPlanID"]);
        //    AspNetLessonPlan aspNetLessonPlan = db.AspNetLessonPlans.FirstOrDefault(x => x.LessonPlanNo == lessonID);
        //    //aspNetLessonPlan.LessonPlanNo = LessonPlan.lessonPlanNo;
        //    aspNetLessonPlan.SubjectID = subjectID;
        //    aspNetLessonPlan.Date = Date;
        //    aspNetLessonPlan.Duration = Duration;
        //    //db.AspNetLessonPlans.Add(aspNetLessonPlan);
        //    db.SaveChanges();

        //    int LessonPlanID = aspNetLessonPlan.Id;

        //    var Lesson_TopicList = (from lesson_topic in db.AspNetLessonPlan_Topic
        //                            where lesson_topic.LessonPlanID == LessonPlanID
        //                            select lesson_topic).ToList();


        //    foreach (var lesson_topic in Lesson_TopicList)
        //    {
        //        foreach (var topic in TopicsList)
        //        {
        //            //AspNetLessonPlan_Topic lessonPlanTopic = new AspNetLessonPlan_Topic();
        //            lesson_topic.TopicID = topic;
        //            lesson_topic.LessonPlanID = LessonPlanID;
        //            db.SaveChanges();

        //        }
        //    }
        //}


        public class EditLessonPlanType
        {
            public int LessonPlanID { get; set; }
            public int lessonPlanNo { get; set; }
            public int classID { get; set; }
            public string className { get; set; }
            public int subjectID { get; set; }
            public DateTime Date { get; set; }
            public int ChapterID { get; set; }
            public int[] Topics { get; set; }
            public List<string> GetTopics { get; set; }
            public int Duration { get; set; }
        }


        public JsonResult TopicByLesson(int id)
        {

            var topics = (from lesson_topic in db.AspNetLessonPlan_Topic
                                                 where lesson_topic.LessonPlanID == id
                                                 select lesson_topic).ToList();

            return Json(topics, JsonRequestBehavior.AllowGet);

        }

        
        public void EditLessonPlan1234(EditLessonPlanType LessonPlan)
        {
            var TransactionObj = db.Database.BeginTransaction();
            try
            {
                var lessonID = LessonPlan.LessonPlanID;
                AspNetLessonPlan aspNetLessonPlan = db.AspNetLessonPlans.FirstOrDefault(x => x.Id == lessonID);
                //aspNetLessonPlan.LessonPlanNo = LessonPlan.lessonPlanNo;
                aspNetLessonPlan.SubjectID = LessonPlan.subjectID;
                aspNetLessonPlan.Date = LessonPlan.Date;
                aspNetLessonPlan.Duration = LessonPlan.Duration;
                //db.AspNetLessonPlans.Add(aspNetLessonPlan);
                db.SaveChanges();

                int LessonPlanID = aspNetLessonPlan.Id;

                var Lesson_TopicList = (from lesson_topic in db.AspNetLessonPlan_Topic
                                        where lesson_topic.LessonPlanID == LessonPlanID
                                        select lesson_topic).ToList();

                int count = 1;
                do
                {
                    var lessonObj = db.AspNetLessonPlan_Topic.FirstOrDefault(x => x.LessonPlanID == LessonPlanID);

                    if (lessonObj == null)
                    { 
                        count = 0;
                        continue;
                    }
                    db.AspNetLessonPlan_Topic.Attach(lessonObj);
                    db.AspNetLessonPlan_Topic.Remove(lessonObj);
                    db.SaveChanges();
                }
                while (count == 1);

                foreach (var topic in LessonPlan.Topics)
                {
                    AspNetLessonPlan_Topic lessonPlanTopic = new AspNetLessonPlan_Topic();
                    lessonPlanTopic.TopicID = topic;
                    lessonPlanTopic.LessonPlanID = LessonPlanID;
                    db.AspNetLessonPlan_Topic.Add(lessonPlanTopic);
                    db.SaveChanges();

                }

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var UserNameLog = User.Identity.Name;
                AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                string UserIDLog = a.Id;
                var logMessage = "LessonPlan Edited, LessonPlanID: " + lessonID + ", Date: " + LessonPlan.Date;

                var LogControllerObj = new AspNetLogsController();
                LogControllerObj.CreateLogSave(logMessage, UserIDLog);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                db.SaveChanges();
                TransactionObj.Commit();
            }
            
            catch(Exception)
            {
                TransactionObj.Dispose();
            }


        }


        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public JsonResult AddLessonPlan(lessonPlan LessonPlan)
        {
            var TransactionObj = db.Database.BeginTransaction();
            try
            {
                AspNetLessonPlan aspNetLessonPlan = new AspNetLessonPlan();
                aspNetLessonPlan.LessonPlanNo = LessonPlan.lessonPlanNo;
                aspNetLessonPlan.SubjectID = LessonPlan.subjectID;
                aspNetLessonPlan.Date = LessonPlan.Date;
                aspNetLessonPlan.Duration = LessonPlan.Duration;
                db.AspNetLessonPlans.Add(aspNetLessonPlan);
                db.SaveChanges();

                int LessonPlanID = db.AspNetLessonPlans.Max(x => x.Id);

                foreach (var topic in LessonPlan.Topics)
                {
                    AspNetLessonPlan_Topic lessonPlanTopic = new AspNetLessonPlan_Topic();
                    lessonPlanTopic.TopicID = topic;
                    lessonPlanTopic.LessonPlanID = LessonPlanID;
                    db.AspNetLessonPlan_Topic.Add(lessonPlanTopic);
                    db.SaveChanges();

                }

                if(LessonPlan.BreakDown != null)
                foreach (var breakdown in LessonPlan.BreakDown)
                {
                    AspNetLessonPlanBreakdown aspNetLessonBreakdown = new AspNetLessonPlanBreakdown();
                    aspNetLessonBreakdown.LessonPlanID = LessonPlanID;
                    aspNetLessonBreakdown.Minutes = breakdown.Minutes;
                    aspNetLessonBreakdown.Resources = breakdown.Resources;
                    aspNetLessonBreakdown.BreakDownHeadingID = breakdown.HeadingID;
                    aspNetLessonBreakdown.Description = WebUtility.HtmlEncode(breakdown.Description).ToString();
                    db.AspNetLessonPlanBreakdowns.Add(aspNetLessonBreakdown);
                    db.SaveChanges();
                }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var UserNameLog = User.Identity.Name;
                AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                string UserIDLog = a.Id;
                var logMessage = "New LessonPlan Added, SubjectID: " + LessonPlan.subjectID + ", Date: " + LessonPlan.Date;

                var LogControllerObj = new AspNetLogsController();
                LogControllerObj.CreateLogSave(logMessage, UserIDLog);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                db.SaveChanges();
                TransactionObj.Commit();
            }

            catch (Exception)
            {
                TransactionObj.Dispose();
            }

            return Json("data", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLessonPlanFromID(int Id)
        {
            var tempLessonPlan = db.AspNetLessonPlans.Where(x => x.Id == Id).FirstOrDefault();

            lessonPlan LessonPlan = new lessonPlan();
            LessonPlan.GetTopics = new List<string>();
            LessonPlan.BreakDown = new List<BreakDowns>();
            LessonPlan.classID = tempLessonPlan.AspNetSubject.AspNetClass.Id;
            LessonPlan.subjectID = tempLessonPlan.AspNetSubject.Id;
            LessonPlan.className = tempLessonPlan.AspNetSubject.AspNetClass.ClassName;
            LessonPlan.subjectName = tempLessonPlan.AspNetSubject.SubjectName;
            foreach (var topic in tempLessonPlan.AspNetLessonPlan_Topic)
            {
                LessonPlan.GetTopics.Add(topic.AspNetTopic.TopicName);
            }
            foreach (var breakdown in tempLessonPlan.AspNetLessonPlanBreakdowns)
            {
                BreakDowns breakDown = new BreakDowns();
                breakDown.Id = breakdown.Id;
                breakDown.HeadingName = breakdown.AspNetLessonPlanBreakdownHeading.BreakDownHeadingName;
                breakDown.Description = WebUtility.HtmlDecode(breakdown.Description);
                breakDown.Minutes = Convert.ToInt32(breakdown.Minutes);
                breakDown.Resources = breakdown.Resources;
                LessonPlan.BreakDown.Add(breakDown);
            }
            LessonPlan.Date = Convert.ToDateTime(tempLessonPlan.Date);
            LessonPlan.Duration = Convert.ToInt32(tempLessonPlan.Duration);


            return Json(LessonPlan, JsonRequestBehavior.AllowGet);
        }

public JsonResult GetLessonPlan(int SubjectID,DateTime Date)
        {

            try
            {
                var tempLessonPlan = (from lessonplan in db.AspNetLessonPlans
                                      where lessonplan.SubjectID == SubjectID && lessonplan.Date == Date
                                      select lessonplan).FirstOrDefault();

                lessonPlan LessonPlan = new lessonPlan();
                LessonPlan.GetTopics = new List<string>();
                LessonPlan.BreakDown = new List<BreakDowns>();
                LessonPlan.classID = tempLessonPlan.AspNetSubject.AspNetClass.Id;
                LessonPlan.subjectID = tempLessonPlan.AspNetSubject.Id;
                LessonPlan.className = tempLessonPlan.AspNetSubject.AspNetClass.ClassName;
                LessonPlan.subjectName = tempLessonPlan.AspNetSubject.SubjectName;
                foreach (var topic in tempLessonPlan.AspNetLessonPlan_Topic)
                {
                    LessonPlan.GetTopics.Add(topic.AspNetTopic.TopicName);
                }
                foreach (var breakdown in tempLessonPlan.AspNetLessonPlanBreakdowns)
                {
                    BreakDowns breakDown = new BreakDowns();
                    breakDown.Id = breakdown.Id;
                    breakDown.HeadingName = breakdown.AspNetLessonPlanBreakdownHeading.BreakDownHeadingName;
                    breakDown.Description = WebUtility.HtmlDecode(breakdown.Description);
                    breakDown.Minutes = Convert.ToInt32(breakdown.Minutes);
                    breakDown.Resources = breakdown.Resources;
                    LessonPlan.BreakDown.Add(breakDown);
                }
                LessonPlan.Date = Convert.ToDateTime(tempLessonPlan.Date);
                LessonPlan.Duration = Convert.ToInt32(tempLessonPlan.Duration);


                return Json(LessonPlan, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void LessonPlanfromFile(AspNetLessonPlan aspNetLessonPlan)
        {
            HttpPostedFileBase file = Request.Files["LessonPlan"];
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

                List<int> TopicsID = new List<int>();
                int LessonPlanID = -1;
                for (int rowIterator = 1; rowIterator <= noOfRow; rowIterator++)
                {
                    string Heading = workSheet.Cells[rowIterator, 1].Value.ToString();
                    if (Heading == "Lesson No")
                    {
                        aspNetLessonPlan.LessonPlanNo = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                    }
                    else if (Heading == "Chapter")
                    {

                    }
                    else if (Heading == "Topic")
                    {
                        
                        while(true)
                        {
                            rowIterator++;
                            string check = workSheet.Cells[rowIterator, 1].Value.ToString();
                            if (check != "Duration")
                            {
                                string topicName = workSheet.Cells[rowIterator, 1].Value.ToString();
                                int topicID = db.AspNetTopics.Where(x => x.TopicName == topicName).Select(x => x.Id).FirstOrDefault();
                                TopicsID.Add(topicID);
                            }
                            else
                            {
                                rowIterator--;
                                break;
                            }
                        }
                       
                    }
                    else if (Heading == "Duration")
                    {
                        aspNetLessonPlan.Duration = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                        db.AspNetLessonPlans.Add(aspNetLessonPlan);
                        db.SaveChanges();
                        LessonPlanID = db.AspNetLessonPlans.Max(x => x.Id);
                        foreach (var topic in TopicsID)
                        {
                            AspNetLessonPlan_Topic lessonPlanTopic = new AspNetLessonPlan_Topic();
                            lessonPlanTopic.TopicID = topic;
                            lessonPlanTopic.LessonPlanID = LessonPlanID;
                            db.AspNetLessonPlan_Topic.Add(lessonPlanTopic);
                            db.SaveChanges();

                        }
                    }
                    else if (Heading == "Breakdown")
                    {
                        AspNetLessonPlanBreakdown aspNetLessonBreakdown = new AspNetLessonPlanBreakdown();
                        while (true)
                        {
                            rowIterator++;
                            string check = workSheet.Cells[rowIterator, 1].Value.ToString();
                            if (check != "End")
                            {
                                if(check=="Heading")
                                {
                                    
                                    aspNetLessonBreakdown.LessonPlanID = LessonPlanID;
                                    string headingName = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    aspNetLessonBreakdown.BreakDownHeadingID = db.AspNetLessonPlanBreakdownHeadings.Where(x=>x.BreakDownHeadingName== headingName).Select(x=>x.Id).FirstOrDefault();
                                }
                                else if(check=="Description")
                                {
                                    aspNetLessonBreakdown.Description = workSheet.Cells[rowIterator, 2].Value.ToString();
                                }
                                else if (check == "Time")
                                {
                                    aspNetLessonBreakdown.Minutes = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value.ToString());
                                }
                                else if (check == "Resources")
                                {
                                    aspNetLessonBreakdown.Resources= workSheet.Cells[rowIterator, 2].Value.ToString();
                                    db.AspNetLessonPlanBreakdowns.Add(aspNetLessonBreakdown);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if(Heading=="End")
                    {
                        break;
                    }
                }
            }
     
        } 
    }
}
