using SEA_Application.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Text;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;

namespace SEA_Application.Controllers
{
    public class GrandTotalController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        //
        // GET: /GrandTotal/
        public ActionResult Index()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }

        public ActionResult ListofStudents(int id)
        {
            if (id == 0)
            {
                var result1 = (from std in db.AspNetStudents
                               join usr in db.AspNetUsers on std.StudentID equals usr.Id
                               join fee_mon in db.StudentFeeMonths on std.Id equals fee_mon.StudentId


                               where usr.Status != "False"
                               select new { fee_mon.Id, usr.Name, usr.PhoneNumber, usr.Email, usr.UserName, std.AspNetClass.ClassName, fee_mon.Months, fee_mon.Status, fee_mon.FeePayable }).ToList();
                return Json(result1, JsonRequestBehavior.AllowGet);


            }

            else
            {
                var SessionName = db.AspNetClasses.Where(x => x.Id == id).FirstOrDefault().ClassName;
                var SessionId = db.AspNetSessions.Where(x => x.SessionName == SessionName).FirstOrDefault().Id;


                var result1 = (from std in db.AspNetStudents
                               join usr in db.AspNetUsers on std.StudentID equals usr.Id
                               join fee_mon in db.StudentFeeMonths on std.Id equals fee_mon.StudentId

                               where usr.Status != "False" && fee_mon.SessionId == SessionId
                               select new { fee_mon.Id, usr.Name, usr.PhoneNumber, usr.Email, usr.UserName, std.AspNetClass.ClassName, fee_mon.Months, fee_mon.Status, fee_mon.FeePayable }).ToList();
                return Json(result1, JsonRequestBehavior.AllowGet);

            }


            //}
            //else
            //{
            //    var result1 = (from std in db.AspNetStudents
            //                   join usr in db.AspNetUsers on std.StudentID equals usr.Id
            //                   where usr.Status != "False" && std.ClassID == id
            //                   select new { usr.Id, usr.Name, usr.PhoneNumber, usr.Email, usr.UserName, std.AspNetClass.ClassName }).ToList();
            //    return Json(result1, JsonRequestBehavior.AllowGet);
            //}

        }

        public JsonResult AllDefaulterStudents()
        {
            //List<int?> DefaulterStudentIds = new List<int?>();

            //var StudentsIds = from student in db.StudentFeeMonths

            //                  group student by student.StudentId into grp
            //                  where grp.Count() == 1 && grp.Any(x => x.FeePayable != 0)
            //                  select grp.Key;

            //var result1 = (from std in db.AspNetStudents
            //               join usr in db.AspNetUsers on std.StudentID equals usr.Id
            //               join fee_mon in db.StudentFeeMonths on std.Id equals fee_mon.StudentId

            //               where StudentsIds.Contains(fee_mon.StudentId)

            //               select new
            //               {
            //                   usr.Name,
            //                   usr.PhoneNumber,
            //                   usr.Email,
            //                   usr.UserName,
            //                   std.AspNetClass.ClassName,
            //                   fee_mon.Months,
            //                   fee_mon.Status,
            //                   fee_mon.FeePayable,
            //                   fee_mon.StudentId,

            //               }).ToList();



            //var FindDefaulterStudent = db.DefaulterStudents().ToList();

            //DefaulterStudentIds.AddRange(StudentsIds);


            //foreach (var student in FindDefaulterStudent)
            //{


            //    DateTime studentDateTime = Convert.ToDateTime(student.IssueDate);

            //    int TotalDays = DateTime.Now.Subtract(studentDateTime).Days;


            //    if (TotalDays > 30)
            //    {

            //        DefaulterStudentIds.Add(student.StudentId);

            //    }


            //}


            //var DefaulterStudentsData = (from std in db.AspNetStudents
            //               join usr in db.AspNetUsers on std.StudentID equals usr.Id
            //               join fee_mon in db.StudentFeeMonths on std.Id equals fee_mon.StudentId
            //                where DefaulterStudentIds.Contains(fee_mon.StudentId)
            //                group fee_mon by fee_mon.StudentId into grp

            //               select new
            //               {
            //                  grp 
            //               }).ToList();

            //db.AspNetStudents.Where(x=> x.c)
            //  var min_payable = result1.Where(x => x.FeePayable = min(x.FeePayable));
            // var results = DefaulterStudentsData.GroupBy(x => x.StudentId).Select(x => x);

            //var StudentsIdsMoreThanOne = from student in db.StudentFeeMonths

            //  group student by student.StudentId into grp
            //  where grp.Count() < 1 && grp.Any(x => x.FeePayable != 0)
            //  select grp.Key;

            //   return Json(result1, JsonRequestBehavior.AllowGet);

            var result = db.DefaulterStudents().ToList();

            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public ActionResult StudentFee()
        {

            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName");
            //  ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");

            ViewBag.ClassID1 = new SelectList(db.AspNetClasses, "Id", "ClassName");


            var UserId = User.Identity.GetUserId();
            var username = db.AspNetUsers.Where(x => x.Id == UserId).Select(x => x.Name).FirstOrDefault();

            ViewBag.UserName = username;

            return View();
        }

       public JsonResult DefaulterStudentsByClass (string ClassName )
        {

            var AllDefaulterStudent = db.DefaulterStudents().Where(x=>x.ClassName == ClassName).ToList();
            return Json(AllDefaulterStudent, JsonRequestBehavior.AllowGet);
        }




        public void FeeChallan_ExcelReport(string month)
        {
            SEA_DatabaseEntities db = new SEA_DatabaseEntities();

            var challanlist = (from feemonth in db.StudentFeeMonths
                               join std in db.AspNetStudents on feemonth.StudentId equals std.Id
                               join challan in db.StudentChallanForms on std.Id equals challan.StudentId
                               where feemonth.Status == "Pending" && feemonth.Months == month && feemonth.Id == challan.StudentFeeMonthId
                               select new { challan.ChallanNo, feemonth.Months, feemonth.FeePayable, feemonth.IssueDate, std.AspNetUser.Name, std.AspNetClass.ClassName, feemonth.DueDate, feemonth.ValildityDate, }).ToList();


            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Challan No";
            ws.Cells["B1"].Value = "Payable before due date";
            ws.Cells["C1"].Value = "Issue Date";
            ws.Cells["D1"].Value = "Registration No";
            ws.Cells["E1"].Value = "Student Name";
            ws.Cells["F1"].Value = "Class";
            ws.Cells["G1"].Value = "Due Date";
            ws.Cells["H1"].Value = "For the month of";
            ws.Cells["I1"].Value = "Payable after due date";
            ws.Cells["J1"].Value = "Validity Date";


            int rowStart = 2;
            foreach (var item in challanlist)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ChallanNo;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.FeePayable;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.IssueDate;
                ws.Cells[string.Format("D{0}", rowStart)].Value = "";
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Name;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.ClassName;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.DueDate;
                ws.Cells[string.Format("H{0}", rowStart)].Value = month;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.FeePayable;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.ValildityDate;

                rowStart++;
            }
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment:filename=ExcelReport.xls");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            //return RedirectToAction("TeacherIndex");
        }
        public ActionResult ListAllStudents()
        {
            //var result1 = (from std in db.AspNetStudents
            //               join usr in db.AspNetUsers on std.StudentID equals usr.Id
            //               where usr.Status != "False"
            //               select new { usr.Id, usr.Name, usr.PhoneNumber, usr.Email, usr.UserName, std.AspNetClass.ClassName }).ToList();
            ///        Er--rorEventArgs--

            var result1 = (from std in db.AspNetStudents
                           join usr in db.AspNetUsers on std.StudentID equals usr.Id
                           join fee_mon in db.StudentFeeMonths on std.Id equals fee_mon.StudentId

                           where usr.Status != "False"
                           select new { fee_mon.Id, usr.Name, usr.PhoneNumber, usr.Email, usr.UserName, std.AspNetClass.ClassName, fee_mon.Months, fee_mon.Status, fee_mon.FeePayable }).ToList();
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddChallanDate(FeeDateSetting FDS)
        {
            string status = "error";
            if (FDS.ValidityDate != null && FDS.DueDate != null)
            {
                if (db.FeeDateSettings.ToList().Count > 0)
                {
                    FeeDateSetting settings = db.FeeDateSettings.Where(x => x.Id == 1).FirstOrDefault();
                    settings.DueDate = FDS.DueDate;
                    settings.ValidityDate = FDS.ValidityDate;

                }
                else
                {
                    db.FeeDateSettings.Add(FDS);
                }
                if (db.SaveChanges() > 0)
                {
                    status = "success";

                }
            }

            return Content(status);
        }
        public ActionResult GetChallanDate()
        {

            string status = "error";
            FeeDateSetting List = db.FeeDateSettings.FirstOrDefault();
            if (List != null)
            {
                status = Newtonsoft.Json.JsonConvert.SerializeObject(List);
            }
            return Content(status);
        }


        public ActionResult ChallanPrintDetails(string S_UID)
        {
            var StudentID = db.AspNetStudents.Where(x => x.StudentID == S_UID).FirstOrDefault().Id;
            var studentFeeMonths = db.StudentFeeMonths.Include(s => s.AspNetStudent).Where(x => x.StudentId == StudentID).ToList();

            return View(studentFeeMonths);
        }

        public ActionResult ClassList()
        {
            List<AspNetClass> list = new List<AspNetClass>();
            list = db.AspNetClasses.ToList();
            string status = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            return Content(status);
        }
        public ActionResult UpdateStudentFeeMonth_Dates(int StudentID, string Month, DateTime validitydate, DateTime duedate)
        {

            string status = "error";
            StudentFeeMonth settings = db.StudentFeeMonths.Where(x => x.StudentId == StudentID && x.Months == Month).FirstOrDefault();
            settings.ValildityDate = validitydate;
            settings.DueDate = duedate;
            settings.IsPrinted = true;
            if (db.SaveChanges() > 0)
            {
                status = "success";

            }

            return Content(status);
        }

        public ActionResult StudentFeeDetails(int sid, string month)
        {

            if (sid != null && month != "")
            {

                var StudentID = sid;
                ViewBag.StudentID = StudentID;
                ViewBag.Month = month;
                string S_UID = db.AspNetStudents.Where(x => x.Id == sid).FirstOrDefault().StudentID;
                AspNetUser studentdetails = db.AspNetUsers.Where(x => x.Id == S_UID).FirstOrDefault();
                ViewBag.RollNo = studentdetails.UserName;
                ViewBag.Name = studentdetails.Name;
                ViewBag.PhoneNo = studentdetails.PhoneNumber;
            }
            else
            {
                ViewBag.Error = "NotFound";
            }
            // AspNetStudent student =  db.AspNetStudents.Where(x => x.Id == StudentID).FirstOrDefault();
            return View();
        }
        public class feeType
        {
            public string typeName { get; set; }
            public int amount { get; set; }
        }
        public class challanform
        {
            public string SchoolName { get; set; }
            public string BranchName { get; set; }
            public List<string> ChallanCopy { get; set; }
            public DateTime? AcademicSessionStart { get; set; }
            public DateTime? AcademicSessionEnd { get; set; }
            public decimal? ChallanID { get; set; }
            public string UserID { get; set; }
            public string StudentName { get; set; }
            public string StudentUserName { get; set; }
            public string StudentClass { get; set; }
            public List<FeeTypes> FeeType { get; set; }
            public List<String> DiscountNotes { get; set; }
            public string DueDate { get; set; }
            public List<String> Notes { get; set; }
            public DateTime PrintedDate { get; set; }
            public double? PayableFee { get; set; }
            public double? TotalAmount { get; set; }
            public double? Penalty { get; set; }
            public string ValidDate { get; set; }
            public double? Arrears { get; set; }
            public string FeeMonth { get; set; }
            public double? TripCharges { get; set; }


        }

        public class FeeTypes
        {
            public int? Amount { get; set; }
            public string Name { get; set; }
        }
        public JsonResult GetChallanForm(string month, string idlist)
        {

            //var userid = idlist.Split(',');
            //var dates = db.FeeDateSettings.FirstOrDefault();
            //// challanform ChallanList = new challanform();
            //challanform ChallanList = new challanform();

            //int feemonthid = Int32.Parse(idlist);
            //var mnth = db.StudentFeeMonths.Where(x => x.Id == feemonthid).FirstOrDefault().Months;
            //month = mnth;
            //var S_ID = db.StudentFeeMonths.Where(x => x.Id == feemonthid).FirstOrDefault().StudentId;
            //var student = db.AspNetStudents.Where(x => x.Id == S_ID).FirstOrDefault();
            //StudentFeeMonth Student_FeeMonth = db.StudentFeeMonths.Where(x => x.StudentId == student.Id && x.Months == month).FirstOrDefault();

            //// Student_FeeMonth.ValildityDate = dates.ValidityDate;
            ////  Student_FeeMonth.DueDate = dates.DueDate;
            //Student_FeeMonth.Status = "Paid";
            //db.SaveChanges();

            var userid = idlist.Split(',');
            int feemonthid = Int32.Parse(idlist);

            challanform ChallanList = new challanform();
            var FeeMonth = db.StudentFeeMonths.Where(x => x.Id == feemonthid).FirstOrDefault();

            ChallanList.FeeMonth = FeeMonth.Months;
            ChallanList.StudentName = FeeMonth.AspNetStudent.AspNetUser.Name;
            ChallanList.StudentUserName = FeeMonth.AspNetStudent.AspNetUser.UserName;
            ChallanList.PayableFee = FeeMonth.FeePayable;
            ChallanList.StudentClass = FeeMonth.AspNetStudent.AspNetClass.ClassName;
            ChallanList.ChallanID = FeeMonth.Id;


            return Json(ChallanList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AllResults(string Month, int StudentID)
        {

            //   double? RecurrenceFee, NonRecurrenceFee,GrandTotal;
            // decimal? Discount, PlentyFee;
            GrandTotal GT = new GrandTotal();
            if (Month != "" && StudentID != null)
            {
                //Recurrence Fee
                try
                {
                    double? fee = db.StudentFeeMonths.Where(x => x.StudentId == StudentID && x.Months == Month && x.Status == "Pending").FirstOrDefault().FeePayable;

                    if (fee == null)
                    {
                        fee = 0;
                    }

                    GT.RecurrenceFee = fee.ToString();
                }

                catch (Exception ex)
                {
                    return RedirectToAction("StudentFeeDetails");

                }

                //NonRecurrence Fee
                List<NonRecurringFeeMultiplier> result = db.NonRecurringFeeMultipliers.Where(x => x.StudentId == StudentID && x.Month == Month && x.Status == "Pending").ToList();
                double? totalfee = 0;

                for (int i = 0; i < result.Count(); i++)
                {
                    totalfee += result[i].TutionFee;
                }
                GT.NonRecurrenceFee = totalfee.ToString();

                //Discount 
                var result1 = (from std_dis in db.StudentDiscounts
                               join fee_dis in db.FeeDiscounts on std_dis.FeeDiscountId equals fee_dis.Id
                               where std_dis.StudentId == StudentID && std_dis.Month == Month
                               select new { fee_dis.Amount }).ToList();
                decimal? totalfee1 = 0;
                for (int i = 0; i < result1.Count(); i++)
                {
                    totalfee1 += result1[i].Amount;
                }
                GT.Discount = totalfee1.ToString();



                //arrears

                var StudentFeeMonthData = db.StudentFeeMonths.Where(x => x.StudentId == StudentID && x.Months == Month).FirstOrDefault();
                GT.StudentFeeMonthId = StudentFeeMonthData.Id;
                var _SessionId = StudentFeeMonthData.SessionId;
                int _Id = StudentFeeMonthData.Id - 1;
                bool Flag = true;
                double? Arrear = 0;
                while (Flag)
                {
                    try
                    {
                        double? payableamount = db.StudentFeeMonths.Where(x => x.StudentId == StudentID && x.SessionId == _SessionId && x.Status == "Pending" && x.Id == _Id).FirstOrDefault().FeePayable;

                        Arrear += payableamount;
                        _Id--;
                    }
                    catch (Exception ex)
                    {
                        Flag = false;
                    }
                }
                GT.Arrear = Arrear.ToString();



                //plentyfee

                var result3 = (from stdplenty in db.StudentPenalties
                               join plenty in db.PenaltyFees on stdplenty.PenaltyId equals plenty.Id
                               where stdplenty.StudentId == StudentID
                               select new { plenty.Amount }).ToList();
                decimal? totalfee3 = 0;
                for (int i = 0; i < result3.Count(); i++)
                {
                    totalfee3 += result3[i].Amount;
                }

                GT.PlentyFee = totalfee3.ToString();

                string Result = Newtonsoft.Json.JsonConvert.SerializeObject(GT);

                return Content(Result);
            }
            return Content("0");

        }
        public ActionResult StudentByClass(int id)
        {


            //   var studentList = db.AspNetStudents.Where(x => x.AspNetStudent_Session_class.Any(y => y.ClassID == id)).Select(x => x.Id).ToList();
            int? sessionId = db.AspNetClasses.Where(x => x.Id == id).FirstOrDefault().SessionID;


            var result1 = db.GetPendingStudentsList(sessionId.ToString());

            //&& !db.StudentFeeMonths.Any(m => m.StudentId == std.Id)
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ChangeStatus(int Id)
        {

            var StudentFeeMonthToUpdate = db.StudentFeeMonths.Where(x => x.Id == Id).FirstOrDefault();

            StudentFeeMonthToUpdate.Status = "Paid";
            db.SaveChanges();



            var id = User.Identity.GetUserId();
            var username = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();



            Voucher voucher = new Voucher();
            voucher.Name = "Student Fee Clear";
            voucher.Notes = "Student Fee Clear";
            voucher.Date = GetLocalDateTime.GetLocalDateTimeFunction();
            voucher.CreatedBy = username;
            voucher.SessionID = SessionID;

            int? VoucherObj = db.Vouchers.Max(x => x.VoucherNo);

            voucher.VoucherNo = Convert.ToInt32(VoucherObj) + 1;

            db.Vouchers.Add(voucher);
            db.SaveChanges();

            var Leadger = db.Ledgers.Where(x => x.Name == "Account Receiveable").FirstOrDefault();

            int AccountRecId = Leadger.Id;

            decimal? CurrentBalance = Leadger.CurrentBalance;


            VoucherRecord voucherRecord = new VoucherRecord();
            decimal? AfterBalance = CurrentBalance - Convert.ToDecimal(StudentFeeMonthToUpdate.FeePayable);

            voucherRecord.LedgerId = AccountRecId;
            voucherRecord.Type = "Cr";
            voucherRecord.Amount = Convert.ToDecimal(StudentFeeMonthToUpdate.FeePayable);

            voucherRecord.CurrentBalance = CurrentBalance;
            voucherRecord.AfterBalance = AfterBalance;
            voucherRecord.VoucherId = voucher.Id;
            voucherRecord.Description = "Student Fee Credit in Account Receiveable";

            Leadger.CurrentBalance = AfterBalance;

            db.VoucherRecords.Add(voucherRecord);
            db.SaveChanges();


            //Second;

            VoucherRecord voucherRecord1 = new VoucherRecord();

            var LeadgerAdminDrawer = db.Ledgers.Where(x => x.Name == "Admin Drawer").FirstOrDefault();

            decimal? CurrentBalanceOfAdminDrawer = LeadgerAdminDrawer.CurrentBalance;

            decimal? AfterBalanceOfAdminDrawer = CurrentBalanceOfAdminDrawer + Convert.ToDecimal(StudentFeeMonthToUpdate.FeePayable);

            voucherRecord1.LedgerId = LeadgerAdminDrawer.Id;
            voucherRecord1.Type = "Dr";
            voucherRecord1.Amount = Convert.ToDecimal(StudentFeeMonthToUpdate.FeePayable);
            voucherRecord1.CurrentBalance = CurrentBalanceOfAdminDrawer;
            voucherRecord1.AfterBalance = AfterBalanceOfAdminDrawer;
            voucherRecord1.VoucherId = voucher.Id;
            voucherRecord1.Description = "Student Fee Debit in Admin Drawr";
            LeadgerAdminDrawer.CurrentBalance = AfterBalanceOfAdminDrawer;

            db.VoucherRecords.Add(voucherRecord1);
            db.SaveChanges();



            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentFeeDetail(int id)
        {
            var studentFeeDetails = db.StudentFeeMonths.Where(x => x.StudentId == id).OrderByDescending(x => x.IssueDate).FirstOrDefault();

            var CountStudentFee = db.StudentFeeMonths.Where(x => x.StudentId == id).Count();
            var FeeCount = "";

            if (CountStudentFee == 1)
            {
                FeeCount = "1st";

            }
            else if (CountStudentFee == 2)
            {
                FeeCount = "2nd";

            }
            else if (CountStudentFee == 3)
            {
                FeeCount = "3rd";

            }
            else if (CountStudentFee == 4)
            {
                FeeCount = "4th";

            }
            else if (CountStudentFee == 5)
            {
                FeeCount = "5th";

            }
            else if (CountStudentFee == 6)
            {
                FeeCount = "6th";

            }

            else if (CountStudentFee == 7)
            {
                FeeCount = "7th";

            }

            else if (CountStudentFee == 8)
            {
                FeeCount = "8th";

            }

            else if (CountStudentFee == 9)
            {
                FeeCount = "9th";

            }

            else if (CountStudentFee == 10)
            {
                FeeCount = "10th";

            }

            else
            {
                FeeCount = "";

            }

            return Json(new
            {
                ReceiptNo = FeeCount,
                Id = studentFeeDetails.Id,
                TotalFee = studentFeeDetails.TotalFee,
                FeePayable = studentFeeDetails.FeePayable,
                studentFeeDetails.FeeType,

            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult StudentFeeUpdate(int id, int ClassId, double RemainingFee = 0, double Discount = 0, double CashReceived = 0,string DueDate = null,string IssueDate = null)
        {
            var FeePayable = RemainingFee + Discount + CashReceived;
            int? sessionId = db.AspNetClasses.Where(x => x.Id == ClassId).FirstOrDefault().SessionID;

            // db.AspNetSessions.Where(x=>x.Id==)
            var SessionName = db.AspNetSessions.Where(x => x.Id == sessionId).FirstOrDefault().SessionName;

            var studenfee = db.StudentFeeMonths.Where(x => x.Id == id).FirstOrDefault();

            StudentFeeMonth stdFeeMonth = new StudentFeeMonth();
            stdFeeMonth.StudentId = studenfee.StudentId;
            
        //    stdFeeMonth.IssueDate = DateTime.Now;
             

            stdFeeMonth.TotalFee = studenfee.TotalFee;
            stdFeeMonth.FeePayable = RemainingFee;
            stdFeeMonth.Discount = Discount;
            stdFeeMonth.FeeType = studenfee.FeeType;
            stdFeeMonth.SessionId = sessionId;
            stdFeeMonth.FeeReceived = CashReceived;
        
            DateTime Date = Convert.ToDateTime(DueDate);

            stdFeeMonth.DueDate = Date;

            DateTime ConvertedIssueDate = Convert.ToDateTime(IssueDate);

            stdFeeMonth.IssueDate = ConvertedIssueDate;



            if (RemainingFee == 0)
            {
                stdFeeMonth.Status = "Paid";
            }
            else
            {

                stdFeeMonth.Status = "Pending";
            }

            //var Month = DateTime.Now.ToString("MMMM");

            var Month = ConvertedIssueDate.ToString("MMMM");

            stdFeeMonth.Months = Month;

            db.StudentFeeMonths.Add(stdFeeMonth);
            db.SaveChanges();

            var idd = User.Identity.GetUserId();
            var username = db.AspNetUsers.Where(x => x.Id == idd).Select(x => x.Name).FirstOrDefault();
            Voucher voucher = new Voucher();

            var UsrId = db.AspNetStudents.Where(x => x.Id == studenfee.StudentId).FirstOrDefault().StudentID;

            var StudentName = db.AspNetUsers.Where(x => x.Id == UsrId).FirstOrDefault().Name;

            voucher.Name = "Fee Received by Admin of Student " + StudentName + " Session Name " + SessionName;
            voucher.Notes = "Fee Received by Admin " + StudentName + " Session Name " + SessionName;
            voucher.StudentId = studenfee.StudentId;
            voucher.Date = GetLocalDateTime.GetLocalDateTimeFunction();
            voucher.CreatedBy = username;
            voucher.SessionID = SessionID;
            int? VoucherObj = db.Vouchers.Max(x => x.VoucherNo);

            voucher.VoucherNo = Convert.ToInt32(VoucherObj) + 1;
            db.Vouchers.Add(voucher);

            db.SaveChanges();


            var Leadger = db.Ledgers.Where(x => x.Name == "Account Receiveable").FirstOrDefault();
            int AccountReceiveableId = Leadger.Id;
            decimal? CurrentBalance = Leadger.CurrentBalance;
            decimal? AfterBalance = 0;
            VoucherRecord voucherRecord = new VoucherRecord();
            if (Discount != 0)
            {

                AfterBalance = CurrentBalance - Convert.ToDecimal(CashReceived) - Math.Round( Convert.ToDecimal(Discount));
            }
            else
            {
                AfterBalance = CurrentBalance -  Convert.ToDecimal(CashReceived);
            }
            voucherRecord.LedgerId = AccountReceiveableId;
            voucherRecord.Type = "Cr";
            voucherRecord.Amount = Convert.ToDecimal(CashReceived) + Math.Round( Convert.ToDecimal(Discount));
            voucherRecord.CurrentBalance = CurrentBalance;

            voucherRecord.AfterBalance = AfterBalance;
            voucherRecord.VoucherId = voucher.Id;
            voucherRecord.Description = "Fee received of Student (" + StudentName + ") (" + SessionName + ") ";

            Leadger.CurrentBalance = AfterBalance;
            try
            {


                db.VoucherRecords.Add(voucherRecord);
                db.SaveChanges();

            }
            catch(Exception ex)
            {
               var a =  ex.Message;
            }

            var LeadgerAD = db.Ledgers.Where(x => x.Name == "Admin Drawer").FirstOrDefault();
            int AdminDrawerId = LeadgerAD.Id;
            decimal? CurrentBalanceAD = LeadgerAD.CurrentBalance;

            VoucherRecord voucherRecord1 = new VoucherRecord();
            decimal? AfterBalanceAD = CurrentBalanceAD + Convert.ToDecimal(CashReceived);
            voucherRecord1.LedgerId = AdminDrawerId;
            voucherRecord1.Type = "Dr";
            voucherRecord1.Amount = Convert.ToDecimal(CashReceived);
            voucherRecord1.CurrentBalance = CurrentBalanceAD;

            voucherRecord1.AfterBalance = AfterBalanceAD;
            voucherRecord1.VoucherId = voucher.Id;
            voucherRecord1.Description = "Fee Collected by student  (" + StudentName + ") (" + SessionName + ") ";

            LeadgerAD.CurrentBalance = AfterBalanceAD;
            db.VoucherRecords.Add(voucherRecord1);
            db.SaveChanges();


            if (Discount != 0)
            {
                VoucherRecord voucherRecord3 = new VoucherRecord();

                var LeadgerDiscount = db.Ledgers.Where(x => x.Name == "Discount").FirstOrDefault();

                decimal? CurrentBalanceOfDiscount = LeadgerDiscount.CurrentBalance;
                decimal? AfterBalanceOfDiscount = CurrentBalanceOfDiscount + Math.Round( Convert.ToDecimal(Discount));
                voucherRecord3.LedgerId = LeadgerDiscount.Id;
                voucherRecord3.Type = "Dr";
                voucherRecord3.Amount = Math.Round( Convert.ToDecimal(Discount));
                voucherRecord3.CurrentBalance = CurrentBalanceOfDiscount;
                voucherRecord3.AfterBalance = AfterBalanceOfDiscount;
                voucherRecord3.VoucherId = voucher.Id;
                voucherRecord3.Description = "Discount given to student (" + StudentName + ") (" + SessionName + ")  on payable fee " + FeePayable;
                LeadgerDiscount.CurrentBalance = AfterBalanceOfDiscount;

                db.VoucherRecords.Add(voucherRecord3);
                db.SaveChanges();


            }


            return Json("", JsonRequestBehavior.AllowGet);

        }
    }
}