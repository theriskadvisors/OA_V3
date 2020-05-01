//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SEA_Application.Models;
//using OfficeOpenXml;
//using System.IO;
//using System.Web.UI;
//using System.Text;
//using NReco.PdfGenerator;

//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
//using iTextSharp.tool.xml.html;
//using iTextSharp.tool.xml.pipeline.html;
//using iTextSharp.tool.xml.pipeline.end;
//using iTextSharp.tool.xml;
//using iTextSharp.tool.xml.pipeline.css;
//using iTextSharp.tool.xml.parser;

//namespace SEA_Application.Controllers.FeeControllers
//{
//    public class FeeManagementController : Controller
//    {
//        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
//        // GET: FeeManagement
//        public ActionResult Dashboard()
//        {
//            return View("BlankPage");
//        }

//        public ViewResult Fee_Type()
//        {
//            var feetypes = db.AspNetFeeTypes.ToList();
//            return View("_Fee_Type", feetypes);
//        }

//        public ViewResult Discount_Type()
//        {
//            var feetypes = db.AspNetDiscountTypes.ToList();
//            return View("_Discount_Type", feetypes);
//        }

//        public ViewResult Duration_Type()
//        {
//            var feetypes = db.AspNetDurationTypes.ToList();
//            return View("_Fee_Duration", feetypes);
//        }

//        public ViewResult Class_Fee()
//        {
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            return View("_Class_Fee");
//        }

//        public ViewResult Student_Discount()
//        {
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            return View("_Student_Discount");
//        }

//        public ViewResult Challan_Form()
//        {
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            return View("_Challan_Form");
//        }

//        public ViewResult Defaulters()
//        {
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            return View("_Defaulters");
//        }

//        public ViewResult Student_History()
//        {
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            return View("_Student_History");
//        }

//        public ViewResult Student_Challan_Form()
//        {
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            return View("_Student_Challan_Form");
//        }



//        public ViewResult Student_Status_Clear()
//        {
//            return View("_Status_Clear");
//        }

//        public JsonResult AllChalan()
//        {
//            var aspnetChallan = db.AspNetFeeChallans.Select(challans => new { challans.Id, challans.TotalAmount, challans.DueDate, challans.EndDate, challans.AspNetDurationType.TypeName, challans.StartDate, challans.AspNetClass.ClassName,Receiveable=db.AspNetStudent_Payment.Where(x=>x.FeeChallanID==challans.Id).Sum(x=>x.TotalAmount) }).ToList();
//            return Json(aspnetChallan, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult FeeChallanByClass(int ClassID)
//        {
//            var aspnetChallan = (from challan in db.AspNetFeeChallans
//                                 where challan.ClassID == ClassID
//                                 select new { challan.Id, challan.Title }).ToList();
//            return Json(aspnetChallan, JsonRequestBehavior.AllowGet);
//        }
//        [HttpGet]
//        public JsonResult FeeTypeByClass(int classID)
//        {

//            var feeTypes = (from class_fee in db.AspNetClass_FeeType
//                            where class_fee.ClassID == classID
//                            select new { class_fee.AspNetClass.ClassName, class_fee.AspNetFinanceLedger.Name, class_fee.Amount, class_fee.Id }).ToList();
//            return Json(feeTypes, JsonRequestBehavior.AllowGet);

//        }

//        [HttpGet]
//        public JsonResult DiscountByStudent(string studentID)
//        {

//            var studentdiscount = (from student_discount in db.AspNetStudent_Discount
//                                   where student_discount.StudentID == studentID
//                                   select new { student_discount.AspNetDiscountType.TypeName, student_discount.Percentage, student_discount.Id }).ToList();
//            return Json(studentdiscount, JsonRequestBehavior.AllowGet);

//        }

//        [HttpGet]
//        public JsonResult SubjectsByClass(int id)
//        {
//            db.Configuration.ProxyCreationEnabled = false;
//            List<AspNetSubject> sub = db.AspNetSubjects.Where(r => r.ClassID == id).OrderByDescending(r => r.Id).ToList();
//            ViewBag.Subjects = sub;
//            return Json(sub, JsonRequestBehavior.AllowGet);

