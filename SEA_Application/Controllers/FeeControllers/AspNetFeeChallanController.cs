//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SEA_Application.Models;
//using System.IO;
//using System.Web.UI;
//using System.Text;
//using System.IO;
//using System.Text;
//using System.Data;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;

//namespace SEA_Application.Controllers.FeeControllers
//{
//    public class AspNetFeeChallanController : Controller
//    {
//        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

//        // GET: AspNetFeeChallan
//        public ActionResult Index()
//        {
//            //var aspNetFeeChallans = db.AspNetFeeChallans.Include(a => a.AspNetClass).Include(a => a.AspNetDurationType);
//            //return View(aspNetFeeChallans);

//            return RedirectToAction("Challan_Form", "FeeManagement");
//        }

//        // GET: AspNetFeeChallan/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFeeChallan aspNetFeeChallan = db.AspNetFeeChallans.Find(id);
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            ViewBag.DurationTypeID = new SelectList(db.AspNetDurationTypes, "Id", "TypeName");
//            if (aspNetFeeChallan == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFeeChallan);
//        }

//        // GET: AspNetFeeChallan/Create
//        public ActionResult Create()
//        {
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            ViewBag.DurationTypeID = new SelectList(db.AspNetDurationTypes, "Id", "TypeName");
//            return View();
//        }



//        public List<string> StudentFeeDetailHeads = new List<string>();
        
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(AspNetFeeChallan aspNetFeeChallan)
//        {

//            AspNetFeeChallan PreviousChallan = db.AspNetFeeChallans.Where(x => x.ClassID == aspNetFeeChallan.ClassID).OrderByDescending(x => x.Id).FirstOrDefault();

//            int[] FeeTypeIncluded = { } ;
//            try{FeeTypeIncluded=Request.Form["onetimefee"].ToString().Split(',').Select(int.Parse).ToArray();}catch{}

//            StudentFeeDetailHeads.Add("Student Name");
//            StudentFeeDetailHeads.Add("Student Reg No");
//            List<string> FeeBreakdownHeads = db.AspNetClass_FeeType.Where(x => x.ClassID == aspNetFeeChallan.ClassID).Select(x => x.AspNetFinanceLedger.Name).ToList();
//            foreach(var item in FeeBreakdownHeads)
//            {
//                StudentFeeDetailHeads.Add(item);
//            }
//            StudentFeeDetailHeads.Add("Previous Fee");
//            StudentFeeDetailHeads.Add("Fine");
//            StudentFeeDetailHeads.Add("Discount");
//            StudentFeeDetailHeads.Add("Total Fee");


//            var Students = (from aspNetStudents in db.AspNetStudents
//                            where aspNetStudents.ClassID == aspNetFeeChallan.ClassID
//                            select new {aspNetStudents.AspNetUser.Id, aspNetStudents.AspNetUser.UserName, aspNetStudents.AspNetUser.Name }).ToList();

//            List<StudentFeeDetail> studentFeeDetail = new List<StudentFeeDetail>();
//            var FeeBreakdown = (from aspNetClass_Feetype in db.AspNetClass_FeeType
//                                where aspNetClass_Feetype.ClassID == aspNetFeeChallan.ClassID
//                                select new { aspNetClass_Feetype.Id, aspNetClass_Feetype.LedgerID, aspNetClass_Feetype.Type, aspNetClass_Feetype.Amount }).ToList();

//            List<AspNetClass_FeeType> Feetypes = new List<AspNetClass_FeeType>();
//            var Duration = db.AspNetDurationTypes.Where(x => x.Id == aspNetFeeChallan.DurationTypeID).Select(x => x).FirstOrDefault();

//            foreach (var item in FeeBreakdown)
//            {
//                AspNetClass_FeeType temp = new AspNetClass_FeeType();
//                temp.Id = item.Id;
//                temp.LedgerID = item.LedgerID;
//                temp.Type = item.Type;
                

//                if (!FeeTypeIncluded.Contains(item.Id) && item.Type == "One Time")
//                {
//                    temp.Amount = 0;
//                }
//                else
//                {
//                    temp.Amount = item.Amount; 
//                }

//                if (item.Type == "Continue")
//                {
//                    temp.Amount = item.Amount * Duration.MonthsDuration;
//                }
//                Feetypes.Add(temp);
//            }

