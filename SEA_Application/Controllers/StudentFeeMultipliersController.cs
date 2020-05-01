//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SEA_Application.Models;

//namespace SEA_Application.Controllers
//{
//    public class StudentFeeMultipliersController : Controller
//    {
//        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

//        // GET: StudentFeeMultipliers
//        public ActionResult Index()
//        {
//            var studentFeeMultipliers = db.StudentFeeMultipliers.Include(s => s.AspNetStudent);
//            return View(studentFeeMultipliers.ToList());
//        }

//        // GET: StudentFeeMultipliers/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            StudentFeeMultiplier studentFeeMultiplier = db.StudentFeeMultipliers.Find(id);
//            if (studentFeeMultiplier == null)
//            {
//                return HttpNotFound();
//            }
//            return View(studentFeeMultiplier);
//        }
//        public JsonResult GetTotalFee(int stdid)
//        {
//            var classid = db.AspNetStudents.Where(x => x.Id == stdid).Select(x => x.ClassId).FirstOrDefault();
//            var totalfee = db.StudentRecurringFees.Where(x => x.ClassId == classid).Select(x => x.TotalFee).FirstOrDefault();
//            return Json(totalfee,JsonRequestBehavior.AllowGet);
//        }
//        // GET: StudentFeeMultipliers/Create
//        public ActionResult Create()
//        {
//            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name");
//            return View();
//        }
//        public JsonResult EditMultiplier(int mul,int stdid)
//       {

//            double? remaining = 0;
//            var list = db.StudentFeeMonths.Where(x => x.StudentId == stdid && x.Status == "Pending").Select(x => x.InstalmentAmount).ToList();
//            foreach (var item in list)
//            {
//                remaining += item;
//            }
//            var Instalments = Convert.ToInt32(12 / mul);
//            var SharePerInstalment = remaining / Instalments;
//           var RemainingInstalments = Convert.ToDouble(remaining / SharePerInstalment);
//            var result = new { remaining, Instalments, SharePerInstalment, RemainingInstalments };
           
//            return Json(result,JsonRequestBehavior.AllowGet);
//        }
//        // POST: StudentFeeMultipliers/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,StudentId,TutionFee,PaidAmount,RemainingAmount,SharePerInstalment,Instalments,PaidInstalments,RemainingInstalments,Multiplier")] StudentFeeMultiplier studentFeeMultiplier)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var checkbox = Request.Form["MonthName"];
//                    if(checkbox==null)
//                    {
//                        ViewBag.ErrorMessage = "No Month Selected, Please Select Month";
//                    }
//                    else
//                    {
//                        List<string> names = new List<string>(checkbox.Split(','));
//                        if(names.Count!=studentFeeMultiplier.Instalments)
//                        {
//                            ViewBag.ErrorMessage = "No of Months and Instalments are not same";
//                        }
//                        else
//                        {
//                            var namecount = names.Count;
//                            db.StudentFeeMultipliers.Add(studentFeeMultiplier);
//                            db.SaveChanges();
//                            foreach (var item in names)
//                            {
//                                StudentFeeMonth stdfeemonth = new StudentFeeMonth();
//                                stdfeemonth.StudentId = studentFeeMultiplier.StudentId;
//                                stdfeemonth.Status = "Pending";
//                                stdfeemonth.InstalmentAmount = studentFeeMultiplier.SharePerInstalment;
//                                var dddd = DateTime.Now;
//                                var d = dddd.ToString("yyyy-MM-dd");
//                                stdfeemonth.IssueDate = d;
//                                stdfeemonth.Months = item;
//                                db.StudentFeeMonths.Add(stdfeemonth);
//                                db.SaveChanges();
//                            }
//                            return RedirectToAction("Index");
//                        }                       
//                    }  
//                }
//            }
//            catch(Exception e)
//            {
//                ViewBag.ErrorMessage = e.Message;
//                return View(studentFeeMultiplier);
//            }


//            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMultiplier.StudentId);
//            return View(studentFeeMultiplier);
//        }

