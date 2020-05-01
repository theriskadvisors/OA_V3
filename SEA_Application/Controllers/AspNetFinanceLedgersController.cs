//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SEA_Application.Models;
//using Excel = Microsoft.Office.Interop.Excel;
//using OfficeOpenXml;


//namespace SEA_Application.Controllers
//{
//    public class AspNetFinanceLedgersController : Controller
//    {
//        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

//        // GET: AspNetFinanceLedgers

//        public class Firstf
//        {
//            public string Name { get; set; }
//            public int Id { set; get; }
//            public string Balance { set; get; }
//            public string Type { set; get; }
//            public List<Second> Second { get; set; }
//        }

//        public class Second
//        {
//            public string Name { get; set; }
//            public int Id { set; get; }
//            public string Balance { set; get; }
//            public string Type { set; get; }
//            public List<Third> Third { get; set; }
//        }

//        public class Third
//        {
//            public string Name { get; set; }
//            public int Id { set; get; }
//            public string Balance { set; get; }
//            public string Type { set; get; }
//            public List<Forth> Forth { set; get; }
//        }

//        public class Forth
//        {
//            public string Name { get; set; }
//            public int Id { set; get; }
//            public string Balance { set; get; }
//            public string Type { set; get; }
//        }

       
//        public ActionResult Index()
//        {
//            List<TreeViewNodeVM> rootNodes = (from aspNetFinanceLedger in db.AspNetFinanceLedgers
//                                              where aspNetFinanceLedger.ShowIndividual == "True"
//                                              select new TreeViewNodeVM()
//                                              {
//                                                  Id = aspNetFinanceLedger.Id,
//                                                  Code = aspNetFinanceLedger.Code,
//                                                  Name=aspNetFinanceLedger.Name,
//                                                  Type = aspNetFinanceLedger.Type,
//                                                  Balance = aspNetFinanceLedger.Balance,
//                                                  //LedgerDebit =(double) (db.AspNetFinanceVoucherDetails.Where(x=> x.LedgerId == aspNetFinanceLedger.Id).Select(x=> x.Debit).Sum() + aspNetFinanceLedger.StartingBalance),
//                                                  //LedgerCredit =(double) db.AspNetFinanceVoucherDetails.Where(x => x.LedgerId == aspNetFinanceLedger.Id).Select(x => x.Credit ).Sum()
//                                              }).ToList();


//            //foreach (var item in rootNodes)
//            //{
//            //    int balance = 0;
//            //    balance = Convert.ToInt32(item.Balance);
//            //    var ledgers = db.AspNetFinanceLedgers.Where(x => x.Type == item.Type && x.Head != 0).ToList();
//            //    foreach (var oneLedger in ledgers)
//            //    {
//            //        balance += Convert.ToInt32(oneLedger.Balance);
//            //    }
//            //    rootNodes.Where(d => d.Type == item.Type).First().TotalBalance = balance.ToString();
//            //}

//            foreach (var rootNode in rootNodes)
//            {
//                BuildChildNode(rootNode);
//            }
            

//            return View("TreeView",rootNodes);
//        }

//        private void BuildChildNode(TreeViewNodeVM rootNode)
//        {
//            if (rootNode != null)
//            {
//                List<TreeViewNodeVM> chidNode = (from aspNetFinanceLedger in db.AspNetFinanceLedgers
//                                                 where aspNetFinanceLedger.Head == rootNode.Id
//                                                 select new TreeViewNodeVM()
//                                                 {
//                                                     Id = aspNetFinanceLedger.Id,
//                                                      Code = aspNetFinanceLedger.Code,
//                                                      Name = aspNetFinanceLedger.Name,
//                                                     Type = aspNetFinanceLedger.Type,
//                                                     Balance = aspNetFinanceLedger.Balance
//                                                 }).ToList<TreeViewNodeVM>();

//                if (chidNode.Count > 0)
//                {
//                    foreach (var childRootNode in chidNode)
//                    {
//                        BuildChildNode(childRootNode);
//                        rootNode.ChildNode.Add(childRootNode);
//                    }
//                }
//            }
//        }

