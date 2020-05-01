using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using System.Threading.Tasks;
using SEA_Application.Models.StudentAssessments;
using Microsoft.AspNet.Identity;

namespace SEA_Application.Controllers
{
    public class StudentRecurringFeesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: StudentRecurringFees
        public ActionResult Index()
        {
            var studentRecurringFees = db.AspNetSessions.ToList();
            return View(db.AspNetSessions.ToList());
        }

        // GET: StudentRecurringFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentRecurringFee studentRecurringFee = db.StudentRecurringFees.Find(id);
            if (studentRecurringFee == null)
            {
                return HttpNotFound();
            }
            return View(studentRecurringFee);
        }

        // GET: StudentRecurringFees/Create

        public ActionResult CheckClassDeplication(int Id)
        {
            string status = "error";

            if (db.StudentRecurringFees.Where(x => x.ClassId == Id).Count() > 0)
            {
                status = "error";
            }
            else
            {
                status = "success";
            }

            return Content(status);
        }
        public ActionResult StudentRecurringFee()
        {
            ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }

        public ActionResult Create()
        {

            //var ClassID = db.StudentRecurringFees.Include(x => x.AspNetClass).FirstOrDefault().ClassId;

            //ViewBag.Class_ID = ClassID;
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName");
            ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }
        public ActionResult AddMultiplier()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");

            return View();
        }
        public ActionResult StudentList(int id)
        {
            var result1 = (from std in db.AspNetStudents
                           join usr in db.AspNetUsers on std.StudentID equals usr.Id
                           where usr.Status != "False" && std.ClassID == id
                           select new { std.Id, usr.Name }).ToList();
            var studentdata = Newtonsoft.Json.JsonConvert.SerializeObject(result1);

            return Content(studentdata);
        }
        public ActionResult SaveMultiplierDetails(int id)
        {
            var feedata = db.StudentFeeMonths.Where(x => x.StudentId == id).FirstOrDefault();
            var installmentamount = feedata.InstalmentAmount * 2;
            var list = db.StudentFeeMonths.Where(x => x.StudentId == id).ToList();
            int i = 2;
            foreach (var items in list)
            {

                StudentFeeMonth data = db.StudentFeeMonths.Where(x => x.Id == items.Id).FirstOrDefault();
                if (i % 2 == 0)
                {
                    data.FeePayable = installmentamount;
                    data.Multiplier = 2;
                }

                else
                {
                    data.FeePayable = 0;
                    data.Multiplier = 0;
                }
                db.SaveChanges();
                i++;
            }
            return RedirectToAction("Index");
        }
        //  AddMultiplier
        // POST: StudentRecurringFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClassId,ComputerLabCharges,SecurityServices,LabCharges,Transportation,SportsShirt,ChineseClassFee,AdvanceTax,Other,TutionFee,TotalFee,SessionId")] StudentRecurringFee studentRecurringFee)
        {
            //  var dbTransaction = db.Database.BeginTransaction();
            try
            {



                if (ModelState.IsValid)
                {
                    db.StudentRecurringFees.Add(studentRecurringFee);
                    db.SaveChanges();
                    //var stdList = db.AspNetStudents.Where(x => x.ClassID == studentRecurringFee.ClassId && x.AspNetUser.Status != "False").ToList().Count();
                    //for (int i = 0; i < stdList; i++)
                    //{
                    //    Ledger ledger = db.Ledgers.Where(x => x.Name == "Student Receivables").FirstOrDefault();
                    //    ledger.StartingBalance += Convert.ToDecimal(studentRecurringFee.TotalFee);
                    //    ledger.CurrentBalance += Convert.ToDecimal(studentRecurringFee.TotalFee);
                    //    db.SaveChanges();

                    //}

                    //for (int i = 0; i < stdList; i++)
                    //{
                    //    Ledger ledger = db.Ledgers.Where(x => x.Name == "Student Fee").FirstOrDefault();
                    //    ledger.StartingBalance += Convert.ToDecimal(studentRecurringFee.TotalFee);
                    //    ledger.CurrentBalance += Convert.ToDecimal(studentRecurringFee.TotalFee);
                    //    db.SaveChanges();

                    //}




                    //multiplier
                    List<AspNetStudent> list = db.AspNetStudents.Where(x => x.ClassID == studentRecurringFee.ClassId && x.AspNetUser.Status != "False").ToList();
                    foreach (var std in list)
                    {
                        var classid = db.AspNetStudents.Where(x => x.Id == std.Id).Select(x => x.ClassID).FirstOrDefault();
                        var totalfee = db.StudentRecurringFees.Where(x => x.ClassId == std.ClassID).FirstOrDefault();
                        var installment = totalfee.TotalFee / 12;
                        string[] Months = { "September", "October", "November", "December", "January", "February", "March", "April", "May", "June", "July", "August" };
                        for (int i = 0; i < Months.Count(); i++)
                        {
                            StudentFeeMonth stdfeemonth = new StudentFeeMonth();
                            stdfeemonth.StudentId = std.Id;
                            stdfeemonth.Multiplier = 1;
                            stdfeemonth.SessionId = studentRecurringFee.SessionId;
                            stdfeemonth.Status = "Pending";
                            stdfeemonth.InstalmentAmount = installment;
                            stdfeemonth.FeePayable = installment;
                            var dddd = DateTime.Now;
                            var d = dddd.ToString("yyyy-MM-dd");
                           //shahzad stdfeemonth.IssueDate = d;
                            stdfeemonth.Months = Months[i];
                            db.StudentFeeMonths.Add(stdfeemonth);
                            //  db.SaveChanges();

                        }
                        //      dbTransaction.Commit();
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                //    dbTransaction.Dispose();
                ViewBag.ErrorMessage = e.Message;
            }

            ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "Name", studentRecurringFee.ClassId);
            return View(studentRecurringFee);
        }
        public JsonResult SaveStudentFee(int cid, string uid, string type)
        {
            string status = "error";
            var student = db.AspNetStudents.Where(x => x.StudentID == uid).FirstOrDefault();
            var isstudent = db.StudentFeeMonths.Where(x => x.StudentId == student.Id).FirstOrDefault();
            if (isstudent == null)
            {
                var totalfee = db.StudentRecurringFees.Where(x => x.ClassId == cid).FirstOrDefault();
                var installment = totalfee.TotalFee;
                installment = totalfee.TotalFee / 2;

                //var totalfee = db.StudentRecurringFees.Where(x => x.ClassId == cid).FirstOrDefault();

                string[] Months = { "September", "October", "November", "December", "January", "February", "March", "April", "May", "June", "July", "August" };
                var month = DateTime.Now.Month;
                for (int i = 0; i < Months.Count(); i++)
                {
                    StudentFeeMonth stdfeemonth = new StudentFeeMonth();
                    stdfeemonth.StudentId = student.Id;
                    stdfeemonth.SessionId = totalfee.SessionId;
                    stdfeemonth.Status = "Pending";
                    stdfeemonth.InstalmentAmount = installment;
                    stdfeemonth.FeePayable = installment;
                    var dddd = DateTime.Now;
                    var d = dddd.ToString("yyyy-MM-dd");
                   //shahzad stdfeemonth.IssueDate = d;
                    stdfeemonth.Months = Months[i];
                    db.StudentFeeMonths.Add(stdfeemonth);
                }
                status = "Success";
                db.SaveChanges();
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(status, JsonRequestBehavior.AllowGet);
            }


        }

        // GET: StudentRecurringFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentRecurringFee studentRecurringFee = db.StudentRecurringFees.Find(id);
            if (studentRecurringFee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "ClassName", studentRecurringFee.ClassId);
            return View(studentRecurringFee);
        }

        // POST: StudentRecurringFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public ActionResult AddData(FeesData data)
        {

            int? sessionId = db.AspNetClasses.Where(x => x.Id == data.ClassId).FirstOrDefault().SessionID;

            if (ModelState.IsValid)
            {
                var TotalFee = db.AspNetSessions.Where(x => x.Id == SessionID).FirstOrDefault().Total_Fee;

                decimal? TotalFeeOfAllStudents = 0;


                var feetype = Request.Form["feeType"];
                IEnumerable<string> selectedstudents = Request.Form["students"].Split(',');
                int count = selectedstudents.Count();

                for (int i = 1; i <= count; i++)
                {
                    TotalFeeOfAllStudents = TotalFee + TotalFeeOfAllStudents;
                }
                StudentRecurringFee studentRecurringFees = new StudentRecurringFee();
                studentRecurringFees.ClassId = data.ClassId;
                studentRecurringFees.SessionId = sessionId;
                studentRecurringFees.TutionFee = TotalFee;
                studentRecurringFees.TotalFee = TotalFee;
                db.StudentRecurringFees.Add(studentRecurringFees);
                db.SaveChanges();

                var Total_Fees = studentRecurringFees.TotalFee;

                if (feetype == "Installment")
                {
                    var CalculatedFee = Total_Fees / 2;
                    foreach (var std in selectedstudents)
                    {

                        AspNetStudent student = db.AspNetStudents.Where(x => x.StudentID == std).FirstOrDefault();

                        var classid = db.AspNetStudents.Where(x => x.Id == student.Id).Select(x => x.ClassID).FirstOrDefault();
                        var Month = DateTime.Now.ToString("MMMM");

                        for (int i = 0; i <= 3; i++)
                        {
                            var FirstMonth = DateTime.Now.ToString("MMMM");
                            StudentFeeMonth stdfeemonth = new StudentFeeMonth();
                            stdfeemonth.StudentId = student.Id;
                            stdfeemonth.Multiplier = 2;
                            stdfeemonth.SessionId = sessionId;
                            stdfeemonth.Status = "Pending";

                            var dddd = DateTime.Now;
                            var d = dddd.ToString("yyyy-MM-dd");
                         //shahzad   stdfeemonth.IssueDate = d;

                            if (i == 0)
                            {
                                stdfeemonth.Months = DateTime.Now.ToString("MMMM");
                                stdfeemonth.InstalmentAmount = CalculatedFee;
                                stdfeemonth.FeePayable = CalculatedFee;
                            }
                            else if (i == 1)
                            {
                                stdfeemonth.InstalmentAmount = 0;
                                stdfeemonth.FeePayable = 0;
                                stdfeemonth.Months = DateTime.Now.Date.AddMonths(1).ToString("MMMM");
                            }
                            else if (i == 2)
                            {
                                stdfeemonth.Months = DateTime.Now.Date.AddMonths(2).ToString("MMMM");
                                stdfeemonth.InstalmentAmount = CalculatedFee;
                                stdfeemonth.FeePayable = CalculatedFee;
                            }
                            else if (i == 3)
                            {
                                stdfeemonth.InstalmentAmount = 0;
                                stdfeemonth.FeePayable = 0;
                                stdfeemonth.Months = DateTime.Now.Date.AddMonths(3).ToString("MMMM");
                            }
                            else
                            {
                                //do nothing
                            }

                            stdfeemonth.FeeType = "Installment";
                            db.StudentFeeMonths.Add(stdfeemonth);
                            db.SaveChanges();

                        }

                    }
                }


                else if (feetype == "PerMonth")
                {
                    foreach (var std in selectedstudents)
                    {
                        var CalculatedFee = Total_Fees / 4;
                        AspNetStudent student = db.AspNetStudents.Where(x => x.StudentID == std).FirstOrDefault();

                        var classid = db.AspNetStudents.Where(x => x.Id == student.Id).Select(x => x.ClassID).FirstOrDefault();
                        var Month = DateTime.Now.ToString("MMMM");

                        for (int i = 0; i <= 3; i++)
                        {
                            var FirstMonth = DateTime.Now.ToString("MMMM");
                            StudentFeeMonth stdfeemonth = new StudentFeeMonth();
                            stdfeemonth.StudentId = student.Id;
                            stdfeemonth.Multiplier = 1;
                            stdfeemonth.SessionId = sessionId;
                            stdfeemonth.Status = "Pending";

                            var dddd = DateTime.Now;
                            var d = dddd.ToString("yyyy-MM-dd");
                          //shahzad  stdfeemonth.IssueDate = d;

                            if (i == 0)
                            {
                                stdfeemonth.Months = DateTime.Now.ToString("MMMM");
                                stdfeemonth.InstalmentAmount = CalculatedFee;
                                stdfeemonth.FeePayable = CalculatedFee;
                            }
                            else
                            {
                                stdfeemonth.InstalmentAmount = CalculatedFee;
                                stdfeemonth.FeePayable = CalculatedFee;
                                stdfeemonth.Months = DateTime.Now.Date.AddMonths(i).ToString("MMMM");
                            }

                            stdfeemonth.FeeType = "PerMonth";
                            db.StudentFeeMonths.Add(stdfeemonth);
                            db.SaveChanges();

                        }

                    }

                }
                else
                {
                    foreach (var std in selectedstudents)
                    {

                        AspNetStudent student = db.AspNetStudents.Where(x => x.StudentID == std).FirstOrDefault();

                        var classid = db.AspNetStudents.Where(x => x.Id == student.Id).Select(x => x.ClassID).FirstOrDefault();
                        var Month = DateTime.Now.ToString("MMMM");

                        for (int i = 0; i <= 3; i++)
                        {
                            var FirstMonth = DateTime.Now.ToString("MMMM");
                            StudentFeeMonth stdfeemonth = new StudentFeeMonth();
                            stdfeemonth.StudentId = student.Id;
                            stdfeemonth.Multiplier = 4;
                            stdfeemonth.SessionId = sessionId; //could be change To SessionID
                            stdfeemonth.Status = "Pending";

                            var dddd = DateTime.Now;
                            var d = dddd.ToString("yyyy-MM-dd");
                          //shahzad  stdfeemonth.IssueDate = d;

                            if (i == 0)
                            {
                                stdfeemonth.Months = DateTime.Now.ToString("MMMM");
                                stdfeemonth.InstalmentAmount = Total_Fees;
                                stdfeemonth.FeePayable = Total_Fees;
                            }
                            else
                            {
                                stdfeemonth.InstalmentAmount = 0;
                                stdfeemonth.FeePayable = 0;
                                stdfeemonth.Months = DateTime.Now.Date.AddMonths(i).ToString("MMMM");
                            }

                            stdfeemonth.FeeType = "Lumsum";
                            db.StudentFeeMonths.Add(stdfeemonth);
                            db.SaveChanges();

                        }

                    }
                }

                var id = User.Identity.GetUserId();
                var username = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();

                Voucher voucher = new Voucher();
                voucher.Name = "Student Fee Created";
                voucher.Notes = "Student Fee Created";
                voucher.Date = DateTime.Now;
                voucher.Name = username;
                voucher.CreatedBy = username;
                voucher.SessionID = sessionId;
                int? VoucherObj = db.Vouchers.Max(x => x.VoucherNo);

                voucher.VoucherNo = Convert.ToInt32(VoucherObj) + 1;
                db.Vouchers.Add(voucher);
                db.SaveChanges();

                var Leadger = db.Ledgers.Where(x => x.Name == "Account Receiveable").FirstOrDefault();

                int AccountRecId = Leadger.Id;
                decimal? CurrentBalance = Leadger.CurrentBalance;
                VoucherRecord voucherRecord = new VoucherRecord();
                decimal? AfterBalance = CurrentBalance + TotalFeeOfAllStudents;
                voucherRecord.LedgerId = AccountRecId;
                voucherRecord.Type = "Dr";
                voucherRecord.Amount = TotalFeeOfAllStudents;
                voucherRecord.CurrentBalance = CurrentBalance;
                voucherRecord.AfterBalance = AfterBalance;
                voucherRecord.VoucherId = voucher.Id;
                voucherRecord.Description = "Student Fee debited in Account Receiveable";
                Leadger.CurrentBalance = AfterBalance;
                db.VoucherRecords.Add(voucherRecord);
                db.SaveChanges();

                //Second;

                VoucherRecord voucherRecord1 = new VoucherRecord();

                var LeadgerStudentFee = db.Ledgers.Where(x => x.Name == "Student Fee").FirstOrDefault();

                decimal? CurrentBalanceOfStudentFee = LeadgerStudentFee.CurrentBalance;
                decimal? AfterBalanceOfStudentFee = CurrentBalanceOfStudentFee + TotalFeeOfAllStudents;
                voucherRecord1.LedgerId = LeadgerStudentFee.Id;
                voucherRecord1.Type = "Cr";
                voucherRecord1.Amount = TotalFeeOfAllStudents;
                voucherRecord1.CurrentBalance = CurrentBalanceOfStudentFee;
                voucherRecord1.AfterBalance = AfterBalanceOfStudentFee;
                voucherRecord1.VoucherId = voucher.Id;
                voucherRecord1.Description = "Student Fee Credit in Income";
                LeadgerStudentFee.CurrentBalance = AfterBalanceOfStudentFee;

                db.VoucherRecords.Add(voucherRecord1);
                db.SaveChanges();


                // db.SaveChanges();



            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassId,ComputerLabCharges,SecurityServices,LabCharges,Transportation,SportsShirt,ChineseClassFee,AdvanceTax,Other,TutionFee,TotalFee")] StudentRecurringFee studentRecurringFee)
        {

            if (ModelState.IsValid)
            {
                db.Entry(studentRecurringFee).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {



                    List<AspNetStudent> list = db.AspNetStudents.Where(x => x.ClassID == studentRecurringFee.ClassId && x.AspNetUser.Status != "False").ToList();
                    foreach (var std in list)
                    {

                        var tdfee = db.StudentFeeMonths.Where(x => x.StudentId == std.Id && x.Status == "Pending").ToList();
                        foreach (var item in tdfee)
                        {
                            StudentFeeMonth student = db.StudentFeeMonths.Where(x => x.Id == item.Id).FirstOrDefault();
                            db.StudentFeeMonths.Remove(student);
                            db.SaveChanges();
                        }

                        var classid = db.AspNetStudents.Where(x => x.Id == std.Id).Select(x => x.ClassID).FirstOrDefault();
                        var totalfee = db.StudentRecurringFees.Where(x => x.ClassId == std.ClassID).FirstOrDefault();
                        var installment = totalfee.TotalFee / 12;
                        string[] Months = { "September", "October", "November", "December", "January", "February", "March", "April", "May", "June", "July", "August" };
                        for (int i = 0; i < Months.Count(); i++)
                        {
                            StudentFeeMonth stdfeemonth = new StudentFeeMonth();
                            stdfeemonth.StudentId = std.Id;
                            stdfeemonth.Status = "Pending";
                            stdfeemonth.InstalmentAmount = installment;
                            stdfeemonth.SessionId = studentRecurringFee.SessionId;
                            stdfeemonth.FeePayable = installment;
                            var dddd = DateTime.Now;
                            var d = dddd.ToString("yyyy-MM-dd");
                          //shahzad  stdfeemonth.IssueDate = d;
                            stdfeemonth.Months = Months[i];
                            db.StudentFeeMonths.Add(stdfeemonth);
                        }
                        db.SaveChanges();
                    }

                }



                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "Name", studentRecurringFee.ClassId);
            return View(studentRecurringFee);
        }

        // GET: StudentRecurringFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentRecurringFee studentRecurringFee = db.StudentRecurringFees.Find(id);
            if (studentRecurringFee == null)
            {
                return HttpNotFound();
            }
            return View(studentRecurringFee);
        }

        // POST: StudentRecurringFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentRecurringFee studentRecurringFee = db.StudentRecurringFees.Find(id);
            db.StudentRecurringFees.Remove(studentRecurringFee);
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

        public ActionResult AddFee()
        {
            //var ClassID = db.StudentRecurringFees.Include(x => x.AspNetClass).FirstOrDefault().ClassId;

            //ViewBag.Class_ID = ClassID;
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName");
            //  ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");

            return View();
        }
        public ActionResult StudentByClass(int id)
        {
            var studentList = db.AspNetStudents.Where(x => x.AspNetStudent_Session_class.Any(y => y.ClassID == id)).Select(x => x.Id).ToList();
            var result1 = (from std in db.AspNetStudents
                           join usr in db.AspNetUsers on std.StudentID equals usr.Id
                           where usr.Status != "False" && std.ClassID == id && !db.StudentFeeMonths.Any(m => m.StudentId == std.Id)
                           select new { usr.Id, usr.Name, usr.PhoneNumber, usr.Email, usr.UserName, std.AspNetClass.ClassName }).ToList();
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

    }
}
