//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SEA_Application.Models;
//using System.Globalization;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using System.IO;
//using Rotativa;
//using OfficeOpenXml;

//namespace SEA_Application.Controllers
//{
//    public class AspNetFinanceVouchersController : Controller
//    {
//        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

//        // GET: AspNetFinanceVouchers
//        public ActionResult Index()
//        {

//            //var date = "23/12/2017";

//            //DateTime dt = DateTime.ParseExact(
//            //date,
//            //"dd/MM/yyyy",
//            //CultureInfo.InvariantCulture).Add(DateTime.Now.TimeOfDay);

//            //DateTime dt = DateTime.ParseExact(
//            //date,
//            //"dd/MM/yyyy",
//            //CultureInfo.GetCultureInfo("en-GB")).Add(DateTime.Now.TimeOfDay);


//            var voucher = db.AspNetFinanceVouchers.Select(x => new
//            {
//                credit = x.AspNetFinanceVoucherDetails.Select(y => y.Credit).Sum().ToString(),
//                debit = x.AspNetFinanceVoucherDetails.Select(y => y.Debit).Sum().ToString(),
//                x.VoucherDescription,
//                x.Time,
//                x.Id
//            }).OrderByDescending(x=> x.Id).ToList();

//            List<VoucherList> Vouchers = new List<VoucherList>();

//            foreach (var item in voucher)
//            {
//                VoucherList Voucher = new VoucherList();
//                Voucher.VoucherDescription = item.VoucherDescription;
//                Voucher.Time = item.Time.ToString();
//                Voucher.Credit = item.credit;
//                Voucher.Debit = item.debit;
//                Voucher.Id = item.Id;
//                Vouchers.Add(Voucher);
//            }

//            return View(Vouchers);

//        }

//        public ActionResult SaveIndex()
//        {
//            var voucher = db.AspNetFinanceVouchers.Select(x => new
//            {
//                credit = x.AspNetFinanceVoucherDetails.Select(y => y.Credit).Sum().ToString(),
//                debit = x.AspNetFinanceVoucherDetails.Select(y => y.Debit).Sum().ToString(),
//                x.VoucherDescription,
//                x.Time,
//                x.Id
//            }).ToList();

//            List<VoucherList> Vouchers = new List<VoucherList>();

//            foreach (var item in voucher)
//            {
//                VoucherList Voucher = new VoucherList();
//                Voucher.VoucherDescription = item.VoucherDescription;
//                Voucher.Time = item.Time.ToString();
//                Voucher.Credit = item.credit;
//                Voucher.Debit = item.debit;
//                Voucher.Id = item.Id;
//                Vouchers.Add(Voucher);
//            }



//            ViewBag.data = "Voucher has been created and updated";
//            return View("Index", Vouchers);
//        }


//        //  Html to PDF
//        //public ActionResult ExportPDF(string Code, string Period)
//        //{

//        //    var period = db.AspNetFinancePeriods.Select(x => x.Id).Max();
//        //    var LedgerId = db.AspNetFinanceLedgers.Where(x => x.Code == Code).Select(x => x.Id).FirstOrDefault();
//        //    var LedgerBalance = db.AspNetFinanceLedgers.Where(x => x.Id == LedgerId).Select(x => x.Balance).FirstOrDefault();
//        //    int periodId = Convert.ToInt32(Period);

//        //    var vouchers = db.AspNetFinanceVouchers.Where(x => x.Status == "Posted").Select(x => x).ToList();

//        //    VoucherPost VoucherPost = new VoucherPost();

//        //    VoucherPost.VoucherDetailList = new List<VoucherDetail>();
//        //    int TotalCredit = 0;
//        //    int TotalDebit = 0;

//        //    foreach (var item in vouchers)
//        //    {
//        //        var DetailList = db.AspNetFinanceVoucherDetails.Where(x => x.VoucherId == item.Id && x.LedgerId == LedgerId).Select(x => x).ToList();
//        //        foreach (var details in DetailList)
//        //        {
//        //            VoucherDetail VoucherDetail = new VoucherDetail();

//        //            VoucherDetail.Transaction = details.TransactionDescription;
//        //            VoucherDetail.balance =Convert.ToDouble( db.AspNetFinanceLedgers.Where(x => x.Id == LedgerId).Select(x => x.Balance).FirstOrDefault());