//        //public ActionResult Index()
//        //{
            
//        //   var HeadLedgers = db.AspNetFinanceLedgers.Where(x => x.Head == "0" ).ToList();
//        //   var Heads = new List<First>();
//        //   var types = db.AspNetFinanceLedgerTypes.Select(x=> x).ToList();
//        //    ViewBag.Assests = 0;
//        //    ViewBag.Liabilities = 0;
//        //    ViewBag.Expense = 0;
//        //    ViewBag.Equity = 0;
//        //    ViewBag.Income = 0;
//        //    foreach (var LedgerType in types)
//        //    {
//        //        foreach (var item in HeadLedgers)
//        //        {
//        //            First first = new First();
//        //            first.Id = item.Id;
//        //            first.Name = item.Code + " " + item.Name;
//        //            first.Balance = item.Balance;
//        //            int type = int.Parse(item.Type);
//        //            first.Type = db.AspNetFinanceLedgerTypes.Where(x => x.Id == type).Select(x => x.Name).FirstOrDefault().ToString();
//        //            string id = item.Id.ToString();
//        //            first.Second = new List<Second>();

//        //            var items = db.AspNetFinanceLedgers.Where(x => x.Head == id).ToList();
//        //            if (items.Count > 0)
//        //            {
//        //                foreach (var secondlvl in items)
//        //                {
//        //                    Second second = new Second();
//        //                    second.Id = secondlvl.Id;
//        //                    second.Name = secondlvl.Code + " " + secondlvl.Name;
//        //                    second.Balance = secondlvl.Balance;
//        //                    int type1 = int.Parse(secondlvl.Type);
//        //                    second.Type = db.AspNetFinanceLedgerTypes.Where(x => x.Id == type1).Select(x => x.Name).FirstOrDefault().ToString();
//        //                    second.Third = new List<Third>();

//        //                    string id1 = secondlvl.Id.ToString();
//        //                    var third = db.AspNetFinanceLedgers.Where(x => x.Head == id1).ToList();
//        //                    if (third.Count > 0)
//        //                    {
//        //                        foreach (var item4 in third)
//        //                        {
//        //                            Third Third = new Third();
//        //                            Third.Id = item4.Id;
//        //                            Third.Name = item4.Code + " " + item4.Name;
//        //                            Third.Balance = item4.Balance;
//        //                            int type2 = int.Parse(item4.Type);
//        //                            Third.Type = db.AspNetFinanceLedgerTypes.Where(x => x.Id == type2).Select(x => x.Name).FirstOrDefault().ToString();
//        //                            Third.Forth = new List<Forth>();

//        //                            string id2 = item4.Id.ToString();
//        //                            var forthlvl = db.AspNetFinanceLedgers.Where(x => x.Head == id2).ToList();
//        //                            if (forthlvl.Count > 0)
//        //                            {
//        //                                foreach (var last in forthlvl)
//        //                                {
//        //                                    Forth Forth = new Forth();
//        //                                    Forth.Id = last.Id;
//        //                                    Forth.Name = last.Code + " " + last.Name;
//        //                                    Forth.Balance = last.Balance;
//        //                                    int type3 = int.Parse(last.Type);
//        //                                    Forth.Type = db.AspNetFinanceLedgerTypes.Where(x => x.Id == type3).Select(x => x.Name).FirstOrDefault().ToString();

//        //                                    Third.Forth.Add(Forth);
//        //                                }
//        //                            }
//        //                            second.Third.Add(Third);
//        //                        }
//        //                    }
//        //                    first.Second.Add(second);
//        //                }
//        //            }
//        //            Heads.Add(first);
//        //        }
//        //        if (LedgerType.Name == "Assests")
//        //            ViewBag.Assests = Heads;
//        //        else if (LedgerType.Name == "Liabilities")
//        //            ViewBag.Liabilities = Heads;
//        //        else if (LedgerType.Name == "Expense")
//        //            ViewBag.Expense = Heads;
//        //        else if (LedgerType.Name == "Equity")
//        //            ViewBag.Equity = Heads;
//        //        else if(LedgerType.Name == "Income")
//        //            ViewBag.Income = Heads;
//        //    }
            
            
//        //    return View();
//        //   // return Json(Heads,JsonRequestBehavior.AllowGet);
//        //}
        