//            int? studentFine = 0;
//            foreach (var item in Students)
//            {
//                if(PreviousChallan!=null)
//                {
//                    AspNetStudent_Payment studentPayment = db.AspNetStudent_Payment.Where(x => x.FeeChallanID == PreviousChallan.Id).Select(x => x).FirstOrDefault();
//                    int? LedgerID = db.AspNetFinanceLedgers.Where(x => x.Name == "Penalty").Select(x => x.Id).FirstOrDefault();
//                    if (studentPayment.Status == "Not Submitted")
//                    {
//                        TimeSpan difference = DateTime.Now - Convert.ToDateTime(PreviousChallan.DueDate);
//                        int days = difference.Days;
//                        int Penalty = days * Convert.ToInt32(PreviousChallan.Penalty);
//                        if (days > 0)
//                        {
//                            AspNetStudent_PaymentDetail aspNetStudentPaymentDetail = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == studentPayment.Id && x.LedgerID== LedgerID).FirstOrDefault();
//                            if(aspNetStudentPaymentDetail==null)
//                            {
//                                AspNetStudent_PaymentDetail student_payment_detail = new AspNetStudent_PaymentDetail();
//                                student_payment_detail.Student_PaymentID = studentPayment.Id;

//                                student_payment_detail.LedgerID = LedgerID;
//                                student_payment_detail.Amount = Penalty;
//                                db.AspNetStudent_PaymentDetail.Add(student_payment_detail);
//                                db.SaveChanges();
//                            }
//                            else
//                            {
//                                aspNetStudentPaymentDetail.Amount = Penalty;
//                                db.SaveChanges();
//                            }
//                        }
//                        else
//                        {
//                            AspNetStudent_PaymentDetail aspNetStudentPaymentDetail = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == studentPayment.Id && x.LedgerID == LedgerID).FirstOrDefault();
//                            if(aspNetStudentPaymentDetail != null)
//                            {
//                                aspNetStudentPaymentDetail.Amount = 0;
//                                db.SaveChanges();
//                            }
//                        }
//                        studentPayment.PaymentAmount = db.AspNetStudent_PaymentDetail.Where(x => x.Student_PaymentID == studentPayment.Id).Sum(x => x.Amount);
//                        db.SaveChanges();
//                    }
                   
//                }
               
//                StudentFeeDetail studentDetail = new StudentFeeDetail();
//                studentDetail.StudentId = item.Id;
//                studentDetail.StudentName = item.Name;
//                studentDetail.StudentRegNo = item.UserName;
//                studentDetail.FeeBreakdown = new List<AspNetClass_FeeType>();

//                /* Previous Fee Adjusment*/
//                var PreviousFeetemp = (from aspNetStudent_Payment in db.AspNetStudent_Payment
//                                              where aspNetStudent_Payment.Status == "Not Submitted" && aspNetStudent_Payment.StudentID == item.Id
//                                              select new { aspNetStudent_Payment.Id, aspNetStudent_Payment.AspNetFeeChallan.Title }).ToList();

//                studentDetail.PreviousFee = new List<Previous>();
//                foreach(var previous in PreviousFeetemp)
//                {
//                    Previous previousfee = new Previous();
//                    previousfee.Id = previous.Id;
//                    previousfee.Title = previous.Title;
//                    studentDetail.PreviousFee.Add(previousfee);
//                }
               

//                int? discountSum = db.AspNetStudent_Discount.Where(x => x.StudentID ==item.Id ).Sum(x => x.Percentage);
//                if (discountSum == null)
//                {
//                    discountSum = 0;
//                }
//                var DiscountApplicableIDs = db.AspNetStudent_Discount_Applicable.Where(x => x.StudentId == item.Id).Select(x => x.ClassFeeTypeId).ToList();

                
//                foreach (var feebreakdown in Feetypes)
//                {
//                    AspNetClass_FeeType aspNetClassFeeType = new AspNetClass_FeeType();
//                    aspNetClassFeeType.Id = feebreakdown.Id;
//                    aspNetClassFeeType.LedgerID = feebreakdown.LedgerID;
//                    aspNetClassFeeType.Type = feebreakdown.Type;
//                    if(DiscountApplicableIDs.Contains(feebreakdown.Id))
//                    {
//                        aspNetClassFeeType.Amount = (feebreakdown.Amount - ((feebreakdown.Amount * discountSum) / 100));
//                    }
//                    else
//                    {
//                        aspNetClassFeeType.Amount = feebreakdown.Amount;
//                    }
//                    studentDetail.FeeBreakdown.Add(aspNetClassFeeType);
//                }

