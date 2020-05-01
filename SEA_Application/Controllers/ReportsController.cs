using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class ReportsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Assessment_Report()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id }, "Id", "CatalogName");
            return View();
        }

        public ViewResult  Multiple_Assessment()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.Subject_CatalogID = new SelectList(from catalog in db.AspNetCatalogs
                                                       join subject_catalog in db.AspNetSubject_Catalog on catalog.Id equals subject_catalog.CatalogID
                                                       select new { catalog.CatalogName, subject_catalog.Id }, "Id", "CatalogName");
            return View();
        }

        public ViewResult Student_Report()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View("_Student_Report");
        }

        public class pieChartReport
        {
            public string result { get; set; }
            public int number { get; set; }
            public string color { get; set; }
        }
        public class bands
        {
            public string color { get; set; }
            public int endValue { get; set; }
            public int startValue { get; set; }
        }
        public class average
        {
            public bands[] band { get; set; }
            public int average_marks { get; set; }
        }
        public class TableResultReport
        {
            public string UserName { get; set; }
            public string Name { get; set; }
            public double? MarksGot { get; set; }
            public double? Percentage { get; set; }
            public string Status { get; set; }
        }
        public class barChartReport
        {
            public string Assessment { get; set; }
            public string Exam { get; set; }
            public string Test { get; set; }
            public int Pass { get; set; }
            public int Fail { get; set; }
        }


        [HttpGet]
        public JsonResult ReportByAssessment(int assessment, int percentage)
        {
            List<int?> stu_assess = (from stu_assessment in db.AspNetStudent_Assessment
                                        where stu_assessment.AssessmentID == assessment
                                        select stu_assessment.MarksGot).ToList();
            double? totalMarks = db.AspNetAssessments.Where(x => x.Id == assessment).Select(x => x.TotalMarks).FirstOrDefault();
            int passcount = 0;
            int failcount = 0;
            foreach (var item in stu_assess)
            {
                double? perc = (item / totalMarks * 100);
                if (perc < percentage)
                {
                    failcount++;
                }
                else
                {
                    passcount++;
                }
            }
            List<pieChartReport> reportAssessment = new List<pieChartReport>();
            pieChartReport passreport = new pieChartReport();
            passreport.result = "Pass";
            passreport.number = passcount;
            passreport.color = "#4F52BA";
            pieChartReport failreport = new pieChartReport();
            failreport.result = "Fail";
            failreport.number = failcount;
            failreport.color = "#ef553a";
            reportAssessment.Add(passreport);
            reportAssessment.Add(failreport);
            return Json(reportAssessment, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult ReportByMultipleAssessment(int subject,int assessmentType, int percentage)
        {
            List<barChartReport> reportMultipleAssessment = new List<barChartReport>();
            List<AspNetAssessment> assessments = db.AspNetAssessments.Where(x => x.AspNetSubject_Catalog.SubjectID == subject && x.AspNetSubject_Catalog.CatalogID==assessmentType).ToList();
            foreach (var assessment in assessments)
            {
                int passcount = 0;
                int failcount = 0;
                List<int?> stu_assess = db.AspNetStudent_Assessment.Where(x => x.AssessmentID == assessment.Id).Select(x => x.MarksGot).ToList();
                foreach (var student_assessment in stu_assess)
                {
                    double? perc = (student_assessment  * 100) / assessment.TotalMarks;
                    if (perc < percentage)
                    {
                        failcount++;
                    }
                    else
                    {
                        passcount++;
                    }
                }
                barChartReport multipleAssessment = new barChartReport();
                multipleAssessment.Assessment = assessment.Title;
                multipleAssessment.Pass = passcount;
                multipleAssessment.Fail = failcount;
                reportMultipleAssessment.Add(multipleAssessment);
            }

            return Json(reportMultipleAssessment, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AverageByAssessment(int assessment, int percentage)
        {
            bands start = new bands();
            double? totalMarks = db.AspNetAssessments.Where(x => x.Id == assessment).Select(x => x.TotalMarks).FirstOrDefault();
            int passignmarks = Convert.ToInt32(totalMarks * percentage / 100);
            start.startValue = 0;
            start.endValue = passignmarks;
            start.color = "#cc4748";
            bands middle = new bands();
            middle.startValue = start.endValue;
            middle.endValue = Convert.ToInt32(totalMarks * 80 / 100);
            middle.color = "#fdd400";
            bands end = new bands();
            end.startValue = middle.endValue;
            end.endValue = Convert.ToInt32(totalMarks);
            end.color = "#84b761";

            average averageAssessment = new average();
            averageAssessment.band = new bands[3];
            averageAssessment.band[0] = start;
            averageAssessment.band[1] = middle;
            averageAssessment.band[2] = end;

            List<int?> stu_assessment = (from student_assessment in db.AspNetStudent_Assessment
                                            where student_assessment.AssessmentID == assessment
                                            select student_assessment.MarksGot).ToList();
            double? count = 0;
            int n = 0;
            foreach (var item in stu_assessment)
            {
                count = count + item;
                n++;
            }
            averageAssessment.average_marks = Convert.ToInt32(count / n);
            return Json(averageAssessment, JsonRequestBehavior.AllowGet);
        }



        public JsonResult AssessmentResult_Report(int assessment, int percentage)
        {
            List<AspNetStudent_Assessment> stu_assessment = db.AspNetStudent_Assessment.Where(x => x.AssessmentID == assessment).ToList();
            List<TableResultReport> assessment_report = new List<TableResultReport>();
            foreach (var item in stu_assessment)
            {
                TableResultReport assessmentreport = new TableResultReport();
                assessmentreport.MarksGot = item.MarksGot;
                assessmentreport.Name = item.AspNetUser.Name;
                assessmentreport.UserName = item.AspNetUser.UserName;
                assessmentreport.Percentage = (item.MarksGot * 100) / item.AspNetAssessment.TotalMarks;
                if (assessmentreport.Percentage < percentage)
                {
                    assessmentreport.Status = "Fail";
                }
                else
                {
                    assessmentreport.Status = "Pass";
                }
                assessment_report.Add(assessmentreport);

            }
            return Json(assessment_report, JsonRequestBehavior.AllowGet);
        }


        //public class stockChartReport
        //{
        //    public String date { get; set; }
        //    public double? value { get; set; }
        //    public string ballontext { get; set; }
        //}

        //public JsonResult StudentAssignment_Report(string studentID, int subjectID)
        //{
        //    Session["ChildID"] = studentID;
        //    Session["subjectID"] = subjectID;
        //    var student_assignments = (from student_assignment in db.Student_Assignment
        //                               join assignment in db.AspNetAssignments on student_assignment.AssignmentID equals assignment.Id
        //                               where assignment.SubjectID == subjectID && student_assignment.StudentID == studentID
        //                               orderby assignment.DueDate ascending
        //                               select student_assignment).ToList();
        //    List<stockChartReport> student_assignment_chart = new List<stockChartReport>();
        //    foreach (var item in student_assignments)
        //    {
        //        stockChartReport student_assignment = new stockChartReport();
        //        student_assignment.date = item.AspNetAssignment.DueDate.ToString();
        //        if (item.MarksGot == null)
        //        {
        //            item.MarksGot = 0;
        //        }
        //        student_assignment.value = item.MarksGot / item.AspNetAssignment.TotalMarks * 100;
        //        student_assignment.ballontext = item.AspNetAssignment.Title;
        //        student_assignment_chart.Add(student_assignment);

        //    }
        //    return Json(student_assignment_chart, JsonRequestBehavior.AllowGet);
        //}


        //public JsonResult StudentTest_Report(string studentID, int subjectID)
        //{
        //    Session["ChildID"] = studentID;
        //    Session["subjectID"] = subjectID;
        //    var student_tests = (from student_test in db.AspNetStudent_Test
        //                         join test in db.AspNetTests on student_test.TestID equals test.Id
        //                         where test.SubjectID == subjectID && student_test.StudentID == studentID
        //                         orderby test.Date ascending
        //                         select student_test).ToList();
        //    List<stockChartReport> student_test_chart = new List<stockChartReport>();
        //    foreach (var item in student_tests)
        //    {
        //        stockChartReport student_test = new stockChartReport();
        //        student_test.date = item.AspNetTest.Date.ToString();
        //        if (item.MarksGot == null)
        //        {
        //            item.MarksGot = 0;
        //        }
        //        student_test.value = item.MarksGot / item.AspNetTest.TotalMarks * 100;
        //        student_test.ballontext = item.AspNetTest.Title;
        //        student_test_chart.Add(student_test);

        //    }
        //    return Json(student_test_chart, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult StudentExam_Report(string studentID, int subjectID)
        //{


        //    Session["ChildID"] = studentID;
        //    Session["subjectID"] = subjectID;
        //    var student_exams = (from student_exam in db.AspNetStudent_Exam
        //                         join exam in db.AspNetExams on student_exam.ExamID equals exam.Id
        //                         where exam.SubjectID == subjectID && student_exam.StudentID == studentID
        //                         orderby exam.Date ascending
        //                         select student_exam).ToList();
        //    List<stockChartReport> student_exam_chart = new List<stockChartReport>();
        //    foreach (var item in student_exams)
        //    {
        //        stockChartReport student_exam = new stockChartReport();
        //        student_exam.date = item.AspNetExam.Date.ToString();
        //        if (item.MarksGot == null)
        //        {
        //            item.MarksGot = 0;
        //        }
        //        student_exam.value = item.MarksGot / item.AspNetExam.TotalMarks * 100;
        //        student_exam.ballontext = item.AspNetExam.Title;
        //        student_exam_chart.Add(student_exam);

        //    }
        //    return Json(student_exam_chart, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult StudentAll_Report(string studentID, int subjectID)
        //{
        //    Session["ChildID"] = studentID;
        //    Session["subjectID"] = subjectID;
        //    var student_exams = (from student_exam in db.AspNetStudent_Exam
        //                         join exam in db.AspNetExams on student_exam.ExamID equals exam.Id
        //                         where exam.SubjectID == subjectID && student_exam.StudentID == studentID
        //                         orderby exam.Date ascending
        //                         select student_exam).ToList();

        //    var student_tests = (from student_test in db.AspNetStudent_Test
        //                         join test in db.AspNetTests on student_test.TestID equals test.Id
        //                         where test.SubjectID == subjectID && student_test.StudentID == studentID
        //                         orderby test.Date ascending
        //                         select student_test).ToList();

        //    var student_assignments = (from student_assignment in db.Student_Assignment
        //                               join assignment in db.AspNetAssignments on student_assignment.AssignmentID equals assignment.Id
        //                               where assignment.SubjectID == subjectID && student_assignment.StudentID == studentID
        //                               orderby assignment.DueDate ascending
        //                               select student_assignment).ToList();

        //    List<stockChartReport> student_all_chart = new List<stockChartReport>();

        //    foreach (var item in student_exams)
        //    {
        //        stockChartReport student_exam = new stockChartReport();
        //        student_exam.date = item.AspNetExam.Date.ToString();
        //        if (item.MarksGot == null)
        //        {
        //            item.MarksGot = 0;
        //        }
        //        student_exam.value = item.MarksGot / item.AspNetExam.TotalMarks * 100;
        //        student_exam.ballontext = item.AspNetExam.Title;
        //        student_all_chart.Add(student_exam);

        //    }

        //    foreach (var item in student_tests)
        //    {
        //        stockChartReport student_test = new stockChartReport();
        //        student_test.date = item.AspNetTest.Date.ToString();
        //        if (item.MarksGot == null)
        //        {
        //            item.MarksGot = 0;
        //        }
        //        student_test.value = item.MarksGot / item.AspNetTest.TotalMarks * 100;
        //        student_test.ballontext = item.AspNetTest.Title;
        //        student_all_chart.Add(student_test);

        //    }

        //    foreach (var item in student_assignments)
        //    {
        //        stockChartReport student_assignment = new stockChartReport();
        //        student_assignment.date = item.AspNetAssignment.DueDate.ToString();
        //        if (item.MarksGot == null)
        //        {
        //            item.MarksGot = 0;
        //        }
        //        student_assignment.value = item.MarksGot / item.AspNetAssignment.TotalMarks * 100;
        //        student_assignment.ballontext = item.AspNetAssignment.Title;
        //        student_all_chart.Add(student_assignment);

        //    }
        //    return Json(student_all_chart, JsonRequestBehavior.AllowGet);
        //}
    }
}