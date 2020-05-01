using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspnetStudentAttachmentsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspnetStudentAttachments
        public ActionResult Index()
        {
            var aspnetStudentAttachments = db.AspnetStudentAttachments.Include(a => a.AspnetLesson);
            return View(aspnetStudentAttachments.ToList());
        }

        // GET: AspnetStudentAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentAttachment aspnetStudentAttachment = db.AspnetStudentAttachments.Find(id);
            if (aspnetStudentAttachment == null)
            {
                return HttpNotFound();
            }
            return View(aspnetStudentAttachment);
        }

        // GET: AspnetStudentAttachments/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name");
            return View();
        }

        // POST: AspnetStudentAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AspnetStudentAttachment aspnetStudentAttachment)
        {

            HttpPostedFileBase file = Request.Files["Attachment"];

            if (ModelState.IsValid)
            {
                if(file == null)
                {


                if (file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/StudentAttachments/") + fileName);
                    aspnetStudentAttachment.Path = fileName;
                       


                    }
                else
                {
                    aspnetStudentAttachment.Path = "-/-";
                }

                aspnetStudentAttachment.CreationDate = DateTime.Now;
                db.AspnetStudentAttachments.Add(aspnetStudentAttachment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
                ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentAttachment.LessonId);
              return View(aspnetStudentAttachment);

            }


            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentAttachment.LessonId);
            return View(aspnetStudentAttachment);
        }

        // GET: AspnetStudentAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentAttachment aspnetStudentAttachment = db.AspnetStudentAttachments.Find(id);
            if (aspnetStudentAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentAttachment.LessonId);
            return View(aspnetStudentAttachment);
        }

        // POST: AspnetStudentAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LessonId,Path,CreationDate")] AspnetStudentAttachment aspnetStudentAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnetStudentAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentAttachment.LessonId);
            return View(aspnetStudentAttachment);
        }

        // GET: AspnetStudentAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentAttachment aspnetStudentAttachment = db.AspnetStudentAttachments.Find(id);
            if (aspnetStudentAttachment == null)
            {
                return HttpNotFound();
            }
            return View(aspnetStudentAttachment);
        }

        // POST: AspnetStudentAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspnetStudentAttachment aspnetStudentAttachment = db.AspnetStudentAttachments.Find(id);
            db.AspnetStudentAttachments.Remove(aspnetStudentAttachment);
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