//        //            VoucherDetail.Credit = details.Credit.ToString();
//        //            TotalCredit += (int)details.Credit;
//        //            VoucherDetail.balance = VoucherDetail.balance - Convert.ToInt32(details.Credit);
//        //            VoucherDetail.Debit = details.Debit.ToString();
//        //            VoucherDetail.balance = VoucherDetail.balance + Convert.ToInt32(details.Debit);
//        //            TotalDebit += (int)details.Debit;

//        //            VoucherDetail.Time = db.AspNetFinanceVouchers.Where(x => x.Id == details.VoucherId).Select(x => x.Time).FirstOrDefault().ToString();
//        //            string Type = db.AspNetFinanceVouchers.Where(x => x.Id == details.VoucherId).Select(x => x.VoucherType).FirstOrDefault();
//        //            var No = db.AspNetFinanceVouchers.Where(x => x.Id == details.VoucherId).Select(x => x.VoucherNo).FirstOrDefault();

//        //            VoucherDetail.Type = Type + "-" + No.ToString();
//        //            VoucherPost.VoucherDetailList.Add(VoucherDetail);
//        //        }
//        //    }
//        //    VoucherPost.Credit = TotalCredit.ToString();
//        //    VoucherPost.Debit = TotalDebit.ToString();

//        //    ViewBag.SearchResult = VoucherPost;
//        //    ViewBag.FinincialYear = db.AspNetFinanceMonths.Where(x => x.Id == periodId).Select(x => x.AspNetFinancePeriod.Year).FirstOrDefault();
//        //    ViewBag.Code = Code;
//        //    ViewBag.Description = db.AspNetFinanceLedgers.Where(x => x.Code == Code).Select(x => x.Name).FirstOrDefault();

//        //    return new PartialViewAsPdf("PostSearchReport")
//        //    {
//        //        FileName = Server.MapPath("~/documentation/file.pdf")
//        //    };
//        //}


//        public ActionResult EditVoucher(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            AspNetFinanceVoucher aspNetFinanceVoucher = db.AspNetFinanceVouchers.Where(x => x.Id == id).Select(x => x).FirstOrDefault();

//            if (aspNetFinanceVoucher == null)
//            {
//                return HttpNotFound();
//            }

//            Voucher Voucher = new Voucher();

//            Voucher.VoucherNo = aspNetFinanceVoucher.VoucherNo.ToString();
//            Voucher.VoucherType = aspNetFinanceVoucher.VoucherType;
//            Voucher.Id = aspNetFinanceVoucher.Id;
//            Voucher.VoucherDescription = aspNetFinanceVoucher.VoucherDescription;
//            //Voucher.PeriodId =(int) aspNetFinanceVoucher.periodId;

//            ViewBag.Detail = Voucher;
//            return View();
//        }

//        public ActionResult CreateVoucher()
//        {

//            var currentmonth = DateTime.Now.ToString("MMMM");

//            ////var period = db.AspNetFinancePeriods.Select(x => x.Id).Max();
//            //var id = db.AspNetFinanceMonths.Where(x => x.Month == currentmonth && x.PeriodId == period).Select(x => x.Id).FirstOrDefault();
//            //ViewBag.PeriodId = new SelectList(db.AspNetFinanceMonths, "Id", "Name");
//            //ViewBag.id = id;
//            return View();
//        }

//        public ActionResult VoucherPosting()
//        {
//            ViewBag.Period = new SelectList(db.AspNetFinanceMonths, "Id", "Name");
//            ViewBag.Period1 = new SelectList(db.AspNetFinanceMonths, "Id", "Name");
//            return View();
//        }

//        public ActionResult TrailBalance()
//        {
//            return View();
//        }

//        [HttpPost]
//        public JsonResult AddVoucher(Voucher Vouchers)
//        {
//            string[] D4 = Vouchers.Time.Split('-');
//            Vouchers.Time = D4[2] + "/" + D4[1] + "/" + D4[0];
//            AspNetFinanceVoucher voucher = new AspNetFinanceVoucher();
//            voucher.Status = Vouchers.Status;

//            var localtime = DateTime.Now.ToLocalTime().ToString();
//            var time = localtime.Split(' ');
//            var timestamps = time[1].Split(':');
//            if (timestamps[0].Count() == 1)
//            {
//                timestamps[0] = "0" + timestamps[0];
//            }
//            if (timestamps[1].Count() == 1)
//            {
//                timestamps[1] = "0" + timestamps[1];
//            }
//            if (timestamps[2].Count() == 1)
//            {
//                timestamps[2] = "0" + timestamps[2];
//            }
//            time[1] = timestamps[0] + ":" + timestamps[1] + ":" + timestamps[2];

