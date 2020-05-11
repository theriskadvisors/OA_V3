using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspnetSubjectTopicsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();


        // GET: AspnetSubjectTopics
        public ActionResult Index()
        {

            //     var aspnetSubjectTopicsTesting = db.AspnetSubjectTopics.Include(a => a.GenericSubject));

            //var UserId = User.Identity.GetUserId();
            //int id  =  db.AspNetEmployees.Where(x => x.UserId == UserId).FirstOrDefault().Id;
            //var aspnetSubjectTopics = db.AspnetSubjectTopics.Include(a => a.GenericSubject.Teacher_GenericSubjects.Where(x=>x.TeacherId== id));


            var aspnetSubjectTopics = db.AspnetSubjectTopics.Include(a => a.GenericSubject);
            return View(aspnetSubjectTopics.ToList());

            //  return View();
        }

        public ActionResult ViewTopicsAndLessons()
        {
            //var aspnetSubjectTopics = db.AspnetSubjectTopics.Include(a => a.GenericSubject);
            //return View(aspnetSubjectTopics.ToList());
            var aspnetSubjectTopics = db.AspnetSubjectTopics.Include(a => a.GenericSubject);
            return View(aspnetSubjectTopics.ToList());

        }
        public ActionResult AllLessonsList()
        {

            var AllLessons = (from lesson in db.AspnetLessons
                              select new
                              {
                                  LessonId = lesson.Id,
                                  LessonSubjectTopicName = lesson.AspnetSubjectTopic.Name,
                                  LessonName = lesson.Name,
                                  LessonVidoeUrl = lesson.Video_Url,
                                  LessonDuration = lesson.DurationMinutes,
                                  LessonDescription = lesson.Description,
                              }).ToList();

            return Json(AllLessons, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllSubjectTopicList()
        {

            var AllSubjectTopicList = (from subjectTopic in db.AspnetSubjectTopics
                                       select new
                                       {
                                           subjectTopic.Id,
                                           subjectTopic.Name,
                                           subjectTopic.StartDate,
                                           subjectTopic.GenericSubject.SubjectName,
                                           subjectTopic.FAQ,
                                           subjectTopic.Description,
                                       }).ToList();



            return Json(AllSubjectTopicList, JsonRequestBehavior.AllowGet);
        }



        // GET: AspnetSubjectTopics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetSubjectTopic aspnetSubjectTopic = db.AspnetSubjectTopics.Find(id);
            if (aspnetSubjectTopic == null)
            {
                return HttpNotFound();
            }
            return View(aspnetSubjectTopic);
        }

        // GET: AspnetSubjectTopics/Create
        public ActionResult Create()
        {
            ViewBag.SubjectId = new SelectList(db.GenericSubjects, "Id", "SubjectName");

            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");



            return View();
        }



        // POST: AspnetSubjectTopics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,SubjectId,FAQ,OrderBy")] AspnetSubjectTopic aspnetSubjectTopic)
        {
            aspnetSubjectTopic.StartDate = null;
            aspnetSubjectTopic.EndDate = null;

            if (ModelState.IsValid)
            {
                db.AspnetSubjectTopics.Add(aspnetSubjectTopic);
                db.SaveChanges();
                return RedirectToAction("ViewTopicsAndLessons");
            }

            var UserId = User.Identity.GetUserId();
            //Teacher Subjects 
            var SubjectofCurrentSessionTeacher = from subject in db.GenericSubjects
                                                 join TeacherSubject in db.Teacher_GenericSubjects on subject.Id equals TeacherSubject.SubjectId
                                                 join employee in db.AspNetEmployees on TeacherSubject.TeacherId equals employee.Id
                                                 where employee.UserId == UserId
                                                 select new
                                                 {
                                                     subject.Id,
                                                     subject.SubjectName,
                                                 };

            ViewBag.SubjectId = new SelectList(SubjectofCurrentSessionTeacher, "Id", "SubjectName", aspnetSubjectTopic.SubjectId);
            return View(aspnetSubjectTopic);
        }


        public ActionResult CheckTopicOrderBy(string SubjectId, string OrderBy)
        {


            int SubId = Convert.ToInt32(SubjectId);
            int OrderByValue = Convert.ToInt32(OrderBy);

            var TopicExist = "";
            AspnetSubjectTopic subjectTopic = db.AspnetSubjectTopics.Where(x => x.SubjectId == SubId && x.OrderBy ==OrderByValue).FirstOrDefault();

            if (subjectTopic == null)
            {
                TopicExist = "No";
            }
            else
            {
                TopicExist = "Yes";
            }


            return Json(TopicExist, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult StudentByClass(int id)
        //{
        //    var result1 = db.AspNetSubjects.Where(x => x.ClassID == id).ToList();
        //    var obj = JsonConvert.SerializeObject(result1);
        //       return Json(obj, JsonRequestBehavior.AllowGet);

        //}


        // GET: AspnetSubjectTopics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetSubjectTopic aspnetSubjectTopic = db.AspnetSubjectTopics.Find(id);
            if (aspnetSubjectTopic == null)
            {
                return HttpNotFound();

            }

            //Teacher Subjects 



            int? SubjectId = aspnetSubjectTopic.SubjectId;

            GenericSubject Subject = db.GenericSubjects.Where(x => x.Id == SubjectId).FirstOrDefault();



            //    ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName", Subject.ClassID);
            //     ViewBag.SubjectId = new SelectList(db.GenericSubjects.Where(x=>x.SubjectType == Subject.SubjectType), "Id", "SubjectName", aspnetSubjectTopic.SubjectId);
            ViewBag.SubjectId = new SelectList(db.GenericSubjects.Where(x => x.SubjectType == Subject.SubjectType), "Id", "SubjectName", aspnetSubjectTopic.SubjectId);


            ViewBag.CTId = Subject.SubjectType;

            ViewBag.OrderBy = aspnetSubjectTopic.OrderBy;

            return View(aspnetSubjectTopic);
        }

        // POST: AspnetSubjectTopics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,SubjectId,FAQ,OrderBy")] AspnetSubjectTopic aspnetSubjectTopic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnetSubjectTopic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewTopicsAndLessons");
            }

            ViewBag.SubjectId = new SelectList(db.GenericSubjects, "Id", "SubjectName", aspnetSubjectTopic.SubjectId);
            return View(aspnetSubjectTopic);
        }

        // GET: AspnetSubjectTopics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetSubjectTopic aspnetSubjectTopic = db.AspnetSubjectTopics.Find(id);
            if (aspnetSubjectTopic == null)
            {
                return HttpNotFound();
            }
            return View(aspnetSubjectTopic);
        }

        // POST: AspnetSubjectTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspnetSubjectTopic aspnetSubjectTopic = db.AspnetSubjectTopics.Find(id);
            db.AspnetSubjectTopics.Remove(aspnetSubjectTopic);
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


        public JsonResult GetSubjectsByClass(int ClassID)
        {
            var SubjectsByClass = db.AspNetSubjects.Where(x => x.ClassID == ClassID).ToList();
            //string status = Newtonsoft.Json.JsonConvert.SerializeObject(SubjectsByClass);

            return Json(SubjectsByClass, JsonRequestBehavior.AllowGet);
            //    return Content(status);

        }

    }
}