//        }

//        [HttpGet]
//        public JsonResult StudentsBySubject(int id)
//        {
//            db.Configuration.ProxyCreationEnabled = false;
//            var students = (from student_subject in db.AspNetStudent_Subject
//                            join student in db.AspNetUsers on student_subject.StudentID equals student.Id
//                            where student_subject.SubjectID == id
//                            select new { student.UserName, student.Name, student.Id }).ToList();

//            return Json(students, JsonRequestBehavior.AllowGet);
//        }


//        [HttpGet]
//        public JsonResult ChallanByClass(int id)
//        {
//            var challan = (from challans in db.AspNetFeeChallans
//                           where challans.ClassID == id
//                           select new { challans.Id, challans.TotalAmount, challans.DueDate, challans.EndDate, challans.AspNetDurationType.TypeName, challans.StartDate, challans.AspNetClass.ClassName, Receiveable = db.AspNetStudent_Payment.Where(x => x.FeeChallanID == challans.Id).Sum(x => x.TotalAmount)}).ToList();

//            return Json(challan, JsonRequestBehavior.AllowGet);
//        }

       
//        [HttpGet]
//        public JsonResult StudentsByClass(int id)
//        {
//            var students = (from student in db.AspNetStudents
//                           where student.ClassID == id
//                            select new { student.AspNetUser.Id, student.AspNetUser.UserName, student.AspNetUser.Name, student.AspNetClass.ClassName }).ToList();
//            return Json(students, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult AllStudents()
//        {
//            var students = (from student in db.AspNetStudents
//                            select new { student.AspNetUser.Id, student.AspNetUser.UserName, student.AspNetUser.Name, student.AspNetClass.ClassName }).ToList();
//            return Json(students, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult FeeByClass(int Id)
//        {
//            var FeeTypes = (from ClassFee in db.AspNetClass_FeeType
//                            where ClassFee.ClassID == Id
//                            select new { ClassFee.Id, ClassFee.AspNetFinanceLedger.Name }).ToList();
//            return Json(FeeTypes, JsonRequestBehavior.AllowGet);
//        }


//        public JsonResult DiscountApplicableByStudent(string studentID)
//        {
//            var FeeTypes = (from Student_Discount_Applicable in db.AspNetStudent_Discount_Applicable
//                            where Student_Discount_Applicable.StudentId == studentID
//                            select new { Student_Discount_Applicable.Id, Student_Discount_Applicable.AspNetClass_FeeType.AspNetFinanceLedger.Name}).ToList();
//            return Json(FeeTypes, JsonRequestBehavior.AllowGet);
//        }
        

//        [HttpGet]
//        public JsonResult DefaultersByClass(int id)
//        {
//            var student_payments = (from a in db.AspNetStudent_Payment
//                                    select a).ToList();

//            var students = (from student in db.AspNetStudents
//                            join student_payment in db.AspNetStudent_Payment on student.StudentID equals student_payment.StudentID
//                            where student.ClassID == id && student_payment.PaymentDate == null
//                            select new { student.AspNetUser.Id, student.AspNetUser.UserName, student.AspNetUser.Name, student.AspNetClass.ClassName }).Distinct().ToList();
           
//            return Json(students, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult AllDefaulters()
//        {
//            var student_payments = (from a in db.AspNetStudent_Payment
//                                    select a).ToList();

//            var students = (from student in db.AspNetUsers
//                            join student_subject in db.AspNetStudent_Subject on student.Id equals student_subject.StudentID
//                            join subject in db.AspNetSubjects on student_subject.SubjectID equals subject.Id
//                            join student_payment in db.AspNetStudent_Payment on student.Id equals student_payment.StudentID
//                            where student_payment.PaymentDate == null
//                            select new { student.Id, student.UserName, student.Name, student_subject.AspNetSubject.AspNetClass.ClassName }).Distinct().ToList();
//            return Json(students, JsonRequestBehavior.AllowGet);
//        }