//                int? FineLedgerID = db.AspNetFinanceLedgers.Where(x => x.Name == "Fine").Select(x => x.Id).FirstOrDefault();
//                if (PreviousChallan != null)
//                {
//                    studentFine = db.AspNetStudent_Fine.Where(x => x.StudentID == item.Id && x.Status == "Not Submitted" && x.Date > PreviousChallan.Created && x.Date<DateTime.Now).Sum(x => x.Amount);
//                }
//                else
//                {
//                    studentFine = db.AspNetStudent_Fine.Where(x => x.StudentID == item.Id && x.Status == "Not Submitted" && x.Date < DateTime.Now).Sum(x => x.Amount);
//                }
//                if (studentFine == null)
//                {
//                    studentFine = 0;
//                }
//                studentDetail.Fine = Convert.ToInt32(studentFine);
//                studentDetail.Discount = Convert.ToInt32(discountSum);
//                studentDetail.GrossFee = Convert.ToInt32(studentDetail.FeeBreakdown.Sum(x => x.Amount));
//                studentDetail.TotalFee = Convert.ToInt32(studentDetail.FeeBreakdown.Sum(x => x.Amount));
//                studentDetail.TotalFee += Convert.ToInt32(studentFine);
//                studentFeeDetail.Add(studentDetail);
//            }
//            ViewBag.Heads = StudentFeeDetailHeads;
//            ViewBag.ClassTitle = "Class: " + db.AspNetClasses.Where(x => x.Id == aspNetFeeChallan.ClassID).Select(x => x.ClassName).FirstOrDefault();
//            Session["AspNetFeeChallan"] = aspNetFeeChallan;

//            return View("FeePreview", studentFeeDetail);
//        }

//        public JsonResult SaveChallanFee(List<StudentFeeDetail> StudentFeeDetails)
//        {

//            var dbContextTransaction = db.Database.BeginTransaction();

//            try
//            {
//                AspNetFeeChallan aspNetFeeChallan = (AspNetFeeChallan)Session["AspNetFeeChallan"];
//                aspNetFeeChallan.Created = DateTime.Now;
//                db.AspNetFeeChallans.Add(aspNetFeeChallan);
//                db.SaveChanges();
//                int FeeChallanNo = db.AspNetFeeChallans.Max(x => x.Id);

//                foreach (var StudentFee in StudentFeeDetails)
//                {
//                    AspNetStudent_Payment studentPayment = new AspNetStudent_Payment();
//                    studentPayment.FeeChallanID = FeeChallanNo;
//                    studentPayment.StudentID = StudentFee.StudentId;
//                    studentPayment.Status = "Not Submitted";
//                    studentPayment.TotalAmount = aspNetFeeChallan.TotalAmount;
//                    studentPayment.PaymentAmount = StudentFee.TotalFee;
//                    db.AspNetStudent_Payment.Add(studentPayment);
//                    db.SaveChanges();
//                    int StudentPaymentID = db.AspNetStudent_Payment.Max(x => x.Id);

//                    if (StudentFee.FeeBreakdown != null)
//                        foreach (var breakdown in StudentFee.FeeBreakdown)
//                        {
//                            AspNetStudent_PaymentDetail studentPaymentDetail = new AspNetStudent_PaymentDetail();
//                            if (breakdown.Amount > 0)
//                            {
//                                int? LedgerID = db.AspNetClass_FeeType.Where(x => x.Id == breakdown.Id).Select(x => x.LedgerID).FirstOrDefault();
//                                studentPaymentDetail.LedgerID = LedgerID;
//                                studentPaymentDetail.Student_PaymentID = studentPayment.Id;
//                                studentPaymentDetail.Amount = breakdown.Amount;
//                                db.AspNetStudent_PaymentDetail.Add(studentPaymentDetail);
//                                db.SaveChanges();
//                            }

