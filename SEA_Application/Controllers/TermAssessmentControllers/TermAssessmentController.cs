using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SEA_Application.Models.StudentAssessments;

namespace SEA_Application.Controllers.TermAssessmentControllers
{
    public class TermAssessmentController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        // GET: TermAssessment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EvaluationForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateTermAssessment()
        {
            try
            {
                var sessionID = db.AspNetSessions.Max(x => x.Id);
                ViewBag.TermID = new SelectList(db.AspNetTerms.Where(x => x.SessionID == sessionID), "Id", "TermName");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Please add Session and Its Term first";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTermAssessments()
        {
            var sessionID = db.AspNetSessions.Max(x => x.Id);
            ViewBag.TermID = new SelectList(db.AspNetTerms.Where(x => x.SessionID == sessionID), "Id", "TermName");

            var terms = Request.Form["TermID"];

            var Student_Subject = db.AspNetStudent_Subject.ToList();

            var Check_term = db.AspNetStudent_Term_Assessment.ToList();
            bool term_flag = true;

            foreach (var item in Check_term)
            {
                if (item.TermID == Convert.ToInt32(terms))
                    term_flag = false;
            }

            if (term_flag)
                foreach (var item in Student_Subject)
                {
                    AspNetStudent_Term_Assessment STA = new AspNetStudent_Term_Assessment();
                    //db.AspNetStudent_Term_Assessment
                    STA.StudentID = item.StudentID;
                    STA.TermID = Convert.ToInt32(terms);
                    STA.SubjectID = item.SubjectID;
                    STA.Status = "Started";


                    db.AspNetStudent_Term_Assessment.Add(STA);
                    db.SaveChanges();


                    var STAID = db.AspNetStudent_Term_Assessment.Max(x => x.Id);


                    var subjectquestions = db.AspNetAssessment_Question.Where(x => x.SubjectID == STA.SubjectID).ToList();
                    foreach (var subject_question in subjectquestions)
                    {
                        AspNetStudent_Term_Assessments_Answers staa = new AspNetStudent_Term_Assessments_Answers();
                        staa.STAID = STAID;
                        staa.Question = subject_question.Question;
                        staa.Catageory = subject_question.AspNetAssessment_Questions_Category.CategoryName;
                        staa.Answer = "";

                        db.AspNetStudent_Term_Assessments_Answers.Add(staa);

                    }

                    db.SaveChanges();
                }

            if (term_flag)
                ViewBag.term = "true";
            else
                ViewBag.term = "false";


            return View();
        }

        public JsonResult CheckInitialAssessment(int Id)
        {
            var initial = db.AspNetStudent_Term_Assessment.Where(x => x.Status == "Started" && x.SubjectID == Id).ToList();

            if (initial.Count > 0)
            {
                return Json("Yes", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult TermQuestionesBySubjectId(int Id)
        {
            int CurrentTerm = db.AspNetTerms.Select(x => x.Id).Max();
            var questiones = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.AspNetStudent_Term_Assessment.SubjectID == Id && x.AspNetStudent_Term_Assessment.TermID == CurrentTerm).Select(x => new { x.Question, x.Catageory }).Distinct();
            return Json(questiones, JsonRequestBehavior.AllowGet);
        }

        public class assessmentQuestions
        {
            public int SubjectID { get; set; }
            public string[] QuestionList { get; set; }
        }
        public JsonResult Approve_Evaluation(assessmentQuestions AssessmentQuestions)
        {
            int CurrentTerm = db.AspNetTerms.Select(x => x.Id).Max();
            var STAIDs = db.AspNetStudent_Term_Assessment.Where(x => x.SubjectID == AssessmentQuestions.SubjectID && x.TermID == CurrentTerm).Select(x => x).ToList();

            foreach (var item in STAIDs)
            {
                item.Status = "Not Submitted";

                if (AssessmentQuestions.QuestionList != null)
                    foreach (var questions in AssessmentQuestions.QuestionList)
                    {
                        var question = new AspNetStudent_Term_Assessments_Answers();
                        question.STAID = item.Id;
                        question.Question = questions;
                        question.Catageory = "General Category";
                        db.AspNetStudent_Term_Assessments_Answers.Add(question);
                    }
            }
            db.SaveChanges();

            return Json("Saved Successfully", JsonRequestBehavior.AllowGet);
        }

        class List
        {
            List<list> list { set; get; }
        }

        class list
        {
            public string List { set; get; }
        }

        public JsonResult Evaluation(string StudentId, int SubjectId)
        {

            int Current_term = (int)db.AspNetStudent_Term_Assessment.Max(x => x.TermID);
            int STAID = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == StudentId && x.SubjectID == SubjectId && x.TermID == Current_term).Select(x => x.Id).FirstOrDefault();


            // var questiones = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.STAID == STAID).GroupBy(x => x.Catageory).ToList();

            var Questiones = (from question in db.AspNetStudent_Term_Assessments_Answers
                              where question.STAID == STAID
                              group question by question.Catageory into g
                              select new
                              {
                                  key = g.Key,
                                  TeacherComment = g.Select(x => x.AspNetStudent_Term_Assessment.TeacherComments).FirstOrDefault(),
                                  status = g.Select(x => x.AspNetStudent_Term_Assessment.Status).FirstOrDefault(),
                                  questions = (from q in g.ToList()
                                               select new { q.Id, q.Question, q.Answer }
                                               ).ToList()
                              }).ToList();

            //var catageory = db.AspNetAssessment_Questions_Category.Select(x => new {
            //    id = x.Id,
            //    name = x.CategoryName
            //});

            //foreach (var item in catageory)
            //{
            //    var AQID = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.STAID == STAID && x.AspNetAssessment_Question.QuestionCategory == item.id).ToList();
            //    Dictionary<int, string> Question_list = new Dictionary<int, string>();

            //    foreach (var Item in AQID)
            //    {
            //        Question_list.Add(Item.Id, Item.AspNetAssessment_Question.Question);
            //    }

            //    if(Question_list.Count != 0)
            //    Catageory.Add(item.name, Question_list);
            //}

            ///ViewBag.questionData = Catageory;
            return Json(Questiones, JsonRequestBehavior.AllowGet);
        }



        public PartialViewResult ClassEvaluation(string StudentId, string type, string TermId)
        {
            int TId = int.Parse(TermId);
            Dictionary<classEvaluationKey, Dictionary<int, string>> catageory = new Dictionary<classEvaluationKey, Dictionary<int, string>>();
            try
            {
                var subjects = db.GetStudentSubjects(StudentId).ToList();
                List<AssessmentsQ_A> allquestions = new List<AssessmentsQ_A>();
                foreach (var sub in subjects)
                {

                    var questions = db.GetStudentTermSubjectAssessment(StudentId, TId, sub.Id).ToList();

                    AssessmentsQ_A snderway = new AssessmentsQ_A();




                    foreach (var que in questions)
                    {
                        AssessmentsQ_A ques = new AssessmentsQ_A();

                        ques.Id = que.Id;
                        ques.Question = que.Question;
                        ques.STAID = que.STAID;
                        ques.Catageory = que.Catageory;
                        ques.Answer = que.Answer;
                        ques.SubjectId = sub.Id;
                        ques.SubjectName = sub.SubjectName;

                        allquestions.Add(ques);
                    }
                }

                int Current_term = (int)db.AspNetStudent_Term_Assessment.Max(x => x.TermID);
                var STAID = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == StudentId && x.TermID == Current_term && (x.Status == "submit" || x.Status == "Teacher_submit" || x.Status == "Principle_submit")).Select(x => x.Id).ToList();

                foreach (var item in STAID)
                {
                    Dictionary<int, string> Question_list = new Dictionary<int, string>();
                    classEvaluationKey CEK = new classEvaluationKey();

                    var AQID = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.STAID == item).ToList();
                    string subject = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.STAID == item).Select(x => x.AspNetStudent_Term_Assessment.AspNetSubject.SubjectName).First();

                    foreach (var data in AQID)
                    {
                        Question_list.Add(data.Id, data.Question);
                    }
                    if (Question_list.Count != 0)
                    {
                        CEK.name = subject;
                        CEK.id = item;
                        CEK.isNotify = AQID.Select(x => x.AspNetStudent_Term_Assessment.NotifyTeacher).FirstOrDefault().ToString();
                        catageory.Add(CEK, Question_list);
                    }

                }

                ViewBag.questionData = subjects;
                ViewBag.type = type;
            }
            catch
            {
                ViewBag.questionData = "";
                ViewBag.type = "";
            }
            return PartialView();
        }

