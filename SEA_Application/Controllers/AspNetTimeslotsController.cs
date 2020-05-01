using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspNetTimeslotsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetTimeslots
        public ActionResult Index()
        {
            return View(db.AspNetTimeslots.ToList());
        }

        // GET: AspNetTimeslots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTimeslot aspNetTimeslot = db.AspNetTimeslots.Find(id);
            if (aspNetTimeslot == null)
            {
                return HttpNotFound();
            }
            return View(aspNetTimeslot);
        }

        // GET: AspNetTimeslots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetTimeslots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Start_Time,End_Time,Minutes")] AspNetTimeslot aspNetTimeslot)
        {


            var StartDate = Convert.ToDateTime(Request.Form["StartDate"]);
            var EndDate = Convert.ToDateTime(Request.Form["EndDate"]);
            aspNetTimeslot.Start_Time = StartDate;
            aspNetTimeslot.End_Time = EndDate;


            if (ModelState.IsValid)
            {
                db.AspNetTimeslots.Add(aspNetTimeslot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetTimeslot);
        }

        // GET: AspNetTimeslots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTimeslot aspNetTimeslot = db.AspNetTimeslots.Find(id);
            if (aspNetTimeslot == null)
            {
                return HttpNotFound();
            }
            return View(aspNetTimeslot);
        }

        // POST: AspNetTimeslots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Start_Time,End_Time,Minutes")] AspNetTimeslot aspNetTimeslot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetTimeslot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetTimeslot);
        }

        // GET: AspNetTimeslots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTimeslot aspNetTimeslot = db.AspNetTimeslots.Find(id);
            if (aspNetTimeslot == null)
            {
                return HttpNotFound();
            }
            return View(aspNetTimeslot);
        }

        // POST: AspNetTimeslots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetTimeslot aspNetTimeslot = db.AspNetTimeslots.Find(id);
            db.AspNetTimeslots.Remove(aspNetTimeslot);
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