//        [HttpGet]
//        public JsonResult HistoryByStudent(string studentID)
//        {

//            var history = (from student_payment in db.AspNetStudent_Payment
//                           where student_payment.StudentID == studentID
//                           orderby student_payment.FeeChallanID descending
//                           select new { student_payment.Id ,student_payment.AspNetFeeChallan.Title, student_payment.FeeChallanID, student_payment.AspNetFeeChallan.AspNetDurationType.TypeName, student_payment.AspNetFeeChallan.StartDate, student_payment.AspNetFeeChallan.EndDate, student_payment.TotalAmount, student_payment.PaymentAmount, student_payment.PaymentDate, student_payment.Status }).ToList();
//            return Json(history, JsonRequestBehavior.AllowGet);

//        }

//        public ActionResult SalaryHistory()
//        {
//            ViewBag.VirtualRoles = new SelectList(db.AspNetVirtualRoles, "Id", "Name");
//            return View();
//        }

//        public JsonResult GetEmployeeSalaryHistory()
//        {
//            var History = db.AspNetSalaryDetails.Select(x => new { Role = x.AspNetEmployee.AspNetVirtualRole.Name, x.AspNetEmployee.Name, x.AspNetEmployee.Id, x.Status, }).Distinct().ToList();
//            return Json(History, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult GetEmployeeSalaryHistorybyRole(int Id)
//        {
//            var History = db.AspNetSalaryDetails.Where(x => x.AspNetEmployee.VirtualRoleId == Id).Select(x => new { Role = x.AspNetEmployee.AspNetVirtualRole.Name, x.AspNetEmployee.Name, x.AspNetEmployee.Id, x.Status }).Distinct().ToList();
//            return Json(History, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult GetEmployeeMonthlyHistory(int EmployeeId)
//        {
//            var EmployeeHistory = db.AspNetSalaryDetails.Where(x => x.EmployeeId == EmployeeId).Select(x => new {x.Id, x.AspNetSalary.Title, x.AspNetSalary.Month, x.AspNetSalary.Year, x.TotalSalary , x.Status}).ToList();
//            return Json(EmployeeHistory, JsonRequestBehavior.AllowGet);
//        }


//        [HttpGet]
//        public JsonResult FeeSubmit(int ChallanID, DateTime SubmitedDateSave)
//        {
//            var TransactionObj = db.Database.BeginTransaction();
//            try
//            {
//                AspNetStudent_Payment aspNetStudentPayment = db.AspNetStudent_Payment.Where(x => x.Id == ChallanID).FirstOrDefault();
//                int? LedgerID = db.AspNetFinanceLedgers.Where(x => x.Name == "Previous Fee").Select(x => x.Id).FirstOrDefault();

//                var PaymentDetails = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == aspNetStudentPayment.Id).ToList();

                
//                Voucher Voucher = new Voucher();
//                Voucher.Status = "Posted";

//                string today = DateTime.Today.ToString();

//                var Today = today.Split(' ');

//                var date = Today[0].Split('/');

//                today = date[2] + "-" + date[0] + "-" + date[1];

//                Voucher.Time = today;
//                Voucher.VoucherType = "JV";
//                Voucher.VoucherNo = FindVoucherNo(Voucher.VoucherType);
//                Voucher.VoucherDescription = "Fee Submition of " + aspNetStudentPayment.AspNetUser.Name + " of Class " + aspNetStudentPayment.AspNetFeeChallan.AspNetClass.ClassName + " at Date: " + DateTime.Now.ToShortDateString();
//                Voucher.VoucherDetail = new List<VoucherDetail>();
//                int BankDebit = 0;
//                foreach (var item in PaymentDetails)
//                {
//                    VoucherDetail VoucherDetail = new VoucherDetail();
//                    VoucherDetail.Code = item.AspNetFinanceLedger.Code;
//                    VoucherDetail.Credit = item.Amount.ToString();
//                    BankDebit += (int)item.Amount;
//                    VoucherDetail.Debit = "0";
//                    VoucherDetail.Transaction = "Credit in " + VoucherDetail.Code + " to Submit fee of " + aspNetStudentPayment.AspNetUser.Name;