//                        }
//                    if (StudentFee.PreviousFee != null)
//                    {
//                        foreach (var previousFee in StudentFee.PreviousFee)
//                        {
//                            AspNetStudent_PaymentDetail studentPaymentDetail = new AspNetStudent_PaymentDetail();
//                            var FeeAmount = db.AspNetStudent_Payment.Where(x => x.Id == previousFee.Id).Select(x => x.PaymentAmount).FirstOrDefault();
//                            int? LedgerID = db.AspNetFinanceLedgers.Where(x => x.Name == "Previous Fee").Select(x => x.Id).FirstOrDefault();
//                            studentPaymentDetail.LedgerID = LedgerID;
//                            studentPaymentDetail.Student_PaymentID = studentPayment.Id;
//                            studentPaymentDetail.Amount = FeeAmount;
//                            studentPaymentDetail.PreviousFeeID = previousFee.Id;
//                            db.AspNetStudent_PaymentDetail.Add(studentPaymentDetail);
//                            db.SaveChanges();
//                        }
//                    }
//                    if (StudentFee.Fine > 0)
//                    {
//                        AspNetStudent_PaymentDetail studentPaymentDetail = new AspNetStudent_PaymentDetail();
//                        int? LedgerID = db.AspNetFinanceLedgers.Where(x => x.Name == "Fine").Select(x => x.Id).FirstOrDefault();
//                        studentPaymentDetail.LedgerID = LedgerID;
//                        studentPaymentDetail.LedgerID = LedgerID;
//                        studentPaymentDetail.Student_PaymentID = studentPayment.Id;
//                        studentPaymentDetail.Amount = StudentFee.Fine;
//                        db.AspNetStudent_PaymentDetail.Add(studentPaymentDetail);
//                        db.SaveChanges();
//                    }

//                }
//                dbContextTransaction.Commit();
//                return Json("True", JsonRequestBehavior.AllowGet);
//            }
//            catch
//            {
//                dbContextTransaction.Dispose();
//                return Json("False", JsonRequestBehavior.AllowGet);

//            }

//        }

//        public JsonResult PrintChallanForm(string StudentId)
//        {
//            var amount = "";
//            AspNetStudent_Payment aspNetStudentPayment = db.AspNetStudent_Payment.Where(x => x.StudentID == StudentId).LastOrDefault();
//            return Json(amount, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult GetPreviousFee(string[] PaymentIDs)
//        {
//            int? amount = 0;
             
//            try
//            {
//                List<int> paymentIDs = PaymentIDs.Select(int.Parse).ToList();
//                amount = db.AspNetStudent_Payment.Where(x => paymentIDs.Contains(x.Id)).Sum(x => x.PaymentAmount);
//            }
//            catch { amount = 0; }
//            return Json(amount, JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult GetFeeAmount(int FeeTypeID,string StudentID)
//        {
//            int? amount = 0;
//            var StudentDiscount = db.AspNetStudent_Discount.Where(x => x.StudentID == StudentID).Sum(x => x.Percentage);
//            var StudentDiscountApplicableCheck = db.AspNetStudent_Discount_Applicable.Where(x => x.StudentId == StudentID && x.ClassFeeTypeId == FeeTypeID).FirstOrDefault();
//            amount = db.AspNetClass_FeeType.Where(x => x.Id == FeeTypeID).Select(x => x.Amount).FirstOrDefault();
//            if (StudentDiscountApplicableCheck!=null)
//            {
//                amount = (amount - ((amount * StudentDiscount) / 100));
//            }
//            return Json(amount, JsonRequestBehavior.AllowGet);
//        }

//        // POST: AspNetFeeChallan/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

       
//        // GET: AspNetFeeChallan/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFeeChallan aspNetFeeChallan = db.AspNetFeeChallans.Find(id);
//            if (aspNetFeeChallan == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetFeeChallan.ClassID);
//            ViewBag.DurationTypeID = new SelectList(db.AspNetDurationTypes, "Id", "TypeName", aspNetFeeChallan.DurationTypeID);
//            return View(aspNetFeeChallan);
//        }

//        // POST: AspNetFeeChallan/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(AspNetFeeChallan aspNetFeeChallan)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aspNetFeeChallan).State = EntityState.Modified;
//                db.SaveChanges();

