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
    public class AspNetFeedBackFormController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: AspNetFeedBackForm
        public ActionResult Index()
        {
            var aspNetFeedBackForms = db.AspNetFeedBackForms.Where(x=> x.SessionID == SessionID).Include(a => a.AspNetPTMFormRole);
            return View(aspNetFeedBackForms.ToList());
        }

        public ActionResult ParentForm()
        {
            ViewBag.data = "Parent Feedback Form";
            var aspNetFeedBackForms = db.AspNetFeedBackForms.Where(x => x.AspNetPTMFormRole.RoleName == "Parent" && x.SessionID == SessionID).Include(a => a.AspNetPTMFormRole);
            return View("Index", aspNetFeedBackForms.ToList());
        }

        public ActionResult TeacherForm()
        {
            ViewBag.data = "Teacher Feedback Form";
            var aspNetFeedBackForms = db.AspNetFeedBackForms.Where(x => x.AspNetPTMFormRole.RoleName == "Teacher" && x.SessionID == SessionID).Include(a => a.AspNetPTMFormRole);
            return View("Index", aspNetFeedBackForms.ToList());
        }
        
        // GET: AspNetFeedBackForm/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFeedBackForm aspNetFeedBackForm = db.AspNetFeedBackForms.Find(id);
            ViewBag.FormFor = new SelectList(db.AspNetPTMFormRoles, "Id", "RoleName", aspNetFeedBackForm.FormFor);
            if (aspNetFeedBackForm == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFeedBackForm);
        }

        // GET: AspNetFeedBackForm/Create
        public ActionResult Create()
        {
            ViewBag.FormFor = new SelectList(db.AspNetPTMFormRoles, "Id", "RoleName");
            return View();
        }

        // POST: AspNetFeedBackForm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Question,FormFor")] AspNetFeedBackForm aspNetFeedBackForm)
        {
            if (ModelState.IsValid)
            {
                aspNetFeedBackForm.SessionID = SessionID;
                db.AspNetFeedBackForms.Add(aspNetFeedBackForm);
                db.SaveChanges();

                if (aspNetFeedBackForm.FormFor == 2)
                {
                    return RedirectToAction("ParentForm");
                }
                else if (aspNetFeedBackForm.FormFor == 1)
                {
                    return RedirectToAction("TeacherForm");
                }
                return RedirectToAction("Index");
            }

            ViewBag.FormFor = new SelectList(db.AspNetPTMFormRoles, "Id", "RoleName", aspNetFeedBackForm.FormFor);
            return View(aspNetFeedBackForm);
        }

        // GET: AspNetFeedBackForm/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFeedBackForm aspNetFeedBackForm = db.AspNetFeedBackForms.Find(id);
            if (aspNetFeedBackForm == null)
            {
                return HttpNotFound();
            }
            ViewBag.FormFor = new SelectList(db.AspNetPTMFormRoles, "Id", "RoleName", aspNetFeedBackForm.FormFor);
            return View(aspNetFeedBackForm);
        }

        // POST: AspNetFeedBackForm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Question,FormFor")] AspNetFeedBackForm aspNetFeedBackForm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetFeedBackForm).State = EntityState.Modified;
                db.SaveChanges();
                if(aspNetFeedBackForm.FormFor==2)
                {
                    return RedirectToAction("ParentForm");
                }
                else if (aspNetFeedBackForm.FormFor == 1)
                {
                    return RedirectToAction("TeacherForm");
                }

            }
            ViewBag.FormFor = new SelectList(db.AspNetPTMFormRoles, "Id", "RoleName", aspNetFeedBackForm.FormFor);
            return View(aspNetFeedBackForm);
        }

        // GET: AspNetFeedBackForm/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFeedBackForm aspNetFeedBackForm = db.AspNetFeedBackForms.Find(id);
            if (aspNetFeedBackForm == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFeedBackForm);
        }

        // POST: AspNetFeedBackForm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetFeedBackForm aspNetFeedBackForm = db.AspNetFeedBackForms.Find(id);
            db.AspNetFeedBackForms.Remove(aspNetFeedBackForm);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCnfm(int id)
        {
            AspNetFeedBackForm aspNetFeedBackForm = db.AspNetFeedBackForms.Find(id);
            try
            {
                db.AspNetFeedBackForms.Remove(aspNetFeedBackForm);
                db.SaveChanges();
                return RedirectToAction("Index");
              
            }
            catch
            {
                ViewBag.Error = "It can't be deleted";
                ViewBag.FormFor = new SelectList(db.AspNetPTMFormRoles, "Id", "RoleName", aspNetFeedBackForm.FormFor);

                return View("Details", aspNetFeedBackForm);
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