//                    Voucher.VoucherDetail.Add(VoucherDetail);
//                }
                

//                VoucherDetail BankDetail = new VoucherDetail();
//                BankDetail.Code = "01-03-01";
//                BankDetail.Credit = "0";
//                BankDetail.Debit = BankDebit.ToString();
//                BankDetail.Transaction = "Debit in " + BankDetail.Code + " to Submit fee of " + aspNetStudentPayment.AspNetUser.Name;

//                Voucher.VoucherDetail.Add(BankDetail);

//                AspNetFinanceVouchersController AspNetFinanceVouchersController = new AspNetFinanceVouchersController();
//                AspNetFinanceVouchersController.AddVoucher(Voucher);

//                var PreviousChallanIDs = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == aspNetStudentPayment.Id && x.LedgerID == LedgerID).Select(x => x.AspNetStudent_Payment1).ToList();
//                foreach(AspNetStudent_Payment previousfee in PreviousChallanIDs)
//                {
//                    previousfee.Status = "Submitted";
//                    previousfee.PaymentDate = SubmitedDateSave;
//                    db.SaveChanges();

//                    var PreviousChallan =db.AspNetFeeChallans.Where(x=>x.Id<previousfee.FeeChallanID).FirstOrDefault();
//                    if (PreviousChallan==null)
//                    {
//                        var studentFine = db.AspNetStudent_Fine.Where(x => x.Date < previousfee.AspNetFeeChallan.Created).ToList();
//                        foreach(var fine in studentFine)
//                        {
//                            fine.Status = "Submitted";
//                            db.SaveChanges();
//                        }
//                    }
//                    else
//                    {
//                        var studentFine = db.AspNetStudent_Fine.Where(x => x.Date < previousfee.AspNetFeeChallan.Created && x.Date>PreviousChallan.Created).ToList();
//                        foreach (var fine in studentFine)
//                        {
//                            fine.Status = "Submitted";
//                            db.SaveChanges();
//                        }
//                    }
//                }
//                aspNetStudentPayment.Status = "Submitted";
//                aspNetStudentPayment.PaymentDate = SubmitedDateSave;

//                var PreviousChallan1 = db.AspNetFeeChallans.Where(x => x.Id < aspNetStudentPayment.FeeChallanID).OrderByDescending(x=>x.Id).FirstOrDefault();
//                if (PreviousChallan1 == null)
//                {
//                    var studentFine = db.AspNetStudent_Fine.Where(x => x.Date < aspNetStudentPayment.AspNetFeeChallan.Created).ToList();
//                    foreach (var fine in studentFine)
//                    {
//                        fine.Status = "Submitted";
//                    }
//                }
//                else
//                {
//                    var studentFine = db.AspNetStudent_Fine.Where(x => x.Date < aspNetStudentPayment.AspNetFeeChallan.Created && x.Date > PreviousChallan1.Created).ToList();
//                    foreach (var fine in studentFine)
//                    {
//                        fine.Status = "Submitted";
//                    }
//                }

//                db.SaveChanges();
//                TransactionObj.Commit();
//                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                /* var UserNameLog = User.Identity.Name;
//                 AspNetUser userObjLog = db.AspNetUsers.First(x => x.UserName == UserNameLog);
//                 string UserIDLog = userObjLog.Id;
//                 var logMessage = "Fee Submitted, Student Name: " + username + ", Student ID: " + UserObj.Id + ", Fee: " + PaymentObj.PaymentAmount + ", Date: " + PaymentObj.PaymentDate;

//                 var LogControllerObj = new AspNetLogsController();
//                 LogControllerObj.CreateLogSave(logMessage, UserIDLog);*/

