using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspNetNotesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: AspNetNotes
        public ActionResult Index()
        {
            var aspNetNotes = db.AspNetNotes.Include(a => a.AspNetSubject);
            return View(aspNetNotes.ToList());
        }

        public ActionResult ApproveOrders(int OrderId, string OrderType)
        {

            AspNetOrder OrderToModify = db.AspNetOrders.Where(x => x.Id == OrderId).FirstOrDefault();
            OrderToModify.Status = "Paid";


            db.Entry(OrderToModify).State = EntityState.Modified;
            db.SaveChanges();


            List<AspNetNotesOrder> NotesOrderToModify = db.AspNetNotesOrders.Where(x => x.OrderId == OrderId).ToList();

            foreach (var NotesOrder in NotesOrderToModify)
            {
                NotesOrder.Status = "Paid";

                db.Entry(NotesOrder).State = EntityState.Modified;

                db.SaveChanges();

            }
            var result = from Notes in db.AspNetNotes
                         join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID
                         join Order in db.AspNetOrders on OrderNotes.OrderId equals Order.Id
                         join Student in db.AspNetStudents on OrderNotes.StudentID equals Student.Id
                         join User in db.AspNetUsers on Student.StudentID equals User.Id
                         where Order.Id == OrderId
                         select new
                         {
                             classId = Student.ClassID,
                             Title = Notes.Title,
                             Quantity = OrderNotes.Quantity,
                             GrandTotal = Notes.GrandTotal,
                             PhotoCopier = Notes.PhotoCopierPrice,
                             NameOfStudent = User.Name,
                             StudentId = Student.Id,
                             
                             TotalPhotoCopierPrice = (Notes.PhotoCopierPrice * OrderNotes.Quantity)
                         };

            var NameOfStudent = result.FirstOrDefault().NameOfStudent;
            var classIdd = result.FirstOrDefault().classId;
            var SessionIdd =  db.AspNetClasses.Where(x => x.Id == classIdd).FirstOrDefault().SessionID;
            var SessionName = db.AspNetSessions.Where(x => x.Id == SessionIdd).FirstOrDefault().SessionName;
                        
            double TotalPriceOfPhotoCopier = 0;

            foreach (var item in result)
            {
                TotalPriceOfPhotoCopier = Convert.ToDouble(TotalPriceOfPhotoCopier + item.TotalPhotoCopierPrice);
            }

            var id = User.Identity.GetUserId();
            var username = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
            Voucher voucher = new Voucher();

            voucher.Name = "Notes paid by Student " + NameOfStudent +" Session Name "+SessionName;
            voucher.Notes = "Cash received for notes";
            voucher.Date = GetLocalDateTime.GetLocalDateTimeFunction();
            voucher.StudentId = result.FirstOrDefault().StudentId;

            voucher.CreatedBy = username;
            voucher.SessionID = SessionID;
            int? VoucherObj = db.Vouchers.Max(x => x.VoucherNo);

            voucher.VoucherNo = Convert.ToInt32(VoucherObj) + 1;
            db.Vouchers.Add(voucher);
            db.SaveChanges();

            if (OrderType == "Postpaid")
            {

                var Leadger = db.Ledgers.Where(x => x.Name == "Admin Drawer").FirstOrDefault();
                int AdminDrawerId = Leadger.Id;
                decimal? CurrentBalance = Leadger.CurrentBalance;
                VoucherRecord voucherRecord = new VoucherRecord();
                decimal? AfterBalance = CurrentBalance + OrderToModify.TotalAmount;
                voucherRecord.LedgerId = AdminDrawerId;
                voucherRecord.Type = "Dr";
                voucherRecord.Amount = OrderToModify.TotalAmount;
                voucherRecord.CurrentBalance = CurrentBalance;

                voucherRecord.AfterBalance = AfterBalance;
                voucherRecord.VoucherId = voucher.Id;
                voucherRecord.Description = "Notes paid by student (" + NameOfStudent + ") (" + SessionName + ")";
                Leadger.CurrentBalance = AfterBalance;
                db.VoucherRecords.Add(voucherRecord);
                db.SaveChanges();

                VoucherRecord voucherRecord1 = new VoucherRecord();

                var LeadgerNotes = db.Ledgers.Where(x => x.Name == "Notes").FirstOrDefault();

                decimal? CurrentBalanceOfNotes = LeadgerNotes.CurrentBalance;
                decimal? AfterBalanceOfNotes = CurrentBalanceOfNotes + OrderToModify.TotalAmount;
                voucherRecord1.LedgerId = LeadgerNotes.Id;
                voucherRecord1.Type = "Cr";
                voucherRecord1.Amount = OrderToModify.TotalAmount;
                voucherRecord1.CurrentBalance = CurrentBalanceOfNotes;
                voucherRecord1.AfterBalance = AfterBalanceOfNotes;
                voucherRecord1.VoucherId = voucher.Id;
                voucherRecord1.Description = "Notes against order ID " + OrderId;
                LeadgerNotes.CurrentBalance = AfterBalanceOfNotes;

                db.VoucherRecords.Add(voucherRecord1);
                db.SaveChanges();
   
            }//end of if

                VoucherRecord voucherRecord2 = new VoucherRecord();

                var IdofLedger = from Ledger in db.Ledgers
                                 join LedgerHd in db.LedgerHeads on Ledger.LedgerHeadId equals LedgerHd.Id
                                 where LedgerHd.Name == "Liabilities" && Ledger.Name == "Photocopier"
                                 select new
                                 {
                                     Ledger.Id

                                 };

                int photoCopierId = Convert.ToInt32(IdofLedger.FirstOrDefault().Id);
                var LeadgerPhotoCopierL = db.Ledgers.Where(x => x.Id == photoCopierId).FirstOrDefault();

                decimal? CurrentBalanceOfPhotoCopiter = LeadgerPhotoCopierL.CurrentBalance;
                decimal? AfterBalanceOfPhotoCopier = CurrentBalanceOfPhotoCopiter + Convert.ToDecimal(TotalPriceOfPhotoCopier);
                voucherRecord2.LedgerId = LeadgerPhotoCopierL.Id;
                voucherRecord2.Type = "Cr";
                voucherRecord2.Amount = Convert.ToDecimal(TotalPriceOfPhotoCopier);
                voucherRecord2.CurrentBalance = CurrentBalanceOfPhotoCopiter;
                voucherRecord2.AfterBalance = AfterBalanceOfPhotoCopier;
                voucherRecord2.VoucherId = voucher.Id;
                voucherRecord2.Description = "Notes against order ID "+OrderId;
                LeadgerPhotoCopierL.CurrentBalance = AfterBalanceOfPhotoCopier;
                db.VoucherRecords.Add(voucherRecord2);

                db.SaveChanges();

                VoucherRecord voucherRecord3 = new VoucherRecord();

                var IdofLedger1 = from Ledger in db.Ledgers
                                  join LedgerHd in db.LedgerHeads on Ledger.LedgerHeadId equals LedgerHd.Id
                                  where LedgerHd.Name == "Expense" && Ledger.Name == "Photocopier"
                                  select new
                                  {
                                      Ledger.Id
                                  };

                int photoCopierIdOfExpense = Convert.ToInt32(IdofLedger1.FirstOrDefault().Id);
                var LeadgerPhotoCopierE = db.Ledgers.Where(x => x.Id == photoCopierIdOfExpense).FirstOrDefault();

                decimal? CurrentBalanceOfPhotoCopiterE = LeadgerPhotoCopierE.CurrentBalance;
                decimal? AfterBalanceOfPhotoCopierE = CurrentBalanceOfPhotoCopiterE + Convert.ToDecimal(TotalPriceOfPhotoCopier);
                voucherRecord3.LedgerId = LeadgerPhotoCopierE.Id;
                voucherRecord3.Type = "Dr";
                voucherRecord3.Amount = Convert.ToDecimal(TotalPriceOfPhotoCopier);
                voucherRecord3.CurrentBalance = CurrentBalanceOfPhotoCopiterE;
                voucherRecord3.AfterBalance = AfterBalanceOfPhotoCopierE;
                voucherRecord3.VoucherId = voucher.Id;
                voucherRecord3.Description = "Notes against order ID " + OrderId;
                LeadgerPhotoCopierE.CurrentBalance = AfterBalanceOfPhotoCopierE;

                db.VoucherRecords.Add(voucherRecord3);
                db.SaveChanges();

            

            return Json("", JsonRequestBehavior.AllowGet);
        }


        // GET: AspNetNotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetNote aspNetNote = db.AspNetNotes.Find(id);
            if (aspNetNote == null)
            {
                return HttpNotFound();
            }
            return View(aspNetNote);
        }

        public ActionResult RecentOrders()
        {
            //  var CurrentUserId = User.Identity.GetUserId();

            //   int StudentId = db.AspNetStudents.Where(x => x.StudentID == CurrentUserId).FirstOrDefault().Id;

            //var result = from Notes in db.AspNetNotes
            //             join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID

            //             select new
            //             {
            //                 Id = OrderNotes.Id,
            //                 Title = Notes.Title,
            //                 Discription = Notes.Description,
            //                 CourseType = Notes.CourseType,
            //                 Price = Notes.Price,
            //                 Quantity = OrderNotes.Quantity,
            //                 Status = OrderNotes.Status,

            //             };

            var result = db.StudentOrderDetails().ToList();





            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult RecentOrdersDetails(int OrderId)
        {

            var result = from Notes in db.AspNetNotes
                         join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID
                         where OrderNotes.OrderId == OrderId
                         select new
                         {
                             Id = OrderNotes.Id,
                             Title = Notes.Title,
                             Discription = Notes.Description,
                             CourseType = Notes.CourseType,
                             Price = Notes.GrandTotal,
                             Quantity = OrderNotes.Quantity,
                             Status = OrderNotes.Status,
                             Total = Notes.GrandTotal * OrderNotes.Quantity,
                             PhotoCopierPrice = Notes.PhotoCopierPrice * OrderNotes.Quantity,
                             OAPrice =Notes.OAPrice * OrderNotes.Quantity,

                         };



            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: AspNetNotes/Create
        public ActionResult Create()
        {
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");

            return View();
        }

        // POST: AspNetNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,SubjectID,CourseType,NotesType,Price,CreationDate,Pages,PerPagePrice,photoCopierHidden")] AspNetNote aspNetNote)
        {
            if (aspNetNote.NotesType == "Notes")
            {
                aspNetNote.NotesType = Request.Form["NotesType"];

                aspNetNote.CourseType = Request.Form["CourseType"];
                aspNetNote.Pages = Convert.ToDouble(Request.Form["Pages"]);
                aspNetNote.PerPagePrice = Convert.ToDouble(Request.Form["perPagePrice"]);
                aspNetNote.BindingPrice = Convert.ToDouble(Request.Form["bindingPrice"]);
                aspNetNote.GrandTotal = Convert.ToDouble(Request.Form["grandTotalHidden"]);
                aspNetNote.OAPrice = Convert.ToDouble(Request.Form["oAHidden"]);
                aspNetNote.PhotoCopierPrice = Convert.ToDouble(Request.Form["photoCopierHidden"]);
                aspNetNote.CreationDate = DateTime.Now;

                if (ModelState.IsValid)
                {
                    db.AspNetNotes.Add(aspNetNote);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (aspNetNote.NotesType == "Books")
            {

                aspNetNote.NotesType = Request.Form["NotesType"];
                aspNetNote.OABookPercentage = Convert.ToDouble(Request.Form["OABookPercentage"]);
                aspNetNote.BindingPrice = Convert.ToDouble(Request.Form["bindingPrice"]);
                aspNetNote.OAPrice = Convert.ToDouble(Request.Form["oAHidden"]);
                aspNetNote.PhotoCopierPrice = Convert.ToDouble(Request.Form["photoCopierHidden"]);
                aspNetNote.GrandTotal = Convert.ToDouble(Request.Form["grandTotalHidden"]);

                aspNetNote.CourseType = Request.Form["CourseType"];

                aspNetNote.CreationDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.AspNetNotes.Add(aspNetNote);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


            } //else if 
            else
            {
                ModelState.AddModelError("NotesType", "Please Select Notes Type");
            }



            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetNote.SubjectID);
            return View(aspNetNote);
        }

        // GET: AspNetNotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetNote aspNetNote = db.AspNetNotes.Find(id);
            if (aspNetNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetNote.SubjectID);
            return View(aspNetNote);
        }

        // POST: AspNetNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,SubjectID,CourseType,Price,CreationDate,NotesType")] AspNetNote aspNetNote)
        {
            if (aspNetNote.NotesType == "Notes")

            {
                aspNetNote.NotesType = Request.Form["NotesType"];

                aspNetNote.CourseType = Request.Form["CourseType"];
                aspNetNote.Pages = Convert.ToDouble(Request.Form["Pages"]);
                aspNetNote.PerPagePrice = Convert.ToDouble(Request.Form["perPagePrice"]);
                aspNetNote.BindingPrice = Convert.ToDouble(Request.Form["bindingPrice"]);
                aspNetNote.GrandTotal = Convert.ToDouble(Request.Form["grandTotalHidden"]);
                aspNetNote.OAPrice = Convert.ToDouble(Request.Form["oAHidden"]);
                aspNetNote.PhotoCopierPrice = Convert.ToDouble(Request.Form["photoCopierHidden"]);
                aspNetNote.CreationDate = DateTime.Now;

                if (ModelState.IsValid)
                {
                    db.Entry(aspNetNote).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            else if (aspNetNote.NotesType == "Books")
            {
                aspNetNote.NotesType = Request.Form["NotesType"];
                aspNetNote.OABookPercentage = Convert.ToDouble(Request.Form["OABookPercentage"]);
                aspNetNote.BindingPrice = Convert.ToDouble(Request.Form["bindingPrice"]);
                aspNetNote.OAPrice = Convert.ToDouble(Request.Form["oAHidden"]);
                aspNetNote.PhotoCopierPrice = Convert.ToDouble(Request.Form["photoCopierHidden"]);
                aspNetNote.GrandTotal = Convert.ToDouble(Request.Form["grandTotalHidden"]);

                aspNetNote.CourseType = Request.Form["CourseType"];

                aspNetNote.CreationDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.Entry(aspNetNote).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            else
            {
                ModelState.AddModelError("NotesType", "Please Select Notes Type");
            }

            

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetNote.SubjectID);
            return View(aspNetNote);
        }

        // GET: AspNetNotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           }
            AspNetNote aspNetNote = db.AspNetNotes.Find(id);
            if (aspNetNote == null)
            {
                return HttpNotFound();
            }
            return View(aspNetNote);
        }

        // POST: AspNetNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetNote aspNetNote = db.AspNetNotes.Find(id);
            db.AspNetNotes.Remove(aspNetNote);
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
    }
}