//        // GET: StudentFeeMultipliers/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            StudentFeeMultiplier studentFeeMultiplier = db.StudentFeeMultipliers.Find(id);
//            if (studentFeeMultiplier == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMultiplier.StudentId);
//            return View(studentFeeMultiplier);
//        }

//        // POST: StudentFeeMultipliers/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,StudentId,TutionFee,PaidAmount,RemainingAmount,SharePerInstalment,Instalments,PaidInstalments,TotalPayableFee,RemainingInstalments,Multiplier")] StudentFeeMultiplier studentFeeMultiplier)
//        {
//            if (ModelState.IsValid)
//            {
//                var checkbox = Request.Form["MonthName"];
//                if(checkbox==null)
//                {
//                    ViewBag.ErrorMessage = "No Month selected, Please select months";

//                }
//                else
//                {
//                    List<string> names = new List<string>(checkbox.Split(','));
//                    if (names.Count != studentFeeMultiplier.Instalments)
//                    {
//                        ViewBag.ErrorMessage = "No. of Months and instalments are not same";

//                    }
//                    else
//                    {
//                        var tdfee = db.StudentFeeMonths.Where(x => x.StudentId == studentFeeMultiplier.StudentId && x.Status == "Pending").ToList();
//                        foreach (var item in tdfee)
//                        {
//                            StudentFeeMonth std = db.StudentFeeMonths.Where(x => x.Id == item.Id).FirstOrDefault();
//                            db.StudentFeeMonths.Remove(std);
//                            db.SaveChanges();
//                        }
//                        foreach (var item in names)
//                        {
//                            StudentFeeMonth stdfeemonth = new StudentFeeMonth();
//                            stdfeemonth.StudentId = studentFeeMultiplier.StudentId;
//                            stdfeemonth.Status = "Pending";
//                            stdfeemonth.InstalmentAmount = studentFeeMultiplier.SharePerInstalment;
//                            var dddd = DateTime.Now;
//                            var d = dddd.ToString("yyyy-MM-dd");
//                            stdfeemonth.IssueDate = d;
//                            stdfeemonth.Months = item;
//                            db.StudentFeeMonths.Add(stdfeemonth);
//                            db.SaveChanges();

//                        }
//                        double? remaining = 0;
//                        var list = db.StudentFeeMonths.Where(x => x.StudentId == studentFeeMultiplier.StudentId && x.Status == "Pending").Select(x => x.InstalmentAmount).ToList();
//                        foreach (var item in list)
//                        {
//                            remaining += item;
//                        }
//                        studentFeeMultiplier.RemainingAmount = remaining;
//                        studentFeeMultiplier.PaidAmount = studentFeeMultiplier.TutionFee - remaining;
//                        studentFeeMultiplier.Instalments = Convert.ToInt32(12 / studentFeeMultiplier.Multiplier);
//                        studentFeeMultiplier.SharePerInstalment = studentFeeMultiplier.RemainingAmount / studentFeeMultiplier.Instalments;
//                        studentFeeMultiplier.RemainingInstalments = Convert.ToDouble(studentFeeMultiplier.RemainingAmount / studentFeeMultiplier.SharePerInstalment);
//                        db.Entry(studentFeeMultiplier).State = EntityState.Modified;
//                        db.SaveChanges();
//                        return RedirectToAction("Index");
//                    }

//                }
//            }
               
//            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMultiplier.StudentId);
//            return View(studentFeeMultiplier);
//        }

//        // GET: StudentFeeMultipliers/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            StudentFeeMultiplier studentFeeMultiplier = db.StudentFeeMultipliers.Find(id);
//            if (studentFeeMultiplier == null)
//            {
//                return HttpNotFound();
//            }
//            return View(studentFeeMultiplier);
//        }

//        // POST: StudentFeeMultipliers/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            StudentFeeMultiplier studentFeeMultiplier = db.StudentFeeMultipliers.Find(id);
//            db.StudentFeeMultipliers.Remove(studentFeeMultiplier);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