//                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//                string submit = "Fee Submitted";
//                return Json(submit, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                TransactionObj.Dispose();
//                string submit = "Something Went Wrong..!";
//                return Json(submit, JsonRequestBehavior.AllowGet);
//            }
//        }
        

//        [HttpGet]
//        public JsonResult GetUserFeeDetail(int ChallanID, DateTime date)
//        {
//            AspNetStudent_Payment aspNetStudentPayment = db.AspNetStudent_Payment.Where(x => x.Id == ChallanID).FirstOrDefault();
//            AspNetSession Session = db.AspNetSessions.OrderByDescending(x => x.Id).Select(x => x).FirstOrDefault();
           

//            TimeSpan diff = date - Convert.ToDateTime(aspNetStudentPayment.AspNetFeeChallan.DueDate);
//            int days = diff.Days;
//            int? LedgerID = db.AspNetFinanceLedgers.Where(x => x.Name == "Penalty").Select(x => x.Id).FirstOrDefault();
//            int Penalty = days * Convert.ToInt32(aspNetStudentPayment.AspNetFeeChallan.Penalty);
//            if (days>0)
//            {
//                AspNetStudent_PaymentDetail aspNetStudentPaymentDetail = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == aspNetStudentPayment.Id && x.LedgerID == LedgerID).FirstOrDefault();
//                if (aspNetStudentPaymentDetail == null)
//                {
//                    AspNetStudent_PaymentDetail student_payment_detail = new AspNetStudent_PaymentDetail();
//                    student_payment_detail.Student_PaymentID = aspNetStudentPayment.Id;

//                    student_payment_detail.LedgerID = LedgerID;
//                    student_payment_detail.Amount = Penalty;
//                    db.AspNetStudent_PaymentDetail.Add(student_payment_detail);
//                    db.SaveChanges();
//                }
//                else
//                {
//                    aspNetStudentPaymentDetail.Amount = Penalty;
//                    db.SaveChanges();
//                }
//            }
//            else
//            {
//                AspNetStudent_PaymentDetail aspNetStudentPaymentDetail = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == aspNetStudentPayment.Id && x.LedgerID == LedgerID).FirstOrDefault();
//                if (aspNetStudentPaymentDetail != null)
//                {
//                    aspNetStudentPaymentDetail.Amount = 0;
//                    db.SaveChanges();
//                }
//            }


//            aspNetStudentPayment.PaymentAmount = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == aspNetStudentPayment.Id).Sum(x => x.Amount);
//            db.SaveChanges();


//            challanform Challan = new challanform();
//            Challan.AcademicSessionStart = Session.SessionStartDate;
//            Challan.AcademicSessionEnd = Session.SessionEndDate;
//            Challan.ChallanID = aspNetStudentPayment.Id;
//            Challan.StudentName = aspNetStudentPayment.AspNetUser.Name;
//            Challan.StudentUserName = aspNetStudentPayment.AspNetUser.UserName;
//            Challan.StudentClass = db.AspNetStudents.Where(x => x.StudentID == aspNetStudentPayment.StudentID).Select(x => x.AspNetClass.ClassName).FirstOrDefault();
//            Challan.SchoolName = "NGS";
//            Challan.BranchName = "Canal Branch";
//            Challan.ChallanCopy = new List<string>();
//            Challan.ChallanCopy.Add("Student Copy");
//            Challan.ChallanCopy.Add("Bank Copy");
//            Challan.ChallanCopy.Add("School Copy");

//            var FeeBreakdown = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == aspNetStudentPayment.Id).ToList();
//            Challan.FeeType = new List<FeeTypes>();
//            foreach (var fee in FeeBreakdown)
//            {
//                FeeTypes feetype = new FeeTypes();
//                feetype.Name = fee.AspNetFinanceLedger.Name;
//                feetype.Amount = Convert.ToInt32(fee.Amount);
//                Challan.FeeType.Add(feetype);
//            }