//        public JsonResult NetProfit()
//        {
//            var Income = Convert.ToInt32( db.AspNetFinanceLedgers.Where(x => x.Code == "02").Select(x => x.Balance).FirstOrDefault());
//            var Expense = Convert.ToInt32( db.AspNetFinanceLedgers.Where(x => x.Code == "04").Select(x => x.Balance).FirstOrDefault());

//            var Netprofit = Income - Expense;

//            return Json(Netprofit, JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult Excel_Data(HttpPostedFileBase excelfile)
//        {

//            var dbTransaction = db.Database.BeginTransaction();
//            try
//            {


//                if (excelfile == null || excelfile.ContentLength == 0)
//                {
//                    TempData["Error"] = "Please select an excel file";
//                    return RedirectToAction("Create", "AspNetFinanceLedgers");
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
//                        ApplicationDbContext context = new ApplicationDbContext();
//                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
//                        {
//                            AspNetFinanceLedger Ledger = new AspNetFinanceLedger();

//                            Ledger.Code = workSheet.Cells[rowIterator, 1].Text.ToString();
//                            Ledger.Name = workSheet.Cells[rowIterator, 2].Text.ToString();
//                            Ledger.Type = workSheet.Cells[rowIterator, 3].Text.ToString();
//                            Ledger.Balance = workSheet.Cells[rowIterator, 4].Text.ToString();
//                            //Ledger.IsActive =Convert.ToBoolean( workSheet.Cells[rowIterator, 5].Text);
//                            //Ledger.IsGroup = Convert.ToBoolean( workSheet.Cells[rowIterator, 6].Text);
//                            //Ledger.TakeAble = Convert.ToBoolean(workSheet.Cells[rowIterator, 7].Text);

//                            var Code = Ledger.Code;
//                            string[] result = Code.Split('-');

//                            var head = "101010";

//                            if (result.Count() == 2)
//                            {
//                                head = result[0];
//                            }
//                            else if (result.Count() == 3)
//                            {
//                                head = result[0] + "-" + result[1];
//                            }
//                            else if (result.Count() == 4)
//                            {
//                                head = result[0] + "-" + result[1] + "-" + result[2];
//                            }

//                            if (head == "101010")
//                            {
//                                Ledger.Head = 0;
//                            }
//                            else
//                            {
//                                Ledger.Head = db.AspNetFinanceLedgers.Where(x => x.Code == head).Select(x => x.Id).FirstOrDefault();
//                            }

//                            Ledger.StartingBalance =Convert.ToDouble(Ledger.Balance);

//                            db.AspNetFinanceLedgers.Add(Ledger);
//                            db.SaveChanges();



//                        }
//                        dbTransaction.Commit();
//                    }

//                    return RedirectToAction("Index", "AspNetFinanceLedgers");
//                }
//                else
//                {
//                    TempData["Error"] = "File type is incorrect";
//                    return RedirectToAction("Create", "AspNetFinanceLedgers");
//                }
//            }
//            catch
//            {

//                dbTransaction.Dispose();
//                TempData["Error"] = "Error in the data of file";
//                return RedirectToAction("Create", "AspNetFinanceLedgers");
//            }

//        }

//        //public ActionResult Index()
//        //{
//        //    var aspNetFinanceLedgers = db.AspNetFinanceLedgers.Include(a => a.AspNetFinancePeriod);
//        //    return View(aspNetFinanceLedgers.ToList());

//        //   // return View(db.AspNetFinanceLedgers.Where(x => x.Head == "0").ToList());
//        //}
        
        
//        // GET: AspNetFinanceLedgers/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceLedger aspNetFinanceLedger = db.AspNetFinanceLedgers.Find(id);
//            if (aspNetFinanceLedger == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFinanceLedger);
//        }

