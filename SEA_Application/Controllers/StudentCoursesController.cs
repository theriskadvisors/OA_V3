using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class StudentCoursesController : Controller
    {
        public static List<question1> QuestionsStaticList = new List<question1>();
        public static string TotalScore { get; set; }
        public static string ReviseLessons { get; set; }

        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        public static int SessionID = Convert.ToInt32(SessionIDStaticController.GlobalSessionID);


        public ActionResult Index()
        {



            return View();
        }

        public ActionResult AllSubjectsOfStudent()
        {

            var userID = User.Identity.GetUserId();
            var UserRole = db.GetUserRoleById(userID).FirstOrDefault();
            int ClassID = db.AspNetClasses.Where(x => x.SessionID == SessionID).FirstOrDefault().Id;


            //var AllSubjectsOfStudent = from Subject in db.AspNetSubjects
            //                           join StudentSubject in db.AspNetStudent_Subject on Subject.Id equals StudentSubject.SubjectID
            //                           where StudentSubject.StudentID == userID
            //                           select new
            //                           {
            //                               Subject.Id,
            //                               Subject.SubjectName,
            //                               Subject.CourseType,
            //                               Subject.Points,
            //                            };

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

        public ActionResult SubjectTopics(int id)
        {


            ViewBag.SubjectId = id;


            return View();
        }

        public ActionResult StudentLessons(int id)
        {

            var Lesson = db.AspnetLessons.Where(x => x.Id == id).FirstOrDefault();

            int? TopicId = Lesson.TopicId;
            string Name = Lesson.Name;

            // tab 6 Data Start
            var questionList_MCQS = new List<question>();
            //    List<AspnetSubjectTopic> SubjectTopics =   db.AspnetSubjectTopics.Where(x => x.Id == 35).ToList();

            List<int> AllLessonofTopics = db.AspnetLessons.Where(x => x.AspnetSubjectTopic.Id == TopicId).Select(x => x.Id).ToList();

            var items = AllLessonofTopics.Select(num => (int?)num).ToList();

            var Questions = from question in db.AspnetQuestions
                            where items.Contains(question.LessonId) && question.Type == "MCQ" && question.Is_Quiz == false
                            select question;

            foreach (var item in Questions)
            {
                var q = new question();
                q.id = item.Id;
                q.name = item.Name;
                q.type = item.Type;

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

            ViewBag.questionList_MCQS = questionList_MCQS;
            //Tab 6 data end

            ViewBag.TopicId = TopicId;
            ViewBag.LessonID = id;

            return View();
        }
        public ActionResult IsLastLesson( int LessonID)
        {
            var Lesson = db.AspnetLessons.Where(x => x.Id == LessonID).FirstOrDefault();

            int? TopicId = Lesson.TopicId;
            string Name = Lesson.Name;

            string LessonLastName = db.AspnetLessons.Where(x => x.TopicId == TopicId).OrderByDescending(x => x.Name).Select(x => x.Name).FirstOrDefault();

            var IsLastLesson = "";

            if (Name == LessonLastName)
            {
                IsLastLesson = "Yes";
            }
            else
            {
                IsLastLesson = "No";

            }

            //ViewBag.TopicId = TopicId;
           // ViewBag.IsLastLesson = IsLastLesson;
           


            return Json(new { TopicId = TopicId , IsLastLesson = IsLastLesson }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetLessonVideo(int LessonID)
        {
            AspnetLesson Lesson = db.AspnetLessons.Where(x => x.Id == LessonID).FirstOrDefault();
            string LessonVideoUrl = "";


            if (Lesson != null)
            {
                LessonVideoUrl = Lesson.Video_Url;

            }

            return Json(new { LessonId = Lesson.Id, LessonName = Lesson.Name, LessonVideo = Lesson.Video_Url, LessonDescription = Lesson.Description }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test(int id )
        {
    
            return View();

        } // End of Test Action Methods


        //public ActionResult submit_question(string Question, string Answer)
        //{
        //    int score = 0;
        //    string[] selectedQuestions = Question.Split(',');
        //    string[] selectedAnswers = Answer.Split(',');

        //    int i = 0;
        //    foreach (var item in selectedQuestions)
        //    {
        //        var ans = db.AspnetQuestions.Where(x => x.Id.ToString() == item).Select(x => x.AnswerId).FirstOrDefault();
        //        string val = selectedAnswers[i];
        //        if (selectedAnswers[i] == ans.ToString())
        //        {
        //            score++;
        //        }
        //        else { }
        //        i++;

        //        // db.SaveChanges();
        //    }
        //    var questionList_MCQS = new List<question>();

        //    i = 0;
        //    foreach (var item in selectedQuestions)
        //    {
        //        var q = new question();

        //        AspnetQuestion Queston = db.AspnetQuestions.Where(x => x.Id.ToString() == item).FirstOrDefault();

        //        q.id = Convert.ToInt32(item);
        //        q.name = Queston.Name;
        //        q.type = Queston.Type;
        //        q.CorrentAnswer = Queston.AnswerId;
        //        q.StudentAnswer = Convert.ToInt32(selectedAnswers[i]);

        //        q.options = new List<option>();
        //        var op = db.AspnetOptions.Where(x => x.QuestionId == q.id).ToList();
        //        foreach (var item1 in op)
        //        {
        //            var op1 = new option();
        //            op1.id = item1.Id;
        //            op1.name = item1.Name;
        //            q.options.Add(op1);
        //        }
        //        questionList_MCQS.Add(q);

        //        i++;
        //    }


        //    // return Content(score.ToString());

        //    //return RedirectToAction("Index");
        //    QuestionsStaticList = questionList_MCQS;
        //    return Json(questionList_MCQS, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult submit_question(string Question, string Answer)
        {
            int score = 0;
            string[] selectedQuestions = Question.Split(',');
            string[] selectedAnswers = Answer.Split(',');

            int i = 0;

            var questionList_MCQS = new List<question1>();

            foreach (var item in selectedQuestions)
            {
                question1 QuestionObj = new question1();

                var ans = db.AspnetQuestions.Where(x => x.Id.ToString() == item).Select(x => x.AnswerId).FirstOrDefault();
                var QuestionFromDB = db.AspnetQuestions.Where(x => x.Id.ToString() == item).FirstOrDefault();

                QuestionObj.name = QuestionFromDB.Name;
                QuestionObj.id = QuestionFromDB.Id;
                QuestionObj.type = QuestionFromDB.Type;

                
                string val = selectedAnswers[i];
                if (selectedAnswers[i] == ans.ToString())
                {


                    score++;

                    var RightAnswer = "Selected Answer is Correct";

                    QuestionObj.Message = RightAnswer;
                    QuestionObj.IsCorrect = "Yes";

                }
                else {


                var AnswerName = db.AspnetOptions.Where(x => x.Id == QuestionFromDB.AnswerId).FirstOrDefault().Name;
                var LessonName = db.AspnetLessons.Where(x => x.Id == QuestionFromDB.LessonId).FirstOrDefault().Name;

                    if(selectedAnswers[i] == "")
                    {

                    QuestionObj.IsCorrect = "No";
                    var WrongAnswer = "Correct Answer  is " + AnswerName +" you need to revise "+ LessonName + " Lesson";

                    QuestionObj.Message = WrongAnswer;

                    }
                    else
                    {

                        QuestionObj.IsCorrect = "No";
                         var WrongAnswer = "Your Answer is Wrong .Correct Answer  is " + AnswerName + " you need to revise " + LessonName +" Lesson";

                        QuestionObj.Message = WrongAnswer;


                    }

                }

                i++;

                questionList_MCQS.Add(QuestionObj);
            }
         
            return Json(questionList_MCQS, JsonRequestBehavior.AllowGet);

        }
        public ActionResult TestResult( )
        {


            ViewBag.TotalScore = TotalScore;
            ViewBag.ReviseLessons = ReviseLessons;
            ViewBag.QuestionsList = QuestionsStaticList;


            return View();
        }

       

        public class question1
        {
            public int id;
            public string name;
            public string type;
            public string Message;
            public string IsCorrect;
          
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
            public int? CorrentAnswer;
            public int? StudentAnswer;
            public List<option> options;
        }

        public ActionResult GetSubjectTopicsAndLessons(int SubjectId)
        {
            //  var SubjectTopics = db.AspnetSubjectTopics.Where(x => x.SubjectId == SubjectId).ToList();


            //var AllSubjectTopicsLessons = from SubjectTopic in db.AspnetSubjectTopics
            //                              join Lesson in db.AspnetLessons on SubjectTopic.Id equals Lesson.TopicId
            //                              where SubjectTopic.SubjectId == SubjectId
            //                              select new
            //                              {
            //                                  TopicId = SubjectTopic.Id,
            //                                  TopicName =   SubjectTopic.Name,
            //                                  LessonId = Lesson.Id,
            //                                  LessonName = Lesson.Name,
            //                                  Lesson.Duration,
            //                                  Lesson.Description

            //                              };

            var SubjectsTopics = db.AspnetSubjectTopics.Where(x => x.SubjectId == SubjectId).ToList();

            var UserId = User.Identity.GetUserId();


            List<Topic> TopicListObj = new List<Topic>();

            int Count = 0;
            foreach (var a in SubjectsTopics)
            {
                int count1 = 0;
                Topic TopicObj = new Topic();

                var list = db.AspnetLessons.Where(x => x.TopicId == a.Id).ToList();

                TopicObj.TopicId = a.Id;
                TopicObj.TopicName = a.Name;



                List<Lesson> LessonsList = new List<Lesson>();


                foreach (var lesson in list)
                {
                    var LessonExist = "";
                    StudentLessonTracking LessonTracking = db.StudentLessonTrackings.Where(x => x.LessonId == lesson.Id & x.StudentId == UserId).FirstOrDefault();

                    if (LessonTracking == null)
                    {
                        LessonExist = "No";
                    }
                    else
                    {
                        LessonExist = "Yes";
                    }
                    Lesson lessonobj = new Lesson();
                    lessonobj.LessonId = lesson.Id;
                    lessonobj.LessonName = lesson.Name;
                    lessonobj.LessonDuration = lesson.Duration;
                    lessonobj.LessonExistInTrackingTable = LessonExist;

                    LessonsList.Add(lessonobj);
                    Count++;
                    count1++;
                }

               List<Lesson> OrderByLessons =  LessonsList.OrderBy(x => x.LessonName).ToList();

                TopicObj.LessonList = OrderByLessons;

                TopicObj.TotalLessons = Count;
                TopicObj.TotalLessons1 = count1;

                TopicListObj.Add(TopicObj);
            }
                
            return Json(TopicListObj.OrderBy(x=>x.TopicName).ToList(), JsonRequestBehavior.AllowGet);

        }
        public ActionResult UpdateStudentLessonTracking(int LessonId)
        {
            var UserId = User.Identity.GetUserId();
            StudentLessonTracking LessonTracking = db.StudentLessonTrackings.Where(x => x.LessonId == LessonId && x.StudentId == UserId).FirstOrDefault();

            if (LessonTracking == null)
            {

                StudentLessonTracking lessonTracking = new StudentLessonTracking();

                lessonTracking.LessonId = LessonId;
                lessonTracking.IsCompleted = true;

                TimeZone time2 = TimeZone.CurrentTimeZone;
                DateTime test = time2.ToUniversalTime(DateTime.Now);
                var pakistan = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
                DateTime pakistantime = TimeZoneInfo.ConvertTimeFromUtc(test, pakistan);
                lessonTracking.StartDate = pakistantime;
                lessonTracking.StudentId = User.Identity.GetUserId();
                lessonTracking.Assignment_Status = "Pending";

                db.StudentLessonTrackings.Add(lessonTracking);
                db.SaveChanges();

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCourseContent(int LessonID)
        {
            var UserId = User.Identity.GetUserId(); 

            var TopicId = db.AspnetLessons.Where(x => x.Id == LessonID).FirstOrDefault().TopicId;
            var SubjectId = db.AspnetSubjectTopics.Where(x => x.Id == TopicId).FirstOrDefault().SubjectId;

            var SubjectsTopics = db.AspnetSubjectTopics.Where(x => x.SubjectId == SubjectId).ToList();

            List<Topic> TopicListObj = new List<Topic>();

            int Count = 0;
            foreach (var a in SubjectsTopics)
            {
                int count1 = 0;
                Topic TopicObj = new Topic();

                var list = db.AspnetLessons.Where(x => x.TopicId == a.Id).ToList();

                TopicObj.TopicId = a.Id;
                TopicObj.TopicName = a.Name;

                List<Lesson> LessonsList = new List<Lesson>();

                foreach (var lesson in list)
                {
                    var LessonExist = "";
                    StudentLessonTracking LessonTracking = db.StudentLessonTrackings.Where(x => x.LessonId == lesson.Id && x.StudentId == UserId).FirstOrDefault();

                    if (LessonTracking == null)
                    {
                        LessonExist = "No";
                    }
                    else
                    {
                        LessonExist = "Yes";
                    }

                    Lesson lessonobj = new Lesson();
                    lessonobj.LessonId = lesson.Id;
                    lessonobj.LessonName = lesson.Name;
                    lessonobj.LessonDuration = lesson.Duration;
                    lessonobj.LessonExistInTrackingTable = LessonExist;

                    LessonsList.Add(lessonobj);
                    Count++;
                    count1++;
                }

                List<Lesson> OrderByLessons = LessonsList.OrderBy(x => x.LessonName).ToList();

                TopicObj.LessonList = OrderByLessons;

                TopicObj.TotalLessons = Count;
                TopicObj.TotalLessons1 = count1;

                TopicListObj.Add(TopicObj);
            }

            // return Json(TopicListObj, JsonRequestBehavior.AllowGet);

            return Json(TopicListObj.OrderBy(x => x.TopicName).ToList(), JsonRequestBehavior.AllowGet);


        }


        public ActionResult StudentAssignment(int LessonID)
        {

            //AspnetStudentAssignment SA  =    db.AspnetStudentAssignments.Where(x => x.LessonId == LessonID).FirstOrDefault();
            //    var AssignmentName = "";
            //    var AssignmentDueDate = "";
            //    int AssignmentId =;
            //    if(SA !=null)
            //    {

            //        AssignmentName = SA.FileName;
            //        AssignmentDueDate =  SA.DueDate.ToString();
            //        AssignmentId = SA.Id;
            //    }

            //  new { StudentAssigmentName = AssignmentName, StudentAssignmentDueDate = AssignmentDueDate, StudentAssignmentId = AssignmentId }
            var UserId = User.Identity.GetUserId();

            int StudentId = db.AspNetStudents.Where(x => x.StudentID == UserId).FirstOrDefault().Id;

            var a = db.AspnetStudentAssignments.Where(x => x.LessonId == LessonID).Select(x => new { x.Id, x.FileName, x.DueDate, x.Name }).FirstOrDefault();


            var AssignmentSubmission = db.AspnetStudentAssignmentSubmissions.Where(x => x.LessonId == LessonID && x.StudentId == StudentId).FirstOrDefault();

            var FileName = "";
            var DueDate = "";
            var AssignmentId = "";
            var AssignName = "";
            if (a != null)
            {
                FileName = a.FileName;
                AssignName = a.Name;

                if (a.DueDate != null)
                {

                    DueDate = Convert.ToString(a.DueDate.Value.Date);
                }

                AssignmentId = Convert.ToString(a.Id);


            }

            var TeacherComments = "";
            if (AssignmentSubmission != null)
            {
                TeacherComments = AssignmentSubmission.TeacherComments;
            }



            return Json(new { StudentAssigmentName = AssignName, FileName = FileName, StudentAssignmentDueDate = DueDate, StudentAssignmentId = AssignmentId, TeacherComments = TeacherComments }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult ReadingMaterials(int LessonID)
        {

            var StudentAttachments = db.AspnetStudentAttachments.Where(x => x.LessonId == LessonID).Select(x => new { AttachmentId = x.Id, AttachmentName = x.Path, AttachmentPath = x.Path }).ToList();

            var StudentLinks = db.AspnetStudentLinks.Where(x => x.LessonId == LessonID).Select(x => new { LinkUrl = x.URL }).ToList();


            return Json(new { StuAttachment = StudentAttachments, StuLinks = StudentLinks }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile(int id)
        {

            AspnetStudentAttachment studentAttachment = db.AspnetStudentAttachments.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/Content/StudentAttachments/"), studentAttachment.Path);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), studentAttachment.Path);


        }

        public ActionResult DownloadFileOfAssignment(string id)
        {
            int idd = Convert.ToInt32(id);

            AspnetStudentAssignment studentAssignment = db.AspnetStudentAssignments.Find(idd);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/Content/StudentAssignments/"), studentAssignment.FileName);

            return File(filepath, MimeMapping.GetMimeMapping(filepath), studentAssignment.FileName);


        }

        public ActionResult StudentAssignmentSubmission(int LessonID)
        {
            var IsSubmitted = "";
            var UserId1 = User.Identity.GetUserId();

            AspNetStudent Student = db.AspNetStudents.Where(x => x.StudentID == UserId1).FirstOrDefault();

            AspnetStudentAssignmentSubmission StudentAssignmentSubmission = db.AspnetStudentAssignmentSubmissions.Where(x => x.LessonId == LessonID && x.StudentId == Student.Id).FirstOrDefault();

            if (StudentAssignmentSubmission == null)
            {
                IsSubmitted = "Submit Assignment Successfully";

                var File = Request.Files["file"];

                var fileName = "";
                if (File.ContentLength > 0)
                {
                    fileName = Path.GetFileName(File.FileName);
                    File.SaveAs(Server.MapPath("~/Content/StudentAssignments/") + fileName);

                }

                AspnetStudentAssignmentSubmission AssignmentSubmission = new AspnetStudentAssignmentSubmission();

                int? TopicId = db.AspnetLessons.Where(x => x.Id == LessonID).FirstOrDefault().TopicId;
                int? SubjectId = db.AspnetSubjectTopics.Where(x => x.Id == TopicId).FirstOrDefault().SubjectId;

                //  db.AspnetSubjectTopics.Where(x => x.Id == LessonID);

                var Subject = db.GenericSubjects.Where(x => x.Id == SubjectId).FirstOrDefault();

                var id = User.Identity.GetUserId();
                var UserId = db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault().Id;

                int StudentId = db.AspNetStudents.Where(x => x.StudentID == UserId).FirstOrDefault().Id;
                int? ClassId = db.AspNetStudents.Where(x => x.StudentID == UserId).FirstOrDefault().ClassID;

                var AssignmentDueDate = db.AspnetStudentAssignments.Where(x => x.LessonId == LessonID).FirstOrDefault().DueDate;


                TimeZone time2 = TimeZone.CurrentTimeZone;
                DateTime test = time2.ToUniversalTime(DateTime.Now);
                var pakistan = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");


                DateTime pakistantime = TimeZoneInfo.ConvertTimeFromUtc(test, pakistan);
                AssignmentSubmission.LessonId = LessonID;
                AssignmentSubmission.TopicId = TopicId;
                AssignmentSubmission.SubjectId = SubjectId;
                AssignmentSubmission.ClassId = ClassId;
                AssignmentSubmission.CourseType = Subject.SubjectType;
                AssignmentSubmission.StudentId = StudentId;
                AssignmentSubmission.AssignmentSubmittedDate = pakistantime;
                AssignmentSubmission.AssignmentDueDate = AssignmentDueDate;
                AssignmentSubmission.AssignmentFileName = fileName;

                db.AspnetStudentAssignmentSubmissions.Add(AssignmentSubmission);
                db.SaveChanges();

            }
            else
            {
                IsSubmitted = "Submit Assignment failed, you have already Submited Assignment";
            }


            StudentLessonTracking LessonTracking = db.StudentLessonTrackings.Where(x => x.LessonId == LessonID && x.StudentId == UserId1).FirstOrDefault();

            if (LessonTracking != null)
            {

                LessonTracking.Assignment_Status = "Submitted";
                db.SaveChanges();

            }


            return Json(IsSubmitted, JsonRequestBehavior.AllowGet);
        }// Student Assignment Submission

        public ActionResult SaveCommentHead(int LessonID, string Title, string Body)
        {

            var id = User.Identity.GetUserId();
            AspnetComment_Head commentHead = new AspnetComment_Head();

            //Comment_Head commentHead = new Comment_Head();
            commentHead.Comment_Head = Title;
            commentHead.CommentBody = Body;
            commentHead.LessonId = LessonID;
            commentHead.CreatedBy = id;
            commentHead.CreationDate = GetLocalDateTime.GetLocalDateTimeFunction();
            db.AspnetComment_Head.Add(commentHead);
            db.SaveChanges();


            var UserId = User.Identity.GetUserId();
            var UserName = db.AspNetUsers.Where(x => x.Id == UserId).FirstOrDefault().Name;
            var NotificationObj = new AspNetNotification();
            NotificationObj.Description = UserName + " asked a Question";
            NotificationObj.Subject = "Student Comment ";
            NotificationObj.SenderID = UserId;
            NotificationObj.Time = GetLocalDateTime.GetLocalDateTimeFunction();
            NotificationObj.Url = "/TeacherCommentsOnCourses/CommentsPage1/" + commentHead.Id;

            db.AspNetNotifications.Add(NotificationObj);
            db.SaveChanges();


            int? TopicId = db.AspnetLessons.Where(x => x.Id == LessonID).FirstOrDefault().TopicId;
            int? SubjectId = db.AspnetSubjectTopics.Where(x => x.Id == TopicId).FirstOrDefault().SubjectId;

            var AllTeachers = db.Teacher_GenericSubjects.Where(x => x.SubjectId == SubjectId).Select(x => x.TeacherId);

            var UnionTeachers = AllTeachers.Distinct();

            var AllEmployeesUserId = from employee in db.AspNetEmployees
                                     where AllTeachers.Contains(employee.Id)
                                     select new
                                     {
                                         employee.UserId,
                                     };



            SEA_DatabaseEntities db2 = new SEA_DatabaseEntities();

            foreach (var receiver in AllEmployeesUserId)
            {


                var notificationRecieve = new AspNetNotification_User();
                notificationRecieve.NotificationID = NotificationObj.Id;
                notificationRecieve.UserID = Convert.ToString(receiver.UserId);
                notificationRecieve.Seen = false;
                db2.AspNetNotification_User.Add(notificationRecieve);
                try
                {

                    db2.SaveChanges();
                }

                catch (Exception ex)
                {


                    var Msg = ex.Message;
                }


            }


            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllCommentsHead(int LessonID)
        {
            var AllCommentHead = from commentHead in db.AspnetComment_Head
                                 join user in db.AspNetUsers on commentHead.CreatedBy equals user.Id
                                 where commentHead.LessonId == LessonID
                                 select new
                                 {
                                     CommentHeadId = commentHead.Id,
                                     Title = commentHead.Comment_Head,
                                     Body = commentHead.CommentBody,
                                     LessonId = commentHead.LessonId,
                                     UserName = user.Name,
                                     Date = commentHead.CreationDate,
                                 };

            return Json(AllCommentHead, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CommentsPage(int? CommentHeadId)
        {
            //var commentHead = db.Comment_Head.Where(x => x.Id == CommentHeadId).FirstOrDefault();

            // commentHead
            return RedirectToAction("CommentsPage1", CommentHeadId);

        }

        public ActionResult CommentsPage1(int id)
        {
            ViewBag.CommentHeadId = id;

            ViewBag.LessonId = db.AspnetComment_Head.Where(x => x.Id == id).FirstOrDefault().LessonId;

            return View("Comments");
        }

        public ActionResult GetCommentHead(int CommentHeadId)
        {
            // var commentHead = db.Comment_Head.Where(x => x.Id == CommentHeadId).FirstOrDefault();

            var CommentHead = (from commentHead in db.AspnetComment_Head
                               join user in db.AspNetUsers on commentHead.CreatedBy equals user.Id
                               where commentHead.Id == CommentHeadId
                               select new
                               {
                                   CommentHeadId = commentHead.Id,
                                   Title = commentHead.Comment_Head,
                                   Body = commentHead.CommentBody,
                                   LessonId = commentHead.LessonId,
                                   UserName = user.Name,
                                   Date = commentHead.CreationDate,
                               }).FirstOrDefault();



            return Json(CommentHead, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CommentReply(int CommentHeadId, string UserComment)
        {

            var id = User.Identity.GetUserId();

            int count = db.AspnetComments.Count();
            AspnetComment commentobj = new AspnetComment();

            if (count == 0)
            {
                commentobj.ParentCommentId = null;
                commentobj.Comment = UserComment;
                commentobj.CreationDate = GetLocalDateTime.GetLocalDateTimeFunction();
                commentobj.HeadId = CommentHeadId;
                commentobj.CreatedBy = id;
                db.AspnetComments.Add(commentobj);
                db.SaveChanges();
            }
            else
            {
                int LastId = db.AspnetComments.OrderByDescending(o => o.Id).FirstOrDefault().Id;
                commentobj.ParentCommentId = LastId;
                commentobj.Comment = UserComment;
                commentobj.CreationDate = GetLocalDateTime.GetLocalDateTimeFunction();
                commentobj.HeadId = CommentHeadId;
                commentobj.CreatedBy = id;

                db.AspnetComments.Add(commentobj);
                db.SaveChanges();
            }


            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllComments(int CommentHeadId)
        {
            var Comments = from comment in db.AspnetComments
                           join user in db.AspNetUsers on comment.CreatedBy equals user.Id
                           where comment.HeadId == CommentHeadId
                           select new
                           {
                               CommentName = comment.Comment,
                               UserName = user.Name,
                               Date = comment.CreationDate,
                           };


            return Json(Comments, JsonRequestBehavior.AllowGet);
        }


        public class Lesson
        {
            public int LessonId { get; set; }
            public string LessonName { get; set; }
            public TimeSpan? LessonDuration { get; set; }

            public string LessonExistInTrackingTable { get; set; }

            public int LessonCount { get; set; }
        }

        public class Topic
        {
            public int TopicId { get; set; }
            public string TopicName { get; set; }
            public int TopicDuration { get; set; }

            public int TotalLessons { get; set; }
            public int TotalLessons1 { get; set; }
            public List<Lesson> LessonList { get; set; }

        }




    }
}