//                //var student_payments = (from studentpayment in db.AspNetStudent_Payment
//                //                        where studentpayment.FeeChallanID == aspNetFeeChallan.Id
//                //                        select studentpayment).ToList();
//                //var studentspayments = getStudentPayAbleFee(aspNetFeeChallan);
//                //int x = 0;
//                //foreach (var item in student_payments)
//                //{
//                //    AspNetStudent_Payment studentpayment = (from student_payment in db.AspNetStudent_Payment
//                //                                            where student_payment.Id == item.Id
//                //                                            select student_payment).SingleOrDefault();

//                //    studentpayment.FeeChallanID = item.FeeChallanID;
//                //    studentpayment.Id = item.Id;
//                //    if (item.PaymentDate == null)
//                //    {
//                //        studentpayment.PaymentAmount = studentspayments[x].PaymentAmount;
//                //    }
//                //    else
//                //    {
//                //        studentpayment.PaymentAmount = studentspayments[x].PaymentAmount - item.PaymentAmount;
//                //        if (studentpayment.PaymentAmount < 0)
//                //        {
//                //            studentpayment.PaymentAmount = studentpayment.PaymentAmount * (-1);
//                //        }
//                //        studentpayment.PaymentDate = null;
//                //    }
//                //    studentpayment.StudentID = item.StudentID;
//                //    x++;
//                //    db.SaveChanges();
//                //}
//                return RedirectToAction("Index");
//            }
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetFeeChallan.ClassID);
//            ViewBag.DurationTypeID = new SelectList(db.AspNetDurationTypes, "Id", "TypeName", aspNetFeeChallan.DurationTypeID);
//            return View(aspNetFeeChallan);
//        }


//        // GET: AspNetFeeChallan/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFeeChallan aspNetFeeChallan = db.AspNetFeeChallans.Find(id);
//            if (aspNetFeeChallan == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFeeChallan);
//        }

//        // POST: AspNetFeeChallan/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            AspNetFeeChallan aspNetFeeChallan = db.AspNetFeeChallans.Find(id);
//            db.AspNetFeeChallans.Remove(aspNetFeeChallan);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        public class fee_challan
//        {
//            public int? amount { get; set; }
//            public DateTime startDate { get; set; }
//            public DateTime endDate { get; set; }
//        }

//        public JsonResult ChallanByDuration(int classId, int durationId)
//        {
//            var duration = db.AspNetDurationTypes.Where(x => x.Id == durationId).Select(x => x).FirstOrDefault();
//            int? amount = (from class_fee in db.AspNetClass_FeeType
//                           where class_fee.ClassID == classId && class_fee.Type != "One Time"
//                           select class_fee.Amount).Sum();
//            DateTime startDate = Convert.ToDateTime(db.AspNetFeeChallans.Where(x => x.ClassID == classId).OrderByDescending(x => x.Id).Select(x => x.EndDate).FirstOrDefault());
//            if (startDate == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
//            {
//                startDate = Convert.ToDateTime(db.AspNetSessions.OrderByDescending(x => x.Id).Select(x => x.SessionStartDate).FirstOrDefault());
//            }
//            DateTime endDate = startDate.AddMonths(Convert.ToInt32(duration.MonthsDuration));
//            amount = amount * duration.MonthsDuration;
//            fee_challan challan = new fee_challan();
//            challan.amount = amount;
//            challan.endDate = endDate;
//            challan.startDate = startDate;
//            return Json(challan, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult GetOneTimeFeeTotal(List<int?> FeeIds)
//        {
//            int? amount = 0;
//            if (FeeIds == null)
//            {
//                amount = 0;
//            }
//            else
//            {
//                amount = (from class_fee in db.AspNetClass_FeeType
//                          where FeeIds.Contains(class_fee.Id)
//                          select class_fee.Amount).Sum();
//            }
//            return Json(amount, JsonRequestBehavior.AllowGet);
//        }


//        public JsonResult GetOneTimeFee(int classId)
//        {
//            var FeeTypeList = (from AspNetClass_Feetyp in db.AspNetClass_FeeType
//                               where AspNetClass_Feetyp.Type == "One Time"
//                               select new { AspNetClass_Feetyp.Id, AspNetClass_Feetyp.AspNetFinanceLedger.Name }).ToList();
//            return Json(FeeTypeList, JsonRequestBehavior.AllowGet);
//        }
        
//    }
//}
