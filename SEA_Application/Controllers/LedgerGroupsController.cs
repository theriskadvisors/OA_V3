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
using OfficeOpenXml;
using Microsoft.AspNet.Identity;

namespace SEA_Application.Controllers
{
    public class LedgerGroupsController : Controller
    {
        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

        // GET: LedgerGroups
        //public ActionResult Index()
        //{
        //    var ledgerGroups = db.LedgerGroups.Include(l => l.AspNetSession).Include(l => l.LedgerHead);
        //    return View(ledgerGroups.ToList());
        //}
        public ActionResult LedgerList()
        {
            return View();
        }
        public JsonResult GetLedgerGroups()
        {
            var ledger = db.LedgerGroups.ToList();
            List<Ledger_Group> group = new List<Ledger_Group>();
            foreach (var item in ledger)
            {
                Ledger_Group lg = new Ledger_Group();
                lg.Name = item.Name;
                lg.CreatedBy = item.CreatedBy;
                lg.LedgerHeadId = item.LedgerHead.Name;
                lg.SessionId = db.AspNetSessions.Where(x => x.Status == "Active").Select(x => x.SessionName).FirstOrDefault();
                group.Add(lg);
            }
            return Json(group,JsonRequestBehavior.AllowGet);
        }
        // GET: LedgerGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LedgerGroup ledgerGroup = db.LedgerGroups.Find(id);
            if (ledgerGroup == null)
            {
                return HttpNotFound();
            }
            return View(ledgerGroup);
        }

        // GET: LedgerGroups/Create
        public ActionResult Create()
        {
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year");
            ViewBag.LedgerHeadID = new SelectList(db.LedgerHeads, "Id", "Name");
            return View();
        }

        // POST: LedgerGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LedgerHeadID,CreatedBy,SessionId")] LedgerGroup ledgerGroup)
        {
            if (ModelState.IsValid)
            {
                var uid = User.Identity.GetUserId();
                ledgerGroup.CreatedBy = db.AspNetUsers.Where(x => x.Id == uid).Select(x => x.Name).FirstOrDefault();
                db.LedgerGroups.Add(ledgerGroup);
                db.SaveChanges();
                return RedirectToAction("LedgerList");
            }

            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName");
            ViewBag.LedgerHeadID = new SelectList(db.LedgerHeads, "Id", "Name", ledgerGroup.LedgerHeadID);
            return View(ledgerGroup);
        }

        // GET: LedgerGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LedgerGroup ledgerGroup = db.LedgerGroups.Find(id);
            if (ledgerGroup == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year", ledgerGroup.SessionId);
            ViewBag.LedgerHeadID = new SelectList(db.LedgerHeads, "Id", "Name", ledgerGroup.LedgerHeadID);
            return View(ledgerGroup);
        }

        // POST: LedgerGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LedgerHeadID,CreatedBy,SessionId")] LedgerGroup ledgerGroup)
        {
            if (ModelState.IsValid)
            {
               if(ledgerGroup.Name=="Cash" || ledgerGroup.Name==null || ledgerGroup.Name=="Bank")
                {
                    ViewBag.ErrorMessage = "Can't Edit this page";

                }
                else
                {
                    db.Entry(ledgerGroup).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ChartsOf_Accounts", "ChartOfAccounts");
                }

               
            }
            //ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year", ledgerGroup.SessionId);
            ViewBag.LedgerHeadID = new SelectList(db.LedgerHeads, "Id", "Name", ledgerGroup.LedgerHeadID);
            return View("Edit", ledgerGroup);
            //return RedirectToAction("Edit","LedgerGroups",new {id=ledgerGroup.Id });
        }

        // GET: LedgerGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LedgerGroup ledgerGroup = db.LedgerGroups.Find(id);
            if (ledgerGroup == null)
            {
                return HttpNotFound();
            }
            return View(ledgerGroup);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LedgerGroupFromFile(RegisterViewModel model)
        {
            // if (ModelState.IsValid)
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["Ledger_Groups"];
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
                        LedgerGroup ledgergroup = new LedgerGroup();

                        ledgergroup.Name = workSheet.Cells[rowIterator, 1].Value.ToString();
                        var Ledgerhead = workSheet.Cells[rowIterator, 2].Value.ToString();
                        ledgergroup.LedgerHeadID = db.LedgerHeads.Where(x => x.Name == Ledgerhead).Select(x => x.Id).FirstOrDefault();
                        var uid = User.Identity.GetUserId();
                        ledgergroup.CreatedBy = db.AspNetUsers.Where(x => x.Id == uid).Select(x => x.Name).FirstOrDefault();
                        
                        //ledgergroup.SessionId = db.AspNetSessions.Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
                        db.LedgerGroups.Add(ledgergroup);
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
                return RedirectToAction("Create", "LedgerGroups");
            }
        }

        // POST: LedgerGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LedgerGroup ledgerGroup = db.LedgerGroups.Find(id);
            db.LedgerGroups.Remove(ledgerGroup);
            db.SaveChanges();
            return RedirectToAction("LedgerList");
        }
        public class Ledger_Group
        {
            public string Name { get; set; }
            public string CreatedBy { get; set; }
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
