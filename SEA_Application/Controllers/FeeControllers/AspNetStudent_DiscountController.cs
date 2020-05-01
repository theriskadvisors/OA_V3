//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SEA_Application.Models;

//namespace SEA_Application.Controllers.FeeControllers
//{
//    public class AspNetStudent_DiscountController : Controller
//    {
//        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

//        // GET: AspNetStudent_Discount
//        public ActionResult Index()
//        {
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            return View();
//        }

//        public JsonResult StudentsDiscountByClass(int id)
//        {
//            var students = (from student in db.AspNetStudents
//                            where student.ClassID == id
//                            join discount in db.AspNetStudent_Discount on student.StudentID equals discount.StudentID
//                            select new { student.AspNetUser.Id, classId = student.AspNetClass.Id, student.AspNetClass.ClassName, student.AspNetUser.UserName, student.AspNetUser.Name }
//                            ).Distinct().ToList();

//            return Json(students, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult StudentsDiscount()
//        {
//            var students = (from student in db.AspNetStudents
//                            join discount in db.AspNetStudent_Discount on student.StudentID equals discount.StudentID
//                            select new { student.AspNetUser.Id, classId = student.AspNetClass.Id, student.AspNetClass.ClassName, student.AspNetUser.UserName, student.AspNetUser.Name }
//                            ).Distinct().ToList();

//            return Json(students, JsonRequestBehavior.AllowGet);
//        }

//        // GET: AspNetStudent_Discount/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetStudent_Discount aspNetStudent_Discount = db.AspNetStudent_Discount.Find(id);
//            if (aspNetStudent_Discount == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetStudent_Discount);
//        }

//        // GET: AspNetStudent_Discount/Create
//        public ActionResult Create()
//        {
//            ViewBag.DiscountID = new SelectList(db.AspNetDiscountTypes, "Id", "TypeName");
//            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
//            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Name");
//            return View();
//        }

//        public JsonResult ClassByStudent(string ID)
//        {
//            var classId = db.AspNetStudents.Where(x => x.StudentID == ID).Select(x => x.ClassID).FirstOrDefault();
//            return Json(classId, JsonRequestBehavior.AllowGet);
//        }

//        // POST: AspNetStudent_Discount/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,StudentID,DiscountID,Percentage")] AspNetStudent_Discount aspNetStudent_Discount)
//        {
//            var TransactionObj = db.Database.BeginTransaction();
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    db.AspNetStudent_Discount.Add(aspNetStudent_Discount);
//                    db.SaveChanges();
//                }
//                TransactionObj.Commit();
//                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//                var UserNameLog = User.Identity.Name;
//                AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
//                string UserIDLog = a.Id;
//                var classObjLog = Request.Form["ClassID"];
//                var logMessage = "Discount Added Percentage: " + aspNetStudent_Discount.Percentage + ", Student ID: " + aspNetStudent_Discount.StudentID + ", Discount ID: " + aspNetStudent_Discount.DiscountID;

//                var LogControllerObj = new AspNetLogsController();
//                LogControllerObj.CreateLogSave(logMessage, UserIDLog);
//                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//                return RedirectToAction("Index");
//            }


//            catch (Exception)
//            {
//                TransactionObj.Dispose();
//            }
//            ViewBag.DiscountID = new SelectList(db.AspNetDiscountTypes, "Id", "TypeName", aspNetStudent_Discount.DiscountID);
//            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Discount.StudentID);
//            return View(aspNetStudent_Discount);
//        }

//        // GET: AspNetStudent_Discount/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetStudent_Discount aspNetStudent_Discount = db.AspNetStudent_Discount.Find(id);
//            if (aspNetStudent_Discount == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.DiscountID = new SelectList(db.AspNetDiscountTypes, "Id", "TypeName", aspNetStudent_Discount.DiscountID);
//            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Discount.StudentID);
//            return View(aspNetStudent_Discount);
//        }

//        // POST: AspNetStudent_Discount/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,StudentID,DiscountID,Percentage")] AspNetStudent_Discount aspNetStudent_Discount)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aspNetStudent_Discount).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.DiscountID = new SelectList(db.AspNetDiscountTypes, "Id", "TypeName", aspNetStudent_Discount.DiscountID);
//            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Discount.StudentID);
//            return View(aspNetStudent_Discount);
//        }

//        // GET: AspNetStudent_Discount/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetStudent_Discount aspNetStudent_Discount = db.AspNetStudent_Discount.Find(id);
//            if (aspNetStudent_Discount == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetStudent_Discount);
//        }

