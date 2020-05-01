using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class ChartOfAccountsController : Controller
    {
        SEA_DatabaseEntities db = new  SEA_DatabaseEntities();
        // GET: ChartOfAccounts
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChartsOf_Accounts()
        {
            return View();
        }
        public JsonResult GetChart()
        {
            var headlist = db.LedgerHeads.OrderBy(x=>x.Name).ToList();
            List<Ledger_Head> ledgerheadlist = new List<Ledger_Head>(); 
            foreach (var h_item in headlist)
            {
                Ledger_Head ledgerhead = new Ledger_Head();
                ledgerhead.HeadName = h_item.Name;
                ledgerhead.HeadId = h_item.Id;

                var grouplist = db.LedgerGroups.Where(x => x.LedgerHeadID == h_item.Id).OrderBy(x=>x.Name).ToList();
                var _ledgerlist = db.Ledgers.Where(x => x.LedgerHeadId == h_item.Id).OrderBy(x=>x.Name).ToList();

                ledgerhead.ledgerGroup = new List<Ledger_Group>();
                ledgerhead.ledger = new List<_Ledger>();
                foreach (var g_item in grouplist)
                {
                    Ledger_Group lg = new Ledger_Group();
                    lg.GroupId = g_item.Id;
                    lg.GroupName = g_item.Name;
                    lg.ledger = new List<_Ledger>();

                    var ledgerlist = db.Ledgers.Where(x => x.Id == lg.GroupId).OrderBy(x=>x.Name).ToList();
                    foreach (var l_item in ledgerlist)
                    {
                        _Ledger l = new _Ledger();
                        l.LedgerId = l_item.Id;
                        l.LedgerName = l_item.Name;
                        lg.ledger.Add(l);
                    }
                    ledgerhead.ledgerGroup.Add(lg);
                }
                foreach (var item in _ledgerlist)
                {
                    _Ledger l = new _Ledger();
                    l.LedgerId = item.Id;
                    l.LedgerName = item.Name;
                    ledgerhead.ledger.Add(l);
                }
                ledgerheadlist.Add(ledgerhead);
            }
            return Json(ledgerheadlist,JsonRequestBehavior.AllowGet);
        } 
        public class Ledger_Head
        {
            public string HeadName { get; set; }
            public int HeadId { get; set; }
            public List<Ledger_Group> ledgerGroup { get; set; }
            public List<_Ledger> ledger { get; set; }

        }
        public class Ledger_Group
        {
            public int GroupId { get; set; }
            public string GroupName { get; set; }
            public List<_Ledger> ledger { get; set; }
        }
        public class _Ledger
        {
            public int LedgerId { get; set; }
            public string LedgerName { get; set; }
        }

    }
}