//        [HttpPost]

//        public JsonResult DualCode(string Code)
//        {
//            var check = "False";

//            try
//            {
//                var code = db.AspNetFinanceLedgers.Where(x => x.Code == Code).Select(x => x).FirstOrDefault();
//                if (code != null)
//                    check = "True";
//                else
//                    check = "False";
//            }
//            catch
//            {
//                check = "False";
//            }

//            return Json(check, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public JsonResult HeadFinder(string Head, string Code)
//        {
//            codesecure secure = new codesecure();
//            bool status;
//             secure.head = db.AspNetFinanceLedgers.Where(x => x.Code == Head).Select(x => x.Id).FirstOrDefault();

//            try
//            {
//               var code = db.AspNetFinanceLedgers.Where(x => x.Code == Code).Select(x => x).FirstOrDefault();
//                if (code != null)
//                    status = true;
//                else
//                    status = false;
//            }
//            catch
//            {
//                status = false;
//            }

//            secure.status = status.ToString();

//            return Json(secure, JsonRequestBehavior.AllowGet);
//        }

//        [HttpGet]
//        public ActionResult LedgerDetail(string Code)
//        {
//            var code = Code.Split(' ');
//            var ledger = code[0];
//            var type = db.AspNetFinanceLedgers.Where(x => x.Code == ledger).Select(x => x.Type).FirstOrDefault();
//            var detail = db.AspNetFinanceVoucherDetails.Where(x => x.AspNetFinanceLedger.Code == ledger).ToList();

//            List<LedgerDetails> Details = new List<LedgerDetails>();

//            LedgerDetails StartLedger = new LedgerDetails();
//            StartLedger.credit = 0;
//            StartLedger.debit = 0;
//            StartLedger.code = ledger;
//            StartLedger.Name = db.AspNetFinanceLedgers.Where(x => x.Code == ledger).Select(x => x.Name).FirstOrDefault();
//            StartLedger.description = "Starting Balance";
//            StartLedger.time = "--:--";
//            StartLedger.Balance = db.AspNetFinanceLedgers.Where(x => x.Code == ledger).Select(x => x.StartingBalance).FirstOrDefault().ToString();

//            Details.Add(StartLedger);

//            var StartBalance = double.Parse(StartLedger.Balance);

//            foreach (var item in detail)
//            {
//                LedgerDetails Ledger = new LedgerDetails();
//                Ledger.code = item.AspNetFinanceLedger.Code;
//                Ledger.credit =(double) item.Credit;
//                Ledger.debit = (double)item.Debit;
//                Ledger.Id = item.AspNetFinanceVoucher.Id;
//                Ledger.Type = item.AspNetFinanceLedger.Type;
//                if (type == "Revenue" || type == "Liabilities" || type == "Equity")
//                {
//                    StartBalance += Ledger.credit;
//                    StartBalance -= Ledger.debit;
//                }
//                else if (type == "Assets" || type == "Expense")
//                {
//                    StartBalance += Ledger.debit;
//                    StartBalance -= Ledger.credit;
//                }

//                Ledger.Balance = StartBalance.ToString();
//                Ledger.Name = item.AspNetFinanceLedger.Name;
//                Ledger.description = item.TransactionDescription;
//                DateTime time = (DateTime) db.AspNetFinanceVouchers.Where(x => x.Id == item.VoucherId).Select(x => x.Time).FirstOrDefault();
//                Ledger.time = time.ToShortDateString();
//                Details.Add(Ledger);
//            }

//            ViewBag.Code = code[0];
//            ViewBag.Name = db.AspNetFinanceLedgers.Where(x=> x.Code == ledger).Select(x=> x.Name).FirstOrDefault();
//            Details.Reverse();
//            return View(Details);
//        }

        

//        // GET: AspNetFinanceLedgers/Create
//        public ActionResult Create(string Ledger, string Type)
//        {
//            ViewBag.PeriodId = new SelectList(db.AspNetFinancePeriods, "Id", "Year");
//            ViewBag.Type = new SelectList(db.AspNetFinanceLedgerTypes, "Name", "Name");
//            ViewBag.LedgerCode = Ledger ;
//            ViewBag.LedgerType = Type;
//            return View();
//        }

