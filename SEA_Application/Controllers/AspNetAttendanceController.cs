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
    public class AspNetAttendanceController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        private string TeacherID;
        public AspNetAttendanceController()
        {

            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }
        // GET: AspNetAttendance
        /////////////////////////////////////////////Teacher Attendance/////////////////////////////////////
        public ActionResult Teacher_Attendance()
        {
            return View();
        }

        public ActionResult UserAttendence()
        {


            return View();
        }

        public ActionResult AttendenceReport( string AttendenceType)
        {

       
          //  if (AttendenceType == "Student")
            //{
                //  List<AspnetUser_Attendence> ListStudent = db.AspnetUser_Attendence.Where(x => x.AspNetUser.AspNetRoles.Select(y => y.Name).Contains("Student")).ToList();

                //   var TodayStudentList = ListStudent.Where(x => x.Date == dt).Select(x => x.UserId).ToList();

                //   var ListStudent = db.AspnetUser_Attendence.Where(x => x.AspNetUser.AspNetRoles.Select(y => y.Name).Contains("Student")).Select(x=>x.UserId).ToList();

                //shahzad
            //    var AllUsers = from user in db.AspNetUsers
            //                   join std in db.AspNetStudents on user.Id equals std.StudentID
            //                   join userAttendence in db.AspnetUser_Attendence on user.Id equals userAttendence.UserId
            //                   select new
            //                   {

            //                       FeeStatus = std.StudentFeeMonths.Where(x => x.Status == "Paid").FirstOrDefault().Status,
            //                       Name = user.Name,
            //                       Section = std.AspNetClass.ClassName,
            //                       RollNo = user.UserName,
            //                       CourseType = std.CourseType,
            //                       Date = userAttendence.Date,
            //                       //FeeStatus = std.StudentFeeMonths.Where(x=>x.Status =="Paid")

            //                   };


            //    return Json(AllUsers, JsonRequestBehavior.AllowGet);

            //}
            //else
            //{

            //    //List<AspnetUser_Attendence> ListStudent = db.AspnetUser_Attendence.Where(x => x.AspNetUser.AspNetRoles.Select(y => y.Name).Contains("Teacher")).ToList();

            //    // var TodayStudentList = ListStudent.Where(x => x.Date == dt).Select(x => x.UserId).ToList();


            //    //shahzad

            //    var ListTeacher = db.AspnetUser_Attendence.Where(x => !(x.AspNetUser.AspNetRoles.Select(y => y.Name).Contains("Student"))).Select(x => x.UserId).ToList();

            //    var AllUsers = from user in db.AspNetUsers
            //                   join userAttendence in db.AspnetUser_Attendence on user.Id equals userAttendence.UserId
            //                   where ListTeacher.Contains(user.Id)
            //                   select new
            //                   {

            //                       Name = user.Name,
            //                       RollNo = user.UserName,
            //                       Date = userAttendence.Date,

            //                       //FeeStatus = std.StudentFeeMonths.Where(x=>x.Status =="Paid")

            //                   };

            //    return Json(AllUsers, JsonRequestBehavior.AllowGet);

            //}

          return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAttendance()
        {
            var currentdate = DateTime.Now.Date;
            var present = db.AspNetEmp_Auto_Attendance.Where(x => x.Date == currentdate).ToList();
            return Json(present, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Auto_Attendance(string value)
        {
            var currentdate = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;
            var name = db.AspNetEmployees.Where(x => x.CellNo == value).Select(x => x.Name).FirstOrDefault();
            var result = db.AspNetEmp_Auto_Attendance.Where(x => x.PhoneNumber == value && x.Date == currentdate).FirstOrDefault();
            if (result == null)
            {
                AspNetEmp_Auto_Attendance eaa = new AspNetEmp_Auto_Attendance();
                eaa.Name = name;
                eaa.Date = currentdate;
                eaa.PhoneNumber = value;
                eaa.TimeIn = time;
                eaa.TimeOut = null;
                db.AspNetEmp_Auto_Attendance.Add(eaa);
                db.SaveChanges();
            }
            else
            {
                AspNetEmp_Auto_Attendance aee = db.AspNetEmp_Auto_Attendance.Where(x => x.Date == currentdate && x.PhoneNumber == value).FirstOrDefault();
                aee.TimeOut = time;
                db.SaveChanges();
            }
            return View();
        }
        ////////////////////////////////////////////////////////End////////////////////////////////////////////
        [HttpPost]
        public JsonResult Attendance(attendance attendances)
        {
            var dbTransection = db.Database.BeginTransaction();
            String ErrorID = null;

            try
            {
                char check = attendances.SubjectID[0];
                if (check != '0')
                {
                    var date = DateTime.Now.Date;
                    int subjectID = Convert.ToInt32(attendances.SubjectID);
                    AspNetAttendance TodayAttendance = db.AspNetAttendances.Where(x => x.SubjectID == subjectID && x.Date == date).FirstOrDefault();
                    if (TodayAttendance == null)
                    {
                        AspNetAttendance Attendance = new AspNetAttendance();
                        Attendance.Date = DateTime.Now.Date;
                        Attendance.SubjectID = subjectID;
                        db.AspNetAttendances.Add(Attendance);
                        db.SaveChanges();
                        int attendanceID = db.AspNetAttendances.Max(x => x.Id);
                        foreach (var student_attendance in attendances.studentAttendance)
                        {
                            AspNetStudent_Attendance stu_attend = new AspNetStudent_Attendance();
                            stu_attend.StudentID = student_attendance.Id;
                            stu_attend.Status = student_attendance.Status;
                            stu_attend.Reason = student_attendance.Reason;
                            stu_attend.AttendanceID = attendanceID;
                            db.AspNetStudent_Attendance.Add(stu_attend);


                        }
                    }
                    else
                    {
                        foreach (var student_attendance in attendances.studentAttendance)
                        {

                            AspNetStudent_Attendance stu_attend = db.AspNetStudent_Attendance.Where(x => x.AttendanceID == TodayAttendance.Id && x.StudentID == student_attendance.Id).FirstOrDefault();
                            if (stu_attend != null)
                            {
                                stu_attend.Status = student_attendance.Status;
                                stu_attend.Reason = student_attendance.Reason;

                            }
                        }
                    }
                }
                else // Second Phase
                {
                    var date = DateTime.Now.Date;
                    int ClassID = Convert.ToInt32(attendances.SubjectID);
                    var sub = db.AspNetSubjects.Where(x => x.ClassID == ClassID).Select(x => x.Id).ToList();
                    AspNetAttendance TodayAttendance = db.AspNetAttendances.Where(x => x.Date == date && sub.Contains(x.SubjectID)).FirstOrDefault();
                    if (TodayAttendance == null)
                    {

                        string Class = attendances.SubjectID;

                        Class = Class.Remove(0, 1);

                        int ClassId = Convert.ToInt32(Class);

                        var subject = (from subjects in db.AspNetSubjects
                                       orderby subjects.Id descending
                                       where subjects.ClassID == ClassId
                                       select new { subjects.Id, subjects.SubjectName }).ToList();


                        foreach (var item in subject)
                        {
                            AspNetAttendance Attendance = new AspNetAttendance();
                            Attendance.Date = DateTime.Now.Date;
                            Attendance.SubjectID = item.Id;
                            db.AspNetAttendances.Add(Attendance);
                            db.SaveChanges();
                            int attendanceID = db.AspNetAttendances.Max(x => x.Id);
                            foreach (var student_attendance in attendances.studentAttendance)
                            {
                                AspNetStudent_Attendance stu_attend = new AspNetStudent_Attendance();
                                stu_attend.StudentID = student_attendance.Id;
                                stu_attend.Status = student_attendance.Status;
                                stu_attend.Reason = student_attendance.Reason;
                                stu_attend.AttendanceID = attendanceID;
                                db.AspNetStudent_Attendance.Add(stu_attend);
                            }
                        }
                    }
                    else
                    {
                        foreach (var student_attendance in attendances.studentAttendance)
                        {

                            AspNetStudent_Attendance stu_attend = db.AspNetStudent_Attendance.Where(x => x.AttendanceID == TodayAttendance.Id && x.StudentID == student_attendance.Id).FirstOrDefault();
                            if (stu_attend != null)
                            {
                                stu_attend.Status = student_attendance.Status;
                                stu_attend.Reason = student_attendance.Reason;

                            }
                        }
                    }
                }
                db.SaveChanges();
                dbTransection.Commit();
            }
            catch (Exception)
            {
                dbTransection.Dispose();
                ViewBag.Error = "Error Adding ID: " + ErrorID;
            }

            return Json("Saved", JsonRequestBehavior.AllowGet);
        }

        public class attendance
        {
            public int Id { get; set; }
            public List<student_attendance> studentAttendance { get; set; }
            public string SubjectID { get; set; }
            public bool Status { get; set; }
        }

        public class student_attendance
        {

            public string Id { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Reason { get; set; }

        }

        public JsonResult AttendanceBySubject(string subjectID)
        {
            if (subjectID == "")
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            char AllCheck = subjectID[0];

            if (AllCheck != '0')
            {
                int SubjectID = Convert.ToInt32(subjectID);
                var date = DateTime.Now.Date;
                var Attendance = db.AspNetAttendances.Where(x => x.SubjectID == SubjectID && x.Date == date).FirstOrDefault();
                attendance attendance = new attendance();
                attendance.studentAttendance = new List<student_attendance>();
                if (Attendance == null)
                {
                    List<string> studentsID = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Student")).Select(s => s.Id).ToList();
                    var students = (from t in db.AspNetUsers.Where(x => x.Status != "False")
                                    join t1 in db.AspNetStudent_Subject on t.Id equals t1.StudentID
                                    where studentsID.Contains(t.Id) && t1.SubjectID == SubjectID
                                    select new { t.Id, t.Name, t.UserName }).ToList();
                    attendance.Status = false;
                    foreach (var student in students)
                    {
                        student_attendance student_attendance = new student_attendance();
                        student_attendance.Id = student.Id;
                        student_attendance.Username = student.UserName;
                        student_attendance.Name = student.Name;
                        student_attendance.Status = "";
                        student_attendance.Reason = "";
                        attendance.studentAttendance.Add(student_attendance);
                    }

                }
                else
                {
                    attendance.Status = true;
                    List<AspNetStudent_Attendance> student_attendances = db.AspNetStudent_Attendance.Where(x => x.AttendanceID == Attendance.Id).ToList();
                    foreach (var stu_atten in student_attendances)
                    {
                        student_attendance student_attendance = new student_attendance();
                        student_attendance.Id = stu_atten.AspNetUser.Id;
                        student_attendance.Username = stu_atten.AspNetUser.UserName;
                        student_attendance.Name = stu_atten.AspNetUser.Name;
                        student_attendance.Status = stu_atten.Status;
                        student_attendance.Reason = stu_atten.Reason;
                        attendance.studentAttendance.Add(student_attendance);
                    }

                }

                return Json(attendance, JsonRequestBehavior.AllowGet);
            }//// Second Phase
            else
            {
                var Class = Int32.Parse(subjectID.Substring(1));
                var ClassID = db.AspNetSubjects.Where(x => x.ClassID == Class).FirstOrDefault().ClassID;
                var date = DateTime.Now.Date;
                var sub = db.AspNetSubjects.Where(x => x.ClassID == ClassID).Select(x => x.Id).ToList();
                var Attendance = db.AspNetAttendances.Where(x => x.Date == date && sub.Contains(x.SubjectID)).FirstOrDefault();
                attendance attendance = new attendance();
                attendance.studentAttendance = new List<student_attendance>();
                if (Attendance == null)
                {
                    List<string> studentsID = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Student")).Select(s => s.Id).ToList();
                    var classId = subjectID.Remove(0, 1);

                    var ClassId = Convert.ToInt32(classId);

                    var subject = (from subjects in db.AspNetSubjects
                                   orderby subjects.Id descending
                                   where subjects.ClassID == ClassId
                                   select new { subjects.Id, subjects.SubjectName }).ToList();

                    foreach (var item in subject)
                    {
                        var students = (from t in db.AspNetUsers.Where(x => x.Status != "False")
                                        join t1 in db.AspNetStudent_Subject on t.Id equals t1.StudentID
                                        where studentsID.Contains(t.Id) && t1.SubjectID == item.Id
                                        select new { t.Id, t.Name, t.UserName }).ToList();

                        attendance.Status = false;
                        foreach (var student in students)
                        {
                            student_attendance student_attendance = new student_attendance();
                            student_attendance.Id = student.Id;
                            student_attendance.Username = student.UserName;
                            student_attendance.Name = student.Name;
                            student_attendance.Status = "";
                            student_attendance.Reason = "";
                            attendance.studentAttendance.Add(student_attendance);
                        }
                        break;
                    }
                }
                else
                {
                    attendance.Status = true;
                    List<AspNetStudent_Attendance> student_attendances = db.AspNetStudent_Attendance.Where(x => x.AttendanceID == Attendance.Id).ToList();
                    foreach (var stu_atten in student_attendances)
                    {
                        student_attendance student_attendance = new student_attendance();
                        student_attendance.Id = stu_atten.AspNetUser.Id;
                        student_attendance.Username = stu_atten.AspNetUser.UserName;
                        student_attendance.Name = stu_atten.AspNetUser.Name;
                        student_attendance.Status = stu_atten.Status;
                        student_attendance.Reason = stu_atten.Reason;
                        attendance.studentAttendance.Add(student_attendance);
                    }

                }

                return Json(attendance, JsonRequestBehavior.AllowGet);
            }

        }

        public void AttendancefromFile(AspNetAttendance aspNetAttendance)
        {
            var dbTransection = db.Database.BeginTransaction();
            String ErrorID = null;
            try
            {
                HttpPostedFileBase file = Request.Files["Attendance"];
                AspNetAttendance TodayAttendance = db.AspNetAttendances.Where(x => x.SubjectID == aspNetAttendance.SubjectID && x.Date == aspNetAttendance.Date).FirstOrDefault();
                List<student_attendance> Student_Attendance = new List<student_attendance>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    for (int rowIterator = 3; rowIterator <= noOfRow; rowIterator++)
                    {

                        student_attendance stu_atten = new student_attendance();
                        string username;
                        try
                        {
                            username = workSheet.Cells[rowIterator, 1].Value.ToString();
                        }
                        catch (Exception)
                        {
                            username = "";
                        }
                        try
                        {
                            stu_atten.Status = workSheet.Cells[rowIterator, 3].Value.ToString();
                        }
                        catch (Exception)
                        {
                            stu_atten.Status = "Present";
                        }
                        try
                        {
                            stu_atten.Reason = workSheet.Cells[rowIterator, 4].Value.ToString();
                        }
                        catch (Exception)
                        {
                            stu_atten.Reason = "";
                        }

                        stu_atten.Id = db.AspNetUsers.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();

                        Student_Attendance.Add(stu_atten);


                    }
                }

                if (TodayAttendance == null)
                {
                    AspNetAttendance Attendance = new AspNetAttendance();
                    Attendance.Date = aspNetAttendance.Date;
                    Attendance.SubjectID = aspNetAttendance.SubjectID;
                    db.AspNetAttendances.Add(Attendance);
                    db.SaveChanges();
                    int attendanceID = db.AspNetAttendances.Max(x => x.Id);

                    foreach (var studentattendance in Student_Attendance)
                    {
                        AspNetStudent_Attendance stu_attend = new AspNetStudent_Attendance();
                        stu_attend.StudentID = studentattendance.Id;
                        stu_attend.Status = studentattendance.Status;
                        stu_attend.Reason = studentattendance.Reason;
                        stu_attend.AttendanceID = attendanceID;
                        db.AspNetStudent_Attendance.Add(stu_attend);
                    }


                }
                else
                {
                    foreach (var student_attendance in Student_Attendance)
                    {

                        AspNetStudent_Attendance stu_attend = db.AspNetStudent_Attendance.Where(x => x.AttendanceID == TodayAttendance.Id && x.StudentID == student_attendance.Id).FirstOrDefault();
                        if (stu_attend != null)
                        {
                            stu_attend.Status = student_attendance.Status;
                            stu_attend.Reason = student_attendance.Reason;

                        }
                    }
                }


                db.SaveChanges();
                dbTransection.Commit();
            }
            catch (Exception)
            {
                dbTransection.Dispose();
                ViewBag.Error = "Error Adding ID: " + ErrorID;
            }
        }
    }
}