        public ActionResult ClassEvaluations(string StudentId, string type, string TermId)
        {
            int TId = int.Parse(TermId);
            Dictionary<classEvaluationKey, Dictionary<int, string>> catageory = new Dictionary<classEvaluationKey, Dictionary<int, string>>();
            var subjects = db.GetStudentSubjects(StudentId).ToList();
            var comments = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == StudentId && x.TermID == TId).FirstOrDefault();
            string parentComent = "";
            string teacherComent = "";
            string prinpipalComent = "";
            if (comments != null)
            {
                parentComent = comments.ParentsComments;
                teacherComent = comments.TeacherComments;
                prinpipalComent = comments.PrincipalComments;
            }
            else
            {
                parentComent = "";
                teacherComent = "";
                prinpipalComent = "";
            }
            var ClassId = db.AspNetStudents.Where(p => p.StudentID == StudentId).FirstOrDefault().ClassID;
            var classname = db.AspNetClasses.Where(x => x.Id == ClassId).Select(x => x.Class).FirstOrDefault();
            var studentname = db.AspNetUsers.Where(x => x.Id == StudentId).Select(x => x.UserName).FirstOrDefault();
            var studentdata = db.GetStudentBasicData(StudentId);
          
          
            var dinterm = db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermEndDate - db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermStartDate;

         
           var tn = User.Identity.GetUserId();
            var teachername = db.AspNetUsers.Where(x => x.Id == tn).Select(x => x.Name).FirstOrDefault();
            var result1 = new { ClassId= ClassId,classname= classname, teachername = teachername, studentname=studentname,parentComent =parentComent,teacherComent=teacherComent,prinpipalComent=prinpipalComent, status = "success", Value = subjects, TId = TId, role = type, studentdata= studentdata, dinterm = dinterm };
            return Json(result1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintPreviewData(string StudentId, string type, string TermId)
        {
            int TId = int.Parse(TermId);
            var subjects = db.GetStudentSubjects(StudentId).ToList();
           
            var comments = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == StudentId && x.TermID == TId).FirstOrDefault();
            string parentComent = "";
            string teacherComent = "";
            string prinpipalComent = "";
            if (comments != null)
            {
                parentComent = comments.ParentsComments;
                teacherComent = comments.TeacherComments;
                prinpipalComent = comments.PrincipalComments;
            }
            else
            {
                parentComent = "";
                teacherComent = "";
                prinpipalComent = "";
            }
            var tn = User.Identity.GetUserId();
            var teachername = db.AspNetUsers.Where(x => x.Id == tn).Select(x => x.Name).FirstOrDefault();
            var ClassId = db.AspNetStudents.Where(p => p.StudentID == StudentId).FirstOrDefault().ClassID;

            var subID = db.AspNetSubjects.Where(x => x.ClassID == ClassId).Select(x => x.Id).ToList();
            


            var secnumber = db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermNo;
            var category = (from sub in db.AspNetSubjects
                            join aq in db.AspNetAssessment_Question on sub.Id equals aq.SubjectID
                            join cat in db.AspNetAssessment_Questions_Category on aq.QuestionCategory equals cat.Id
                            where sub.SubjectName == "English Language Development" && sub.ClassID == ClassId
                            select new { cat.CategoryName }).Distinct().ToList();
            var classname = db.AspNetClasses.Where(x => x.Id == ClassId).Select(x => x.Class).FirstOrDefault();
            var studentname = db.AspNetUsers.Where(x => x.Id == StudentId).Select(x => x.UserName).FirstOrDefault();
            var dinterm = db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermEndDate - db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermStartDate;
           
                var result2 = new { status = "success",subID=subID, categoryname = category, TId = secnumber, ClassId = ClassId, classname = classname, teachername = teachername, studentname = studentname, parentComent = parentComent, teacherComent = teacherComent, prinpipalComent = prinpipalComent,  subValue = subjects, dinterm = dinterm };

                return Json(result2, JsonRequestBehavior.AllowGet);
          
                
           
        }
        public ActionResult Assessment_PrintPreview(string StudentId, int SID)
        {
            var sessionid = db.AspNetSessions.Where(p => p.Status == "Active").FirstOrDefault().Id;

            var sessionassesques = db.GetStudentSessionSubjectAssessment(StudentId, sessionid, SID).ToList();
            var cgjhk = sessionassesques.Count();
            if (cgjhk > 0)
            {
                var result2 = new { Value = sessionassesques};

                return Json(result2, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public void SaveComments(string tc,string stdid,string termid, string type)
        {
            int TID = int.Parse(termid);
            if(type=="Principle")
            {
                db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == stdid && x.TermID == TID).FirstOrDefault().PrincipalComments = tc;
            }
            else if(type=="Parent")
            {
                AspNetStudent_Term_Assessment at = new AspNetStudent_Term_Assessment();
                  at = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == stdid && x.TermID == TID).FirstOrDefault();
                  at.ParentsComments = tc;
                  db.AspNetStudent_Term_Assessment.Add(at);
            }
            else
            {
                AspNetStudent_Term_Assessment test = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == stdid && x.TermID == TID).FirstOrDefault();
                db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == stdid && x.TermID == TID).FirstOrDefault().TeacherComments = tc;
            }
            db.SaveChanges();
        }
        public void PrincipalSubmited(string studentid, string termid)
        {
            int TID = int.Parse(termid);
            db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == studentid && x.TermID == TID).FirstOrDefault().Status= "Principle_submit";
            db.SaveChanges();
        }
        public ActionResult subqueEvaluations(string StudentId, string SubId, string TermId)
        {
            int TId = int.Parse(TermId);
            int SID = int.Parse(SubId);
            var ClassId = db.AspNetStudents.Where(p => p.StudentID == StudentId).FirstOrDefault().ClassID;
            var secnumber = db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermNo;
            var category = (from sub in db.AspNetSubjects
                            join aq in db.AspNetAssessment_Question on sub.Id equals aq.SubjectID
                            join cat in db.AspNetAssessment_Questions_Category on aq.QuestionCategory equals cat.Id
                            where sub.SubjectName == "English Language Development" && sub.ClassID == ClassId
                            select new { cat.CategoryName }).Distinct().ToList();
           
            //var questions = db.GetStudentTermSubjectAssessment(StudentId, TId, SID).ToList();
            var sessionid = db.AspNetSessions.Where(p => p.Status == "Active").FirstOrDefault().Id;

            var sessionassesques = db.GetStudentSessionSubjectAssessment(StudentId, sessionid, SID).ToList();
            var cgjhk = sessionassesques.Count();

            if (cgjhk > 0)
            {
                var result1 = new { status = "success", Value = sessionassesques,categoryname=category, TId = secnumber, ClassId = ClassId };
                
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Aquestions = db.GetSubjctAssessmentQuestions(SID).ToList();

                foreach(var ques in Aquestions)
                {
                    AspNetStudent_Term_Assessment obj = new AspNetStudent_Term_Assessment();

                    obj.StudentID = StudentId;
                    obj.SubjectID = SID;
                    obj.SessionId = sessionid;
                    obj.TermID = TId;

                    db.AspNetStudent_Term_Assessment.Add(obj);
                    db.SaveChanges();

                    AspNetStudent_Term_Assessments_Answers obje = new AspNetStudent_Term_Assessments_Answers();

                    obje.STAID = obj.Id;
                    obje.Question = ques.Question;
                    obje.Catageory = ques.CategoryName;
                    obje.FirstTermGrade = "";
                    obje.SecondTermGrade = "";
                    obje.ThirdTermGrade = "";
                    //obje = ;
                    db.AspNetStudent_Term_Assessments_Answers.Add(obje);
                    db.SaveChanges();

                }

                var sessionassesrtques = db.GetStudentSessionSubjectAssessment(StudentId, sessionid, SID).ToList();
                var result1 = new { status = "success", Value = sessionassesrtques, categoryname = category, TId = secnumber, ClassId = ClassId };
                 
                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            
        }

        public ActionResult addgrade(string Ques, string Grade)
        {
            var fggf = Ques.Split('t');
            var ty = fggf[1];
            int QueserId = int.Parse(ty);
            var QuesId = QueserId;
            var jhj = db.AspNetStudent_Term_Assessments_Answers.Where(p => p.Id == QuesId).FirstOrDefault().Answer;
            if (fggf[0] == "1")
            {
                db.AspNetStudent_Term_Assessments_Answers.Where(p => p.Id == QuesId).FirstOrDefault().FirstTermGrade = Grade;
            }
            else if (fggf[0] == "2")
            {
                db.AspNetStudent_Term_Assessments_Answers.Where(p => p.Id == QuesId).FirstOrDefault().SecondTermGrade = Grade;
            }
            else
            {
                db.AspNetStudent_Term_Assessments_Answers.Where(p => p.Id == QuesId).FirstOrDefault().ThirdTermGrade = Grade;
            }

            db.SaveChanges();

            var result1 = new { status = "success", Value = Ques, TId = Grade };
            return Json(result1, JsonRequestBehavior.AllowGet);
        }

        public void save_comment(int Id, string comment)
        {
            db.AspNetStudent_Term_Assessment.FirstOrDefault(x => x.Id == Id).TeacherComments = comment;
            db.AspNetStudent_Term_Assessment.FirstOrDefault(x => x.Id == Id).NotifyTeacher = false;
            db.SaveChanges();
        }

        public void Notify(int Id)
        {
            db.AspNetStudent_Term_Assessment.FirstOrDefault(x => x.Id == Id).NotifyTeacher = true;

            var STAID = db.AspNetStudent_Term_Assessment.FirstOrDefault(x => x.Id == Id);

            AspNetNotification AspNetNotification = new AspNetNotification();
            AspNetNotification.Time = DateTime.Now;
            AspNetNotification.Subject = STAID.AspNetSubject.SubjectName;
            AspNetNotification.SenderID = User.Identity.GetUserId();
            AspNetNotification.Description = "Correction of term assessment of student " + STAID.AspNetUser.Name + " of class " + STAID.AspNetSubject.AspNetClass.ClassName;
            db.AspNetNotifications.Add(AspNetNotification);
            db.SaveChanges();

            AspNetNotification_User AspNetNotification_User = new AspNetNotification_User();
            AspNetNotification_User.Seen = false;
            AspNetNotification_User.NotificationID = AspNetNotification.Id;
            AspNetNotification_User.UserID = STAID.AspNetSubject.AspNetClass.TeacherID;
            db.AspNetNotification_User.Add(AspNetNotification_User);
            db.SaveChanges();
        }

        [HttpPost]
        public void EvaluationService(EvaluationDATA assessement_data)
        {
            int STAID = 0;

            foreach (var item in assessement_data.Evaluation_List)
            {
                var Answer = db.AspNetStudent_Term_Assessments_Answers.FirstOrDefault(x => x.Id == item.ID);
                Answer.Answer = item.answer;
                db.SaveChanges();
                STAID = (int)Answer.STAID;
            }

            var staid = db.AspNetStudent_Term_Assessment.FirstOrDefault(x => x.Id == STAID);

            staid.Status = assessement_data.status;
            staid.TeacherComments = assessement_data.comment;


            db.SaveChanges();
        }

        public void submit_Evaluation_Teacher(string StudentId, int ClassID, string Comment)
        {

            var SubjectID = db.AspNetSubjects.Where(x => x.ClassID == ClassID).Select(x => x.Id).ToList();

            int Current_term = (int)db.AspNetStudent_Term_Assessment.Max(x => x.TermID);

            foreach (var item in SubjectID)
            {
                int STAID = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == StudentId && x.SubjectID == item && x.TermID == Current_term).Select(x => x.Id).Single();

                var status = db.AspNetStudent_Term_Assessment.Where(x => x.Id == STAID).FirstOrDefault();
                status.Status = "Principle_submit";
                status.PrincipalComments = Comment;

            }


            db.SaveChanges();

        }

        public void submit_Evaluation(string StudentId, int ClassID)
        {
            var teacherId = User.Identity.GetUserId();

            int SubjectID = db.AspNetSubjects.Where(x => x.TeacherID == teacherId && x.ClassID == ClassID).Select(x => x.Id).FirstOrDefault();

            int Current_term = (int)db.AspNetStudent_Term_Assessment.Max(x => x.TermID);

            int STAID = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == StudentId && x.SubjectID == SubjectID && x.TermID == Current_term).Select(x => x.Id).Single();

            var status = db.AspNetStudent_Term_Assessment.Where(x => x.Id == STAID).FirstOrDefault();
            status.Status = "Teacher_submit";

            db.SaveChanges();

        }

        [HttpPost]
        public JsonResult EvaluationChecking_Class(int[] Id, int[] STA_ID)
        {
            Evaluationchecking Evaluation_checking = new Evaluationchecking();
            Evaluation_checking.answer = new List<Answers>();
            Evaluation_checking.Comment = new List<Comment>();
            int P_Comment = 0;

            if (Id != null)
                P_Comment = STA_ID[0];

            try
            {
                Evaluation_checking.PrincipleComment = db.AspNetStudent_Term_Assessment.Where(x => x.Id == P_Comment).Select(x => x.PrincipalComments).Single();
            }
            catch (Exception ex)
            {
                Evaluation_checking.PrincipleComment = "";
            }


            Evaluation_checking.savebtn = "false";

            if (Id != null)
            {

                int current_term = db.AspNetTerms.Select(x => x.Id).Max();

                int ID = STA_ID[0];
                var StudentId = db.AspNetStudent_Term_Assessment.Where(x => x.Id == ID).Select(x => x.StudentID).First();

                var STAID = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == StudentId && x.TermID == current_term).Select(x => x.Id).ToList();
                int count = 0;
                int submit = 0;
                int Principle = 0;


                foreach (var item in STAID)
                {
                    string status = db.AspNetStudent_Term_Assessment.Where(x => x.Id == item).Select(x => x.Status).Single();
                    if (status == "submit" || status == "Teacher_submit" || status == "Principle_submit")
                        count++;

                    if (status == "Teacher_submit")
                        submit++;

                    if (status == "Principle_submit")
                        Principle++;

                }

                if (count == STAID.Count)
                    Evaluation_checking.savebtn = "true";

                if (submit != 0)
                    Evaluation_checking.savebtn = "submit";

                if (Principle != 0)
                    Evaluation_checking.savebtn = "Teacher_submit";

                for (int i = 0; i < STA_ID.Length; i++)
                {
                    Comment Comment = new Comment();
                    Comment.id = STA_ID[i];
                    Comment.comment = db.AspNetStudent_Term_Assessment.Where(x => x.Id == Comment.id).Select(x => x.TeacherComments).Single();

                    Evaluation_checking.Comment.Add(Comment);
                }

                for (int i = 0; i < Id.Length; i++)
                {
                    Answers Answer = new Answers();
                    Answer.id = Id[i];
                    Answer.answer = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.Id == Answer.id).Select(x => x.Answer).Single();

                    Evaluation_checking.answer.Add(Answer);
                }
            }

            return Json(Evaluation_checking, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StudentsByClass_Teacher(int id)
        {
            string ClassHead = db.AspNetClasses.Where(x => x.Id == id).Select(x => x.TeacherID).First();
            string currentTeacher = User.Identity.GetUserId();

            var StudentList = new List<Student>();

            var students = (from student in db.AspNetUsers
                            join student_subject in db.AspNetStudent_Subject 
                            on student.Id equals student_subject.StudentID
                            join subject in db.AspNetSubjects 
                            on student_subject.SubjectID equals subject.Id
                            where subject.ClassID == id
                            select new { student.Id, student.UserName, student.Name }).Distinct().ToList();

            int CurrentTerm;

            try
            {
                CurrentTerm = db.AspNetTerms.Select(x => x.Id).Max();
            }
            catch (Exception ex)
            {
                return Json("Term of the latest session is not started yet", JsonRequestBehavior.AllowGet);
            }

            foreach (var item in students)
            {
                Student student = new Student();
                var status = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == CurrentTerm && item.Id == x.StudentID).Select(x => x.Status).ToList();
                int count = 0;
                foreach (var Status in status)
                {
                    if (Status == "submit")
                    {
                        count++;
                    }
                    else if (Status == "Teacher_submit" || Status == "Principle_submit")
                    {
                        count = 100;
                        break;
                    }
                }

                student.Id = item.Id;
                student.Name = item.Name;
                student.Username = item.UserName;

                if (db.AspNetSubjects.Where(x => x.ClassID == id).Count() == count)
                {
                    student.Status = "pending";
                }
                else if (count == 100)
                {
                    student.Status = "ok";
                }
                else if (count < db.AspNetSubjects.Where(x => x.ClassID == id).Count())
                {
                    student.Status = "not submitted";
                }

                StudentList.Add(student);
            }
            return Json(StudentList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult StudentsByClass_Principal(int id)
        {
            try
            {


                string ClassHead = db.AspNetClasses.Where(x => x.Id == id).Select(x => x.TeacherID).First();
                string currentTeacher = User.Identity.GetUserId();

                var StudentList = new List<Student>();

                var students = (from student in db.AspNetUsers
                                join student_subject in db.AspNetStudent_Subject on student.Id equals student_subject.StudentID
                                join subject in db.AspNetSubjects on student_subject.SubjectID equals subject.Id
                                where subject.ClassID == id
                                select new { student.Id, student.UserName, student.Name }).Distinct().ToList();

                var CurrentTerm = db.AspNetTerms.Select(x => x.Id).Max();

                foreach (var item in students)
                {
                    Student student = new Student();
                    var status = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == CurrentTerm && item.Id == x.StudentID).Select(x => x.Status).ToList();
                    int count = 0;
                    foreach (var Status in status)
                    {
                        if (Status == "submit" || Status == "Not Submitted" || Status == "save")
                        {
                            count++;
                        }
                        else if (Status == "Teacher_submit")
                        {
                            count = 100;
                            break;
                        }
                        else if (Status == "Principle_submit")
                        {
                            count = 200;
                            break;
                        }
                    }

                    student.Id = item.Id;
                    student.Name = item.Name;
                    student.Username = item.UserName;

                    if (count == db.AspNetSubjects.Where(x => x.ClassID == id).Count())
                    {
                        student.Status = "not submitted";
                    }
                    else if (count == 100)
                    {
                        student.Status = "pending";
                    }
                    else if (count == 200)
                    {
                        student.Status = "ok";
                    }

                    StudentList.Add(student);
                }
                return Json(StudentList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult AssessmentBySubject(int SubjectID)
        {
            List<Student> STudentAssesssment = new List<Student>();

            List<string> studentsID = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Student")).Select(s => s.Id).ToList();
            var students = (from t in db.AspNetUsers
                            join t1 in db.AspNetStudent_Subject on t.Id equals t1.StudentID
                            where studentsID.Contains(t.Id) && t1.SubjectID == SubjectID
                            select new { t.Id, t.Name, t.UserName }).ToList();

            int CurrentTerm;

            try
            {
                CurrentTerm = db.AspNetTerms.Select(x => x.Id).Max();
            }
            catch (Exception ex)
            {
                return Json("Your term of this session hasn't started yet", JsonRequestBehavior.AllowGet);
            }


            foreach (var item in students)
            {
                Student student = new Student();
                student.Id = item.Id;
                student.Name = item.Name;
                student.Username = item.UserName;

                var status = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == CurrentTerm && x.SubjectID == SubjectID && x.StudentID == item.Id).Select(x => x.Status).FirstOrDefault();
                student.Status = status;

                STudentAssesssment.Add(student);
            }

            return Json(STudentAssesssment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EvaluationChecking_Subject(int[] Id)
        {
            Evaluationchecking Evaluation_checking = new Evaluationchecking();
            Evaluation_checking.answer = new List<Answers>();

            if (Id != null)
            {
                int STAA = Id[0];

                int STAID = (int)db.AspNetStudent_Term_Assessments_Answers.Where(x => x.Id == STAA).Select(x => x.STAID).Single();

                for (int i = 0; i < Id.Length; i++)
                {
                    Answers Answer = new Answers();
                    Answer.id = Id[i];
                    Answer.answer = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.Id == Answer.id).Select(x => x.Answer).Single();
                    Evaluation_checking.answer.Add(Answer);

                }

                Evaluation_checking.status = db.AspNetStudent_Term_Assessment.Where(x => x.Id == STAID).Select(x => x.Status).Single();
                Evaluation_checking.comment = db.AspNetStudent_Term_Assessment.Where(x => x.Id == STAID).Select(x => x.TeacherComments).Single();
                Evaluation_checking.PrincipleComment = db.AspNetStudent_Term_Assessment.Where(x => x.Id == STAID).Select(x => x.PrincipalComments).Single();
            }

            return Json(Evaluation_checking, JsonRequestBehavior.AllowGet);
        }

        public class Student
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
        }

        public class Evaluationchecking
        {
            public string status { get; set; }
            public List<Answers> answer { get; set; }
            public List<Comment> Comment { get; set; }
            public string comment { set; get; }
            public string savebtn { get; set; }
            public string PrincipleComment { set; get; }

        }

        public class Answers
        {
            public int id { get; set; }
            public string answer { get; set; }
        }

        public class Comment
        {
            public int id { get; set; }
            public string comment { get; set; }
        }

        public class EvaluationDATA
        {
            public string status { set; get; }
            public string comment { get; set; }
            public List<evaluation> Evaluation_List { set; get; }
        }

        public class evaluation
        {
            public int ID { set; get; }
            public string answer { set; get; }
        }

        public class classEvaluationKey
        {
            public int id { set; get; }
            public string name { set; get; }
            public string isNotify { set; get; }
        }


    }//Class

} // NameSpace