using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspNetVirtualRolesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetVirtualRoles
        public ActionResult Index()
        {
            return View(db.AspNetVirtualRoles.ToList());
        }

        public ActionResult Excel_Data(HttpPostedFileBase excelfile)
        {
            var dbTransaction = db.Database.BeginTransaction();

            try
            {

                if (excelfile == null || excelfile.ContentLength == 0)
                {
                    TempData["Error"] = "Please select an excel file";
                    return RedirectToAction("Create", "AspNetVirtualRoles");
                }
                else if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {

                    HttpPostedFileBase file = excelfile;   // Request.Files["excelfile"];

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        ApplicationDbContext context = new ApplicationDbContext();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            AspNetVirtualRole Vr = new AspNetVirtualRole();

                            Vr.Name = workSheet.Cells[rowIterator, 1].Text.ToString();

                            db.AspNetVirtualRoles.Add(Vr);
                            db.SaveChanges();
                        }
                        dbTransaction.Commit();
                    }
                    return RedirectToAction("Index", "AspNetVirtualRoles");
                }
                else
                {
                    TempData["Error"] = "File type is incorrect";
                    return RedirectToAction("Create", "AspNetVirtualRoles");
                }
            }
            catch
            {
                dbTransaction.Dispose();
                TempData["Error"] = "Incorrect Data in files";
                return RedirectToAction("Create", "AspNetVirtualRoles");
            }
        }

        // GET: AspNetVirtualRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetVirtualRole aspNetVirtualRole = db.AspNetVirtualRoles.Find(id);
            if (aspNetVirtualRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetVirtualRole);
        }

        // GET: AspNetVirtualRoles/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetVirtualRoles, "Id", "Name");
            ViewBag.Id = new SelectList(db.AspNetVirtualRoles, "Id", "Name");
            return View();
        }

        // POST: AspNetVirtualRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AspNetVirtualRole aspNetVirtualRole)
        {
            if (ModelState.IsValid)
            {
                db.AspNetVirtualRoles.Add(aspNetVirtualRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetVirtualRole.Id);
            ViewBag.Id = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetVirtualRole.Id);
            return View(aspNetVirtualRole);
        }

        // GET: AspNetVirtualRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetVirtualRole aspNetVirtualRole = db.AspNetVirtualRoles.Find(id);
            if (aspNetVirtualRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetVirtualRole.Id);
            ViewBag.Id = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetVirtualRole.Id);
            return View(aspNetVirtualRole);
        }

        // POST: AspNetVirtualRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AspNetVirtualRole aspNetVirtualRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetVirtualRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetVirtualRole.Id);
            ViewBag.Id = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetVirtualRole.Id);
            return View(aspNetVirtualRole);
        }

        // GET: AspNetVirtualRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetVirtualRole aspNetVirtualRole = db.AspNetVirtualRoles.Find(id);
            if (aspNetVirtualRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetVirtualRole);
        }

        // POST: AspNetVirtualRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetVirtualRole aspNetVirtualRole = db.AspNetVirtualRoles.Find(id);
            db.AspNetVirtualRoles.Remove(aspNetVirtualRole);
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
