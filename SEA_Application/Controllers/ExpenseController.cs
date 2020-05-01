using Microsoft.AspNet.Identity;
using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity.Core.Objects;

namespace SEA_Application.Controllers
{
    public class ExpenseController : Controller
    {
        SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: Expense
        public ActionResult Index()
        {

           ViewBag.CurrentBalance = db.Ledgers.Where(x => x.Name == "Admin Drawer").FirstOrDefault().CurrentBalance;
            


            return View();
        }

        public JsonResult FindVoucherNo()
        {
            int No;
            try
            {
                No = (int)db.Vouchers.Select(x => x.VoucherNo).Max();
                No++;

            }
            catch
            {
                No = 1;
            }

            return Json(No, JsonRequestBehavior.AllowGet);

        }
        public class _Voucher
        {
            public string VoucherName { get; set; }
            public string VoucherDescription { get; set; }
            public string Narration { get; set; }
            public string Time { get; set; }
            public string upperdesc { get; set; }
            public int uppercode { get; set; }
            public string accounttype { get; set; }
            public string uppertotal { get; set; }
            public int VoucherNo { get; set; }
            public List<Voucher_Detail> VoucherDetail { set; get; }
        }
        public ActionResult AddCashVoucher(_Voucher Vouchers)
            {
            try
            {

                var LeadgerAdminDrawer = db.Ledgers.Where(x => x.Name == "Admin Drawer").FirstOrDefault();

                decimal? CurrentBalanceOfAdminDrawer = LeadgerAdminDrawer.CurrentBalance;

                decimal Total =  Convert.ToDecimal( Vouchers.uppertotal);

                if (CurrentBalanceOfAdminDrawer > Total)
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

                  //  DateTime dt = //DateTime.ParseExact(date, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

                    v.Date = GetLocalDateTime.GetLocalDateTimeFunction();
                    v.Notes = Vouchers.Narration;
                    v.VoucherNo = Vouchers.VoucherNo;
                    v.Name = Vouchers.VoucherName;
                    var id = User.Identity.GetUserId();
                    v.SessionID = SessionID;

                    var username = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
                    v.CreatedBy = username;
                    db.Vouchers.Add(v);
                    db.SaveChanges();

                    foreach (Voucher_Detail voucher in Vouchers.VoucherDetail)
                    {
                        VoucherRecord VR = new VoucherRecord();
                        int idd = Convert.ToInt32( voucher.Code);
                        
                        var Ledger =  db.Ledgers.Where(x => x.Id == idd).FirstOrDefault();
                        decimal? CurrentBlance = Ledger.CurrentBalance;
                        decimal? AfterBalanceOfLedger = CurrentBlance + Convert.ToDecimal(voucher.Credit);
                        VR.LedgerId = idd;
                        VR.Type = "Dr";
                        VR.Amount = Convert.ToDecimal(voucher.Credit);
                        VR.CurrentBalance = CurrentBlance;
                        VR.AfterBalance = AfterBalanceOfLedger;
                        VR.VoucherId = v.Id;
                        VR.Description = voucher.Transaction;
                        Ledger.CurrentBalance = AfterBalanceOfLedger;

                        db.VoucherRecords.Add(VR);
                        db.SaveChanges();

                    }

                    var LeadgerAdminDrawer1 = db.Ledgers.Where(x => x.Name == "Admin Drawer").FirstOrDefault();

                    decimal? CurrentBalanceOfAdminDrawer1 = LeadgerAdminDrawer1.CurrentBalance;


                    decimal? AfterBalanceOfAdminDrawer1 = CurrentBalanceOfAdminDrawer1 - Convert.ToDecimal(Vouchers.uppertotal);
                    VoucherRecord voucherRecord1 = new VoucherRecord();

                    voucherRecord1.LedgerId = LeadgerAdminDrawer.Id;
                    voucherRecord1.Type = "Cr";
                    voucherRecord1.Amount = Convert.ToDecimal(Vouchers.uppertotal);
                    voucherRecord1.CurrentBalance = CurrentBalanceOfAdminDrawer;
                    voucherRecord1.AfterBalance = AfterBalanceOfAdminDrawer1;
                    voucherRecord1.VoucherId = v.Id;
                    voucherRecord1.Description = "Expense Credited";
                    LeadgerAdminDrawer1.CurrentBalance = AfterBalanceOfAdminDrawer1;

                    db.VoucherRecords.Add(voucherRecord1);
                    db.SaveChanges();

                    var result = "yes";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var result = "No";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }

            }
            catch
            {

            }


            return RedirectToAction("Cash");
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
        public JsonResult SelectListLedgers()
        {

            var ledger = db.Ledgers.Where(x => x.LedgerGroup.Name == "Cash").ToList();
            List<Ledger> List = new List<Ledger>();

            foreach (var item in ledger)
            {
                Ledger Ledger = new Ledger();
                Ledger.Id = item.Id;
                Ledger.Name = item.Name;
                List.Add(Ledger);
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckaccountHead(int id)
        {
            var flag = false;
            var headName = db.Ledgers.Where(x => x.Id == id).Select(x => x.LedgerHead.Name).FirstOrDefault();
            if (headName == "Income" || headName == "Expense")
            {
                flag = true;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        public class HeadList
        {
            public int HeadId { get; set; }
            public string HeadName { get; set; }
            public List<AccountsList> accountlist { get; set; }
        }
        public class AccountsList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public JsonResult SelectAllLedgers()
        {

            var headlist = db.LedgerHeads.Where(x => x.Name == "Expense").ToList();

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
        public ActionResult Report()
        {

         
            return View();
        }
        public ActionResult ReportRecords()
        {

            //var Date = DateTime.Now.ToString("d/M/yyyy");
            //var result = from Voucher in db.Vouchers
            //             join VoucherRecord in db.VoucherRecords on Voucher.Id equals VoucherRecord.VoucherId
            //             join Leadger in db.Ledgers on VoucherRecord.LedgerId equals Leadger.Id
            //             where VoucherRecord.Ledger.Name == "Admin Drawer" && Voucher.Date.ToString() == Date
            //             select new
            //             {
            //                 VoucherRecord.Type,
            //                 Voucher.Date,
            //                 VoucherRecord.Amount,
            //                 Voucher.Id,
            //                 VoucherRecord.Description,
            //             };

            //var x = 0;

           var result =  db.TodayExpense();

            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public ActionResult DebitRecords()
        {

            var result = db.TodayDebitList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}