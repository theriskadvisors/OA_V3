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
    public class AspNetPTMFormRoleController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: AspNetPTMFormRole
        public ActionResult Index()
        {
            return View(db.AspNetPTMFormRoles.Where(x=> x.AspNetFeedBackForms.Any(y=> y.SessionID == SessionID )).ToList());
        }

        // GET: AspNetPTMFormRole/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTMFormRole aspNetPTMFormRole = db.AspNetPTMFormRoles.Find(id);
            if (aspNetPTMFormRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTMFormRole);
        }

        // GET: AspNetPTMFormRole/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetPTMFormRole/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoleName")] AspNetPTMFormRole aspNetPTMFormRole)
        {
            if (ModelState.IsValid)
            {
                db.AspNetPTMFormRoles.Add(aspNetPTMFormRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetPTMFormRole);
        }

        // GET: AspNetPTMFormRole/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTMFormRole aspNetPTMFormRole = db.AspNetPTMFormRoles.Find(id);
            if (aspNetPTMFormRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTMFormRole);
        }

        // POST: AspNetPTMFormRole/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoleName")] AspNetPTMFormRole aspNetPTMFormRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetPTMFormRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetPTMFormRole);
        }

        // GET: AspNetPTMFormRole/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTMFormRole aspNetPTMFormRole = db.AspNetPTMFormRoles.Find(id);
            if (aspNetPTMFormRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTMFormRole);
        }

        // POST: AspNetPTMFormRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetPTMFormRole aspNetPTMFormRole = db.AspNetPTMFormRoles.Find(id);
            db.AspNetPTMFormRoles.Remove(aspNetPTMFormRole);
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