//            var date = Vouchers.Time + " " + time[1] + " " + time[2];

//            DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).Add(DateTime.Now.TimeOfDay);
//            voucher.Time = dt;
//            voucher.VoucherNo = Convert.ToInt32(Vouchers.VoucherNo);
//            voucher.VoucherType = Vouchers.VoucherType;
//            voucher.VoucherDescription = Vouchers.VoucherDescription;

//            db.AspNetFinanceVouchers.Add(voucher);
//            db.SaveChanges();

//            foreach (var item in Vouchers.VoucherDetail)
//            {
//                AspNetFinanceVoucherDetail detail = new AspNetFinanceVoucherDetail();
//                detail.Credit = Convert.ToInt32(item.Credit);
//                detail.Debit = Convert.ToInt32(item.Debit);
//                detail.TransactionDescription = item.Transaction;
//                detail.VoucherId = db.AspNetFinanceVouchers.Select(x => x.Id).Max();
//                detail.LedgerId = db.AspNetFinanceLedgers.Where(x => x.Code == item.Code).Select(x => x.Id).FirstOrDefault();
//                string type = db.AspNetFinanceLedgers.Where(x => x.Code == item.Code).Select(x => x.Type).FirstOrDefault();
//                int typeId = db.AspNetFinanceLedgerTypes.Where(x => x.Name == type).Select(x => x.Id).FirstOrDefault();
//                detail.LedgertypeId = db.AspNetFinanceLedgerTypes.Where(x => x.Id == typeId).Select(x => x.Id).FirstOrDefault();

//                int track = 0;
//                string ledgerType = db.AspNetFinanceLedgers.Where(x => x.Id == detail.LedgerId).Select(x => x.Type).FirstOrDefault();
//                var ledger = db.AspNetFinanceLedgers.Where(x => x.Id == detail.LedgerId).Select(x => x).FirstOrDefault();
//                ledgerType = ledgerType.Trim();
//                if (Vouchers.Status == "Posted")
//                {

//                    if (ledgerType == "Assets" || ledgerType == "Expense")
//                    {
//                        track = track - Convert.ToInt32(detail.Credit);
//                        track = track + Convert.ToInt32(detail.Debit);
//                        int balance = Convert.ToInt32(ledger.Balance);
//                        balance = balance + track;
//                        ledger.Balance = balance.ToString();
//                    }
//                    else if (ledgerType == "Revenue" || ledgerType == "Liabilities" || ledgerType == "Equity")
//                    {
//                        track = track + Convert.ToInt32(detail.Credit);
//                        track = track - Convert.ToInt32(detail.Debit);
//                        int balance = Convert.ToInt32(ledger.Balance);
//                        balance = balance + track;
//                        ledger.Balance = balance.ToString();
//                    }
//                }
//                db.AspNetFinanceVoucherDetails.Add(detail);
//                db.SaveChanges();
//                if (ledger.Head != 0)
//                    AddBalanceToParent(Convert.ToInt32(detail.Credit), Convert.ToInt32(detail.Debit), ledger.Head);
//            }


//            return Json("Success", JsonRequestBehavior.AllowGet);
//        }

//        public void AddBalanceToParent(int Credit, int Debit, int HeadId)
//        {
//            int track = 0;
//            var ledger = db.AspNetFinanceLedgers.Where(x => x.Id == HeadId).Select(x => x).FirstOrDefault();
//            if (ledger.Type == "Assets" || ledger.Type == "Expense")
//            {
//                track = track - Credit;
//                track = track + Debit;

//                int balance = Convert.ToInt32(ledger.Balance);

//                balance = balance + track;
//                ledger.Balance = balance.ToString();
//            }
//            else if (ledger.Type == "Revenue" || ledger.Type == "Liabilities" || ledger.Type == "Equity")
//            {
//                track = track + Credit;
//                track = track - Debit;

//                int balance = Convert.ToInt32(ledger.Balance);

//                balance = balance + track;
//                ledger.Balance = balance.ToString();
//            }
//            db.SaveChanges();

//            if (ledger.Head != 0)
//            {
//                AddBalanceToParent(Credit, Debit, ledger.Head);
//            }
//        }

