using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System.Threading.Tasks;

namespace SEA_Application.Controllers
{
    public class LedgersController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: Ledgers
        //public ActionResult Index()
        //{
        //    var ledgers = db.Ledgers.Include(l => l.AspNetSession).Include(l => l.LedgerGroup);
        //    return View(ledgers.ToList());
        //}
        public ActionResult LedgerList()
        {
            return View();
        }
        public JsonResult GetLedgerList()
        {
            var list = db.Ledgers.ToList();
            List<Ledger_Group> group = new List<Ledger_Group>();
            foreach (var item in list)
            {
                Ledger_Group lg = new Ledger_Group();
                lg.Id = item.Id;
                lg.Name = item.Name;
                lg.CreatedBy = item.CreatedBy;
                //lg.SessionId = item.AspNetSession.Year;
                lg.StartingBalance = item.StartingBalance;
                lg.CurrentBalance = item.CurrentBalance;
               if(item.LedgerGroupId==null)
                {
                    lg.LedgerGroupId = null;
                }
                else
                {
                    lg.LedgerGroupId = item.LedgerGroup.Name;
                }

                if (item.LedgerHeadId == null)
                {
                    lg.LedgerHeadId = null;
                }
                else
                {
                    lg.LedgerHeadId = item.LedgerHead.Name;
                }
                group.Add(lg);
            }
            return Json(group,JsonRequestBehavior.AllowGet);
        }
        // GET: Ledgers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ledger ledger = db.Ledgers.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            return View(ledger);
        }

        public ActionResult CashCreate()
        {
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName");
            ViewBag.LedgerGroupId = new SelectList(db.LedgerGroups.Where(x => x.Name == "Cash"), "Id", "Name");

            return View();
        }
        public ActionResult NewCashAccount()
        {
            Ledger ledger = new Ledger();

            try
            {

                var name = Request.Form["name"];
                var starting = Request.Form["startingbalance"];
                var group = Request.Form["LedgerGroupId"];
                var session = Request.Form["SessionId"];

                ledger.Name = name;
                var uid = User.Identity.GetUserId();
                var accountantname = db.AspNetUsers.Where(x => x.Id == uid).Select(x => x.Name).FirstOrDefault();
                ledger.CreatedBy = accountantname;
                ledger.StartingBalance = Convert.ToDecimal(starting);
                ledger.CurrentBalance = ledger.StartingBalance;
                ledger.SessionId = Convert.ToInt32(session);
                ledger.LedgerGroupId = Convert.ToInt32(group);
                ledger.LedgerHeadId = db.LedgerGroups.Where(x => x.Id == ledger.LedgerGroupId).Select(x => x.LedgerHeadID).FirstOrDefault();
                db.Ledgers.Add(ledger);
                db.SaveChanges();

                return RedirectToAction("CashIndex", "FinanceSummary");
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("CashCreate", ledger);
            }

        }

        public ActionResult BankCreate()
        {
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName");
            ViewBag.LedgerGroupId = new SelectList(db.LedgerGroups.Where(x => x.Name == "Bank"), "Id", "Name");

            return View();
        }
        public ActionResult NewBankAccount()
        {
            Ledger ledger = new Ledger();

            try
            {
            
                var name = Request.Form["name"];
                var starting = Request.Form["startingbalance"];
                var group = Request.Form["LedgerGroupId"];
                var session = Request.Form["SessionId"];
                if(starting=="")
                {
                    starting = "0";
                }
                ledger.Name = name;
                var uid = User.Identity.GetUserId();
                var accountantname = db.AspNetUsers.Where(x => x.Id == uid).Select(x => x.Name).FirstOrDefault();
                ledger.CreatedBy = accountantname;
                ledger.StartingBalance = Convert.ToDecimal(starting);
                ledger.CurrentBalance = ledger.StartingBalance;
                ledger.SessionId = Convert.ToInt32(session);
                ledger.LedgerGroupId = Convert.ToInt32(group);
                ledger.LedgerHeadId = db.LedgerGroups.Where(x => x.Id == ledger.LedgerGroupId).Select(x => x.LedgerHeadID).FirstOrDefault();
                db.Ledgers.Add(ledger);
                db.SaveChanges();

                return RedirectToAction("BankIndex", "FinanceSummary");
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("BankCreate", ledger);
            }
           
        }

        // GET: Ledgers/Create
        public ActionResult Create()
        {
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName");
            ViewBag.LedgerGroupId = new SelectList(db.LedgerGroups.Where(x=>x.Name!="Cash" && x.Name!="Bank"), "Id", "Name");
            ViewBag.LedgerHeadId = new SelectList(db.LedgerHeads, "Id", "Name");

            return View();
        }

        // POST: Ledgers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartingBalance,CurrentBalance,CreatedBy,LedgerGroupId,SessionId,Is_Default,LedgerHeadId")] Ledger ledger)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ledger.Is_Default = true;

                    if (ledger.LedgerGroupId != null)
                    {
                        ledger.LedgerHeadId = db.LedgerGroups.Where(x => x.Id == ledger.LedgerGroupId).Select(x => x.LedgerHeadID).FirstOrDefault();
                    }
                    else
                    {
                        ledger.LedgerGroupId = null;
                    }
                    if(ledger.StartingBalance==null)
                    {
                        ledger.StartingBalance = 0;
                    }
                    if(ledger.CurrentBalance==null)
                    {
                        ledger.CurrentBalance = 0;
                    }
                    var uid = User.Identity.GetUserId();
                    ledger.CreatedBy = db.AspNetUsers.Where(x => x.Id == uid).Select(x => x.Name).FirstOrDefault();
                    ledger.CurrentBalance = ledger.StartingBalance;
                    db.Ledgers.Add(ledger);
                    db.SaveChanges();
                    return RedirectToAction("LedgerList");
                }
                catch
                {
                    return RedirectToAction("Create");
                }
            }

            ViewBag.LedgerHeadId = new SelectList(db.LedgerHeads, "Id", "Name", ledger.LedgerHeadId);
            //ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year", ledger.SessionId);
            ViewBag.LedgerGroupId = new SelectList(db.LedgerGroups, "Id", "Name", ledger.LedgerGroupId);
            return View(ledger);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LedgerFromFile(RegisterViewModel model)
        {
            // if (ModelState.IsValid)
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["Ledger"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                var teacherList = new List<RegisterViewModel>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    ApplicationDbContext context = new ApplicationDbContext();
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        Ledger ledger = new  Ledger();

                        ledger.Name = workSheet.Cells[rowIterator, 1].Value.ToString();
                        var Ledgergroup = workSheet.Cells[rowIterator, 2].Value.ToString();
                        if(Ledgergroup=="-")
                        {
                            ledger.LedgerGroupId = null;
                            var head = workSheet.Cells[rowIterator, 3].Value.ToString();
                            ledger.LedgerHeadId = db.LedgerHeads.Where(x => x.Name == head).Select(x => x.Id).FirstOrDefault();
                        }
                        else
                        {
                            ledger.LedgerGroupId = db.LedgerGroups.Where(x => x.Name == Ledgergroup).Select(x => x.Id).FirstOrDefault();
                            ledger.LedgerHeadId = db.LedgerGroups.Where(x => x.Id == ledger.LedgerGroupId).Select(x => x.LedgerHeadID).FirstOrDefault();
                            var head= workSheet.Cells[rowIterator, 3].Value.ToString();
                        }

                        var uid = User.Identity.GetUserId();
                        ledger.CreatedBy = db.AspNetUsers.Where(x => x.Id == uid).Select(x => x.Name).FirstOrDefault();
                        ledger.SessionId = SessionID;
                       
                        var StartingBalance= workSheet.Cells[rowIterator, 4].Value.ToString();
                        ledger.StartingBalance = decimal.Parse(StartingBalance);

                        var CurrentBalance = workSheet.Cells[rowIterator, 5].Value.ToString();
                        ledger.CurrentBalance = decimal.Parse(CurrentBalance);
                        db.Ledgers.Add(ledger);
                        db.SaveChanges();
                    }
                    dbTransaction.Commit();
                    return RedirectToAction("LedgerList");
                }
            }
            catch (Exception e)
            {
                //   ModelState.AddModelError("Error", e.InnerException);
                dbTransaction.Dispose();
                return RedirectToAction("Create", "Ledgers");
            }
        }
        // GET: Ledgers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ledger ledger = db.Ledgers.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            ViewBag.LedgerHeadId = new SelectList(db.LedgerHeads, "Id", "Name", ledger.LedgerHeadId);
            //ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year", ledger.SessionId);
            ViewBag.LedgerGroupId = new SelectList(db.LedgerGroups, "Id", "Name", ledger.LedgerGroupId);
            return View(ledger);
        }

        // POST: Ledgers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartingBalance,CurrentBalance,CreatedBy,LedgerGroupId,SessionId,LedgerHeadId")] Ledger ledger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ledger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ChartsOf_Accounts", "ChartOfAccounts");
            }
            ViewBag.SessionId = new SelectList(db.LedgerHeads, "Id", "Name", ledger.LedgerHeadId);
            //ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year", ledger.SessionId);
            ViewBag.LedgerGroupId = new SelectList(db.LedgerGroups, "Id", "Name", ledger.LedgerGroupId);
            return View(ledger);
        }

        // GET: Ledgers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ledger ledger = db.Ledgers.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            return View(ledger);
        }

        // POST: Ledgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ledger ledger = db.Ledgers.Find(id);
            db.Ledgers.Remove(ledger);
            db.SaveChanges();
            return RedirectToAction("LedgerList");
        }
        public class Ledger_Group
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal? StartingBalance { get; set; }
            public decimal? CurrentBalance { get; set; }
            public string CreatedBy { get; set; }
            public string LedgerGroupId { get; set; }
            public string SessionId { get; set; }
            public string LedgerHeadId { get; set; }

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
