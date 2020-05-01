using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using System.Globalization;
using Microsoft.AspNet.Identity;

namespace SEA_Application.Controllers
{
    public class VouchersController : Controller
    {
        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

        // GET: Vouchers
        //public ActionResult Index()
        //{
        //    return View(db.Vouchers.ToList());
        //}
        public ActionResult Voucher()
        {
            return View();
        }
        public JsonResult GetVoucher()
        {
            var result = (from voucher in db.Vouchers
                          join record in db.VoucherRecords on voucher.Id equals record.VoucherId
                          where voucher.Id == record.VoucherId
                          select new { voucher.Id,voucher.VoucherNo, voucher.Name, record.Description, voucher.Date, record.Amount, record.Type,l_name=record.Ledger.Name }).ToList();
          //  var voucherresult = db.Vouchers.ToList();

            List<Voucher_list> list = new List<Voucher_list>();
            foreach (var item in result)
            {
                Voucher_list vl = new Voucher_list();
                vl.ID = item.Id;
                vl.VoucherNo = item.VoucherNo;
                vl.Name = item.Name;
                vl.Notes = item.Description;
                vl.Date = item.Date;
                vl.Amount = item.Amount;
                vl.LedgerName = item.l_name;
                vl.type = item.Type;
                list.Add(vl);
            }
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        public ActionResult JournalEntry()
        {
            ViewBag.Ledger = new SelectList(db.Ledgers, "Id", "Name");
            return View();
        }
        public JsonResult SelectListLedgers()
        {
            var headlist = db.LedgerHeads.ToList();

            List<HeadList> Head_list = new List<HeadList>();
            foreach (var item in headlist)
            {
                HeadList hl = new HeadList();
                hl.HeadId = item.Id;
                hl.HeadName = item.Name;

                var ledger = db.Ledgers.Where(x => x.LedgerGroup.Name != "Cash" && x.LedgerGroup.Name != "Bank" && x.LedgerHeadId == item.Id).ToList();
                hl.accountlist = new List<AccountsList>();
                foreach (var l_item in ledger)
                {
                    AccountsList Ledger = new AccountsList();
                    Ledger.Id = l_item.Id;
                    Ledger.Name = l_item.Name;
                    hl.accountlist.Add(Ledger);
                }
                Head_list.Add(hl);
            }
            return Json(Head_list, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CheckaccountHead(int id)
        {
            var flag = false;
            var headName = db.Ledgers.Where(x => x.Id == id).Select(x => x.LedgerHead.Name).FirstOrDefault();
            if(headName=="Income" || headName== "Expense")
            {
                flag = true;
            }
            return Json(flag,JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelectListBranch()
        {
            //var branch = db.AspNetBranches.ToList();
            //List<AspNetBranch> branchlist = new List<AspNetBranch>();
            //foreach (var item in branch)
            //{
            //    AspNetBranch b = new AspNetBranch();
            //    b.Id = item.Id;
            //    b.Name = item.Name;
            //    branchlist.Add(b);
            //}
            return Json("", JsonRequestBehavior.AllowGet);
        }
       // GET: Vouchers/Details/5
        public ActionResult AddVoucher(_Voucher Vouchers)
        {
            try
            {
          
                string[] D4 = Vouchers.Time.Split('-');
                Vouchers.Time = D4[2] + "/" + D4[1] + "/" + D4[0];
                Voucher v = new Voucher();

                var localtime = DateTime.Now.ToLocalTime().ToString();
                var time = localtime.Split(' ');
                var timestamps = time[1].Split(':');
                if (timestamps[0].Count() == 1)
                {
                    timestamps[0] = "0" + timestamps[0];
                }
                if (timestamps[1].Count() == 1)
                {
                    timestamps[1] = "0" + timestamps[1];
                }
                if (timestamps[2].Count() == 1)
                {
                    timestamps[2] = "0" + timestamps[2];
                }
                time[1] = timestamps[0] + ":" + timestamps[1] + ":" + timestamps[2];

                var date = Vouchers.Time + " " + time[1] + " " + time[2];

                DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                v.Date = dt;
                v.VoucherNo = Vouchers.VoucherNo;
                v.Notes = Vouchers.Narration;
                v.Name = Vouchers.VoucherName;
                var id = User.Identity.GetUserId();
                var username = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
                v.CreatedBy = username;
                db.Vouchers.Add(v);
                db.SaveChanges();

                foreach (var item in Vouchers.VoucherDetail)
                {
                    VoucherRecord voucherrecord = new VoucherRecord();

                    var ledgerid = Int32.Parse(item.Code);
                    voucherrecord.LedgerId = ledgerid;
                    voucherrecord.VoucherId = db.Vouchers.Select(x => x.Id).Max();
                    var debit = item.Debit;
                    var credit = item.Credit;
                    var amount = 0;
                    if (debit != "" && debit != null)
                    {
                        amount = Int32.Parse(debit);
                        voucherrecord.Type = "Dr";
                    }
                    else
                    {
                        amount = Int32.Parse(credit);
                        voucherrecord.Type = "Cr";
                    }
                    voucherrecord.Amount = amount;
                    //if (item.BranchId == 0)
                    //{
                    //    voucherrecord.BranchId = null;
                    //}
                    //else
                    //{
                    //    voucherrecord.BranchId = item.BranchId;
                    //}
                    voucherrecord.Description = item.Transaction;
                    var ledgerrecord = db.Ledgers.Where(x => x.Id == voucherrecord.LedgerId).FirstOrDefault();
                    voucherrecord.CurrentBalance = ledgerrecord.CurrentBalance;
                    //////////////////////////Game/////////////////////
                    var groupid = ledgerrecord.LedgerGroupId;
                    var ledgerHead = "";
                    if(groupid!=null)
                    {
                        var ledgerheadId = db.LedgerGroups.Where(x => x.Id == groupid).Select(x => x.LedgerHeadID).FirstOrDefault();
                         ledgerHead = db.LedgerHeads.Where(x => x.Id == ledgerheadId).Select(x => x.Name).FirstOrDefault();
                    }
                    else
                    {
                        var headid = ledgerrecord.LedgerHeadId;
                         ledgerHead = db.LedgerHeads.Where(x => x.Id == headid).Select(x => x.Name).FirstOrDefault();

                    }

                    if (ledgerHead == "Assets" || ledgerHead == "Expense")
                    {
                        if (voucherrecord.Type == "Cr")
                        {
                            voucherrecord.AfterBalance = ledgerrecord.CurrentBalance - amount;

                            Ledger ledger = db.Ledgers.Where(x => x.Id == voucherrecord.LedgerId).FirstOrDefault();
                            ledger.CurrentBalance = voucherrecord.AfterBalance;
                            db.SaveChanges();

                        }
                        else if (voucherrecord.Type == "Dr")
                        {
                            voucherrecord.AfterBalance = ledgerrecord.CurrentBalance + amount;

                            Ledger ledger = db.Ledgers.Where(x => x.Id == voucherrecord.LedgerId).FirstOrDefault();
                            ledger.CurrentBalance = voucherrecord.AfterBalance;
                            db.SaveChanges();
                        }
                    }
                    else if (ledgerHead == "Equity" || ledgerHead == "Liabilities" || ledgerHead == "Income")
                    {
                        if (voucherrecord.Type == "Cr")
                        {
                            voucherrecord.AfterBalance = ledgerrecord.CurrentBalance + amount;

                            Ledger ledger = db.Ledgers.Where(x => x.Id == voucherrecord.LedgerId).FirstOrDefault();
                            ledger.CurrentBalance = voucherrecord.AfterBalance;
                            db.SaveChanges();
                        }
                        else if (voucherrecord.Type == "Dr")
                        {
                            voucherrecord.AfterBalance = ledgerrecord.CurrentBalance - amount;

                            Ledger ledger = db.Ledgers.Where(x => x.Id == voucherrecord.LedgerId).FirstOrDefault();
                            ledger.CurrentBalance = voucherrecord.AfterBalance;
                            db.SaveChanges();
                        }
                    }
                    db.VoucherRecords.Add(voucherrecord);
                    db.SaveChanges();
                }


                var result = "yes";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                var result = "No";
                ViewBag.Error = e.Message;
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // GET: Vouchers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vouchers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Notes,Date,CreatedBy")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Vouchers.Add(voucher);
                db.SaveChanges();
                return RedirectToAction("Voucher");
            }

            return View(voucher);
        }

        // GET: Vouchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // POST: Vouchers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Notes,Date,CreatedBy")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voucher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Voucher");
            }
            return View(voucher);
        }

        // GET: Vouchers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // POST: Vouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Voucher voucher = db.Vouchers.Find(id);
            db.Vouchers.Remove(voucher);
            db.SaveChanges();
            return RedirectToAction("Voucher");
        }
        public class _Voucher
        {
            public string VoucherName { get; set; }
            public string VoucherDescription { get; set; }
            public string Narration { get; set; }
            public string Time { get; set; }
            public int VoucherNo { get; set; }
            public List<Voucher_Detail> VoucherDetail { set; get; }
        }

        public class Voucher_Detail
        {
            public string Type { get; set; }
            public string VoucherNo { get; set; }
            public string Time { get; set; }
            public string Code { get; set; }
            public string Transaction { get; set; }
            public string Credit { get; set; }
            public string Debit { get; set; }
            public double balance { get; set; }
            public int BranchId { get; set; }

        }
        public class AccountsList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class HeadList
        {
            public int HeadId { get; set; }
            public string HeadName { get; set; }
            public List<AccountsList> accountlist { get; set; }
        }
        public class Voucher_list
        {
            public int ID { get; set; }
            public int? VoucherNo { get; set; }
            public string Name { get; set; }
            public string Notes { get; set; }
            public DateTime? Date { get; set; }
            public decimal? Amount { get; set; }
            public string LedgerName { get; set; }
            public string type { get; set; }

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