//        [HttpPost]
//        public JsonResult SaveEdit(Voucher Vouchers)
//        {
//            string[] D4 = Vouchers.Time.Split('-');
//            Vouchers.Time = D4[1] + "/" + D4[2] + "/" + D4[0];
//            AspNetFinanceVoucher voucher = db.AspNetFinanceVouchers.Where(x => x.Id == Vouchers.Id).Select(x => x).FirstOrDefault();
//            voucher.Status = Vouchers.Status;
//            DateTime dt = DateTime.ParseExact(Vouchers.Time, "MM/dd/yyyy", CultureInfo.InvariantCulture);
//            voucher.Time = dt;
//            voucher.VoucherNo = Convert.ToInt32(Vouchers.VoucherNo);
//            voucher.VoucherType = Vouchers.VoucherType;
//            voucher.VoucherDescription = Vouchers.VoucherDescription;

//            db.SaveChanges();

//            int id = Vouchers.Id;

//            var details = db.AspNetFinanceVoucherDetails.Where(x => x.VoucherId == id).ToList();

//            foreach (var row in details)
//            {
//                db.AspNetFinanceVoucherDetails.Remove(row);
//            }

//            db.SaveChanges();

//            foreach (var item in Vouchers.VoucherDetail)
//            {
//                AspNetFinanceVoucherDetail detail = new AspNetFinanceVoucherDetail();
//                detail.Credit = Convert.ToInt32(item.Credit);
//                detail.Debit = Convert.ToInt32(item.Debit);
//                detail.TransactionDescription = item.Transaction;
//                detail.VoucherId = Vouchers.Id;
//                detail.LedgerId = db.AspNetFinanceLedgers.Where(x => x.Code == item.Code).Select(x => x.Id).FirstOrDefault();
//                string type = db.AspNetFinanceLedgers.Where(x => x.Code == item.Code).Select(x => x.Type).FirstOrDefault();
//                int typeId = Convert.ToInt32(type);
//                detail.LedgertypeId = db.AspNetFinanceLedgerTypes.Where(x => x.Id == typeId).Select(x => x.Id).FirstOrDefault();

//                db.AspNetFinanceVoucherDetails.Add(detail);
//            }
//            db.SaveChanges();

//            return Json("Success", JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public JsonResult codeDescription(string code)
//        {
//            codeData data = new codeData();
//            try
//            {
//                data.description = db.AspNetFinanceLedgers.Where(x => x.Code == code).Select(x => x.Name).FirstOrDefault();
//                data.type = db.AspNetFinanceLedgers.Where(x => x.Code == code).Select(x => x.Type).FirstOrDefault();
//                //int temp = Convert.ToInt32(data.type);
//                //data.type = db.AspNetFinanceLedgerTypes.Where(x => x.Id == temp).Select(x => x.Name).FirstOrDefault();
//                //data.type = data.type.Substring(0,1);
//            }
//            catch
//            {
//                data.description = "not Found";
//                data.type = "Error";
//            }

//            if (data.description == null)
//                data.description = "not Found";
//            if (data.type == null)
//                data.type = "Error";

//            return Json(data, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public JsonResult FindVoucherNo(string type)
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

//            return Json(No, JsonRequestBehavior.AllowGet);

//        }
//        public JsonResult VoucherDetails(int id)
//        {
//            var details = db.AspNetFinanceVoucherDetails.Where(x => x.VoucherId == id).ToList();

//            List<VoucherDetail> DetailList = new List<VoucherDetail>();

//            foreach (var item in details)
//            {
//                VoucherDetail Detail = new VoucherDetail();
//                Detail.Transaction = item.TransactionDescription;
//                Detail.Credit = item.Credit.ToString();
//                Detail.Debit = item.Debit.ToString();
//                Detail.Type = db.AspNetFinanceLedgerTypes.Where(x => x.Id == item.LedgertypeId).Select(x => x.Name).FirstOrDefault();
//                Detail.Type = Detail.Type.Substring(0, 1);
//                Detail.Code = db.AspNetFinanceLedgers.Where(x => x.Id == item.LedgerId).Select(x => x.Code).FirstOrDefault();
//                DetailList.Add(Detail);
//            }

//            return Json(DetailList, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult TrailReport(string D1, string D2, string[] Types)
//        {
//            string[] D3 = D1.Split('-');
//            string[] D4 = D2.Split('-');
//            D1 = D3[1] + "/" + D3[2] + "/" + D3[0];
//            D2 = D4[1] + "/" + D4[2] + "/" + D4[0];
//            DateTime d1 = DateTime.ParseExact(D1, "MM/dd/yyyy", CultureInfo.InvariantCulture);
//            DateTime d2 = DateTime.ParseExact(D2, "MM/dd/yyyy", CultureInfo.InvariantCulture);

