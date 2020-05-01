using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SEA_Application.Controllers
{
    public class StudentAssignmentSubmittedController : Controller
    {

        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        public ActionResult Index()
        {

            return View();


        }

        [HttpGet]
        public ActionResult TeacherComments(int id)
        {
            ViewBag.Id = id;
          
            return View();

        }
        [HttpPost]
        public ActionResult TeacherCommentsMethod(int id, string TeacherComments)

        {
            AspnetStudentAssignmentSubmission assignmentSubmission = db.AspnetStudentAssignmentSubmissions.Where(x => x.Id == id).FirstOrDefault();
            assignmentSubmission.TeacherComments = TeacherComments;
            db.SaveChanges();


            int? LessonId = assignmentSubmission.LessonId;
            int? StudentId = assignmentSubmission.StudentId;

            var StudentIdInString =    db.AspNetStudents.Where(x => x.Id == StudentId).FirstOrDefault().StudentID;

            StudentLessonTracking LessonTracking =  db.StudentLessonTrackings.Where(x => x.LessonId == LessonId && x.StudentId == StudentIdInString).FirstOrDefault();

            if(LessonTracking !=null)
            {
                LessonTracking.Assignment_Status = "Approved";
                db.SaveChanges();

            }

            return Json("", JsonRequestBehavior.AllowGet);

        }
        public ActionResult StudentLessonTracking()
        {

            

            return View(db.StudentLessonTrackings.ToList());
        }


        public ActionResult StudentAssignments()
        {


            List<AspnetStudentAssignmentSubmission> list = db.AspnetStudentAssignmentSubmissions.ToList();

            List<AssignmentViewModel> listAssignmentViewModel = new List<AssignmentViewModel>();

            foreach (var submittedAssignment in list)
            {

                AssignmentViewModel assignmentViewModel = new AssignmentViewModel();

                int? ClassId = submittedAssignment.ClassId;
                string CourseType = submittedAssignment.CourseType;
                DateTime? DueDate = submittedAssignment.AssignmentDueDate;
                DateTime? SubmittedDate = submittedAssignment.AssignmentSubmittedDate;
                string FileName = submittedAssignment.AssignmentFileName;
                int? LessonId = submittedAssignment.LessonId;
                int? SubjectId = submittedAssignment.SubjectId;
                int? TopicId = submittedAssignment.TopicId;
                int? StudentId = submittedAssignment.StudentId;
                int AssignemntId = submittedAssignment.Id;

                var ClassName = db.AspNetClasses.Where(x => x.Id == ClassId).FirstOrDefault().ClassName;
                var SubjectName = db.GenericSubjects.Where(x => x.Id == SubjectId).FirstOrDefault().SubjectName;
                var TopicName = db.AspnetSubjectTopics.Where(x => x.Id == TopicId).FirstOrDefault().Name;
                var LessonName = db.AspnetLessons.Where(x => x.Id == LessonId).FirstOrDefault().Name;
                var StudentID = db.AspNetStudents.Where(x => x.Id == StudentId).FirstOrDefault().StudentID;
                var StudentName = db.AspNetUsers.Where(x => x.Id == StudentID).FirstOrDefault().Name;

            //   var AssignmentId =  db.AspnetStudentAssignmentSubmissions.Where(x => x.LessonId == LessonId).FirstOrDefault().Id;

                assignmentViewModel.AssignmentId = AssignemntId;
                assignmentViewModel.AssignmentName = FileName;
                assignmentViewModel.AssignmnetDueDate = DueDate;
                assignmentViewModel.AssignmentSubmittedDate = SubmittedDate;
                assignmentViewModel.Section = ClassName;
                assignmentViewModel.SubjectName = SubjectName;
                assignmentViewModel.Topic = TopicName;
                assignmentViewModel.Lesson = LessonName;
                assignmentViewModel.NameOfStudent = StudentName;
                assignmentViewModel.CourseType = CourseType;

                listAssignmentViewModel.Add(assignmentViewModel);


            }

            return View(listAssignmentViewModel);
        }

        public ActionResult DownloadFileOfAssignment(int id)
        {
            AspnetStudentAssignmentSubmission studentAssignment = db.AspnetStudentAssignmentSubmissions.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/Content/StudentAssignments/"), studentAssignment.AssignmentFileName);

            return File(filepath, MimeMapping.GetMimeMapping(filepath), studentAssignment.AssignmentFileName);

        }




    }

    



    //public class AssignmentViewModel
    //{

    //    [Display(Name = "Student Name")]
    //    public string NameOfStudent { get; set; }
    //    public string Section { get; set; }
    //    [Display(Name = "Course Type")]
    //    public string CourseType { get; set; }

    //    public string Topic { get; set; }
    //    public string Lesson { get; set; }
    //    [Display(Name = "Assignment")]

    //    public string AssignmentName { get; set; }

    //    [Display(Name = "Subject Name")]

    //    public string SubjectName { get; set; }

    //    [Display(Name = "Assignment Due Date")]
    //    public DateTime? AssignmnetDueDate { get; set; }

    //    [Display(Name = "Assignment Submitted Date")]

    //    public DateTime? AssignmentSubmittedDate { get; set; }

    //}


}