//            Challan.DueDate = aspNetStudentPayment.AspNetFeeChallan.DueDate;
//            Challan.TotalAmount = Convert.ToInt32(Challan.FeeType.Sum(x => x.Amount));
//            Challan.Penalty = aspNetStudentPayment.AspNetFeeChallan.Penalty;

//            return Json(Challan, JsonRequestBehavior.AllowGet);
//        }
        
//        public class feeType
//        {
//            public string typeName { get; set; }
//            public int amount { get; set; }
//        }
//        public class challanform
//        {
//            public string SchoolName { get; set; }
//            public string BranchName { get; set; }
//            public List<string> ChallanCopy { get; set; }
//            public DateTime? AcademicSessionStart { get; set; }
//            public DateTime? AcademicSessionEnd { get; set; }
//            public int? ChallanID { get; set; }
//            public string UserID { get; set; }
//            public string StudentName { get; set; }
//            public string StudentUserName { get; set; }
//            public string StudentClass { get; set; }
//            public List<FeeTypes> FeeType { get; set; }
//            public List<String> DiscountNotes { get; set; }
//            public DateTime? DueDate { get; set; }
//            public List<String> Notes { get; set; }
//            public DateTime PrintedDate { get; set; }
//            public int TotalAmount { get; set; }
//            public int? Penalty { get; set; }
//            public DateTime? ValidDate { get; set; }
//            public int? Previous { get; set; }


//        }

//        public class FeeTypes
//        {
//            public int? Amount { get; set; }
//            public string Name { get; set; }
//        }
        
//        public JsonResult GetChallanForm(string StudentID,int ChallanID)
//        {
//            var studentClassID = db.AspNetStudents.Where(x => x.StudentID == StudentID).Select(x => x.ClassID).FirstOrDefault();
//            AspNetClass studentClass = (from aspNetClass in db.AspNetClasses
//                                        where aspNetClass.Id == studentClassID
//                                        select aspNetClass).FirstOrDefault();
//            AspNetFeeChallan FeeChallan = db.AspNetFeeChallans.Where(x => x.Id== ChallanID).Select(x => x).FirstOrDefault();
//            AspNetStudent_Payment Student_Payment = db.AspNetStudent_Payment.Where(x => x.FeeChallanID == ChallanID && x.StudentID==StudentID).Select(x => x).FirstOrDefault();
//            AspNetSession Session = db.AspNetSessions.OrderByDescending(x => x.Id).Select(x => x).FirstOrDefault();
//            AspNetUser Student = db.AspNetUsers.Where(x => x.Id == StudentID).Select(x => x).FirstOrDefault();

//            AspNetStudent_Payment studentPayment = db.AspNetStudent_Payment.Where(x => x.FeeChallanID == ChallanID).Select(x => x).FirstOrDefault();
//            int? LedgerID = db.AspNetFinanceLedgers.Where(x => x.Name == "Penalty").Select(x => x.Id).FirstOrDefault();
//            if (studentPayment.Status == "Not Submitted")
//            {
//                TimeSpan difference = DateTime.Now - Convert.ToDateTime(FeeChallan.DueDate);
//                int days = difference.Days;
//                int Penalty = days * Convert.ToInt32(FeeChallan.Penalty);
//                if (days > 0)
//                {
//                    AspNetStudent_PaymentDetail aspNetStudentPaymentDetail = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == studentPayment.Id && x.LedgerID == LedgerID).FirstOrDefault();
//                    if (aspNetStudentPaymentDetail == null)
//                    {
//                        AspNetStudent_PaymentDetail student_payment_detail = new AspNetStudent_PaymentDetail();
//                        student_payment_detail.Student_PaymentID = studentPayment.Id;