//            List<AspNetFinanceVoucher> vouchers = new List<AspNetFinanceVoucher>();

//            foreach (var item in Types)
//            {
//                var voucher = db.AspNetFinanceVouchers.Where(x => x.Time <= d2 && x.Time >= d1 && x.VoucherType == item).Select(x => x).ToList();
//                foreach (var items in voucher)
//                {
//                    vouchers.Add(items);
//                }
//            }


//            VoucherPost VoucherPost = new VoucherPost();

//            VoucherPost.VoucherDetailList = new List<VoucherDetail>();
//            int TotalCredit = 0;
//            int TotalDebit = 0;

//            foreach (var item in vouchers)
//            {
//                var DetailList = db.AspNetFinanceVoucherDetails.Where(x => x.VoucherId == item.Id).Select(x => x).ToList();
//                foreach (var details in DetailList)
//                {
//                    VoucherDetail VoucherDetail = new VoucherDetail();
//                    VoucherDetail.Code = db.AspNetFinanceLedgers.Where(x => x.Id == details.LedgerId).Select(x => x.Code).FirstOrDefault();
//                    VoucherDetail.Transaction = db.AspNetFinanceLedgers.Where(x => x.Id == details.LedgerId).Select(x => x.Name).FirstOrDefault();
//                    VoucherDetail.Credit = details.Credit.ToString();
//                    TotalCredit += (int)details.Credit;
//                    VoucherDetail.Debit = details.Debit.ToString();
//                    TotalDebit += (int)details.Debit;
//                    VoucherPost.VoucherDetailList.Add(VoucherDetail);
//                }
//            }

//            VoucherPost.Credit = TotalCredit.ToString();
//            VoucherPost.Debit = TotalDebit.ToString();
//            return Json(VoucherPost, JsonRequestBehavior.AllowGet);
//        }


//        public JsonResult PostSearch(string Code, string Period)
//        {
//            var period = db.AspNetFinancePeriods.Select(x => x.Id).Max();
//            var LedgerId = db.AspNetFinanceLedgers.Where(x => x.Code == Code).Select(x => x.Id).FirstOrDefault();
//            var LedgerBalance = db.AspNetFinanceLedgers.Where(x => x.Code == Code).Select(x => x.StartingBalance).FirstOrDefault();
//            int periodId = int.Parse(Period);

//            var vouchers = db.AspNetFinanceVouchers.Where(x => x.Status == "Posted").Select(x => x).ToList();

//            VoucherPost VoucherPost = new VoucherPost();

//            VoucherPost.VoucherDetailList = new List<VoucherDetail>();
//            int TotalCredit = 0;
//            int TotalDebit = 0;

//            foreach (var item in vouchers)
//            {
//                var DetailList = db.AspNetFinanceVoucherDetails.Where(x => x.VoucherId == item.Id && x.LedgerId == LedgerId).Select(x => x).ToList();
//                foreach (var details in DetailList)
//                {
//                    VoucherDetail VoucherDetail = new VoucherDetail();

//                    VoucherDetail.Transaction = details.TransactionDescription;
//                    VoucherDetail.balance = (double)LedgerBalance;

//                    VoucherDetail.Credit = details.Credit.ToString();
//                    TotalCredit += (int)details.Credit;
//                    VoucherDetail.balance = VoucherDetail.balance - Convert.ToInt32(details.Credit);
//                    VoucherDetail.Debit = details.Debit.ToString();
//                    VoucherDetail.balance = VoucherDetail.balance + Convert.ToInt32(details.Debit);
//                    TotalDebit += (int)details.Debit;

//                    VoucherDetail.Time = db.AspNetFinanceVouchers.Where(x => x.Id == details.VoucherId).Select(x => x.Time).FirstOrDefault().ToString();
//                    string Type = db.AspNetFinanceVouchers.Where(x => x.Id == details.VoucherId).Select(x => x.VoucherType).FirstOrDefault();
//                    var No = db.AspNetFinanceVouchers.Where(x => x.Id == details.VoucherId).Select(x => x.VoucherNo).FirstOrDefault();

