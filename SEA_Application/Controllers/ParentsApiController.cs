using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SEA_Application.Models;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class ParentsApiController : ApiController
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        // GET: api/ParentsApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ParentsApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ParentsApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ParentsApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ParentsApi/5
        public void Delete(int id)
        {
        }

        // ----------------------------------------------          Get Children  --------------------------------------------

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetChildren(string Id)
        {
            var Children = db.AspNetParent_Child.Where(x => x.ParentID == Id).Select(x => new { x.ChildID, x.AspNetUser.Name }).ToList();
            
            var javaScriptSerializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(Children);

            return jsonString;
        }

        // ----------------------------------------------          Term Assessment --------------------------------------------

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetSession()
        {
            var session = db.AspNetSessions.Select(x=> new { x.SessionName , x.Id}).ToList();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(session);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetTerm(int Id)
        {
            var Term = db.AspNetTerms.Where(x => x.SessionID == Id).Select(x => new { x.Id, x.TermName }).ToList();

            var javaScriptSerializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(Term);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string StudentAssessment(string ChildId, int termID)
        {

            bool Submited_flag;

            try
            {
                var check = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termID && x.StudentID == ChildId && x.Status == "Principle_submit").Select(x => x.Id).ToList();

                if (check.Count == 0)
                    Submited_flag = false;
                else
                    Submited_flag = true;
            }
            catch
            {
                Submited_flag = false;
            }



            Student_data Assessment_DATA = new Student_data();
            Assessment_DATA.assessment_data = new List<subject>();

            if (Submited_flag)
            {
                var STAID = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termID && x.StudentID == ChildId).Select(x => x.Id).ToList();
                Assessment_DATA.PrincipleComment = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termID && x.StudentID == ChildId).Select(x => x.PrincipalComments).FirstOrDefault();


                var catageory = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.AspNetStudent_Term_Assessment.TermID == termID && x.AspNetStudent_Term_Assessment.StudentID == ChildId).Select(x => x.Catageory).Distinct().ToList();

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

            var javaScriptSerializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(Assessment_DATA);

            return jsonString;
        }

        // ----------------------------------------------          Parent Teacher Meeting --------------------------------------------

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string ParentQuestiones(string ChildId, int SubjectID)
        {
            
            string ChildID = ChildId;
            string parentID = db.AspNetParent_Child.Where(x => x.ChildID == ChildID).Select(x => x.ParentID).FirstOrDefault();
            int? subjectID = SubjectID;
            int meetingID = db.AspNetParentTeacherMeetings.Max(x => x.Id);

            int PTMID = db.AspNetPTMAttendances.Where(x => x.MeetingID == meetingID && x.SubjectID == subjectID && x.ParentID == parentID).Select(x => x.Id).FirstOrDefault();

            var data = db.AspNetPTM_ParentFeedback.Where(x => x.PTMID == PTMID).Select(x => new {x.AspNetFeedBackForm.Question, x.Id , x.FeedBack } ).ToList();

            var javaScriptSerializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(data);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string parentFeedback(List<feedbacks> feedbacks)
        {
            foreach (var item in feedbacks)
            {
                if (item.feedback == null)
                {
                    item.feedback = "";
                }
                AspNetPTM_ParentFeedback aspnetptmparentfeedback = db.AspNetPTM_ParentFeedback.Where(x => x.Id == item.Id).FirstOrDefault();
                aspnetptmparentfeedback.FeedBack = item.feedback;
                db.SaveChanges();
            }
            return "Feedback has been saved";
        }

        // ----------------------------------------------          Subjects By Student --------------------------------------------

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string SubjectsByStudent(string ChildId)
        {
            var subjects = (from student_subject in db.AspNetStudent_Subject
                            where student_subject.StudentID == ChildId
                            select new { student_subject.SubjectID, student_subject.AspNetSubject.SubjectName }).ToList();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(subjects);

            return jsonString;
        }

        // ----------------------------------------------          Attendance --------------------------------------------

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetSubjectAttendance(string ChildId, int SubjectID)
        {
            var attendances = (from attendance in db.AspNetStudent_Attendance
                               where attendance.StudentID == ChildId && attendance.AspNetAttendance.SubjectID == SubjectID
                               select new { attendance.Id, attendance.Reason, attendance.AspNetAttendance.AspNetSubject.SubjectName, attendance.Status, attendance.AspNetAttendance.Date }).ToList();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(attendances);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetAttendance(string ChildId)
        {
            var attendances = (from attendance in db.AspNetStudent_Attendance
                               where attendance.StudentID == ChildId
                               select new { attendance.Id ,attendance.Reason, attendance.AspNetAttendance.AspNetSubject.SubjectName, attendance.Status, attendance.AspNetAttendance.Date }).ToList();



            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(attendances);

            return jsonString;
        }


        // ----------------------------------------------         Project --------------------------------------------

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetProjet(string ChildId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var projects = (from project in db.AspNetStudent_Project
                            where project.StudentID == ChildId
                            select project.AspNetProject).OrderByDescending(x=> x.PublishDate).ToList();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(projects);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetProjetDetail(int Id, string StudentID)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            var aspNetStudentProject = db.AspNetStudent_Project.Where(x => x.ProjectID == Id && x.StudentID == StudentID).Select(x=> new { x.Id, teacherName = x.AspNetProject.AspNetSubject.AspNetUser.Name, x.SubmissionDate, x.SubmissionStatus, StudentName = x.AspNetUser.Name, x.AspNetProject.Title, x.AspNetProject.PublishDate, x.AspNetProject.AspNetSubject.SubjectName }).FirstOrDefault();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(aspNetStudentProject);

            return jsonString;
        }

        // ----------------------------------------------          Announcement      --------------------------------------------

        public string GetAnnouncement(string ChildId)
        {
            var announcement = (from t1 in db.AspNetAnnouncement_Subject
                                join t3 in db.AspNetStudent_Subject on t1.SubjectID equals t3.SubjectID
                                join t4 in db.AspNetUsers on t3.StudentID equals t4.Id
                                join t2 in db.AspNetStudent_Announcement on t1.AnnouncementID equals t2.AnnouncementID

                                where t2.StudentID == ChildId && t4.Id == ChildId
                                select new { t1.AspNetAnnouncement.Title, t2.Id, t1.AspNetSubject.SubjectName, t2.Seen, t1.AspNetAnnouncement.Description, t1.AspNetAnnouncement.Date }).ToList();
            

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(announcement);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public void SeeAnnouncement(int Id)
        {
            var announcementID = db.AspNetAnnouncement_Subject.Where(s => s.Id == Id).Select(s => s.AnnouncementID).SingleOrDefault();
            AspNetStudent_Announcement result = db.AspNetStudent_Announcement.Where(x => x.Id == Id).Select(x => x).FirstOrDefault();

            result.Seen = true;
            db.SaveChanges();
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetSubjectAnnouncement(string ChildId, int SubjectID)
        {
            var announcement = (from t1 in db.AspNetAnnouncement_Subject
                                join t3 in db.AspNetStudent_Subject on t1.SubjectID equals t3.SubjectID
                                join t4 in db.AspNetUsers on t3.StudentID equals t4.Id
                                join t2 in db.AspNetStudent_Announcement on t1.AnnouncementID equals t2.AnnouncementID

                                where t2.StudentID == ChildId && t4.Id == ChildId && t3.SubjectID == SubjectID
                                select new { t1.AspNetAnnouncement.Title, t1.Id, t1.AspNetSubject.SubjectName }).ToList();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(announcement);

            return jsonString;

        }

        // ----------------------------------------------        Homework Diary      --------------------------------------------

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetAllDiary(string ChildId)
        {
            var diary = (from stHW in db.AspNetStudent_HomeWork
                          join hw in db.AspNetHomeworks on
                          stHW.HomeworkID equals hw.Id
                          where stHW.StudentID==ChildId
                          select new { stHW.HomeworkID, stHW.ParentComments, stHW.TeacherComments, stHW.Status, stHW.AspNetUser.Name, hw.Date }).OrderByDescending(x => x.Date).ToList();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(diary);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string ParentCommentAPI(string comment, int HomeworkId, String StudentID)
        {
            string status = "error";
            var dbTransection = db.Database.BeginTransaction();
            db.AspNetStudent_HomeWork.Where(x => x.StudentID == StudentID && x.HomeworkID == HomeworkId).FirstOrDefault().ParentComments = comment;
            db.AspNetStudent_HomeWork.Where(x => x.StudentID == StudentID && x.HomeworkID == HomeworkId).FirstOrDefault().Status = "Seen by Parent";
            //studenthomework.ParentComments = comment;
            //db.AspNetStudent_HomeWork.Add(studenthomework);
            if (db.SaveChanges() > 0)
            {
                status = "success";
            }
            dbTransection.Commit();

            return status;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetParentFilterDiary(string ChildId, DateTime Date)
        {
            var diary = (from stHW in db.AspNetStudent_HomeWork
                         join hw in db.AspNetHomeworks on
                         stHW.HomeworkID equals hw.Id
                         where stHW.StudentID == ChildId
                         select new { stHW.HomeworkID, stHW.ParentComments, stHW.TeacherComments, stHW.Status, stHW.AspNetUser.Name, hw.Date }).OrderByDescending(x => x.Date).ToList();
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(diary);

            return jsonString;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetParentDiary(int Id)
        {

            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            Diary Diary = new Diary();
            Diary.Homework = db.AspNetHomeworks.Find(Id);
            var down = db.AspNetSubject_Homework.Where(x => x.HomeworkID == Diary.Homework.Id).Select(x => new { x.Id, x.HomeworkDetail, x.SubjectID }).ToList();
            Diary.DairyDetail = new List<DairyDetail>();
            foreach (var item in down)
            {
                DairyDetail DOWN = new DairyDetail();
                DOWN.SubjectName = db.AspNetSubjects.Where(x => x.Id == item.SubjectID).Select(x => x.SubjectName).FirstOrDefault();
                DOWN.SubjectWork = item.HomeworkDetail;
                Diary.DairyDetail.Add(DOWN);
            }
            DairyDetail DOWN1 = new DairyDetail();
            DOWN1.SubjectName = "Reading";
            DOWN1.SubjectWork = db.AspNetStudent_HomeWork.Where(x => x.HomeworkID == Id).Select(x => x.Reading).FirstOrDefault();
            Diary.DairyDetail.Add(DOWN1);        
            var idd = db.AspNetStudent_HomeWork.Where(x => x.Id == Id).Select(x => x.StudentID).FirstOrDefault();
            Diary.Name = db.AspNetUsers.Where(x => x.Id == idd).Select(x => x.Name).FirstOrDefault();
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(Diary.DairyDetail);
            return jsonString;
        }

        // ----------------------------------------------        Classes      --------------------------------------------


        public class Diary
        {
            public string Name { set; get; }
            public AspNetHomework Homework { set; get; }
            public List<DairyDetail> DairyDetail { set; get; }
        }
        public class DairyDetail
        {
            public string SubjectName { get; set; }
            public string SubjectWork { get; set; }
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

        public class feedbacks
        {
            public int Id { get; set; }
            public string feedback { get; set; }
        }

    }
}
