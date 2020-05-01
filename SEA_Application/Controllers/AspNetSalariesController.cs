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
    public class AspNetSalariesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetSalaries
        public ActionResult Index()
        {
            var aspNetSalaries = db.AspNetSalaries.Include(a => a.AspNetVirtualRole);
            return View(aspNetSalaries.ToList());
        }

        public ActionResult SaveIndex()
        {
            ViewBag.data = "Salaries has been created and updated";
            var aspNetSalaries = db.AspNetSalaries.Include(a => a.AspNetVirtualRole);
            return View("Index" , aspNetSalaries.ToList());
        }

        // GET: AspNetSalaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSalary aspNetSalary = db.AspNetSalaries.Find(id);
            if (aspNetSalary == null)
            {
                return HttpNotFound();
            }
            ViewBag.VirtualRoleID = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetSalary.VirtualRoleID);
            return View(aspNetSalary);
        }

        // GET: AspNetSalaries/Create
        public ActionResult Create()
        {
            ViewBag.VirtualRoleID = new SelectList(db.AspNetVirtualRoles, "Id", "Name");
            return View();
        }

        // POST: AspNetSalaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Year,Month,Title,VirtualRoleID")] AspNetSalary aspNetSalary)
        {
            if (ModelState.IsValid)
            {
                db.AspNetSalaries.Add(aspNetSalary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VirtualRoleID = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetSalary.VirtualRoleID);
            return View(aspNetSalary);
        }

        // GET: AspNetSalaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSalary aspNetSalary = db.AspNetSalaries.Find(id);
            if (aspNetSalary == null)
            {
                return HttpNotFound();
            }
            ViewBag.VirtualRoleID = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetSalary.VirtualRoleID);
            return View(aspNetSalary);
        }

        // POST: AspNetSalaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Month,Title,VirtualRoleID")] AspNetSalary aspNetSalary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetSalary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VirtualRoleID = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetSalary.VirtualRoleID);
            return View(aspNetSalary);
        }

        // GET: AspNetSalaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSalary aspNetSalary = db.AspNetSalaries.Find(id);
            if (aspNetSalary == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSalary);
        }

        // POST: AspNetSalaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetSalary aspNetSalary = db.AspNetSalaries.Find(id);
            db.AspNetSalaries.Remove(aspNetSalary);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCnfm(int id)
        {
            AspNetSalary aspNetSalary = db.AspNetSalaries.Find(id);
            try
            {
                db.AspNetSalaries.Remove(aspNetSalary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "It can't be deleted";
                ViewBag.VirtualRoleID = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetSalary.VirtualRoleID);

                return View("Details", aspNetSalary);
            }
          
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