//                    VoucherDetail.Type = Type + "-" + No.ToString();
//                    LedgerBalance = VoucherDetail.balance;
//                    VoucherPost.VoucherDetailList.Add(VoucherDetail);
//                }
//            }
//            VoucherPost.Credit = TotalCredit.ToString();
//            VoucherPost.Debit = TotalDebit.ToString();

//            return Json(VoucherPost, JsonRequestBehavior.AllowGet);

//        }

//        public JsonResult postVouchers(string D1, string D2, int periodId)
//        {
//            string[] D3 = D1.Split('-');
//            string[] D4 = D2.Split('-');
//            D1 = D3[1] + "/" + D3[2] + "/" + D3[0];
//            D2 = D4[1] + "/" + D4[2] + "/" + D4[0];
//            DateTime d1 = DateTime.ParseExact(D1, "MM/dd/yyyy", CultureInfo.InvariantCulture);
//            DateTime d2 = DateTime.ParseExact(D2, "MM/dd/yyyy", CultureInfo.InvariantCulture);

//            var vouchers = db.AspNetFinanceVouchers.Where(x => x.Time <= d2 && x.Time >= d1 && x.Status == "Not Posted").Select(x => x).ToList();

//            foreach (var item in vouchers)
//            {
//                item.Status = "Posted";
//            }

//            var Ledgers = db.AspNetFinanceLedgers.ToList();

//            foreach (var Code in Ledgers)
//            {
//                var LedgerId = db.AspNetFinanceLedgers.Where(x => x.Code == Code.Code).Select(x => x.Id).FirstOrDefault();
//                var LedgerBalance = db.AspNetFinanceLedgers.Where(x => x.Id == LedgerId).Select(x => x.Balance).FirstOrDefault();

//                foreach (var item in vouchers)
//                {
//                    int track = 0;
//                    var DetailList = db.AspNetFinanceVoucherDetails.Where(x => x.VoucherId == item.Id && x.LedgerId == LedgerId).Select(x => x).ToList();
//                    if (DetailList.Count > 0)
//                    {
//                        foreach (var details in DetailList)
//                        {
//                            string ledgerType = db.AspNetFinanceLedgers.Where(x => x.Id == details.LedgerId).Select(x => x.Type).FirstOrDefault();
//                            ledgerType = ledgerType.Trim();
//                            if (ledgerType == "Assests" || ledgerType == "Expense")
//                            {
//                                track = track - Convert.ToInt32(details.Credit);
//                                track = track + Convert.ToInt32(details.Debit);
//                            }
//                            else if (ledgerType == "Revenue" || ledgerType == "Liabilities" || ledgerType == "Equity")
//                            {
//                                track = track + Convert.ToInt32(details.Credit);
//                                track = track - Convert.ToInt32(details.Debit);
//                            }

//                        }
//                        var Ledger = db.AspNetFinanceLedgers.Where(x => x.Id == LedgerId).Select(x => x).FirstOrDefault();
//                        Ledger.Balance = track.ToString();
//                        db.SaveChanges();
//                    }
//                }
//            }

//            db.SaveChanges();

//            return Json("Success", JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult unpostedVouchers(string D1, string D2, int periodId)
//        {
//            string[] D3 = D1.Split('-');
//            string[] D4 = D2.Split('-');
//            D1 = D3[1] + "/" + D3[2] + "/" + D3[0];
//            D2 = D4[1] + "/" + D4[2] + "/" + D4[0];
//            DateTime d1 = DateTime.ParseExact(D1, "MM/dd/yyyy", CultureInfo.InvariantCulture);
//            DateTime d2 = DateTime.ParseExact(D2, "MM/dd/yyyy", CultureInfo.InvariantCulture);

//            int count = db.AspNetFinanceVouchers.Where(x => x.Time <= d2 && x.Time >= d1 && x.Status == "Not Posted").Count();
//            return Json(count, JsonRequestBehavior.AllowGet);
//        }

//        // GET: AspNetFinanceVouchers/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceVoucher aspNetFinanceVoucher = db.AspNetFinanceVouchers.Find(id);
//            if (aspNetFinanceVoucher == null)
//            {
//                return HttpNotFound();
//            }

//            //var details = db.AspNetFinanceVoucherDetails.Where(x => x.VoucherId == id ).ToList();
//            return View(aspNetFinanceVoucher);
//        }
//        public ActionResult VoucherPrint(int? vid)
//        {
//            if (vid == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceVoucher aspNetFinanceVoucher = db.AspNetFinanceVouchers.Find(vid);
//            if (aspNetFinanceVoucher == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFinanceVoucher);