//        public ActionResult DeleteConfirmed(int id)
//        {
//            AspNetStudent_Discount aspNetStudent_Discount = db.AspNetStudent_Discount.Find(id);
//            try
//            {
//                db.AspNetStudent_Discount.Remove(aspNetStudent_Discount);
//                db.SaveChanges();
//                return Json("True", JsonRequestBehavior.AllowGet);
//            }
//            catch
//            {
//                ViewBag.Error = "It can't be deleted";
//                return Json("False", JsonRequestBehavior.AllowGet);
//            }
//        }

//        public JsonResult GetStudentDiscountsApplicable(string id)
//        {
//            var studentDiscountsApplicable = db.AspNetStudent_Discount_Applicable.Where(x => x.StudentId == id).Select(x => new { x.ClassFeeTypeId }).ToList();
//            return Json(studentDiscountsApplicable, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult GetStudentDiscounts(string id)
//        {
//            var studentDiscounts = db.AspNetStudent_Discount.Where(x => x.StudentID == id).Select(x => new { x.Id, x.DiscountID, x.Percentage }).ToList();
//            return Json(studentDiscounts, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult FeeByClass(int id)
//        {
//            var FeeTypes = (from ClassFee in db.AspNetClass_FeeType
//                            where ClassFee.ClassID == id
//                            select new { ClassFee.Id, ClassFee.AspNetFinanceLedger.Name }).ToList();
//            return Json(FeeTypes, JsonRequestBehavior.AllowGet);
//        }


//        public JsonResult StudentsByClass(int id)
//        {
//            var students = (from student in db.AspNetStudents
//                            where student.ClassID == id
//                            select new { student.AspNetUser.Id, student.AspNetUser.UserName, student.AspNetUser.Name, student.AspNetClass.ClassName }).ToList();
//            return Json(students, JsonRequestBehavior.AllowGet);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        public class Discount
//        {
//            public int Id { get; set; }
//            public int DiscountID { get; set; }
//            public int Percentage { get; set; }
//        }

//        public class StudentDiscount
//        {
//            public string StudentID { get; set; }
//            public List<Discount> Discounts { get; set; }
//            public string[] DiscountApplicable { get; set; }
//        }


//        [HttpPost]
//        public JsonResult AddStudent_Discount(StudentDiscount StudentDiscounts)
//        {
//            string StudentID = StudentDiscounts.StudentID;
//            var dbContextTransaction = db.Database.BeginTransaction();

//            try
//            {
//                foreach (var discounts in StudentDiscounts.Discounts)
//                {
//                    if (discounts.Id == -1)
//                    {
//                        AspNetStudent_Discount studentdiscount = new AspNetStudent_Discount();
//                        studentdiscount.StudentID = StudentID;
//                        studentdiscount.Percentage = discounts.Percentage;
//                        studentdiscount.DiscountID = discounts.DiscountID;
//                        db.AspNetStudent_Discount.Add(studentdiscount);
//                        db.SaveChanges();
//                    }
//                    else
//                    {
//                        AspNetStudent_Discount studentdiscount = db.AspNetStudent_Discount.Where(x => x.Id == discounts.Id).FirstOrDefault();
//                        studentdiscount.StudentID = StudentID;
//                        studentdiscount.Percentage = discounts.Percentage;
//                        studentdiscount.DiscountID = discounts.DiscountID;
//                        db.SaveChanges();
//                    }

//                }
//            }
//            catch
//            {

//                dbContextTransaction.Dispose();
//                return Json("False", JsonRequestBehavior.AllowGet);
//            }
//            try
//            {
//                int[] discountApplicable = StudentDiscounts.DiscountApplicable.Select(int.Parse).ToArray();
//                var existingStudentDiscounts = db.AspNetStudent_Discount_Applicable.Where(x => x.StudentId == StudentID).ToList();
//                foreach (var item in existingStudentDiscounts)
//                {
//                    db.AspNetStudent_Discount_Applicable.Remove(item);
//                    db.SaveChanges();
//                }
//                foreach (var discountapplicable in discountApplicable)
//                {
//                    AspNetStudent_Discount_Applicable student_Discoint_Applicable = new AspNetStudent_Discount_Applicable();
//                    student_Discoint_Applicable.StudentId = StudentID;
//                    student_Discoint_Applicable.ClassFeeTypeId = discountapplicable;
//                    db.AspNetStudent_Discount_Applicable.Add(student_Discoint_Applicable);
//                    db.SaveChanges();
//                }
//            }
//            catch
//            {

//                dbContextTransaction.Dispose();
//                return Json("False", JsonRequestBehavior.AllowGet);
//            }

//            dbContextTransaction.Commit();
//            return Json("True", JsonRequestBehavior.AllowGet);
//        }
//    }
//}