//        public class SelectLedgerList
//        {
//            public string Value { get; set; }
//            public string Name { get; set; }
//        }

//        public JsonResult SelectListLedgers()
//        {
//            var Ledgers = db.AspNetFinanceLedgers.ToList();
//            List<SelectLedgerList> List = new List<SelectLedgerList>();

//            foreach (var item in Ledgers)
//            {
//                SelectLedgerList Ledger = new SelectLedgerList();
//                Ledger.Value = item.Code;
//                Ledger.Name = item.Code + " " + item.Name;
//                List.Add(Ledger);
//            }
//            return Json(List, JsonRequestBehavior.AllowGet);
//        }

//        // POST: AspNetFinanceLedgers/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,Code,Name,Type,Balance,IsActive,IsGroup,TakeAble,Head")] AspNetFinanceLedger aspNetFinanceLedger)
//        {
//            if (ModelState.IsValid)
//            {
//                aspNetFinanceLedger.Type = Request.Form["Type"];
//                aspNetFinanceLedger.StartingBalance =Convert.ToDouble(aspNetFinanceLedger.Balance);
//                aspNetFinanceLedger.ShowIndividual = Request.Form["individual"];
//                db.AspNetFinanceLedgers.Add(aspNetFinanceLedger);
//                db.SaveChanges();
//                int ammount = Convert.ToInt32(aspNetFinanceLedger.Balance);
//                AddBalanceToParent(ammount, aspNetFinanceLedger.Head);
//                return RedirectToAction("Index");
//            }

//            return View(aspNetFinanceLedger);
//        }

//        void AddBalanceToParent(int ammount,int HeadId)
//        {
//            try
//            {
//                var ledger = db.AspNetFinanceLedgers.Where(x => x.Id == HeadId).Select(x => x).FirstOrDefault();

//                int track = Convert.ToInt32(ledger.Balance) + ammount;
//                ledger.Balance = track.ToString();
//                db.SaveChanges();
//                if (ledger.Head != 0)
//                {
//                    AddBalanceToParent(ammount, ledger.Head);
//                }
//            }
//            catch
//            {

//            }
            
//        }

//        // GET: AspNetFinanceLedgers/Edit/5
//        public ActionResult Edit(string Ledger)
//        {
//            if (Ledger == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceLedger aspNetFinanceLedger = db.AspNetFinanceLedgers.Where(x=> x.Code == Ledger).Select(x=> x).FirstOrDefault();
//            if (aspNetFinanceLedger == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFinanceLedger);
//        }

//        // POST: AspNetFinanceLedgers/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,Code,Name,Type,Balance,IsActive,IsGroup,TakeAble,Head,ShowIndividual,StartBalance")] AspNetFinanceLedger aspNetFinanceLedger)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aspNetFinanceLedger).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(aspNetFinanceLedger);
//        }

//        // GET: AspNetFinanceLedgers/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceLedger aspNetFinanceLedger = db.AspNetFinanceLedgers.Find(id);
//            if (aspNetFinanceLedger == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFinanceLedger);
//        }

//        // POST: AspNetFinanceLedgers/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            AspNetFinanceLedger aspNetFinanceLedger = db.AspNetFinanceLedgers.Find(id);
//            db.AspNetFinanceLedgers.Remove(aspNetFinanceLedger);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        public JsonResult CheckCode(string Code)
//        {
//            bool check;

//            try
//            {
//                var code = db.AspNetFinanceLedgers.Where(x => x.Code == Code);
//                if (code == null || code.Count() == 0)
//                    check = false;
//                else
//                    check = true;
//            }
//            catch
//            {
//                check = false;
//            }

//            return Json(check, JsonRequestBehavior.AllowGet);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//        public class codesecure
//        {
//            public int head { get; set; }
//            public string status { get; set; }
//        }
        
//    }
//}