//        }

//        public ActionResult VoucherFromExcel(HttpPostedFileBase excelfile)
//        {
//            int error = 0;
//            var dbTransaction = db.Database.BeginTransaction();
//            try
//            {

//                if (excelfile == null || excelfile.ContentLength == 0)
//                {
//                    TempData["Error"] = "Please select an excel file";
//                    return RedirectToAction("Create");
//                }
//                else if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
//                {

//                    HttpPostedFileBase file = excelfile;   // Request.Files["excelfile"];

//                    using (var package = new ExcelPackage(file.InputStream))
//                    {
//                        var currentSheet = package.Workbook.Worksheets;
//                        var workSheet = currentSheet.First();
//                        var noOfCol = workSheet.Dimension.End.Column;
//                        var noOfRow = workSheet.Dimension.End.Row;
//                        int voucherId = 0;

//                        for (int rowIterator = 4; rowIterator <= noOfRow; rowIterator++)
//                        {
//                            error = rowIterator;
//                            string Heading = workSheet.Cells[rowIterator, 1].Value.ToString();
//                            if (Heading == "Voucher Start")
//                            {
//                                rowIterator++;
//                                error = rowIterator;
//                                var voucher = new AspNetFinanceVoucher();
//                                voucher.Status = "Posted";
//                                voucher.VoucherType = workSheet.Cells[rowIterator, 1].Value.ToString();
//                                voucher.VoucherNo = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
//                                var date = workSheet.Cells[rowIterator, 3].Value.ToString();
//                                DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).Add(DateTime.Now.TimeOfDay);
//                                voucher.Time = dt;
//                                voucher.VoucherDescription = workSheet.Cells[rowIterator, 4].Value.ToString();

//                                db.AspNetFinanceVouchers.Add(voucher);
//                                db.SaveChanges();
//                                voucherId = voucher.Id;
//                                error = rowIterator;
//                            }
//                            else if (Heading == "Details")
//                            {
//                                rowIterator++;
//                                double credit = 0;
//                                double debit = 0;
//                                for (; rowIterator <= noOfRow; rowIterator++)
//                                {

//                                    //if (rowIterator - error == 1 && workSheet.Cells[rowIterator, 1].Value.ToString() == "End Voucher")
//                                    //{
//                                    //    dbTransaction.Dispose();
//                                    //    TempData["Error"] = "There is no ledger entries in voucher of line " + error;
//                                    //    return RedirectToAction("Create");
//                                    //}else 


//                                    if (workSheet.Cells[rowIterator, 1].Value.ToString() == "End Voucher")
//                                    {
//                                        if (credit != debit)
//                                        {
//                                            dbTransaction.Dispose();
//                                            TempData["Error"] = "Voucher entries are unbalanced. Error line no. " + error;
//                                            return RedirectToAction("CreateVoucher");
//                                        }
//                                        else
//                                        {
//                                            break;
//                                        }
//                                    }
//                                    else
//                                    {
//                                        error = rowIterator;
//                                        var voucherDetail = new AspNetFinanceVoucherDetail();
//                                        string code = workSheet.Cells[rowIterator, 1].Value.ToString();
//                                        var ledger = db.AspNetFinanceLedgers.Where(x => x.Code == code).Select(x => x).FirstOrDefault();
//                                        voucherDetail.LedgerId = ledger.Id;
//                                        voucherDetail.VoucherId = voucherId;
//                                        voucherDetail.TransactionDescription = workSheet.Cells[rowIterator, 2].Value.ToString();
//                                        voucherDetail.Credit = Convert.ToDouble(workSheet.Cells[rowIterator, 3].Value.ToString());
//                                        voucherDetail.Debit = Convert.ToDouble(workSheet.Cells[rowIterator, 4].Value.ToString());
//                                        credit += (double)voucherDetail.Credit;
//                                        debit += (double)voucherDetail.Debit;