//                        student_payment_detail.LedgerID = LedgerID;
//                        student_payment_detail.Amount = Penalty;
//                        db.AspNetStudent_PaymentDetail.Add(student_payment_detail);
//                        db.SaveChanges();
//                    }
//                    else
//                    {
//                        aspNetStudentPaymentDetail.Amount = Penalty;
//                        db.SaveChanges();
//                    }
//                }
//                else
//                {
//                    AspNetStudent_PaymentDetail aspNetStudentPaymentDetail = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == studentPayment.Id && x.LedgerID == LedgerID).FirstOrDefault();
//                    if (aspNetStudentPaymentDetail != null)
//                    {
//                        aspNetStudentPaymentDetail.Amount = 0;
//                        db.SaveChanges();
//                    }
//                }
//                studentPayment.PaymentAmount = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == studentPayment.Id).Sum(x => x.Amount);
//                db.SaveChanges();
//            }
//            challanform Challan = new challanform();
//            Challan.AcademicSessionStart = Session.SessionStartDate;
//            Challan.AcademicSessionEnd = Session.SessionEndDate;
//            Challan.ChallanID = Student_Payment.Id; // Student_Payment.Id;
//            Challan.StudentName = Student.Name;
//            Challan.StudentUserName = Student.UserName;
//            Challan.StudentClass = studentClass.ClassName;
//            Challan.SchoolName = "NGS-IPC PRESCHOOL";
//            Challan.BranchName = "Canal Branch";
//            Challan.ChallanCopy = new List<string>();
//            Challan.ChallanCopy.Add("Parent Copy");
//            Challan.ChallanCopy.Add("Bank Copy");
//            Challan.ChallanCopy.Add("School Copy");
//            var feetypes = (from student_payment in db.AspNetStudent_PaymentDetail
//                            where student_payment.Student_PaymentID == Challan.ChallanID
//                            select new { student_payment.Amount, student_payment.LedgerID,student_payment.AspNetFinanceLedger }).ToList();

//            Challan.FeeType = new List<FeeTypes>();
            
//            foreach (var item in feetypes)
//            {
//                FeeTypes stu_pay = new FeeTypes();
//                stu_pay.Amount = item.Amount;
//                stu_pay.Name = item.AspNetFinanceLedger.Name;
//                Challan.FeeType.Add(stu_pay);
//            }

//            Challan.DueDate = FeeChallan.DueDate;
//            Challan.TotalAmount = Convert.ToInt32(Challan.FeeType.Sum(x => x.Amount));
//            Challan.Penalty = FeeChallan.Penalty;
//            Challan.ValidDate = FeeChallan.ValidDate;



//            return Json(Challan, JsonRequestBehavior.AllowGet);

//        }

//        public string FindVoucherNo(string type)
//        {
//            int No;
//            try
//            {
//                var Type = db.AspNetFinanceVouchers.Where(x => x.VoucherType == type).OrderByDescending(x => x.Id).Take(1).Select(x => x).FirstOrDefault();
//                string time = Type.Time.ToString();
//                var month = DateTime.Now.ToString("MM");
//                string[] thismonth = time.Split('/');
//                string DbMonth = thismonth[0];
//                if (DbMonth.Length == 1)
//                {
//                    DbMonth = "0" + DbMonth;
//                }
//                if (month == DbMonth)
//                {
//                    No = (int)db.AspNetFinanceVouchers.Where(x => x.VoucherType == type).Select(x => x.VoucherNo).Max();
//                    No++;
//                }
//                else
//                {
//                    No = 1;
//                }

//            }
//            catch
//            {
//                No = 1;
//            }

//            return No.ToString();

//        }

//        //public class Voucher
//        //{
//        //    public string Status { get; set; }
//        //    public string VoucherDescription { get; set; }
//        //    public string VoucherNo { get; set; }
//        //    public string VoucherType { get; set; }
//        //    public string Time { get; set; }
//        //    public int Id { get; set; }
//        //    public List<VoucherDetail> VoucherDetail { set; get; }
//        //}

//        //public class VoucherDetail
//        //{
//        //    public string Type { get; set; }
//        //    public string VoucherNo { get; set; }
//        //    public string Time { get; set; }
//        //    public string Code { get; set; }
//        //    public string Transaction { get; set; }
//        //    public string Credit { get; set; }
//        //    public string Debit { get; set; }
//        //    public double balance { get; set; }

//        //}


//    }
//}