//                                        string ledgerType = ledger.Type;
//                                        int track = 0;
//                                        if (ledgerType == "Assets" || ledgerType == "Expense")
//                                        {
//                                            track = track - Convert.ToInt32(voucherDetail.Credit);
//                                            track = track + Convert.ToInt32(voucherDetail.Debit);
//                                            int balance = Convert.ToInt32(ledger.Balance);
//                                            balance = balance + track;
//                                            ledger.Balance = balance.ToString();
//                                        }
//                                        else if (ledgerType == "Revenue" || ledgerType == "Liabilities" || ledgerType == "Equity")
//                                        {
//                                            track = track + Convert.ToInt32(voucherDetail.Credit);
//                                            track = track - Convert.ToInt32(voucherDetail.Debit);
//                                            int balance = Convert.ToInt32(ledger.Balance);
//                                            balance = balance + track;
//                                            ledger.Balance = balance.ToString();
//                                        }

//                                        ///////////////// to add balance in parent ledgers
//                                        while (true)
//                                        {
//                                            var head = ledger.Head;

//                                            if (ledger.Head == 0)
//                                            {
//                                                break;
//                                            }

//                                            ledger = db.AspNetFinanceLedgers.Where(x => x.Id == head).Select(x => x).FirstOrDefault();

//                                            ledgerType = ledger.Type;
//                                            track = 0;
//                                            if (ledgerType == "Assets" || ledgerType == "Expense")
//                                            {
//                                                track = track - Convert.ToInt32(voucherDetail.Credit);
//                                                track = track + Convert.ToInt32(voucherDetail.Debit);
//                                                int balance = Convert.ToInt32(ledger.Balance);
//                                                balance = balance + track;
//                                                ledger.Balance = balance.ToString();
//                                            }
//                                            else if (ledgerType == "Revenue" || ledgerType == "Liabilities" || ledgerType == "Equity")
//                                            {
//                                                track = track + Convert.ToInt32(voucherDetail.Credit);
//                                                track = track - Convert.ToInt32(voucherDetail.Debit);
//                                                int balance = Convert.ToInt32(ledger.Balance);
//                                                balance = balance + track;
//                                                ledger.Balance = balance.ToString();
//                                            }
//                                            db.SaveChanges();
//                                        }

//                                        db.AspNetFinanceVoucherDetails.Add(voucherDetail);
//                                        db.SaveChanges();

//                                    }

//                                } // ledger entries for loop
//                            } // heading details
//                        }// main foreach to run excel files
//                    }// excel using package

//                    dbTransaction.Commit();
//                    return RedirectToAction("SaveIndex");

//                }
//                else
//                {
//                    TempData["Error"] = "File type is incorrect";
//                    return RedirectToAction("CreateVoucher");
//                }

//            }
//            catch (Exception ex)
//            {
//                dbTransaction.Dispose();
//                TempData["Error"] = "Incorrect Data at line No. " + error;
//                return RedirectToAction("CreateVoucher");
//            }
//        }

//        // GET: AspNetFinanceVouchers/Create
//        public ActionResult Create()
//        {
//            ViewBag.PeriodId = new SelectList(db.AspNetFinanceMonths, "Id", "Name");
//            return View();
//        }

//        // POST: AspNetFinanceVouchers/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,Time,VoucherNo,Status,VoucherType,periodId")] AspNetFinanceVoucher aspNetFinanceVoucher)
//        {
//            if (ModelState.IsValid)
//            {
//                db.AspNetFinanceVouchers.Add(aspNetFinanceVoucher);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(aspNetFinanceVoucher);
//        }

//        // GET: AspNetFinanceVouchers/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceVoucher aspNetFinanceVoucher = db.AspNetFinanceVouchers.Find(id);
//            if (aspNetFinanceVoucher == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFinanceVoucher);
//        }

//        // POST: AspNetFinanceVouchers/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,Time,VoucherNo,Status,VoucherType")] AspNetFinanceVoucher aspNetFinanceVoucher)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aspNetFinanceVoucher).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(aspNetFinanceVoucher);
//        }

//        // GET: AspNetFinanceVouchers/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceVoucher aspNetFinanceVoucher = db.AspNetFinanceVouchers.Find(id);
//            if (aspNetFinanceVoucher == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFinanceVoucher);
//        }

//        // POST: AspNetFinanceVouchers/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            AspNetFinanceVoucher aspNetFinanceVoucher = db.AspNetFinanceVouchers.Find(id);
//            db.AspNetFinanceVouchers.Remove(aspNetFinanceVoucher);
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

//        public class VoucherPost
//        {
//            public List<VoucherDetail> VoucherDetailList { get; set; }
//            public string Credit { get; set; }
//            public string Debit { get; set; }
//        }

//        public class codeData
//        {
//            public string description { get; set; }
//            public string type { get; set; }
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
