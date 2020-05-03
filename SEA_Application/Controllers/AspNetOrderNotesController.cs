using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspNetOrderNotesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetOrderNotes
        public ActionResult Index()
        {
            var aspNetNotes = db.AspNetNotes.Include(a => a.AspNetSubject);
            return View(aspNetNotes.ToList());
        }
        public ActionResult PrepaidNotesIndex()
        {
            //TimeZone time2 = TimeZone.CurrentTimeZone;
            //DateTime test = time2.ToUniversalTime(DateTime.Now);
            //var singapore = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            //var singaporetime = TimeZoneInfo.ConvertTimeFromUtc(test, singapore);

            var aspNetNotes = db.AspNetNotes.Include(a => a.AspNetSubject);

            var ListOfIds = (from Notes in db.AspNetNotes
                             join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID
                             where OrderNotes.OrderType == "Prepaid" 
                             select new
                             {
                                 Notes.Id
                             }).Distinct().ToList() ;

            ViewBag.Id = ListOfIds;


            return View(aspNetNotes.ToList());



        }

        public ActionResult DisableOrderButtons()
        {
            var CurrentUserId = User.Identity.GetUserId();
            int StudentId = db.AspNetStudents.Where(x => x.StudentID == CurrentUserId).FirstOrDefault().Id;

            var ListOfIds = (from Notes in db.AspNetNotes
                             join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID
                             where OrderNotes.OrderType == "Prepaid"  && OrderNotes.StudentID == StudentId
                             select new
                             {
                                 Notes.Id
                             }).Distinct().ToList();

            return Json(ListOfIds, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrepaidOrderToCart(int Id )
        {
            var CurrentUserId = User.Identity.GetUserId();
            int StudentId = db.AspNetStudents.Where(x => x.StudentID == CurrentUserId).FirstOrDefault().Id;
            AspNetNotesOrder NotesOrder = new AspNetNotesOrder();
            NotesOrder.NotesID = Id;
            NotesOrder.StudentID = StudentId;
            NotesOrder.CreationDate = DateTime.Now;
            NotesOrder.Quantity = 1;
            NotesOrder.Status = "Draft";
            NotesOrder.OrderType = "Prepaid";
            db.AspNetNotesOrders.Add(NotesOrder);


            db.SaveChanges();

            return RedirectToAction("PrepaidNotesIndex");

        }
        public ActionResult PrepaidOrdersToShowInCart()
        {
            var CurrentUserId = User.Identity.GetUserId();

            int StudentId = db.AspNetStudents.Where(x => x.StudentID == CurrentUserId).FirstOrDefault().Id;

            var result = from Notes in db.AspNetNotes
                         join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID
                         where OrderNotes.StudentID == StudentId && OrderNotes.Status =="Draft" &&
                         OrderNotes.OrderType== "Prepaid"

                         select new
                         {
                             Id = OrderNotes.Id,
                             Title = Notes.Title,
                             Discription = Notes.Description,
                             CourseType = Notes.CourseType,
                             Price = Notes.GrandTotal,
                             Quantity = OrderNotes.Quantity,
                             Total = Notes.GrandTotal * OrderNotes.Quantity,
                             Status = OrderNotes.Status,

                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrepaidDeleteOrders(int[] DeleteOrders)
        {
            List<AspNetNotesOrder> AllNotesOrder = db.AspNetNotesOrders.ToList();

            List<AspNetNotesOrder> OrdersToDelete = new List<AspNetNotesOrder>();

            foreach (var deleteOrderIds in DeleteOrders)
            {

                foreach (var order in AllNotesOrder)
                {

                    if (deleteOrderIds == order.Id)

                    {
                        OrdersToDelete.Add(order);


                    }
                }


            }

            db.AspNetNotesOrders.RemoveRange(OrdersToDelete);
            db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);


        }

        public ActionResult PrepaidConfirmOdr(int TotalAmount, int[] IDs)
        {

            AspNetOrder order = new AspNetOrder();
            order.TotalAmount = TotalAmount;
            order.OrderType = "Prepaid";
            order.Status = "Pending";

            order.PublishDate = DateTime.Now;


            db.AspNetOrders.Add(order);

            db.SaveChanges();


            int OrderId = order.Id;

            List<AspNetNotesOrder> AllNotesOrder = db.AspNetNotesOrders.ToList();

            List<AspNetNotesOrder> OrdersToModify = new List<AspNetNotesOrder>();

            foreach (var OrderIds in IDs)
            {

                foreach (var findOrder in AllNotesOrder)
                {

                    if (OrderIds == findOrder.Id)

                    {
                        OrdersToModify.Add(findOrder);


                    }
                }


            }

            foreach (var OrderModify in OrdersToModify)
            {

                OrderModify.OrderId = OrderId;
                OrderModify.Status = "Pending";

                db.Entry(OrderModify).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }



        // GET: AspNetOrderNotes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetNote aspNetNote = db.AspNetNotes.Where(x => x.EncryptedID == id).FirstOrDefault();
            if (aspNetNote == null)
            {
                return HttpNotFound();
            }
            return View(aspNetNote);
        }
        public ActionResult RecentOrders()
        {
            var CurrentUserId = User.Identity.GetUserId();

            int StudentId = db.AspNetStudents.Where(x => x.StudentID == CurrentUserId).FirstOrDefault().Id;

            var result = from Notes in db.AspNetNotes
                         join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID
                         join Order in db.AspNetOrders on OrderNotes.OrderId equals Order.Id
                         where OrderNotes.StudentID == StudentId && OrderNotes.OrderType == "Postpaid" && OrderNotes.Status =="Pending"

                         select new
                         {
                             OrderId = Order.Id,
                            // Id = OrderNotes.Id,
                             Title = Notes.Title,
                             Discription = Notes.Description,
                             CourseType = Notes.CourseType,
                             Price = Notes.GrandTotal,
                             Quantity = OrderNotes.Quantity,
                             Status = OrderNotes.Status,

                         };
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrepaidRecentOrders()
        {
            var CurrentUserId = User.Identity.GetUserId();

            int StudentId = db.AspNetStudents.Where(x => x.StudentID == CurrentUserId).FirstOrDefault().Id;

            var result = from Notes in db.AspNetNotes
                         join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID
                         join Order in db.AspNetOrders on OrderNotes.OrderId equals Order.Id
                         where OrderNotes.StudentID == StudentId && OrderNotes.OrderType == "Prepaid" && OrderNotes.Status == "Pending"
                         select new
                         {
                             OrderId = Order.Id,
                             Id = OrderNotes.Id,
                             Title = Notes.Title,
                             Discription = Notes.Description,
                             CourseType = Notes.CourseType,
                             Price = Notes.GrandTotal,
                             Quantity = OrderNotes.Quantity,
                             Status = OrderNotes.Status,

                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult ConfirmOdr(int TotalAmount, int[] IDs)
        {
            var CurrentUserId = User.Identity.GetUserId();
            AspNetOrder order = new AspNetOrder();
            order.TotalAmount=TotalAmount;
            order.Status = "Pending";
            order.OrderType = "Postpaid";


            order.PublishDate = DateTime.Now;


            db.AspNetOrders.Add(order);

            db.SaveChanges();


            int OrderId = order.Id;

            List<AspNetNotesOrder> AllNotesOrder = db.AspNetNotesOrders.ToList();

            List<AspNetNotesOrder> OrdersToModify = new List<AspNetNotesOrder>();

            foreach (var OrderIds in IDs)
            {

                foreach (var findOrder in AllNotesOrder)
                {

                    if (OrderIds == findOrder.Id)

                    {
                        OrdersToModify.Add(findOrder);


                    }
                }


            }

            foreach(var OrderModify in OrdersToModify)
            {

                OrderModify.OrderId = OrderId;
                OrderModify.Status = "Pending";

                db.Entry(OrderModify).State = EntityState.Modified;
                db.SaveChanges();
            }


            

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteOrders( int[] DeleteOrders)
        {
            
           List<AspNetNotesOrder> AllNotesOrder= db.AspNetNotesOrders.ToList();
           
            List<AspNetNotesOrder> OrdersToDelete = new List<AspNetNotesOrder>() ;

            foreach (var deleteOrderIds in DeleteOrders)
            {

                foreach(var order in AllNotesOrder)
                {

                    if(deleteOrderIds == order.Id)

                    {
                        OrdersToDelete.Add(order);

                    
                    }
                }
               

            }


            db.AspNetNotesOrders.RemoveRange(OrdersToDelete);
            db.SaveChanges();
            




            return Json("", JsonRequestBehavior.AllowGet);
        }

            public ActionResult CartOrders()
            {
            var CurrentUserId = User.Identity.GetUserId();

            int StudentId = db.AspNetStudents.Where(x => x.StudentID == CurrentUserId).FirstOrDefault().Id;

            var result = from Notes in db.AspNetNotes
                         join OrderNotes in db.AspNetNotesOrders on Notes.Id equals OrderNotes.NotesID
                         where OrderNotes.StudentID == StudentId && OrderNotes.Status == "Draft" &&
                          OrderNotes.OrderType == "Postpaid"

                         select new
                         {
                             Id = OrderNotes.Id,
                             Title = Notes.Title,
                             Discription = Notes.Description,
                             CourseType = Notes.CourseType,
                             Price = Notes.GrandTotal,
                             Quantity = OrderNotes.Quantity,
                             Total = Notes.GrandTotal * OrderNotes.Quantity,
                             Status = OrderNotes.Status,

                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelOrder(int OrderId)
        {

            AspNetNotesOrder NotesOrder = db.AspNetNotesOrders.Where(x => x.Id == OrderId).FirstOrDefault();
            NotesOrder.Status = "Cancelled";

            db.Entry(NotesOrder).State = EntityState.Modified;

            db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }



        public ActionResult ConfirmOrder(string NotesId, int Quantity)
        {

            var CurrentUserId = User.Identity.GetUserId();

            int StudentId = db.AspNetStudents.Where(x => x.StudentID == CurrentUserId).FirstOrDefault().Id;


            AspNetNotesOrder NotesOrder = new AspNetNotesOrder();
            int NotesID=   db.AspNetNotes.Where(x => x.EncryptedID == NotesId).FirstOrDefault().Id;
            NotesOrder.NotesID = NotesID;
            NotesOrder.StudentID = StudentId;
            NotesOrder.CreationDate = DateTime.Now;
            NotesOrder.Quantity = Quantity;
            NotesOrder.Status = "Draft";
            NotesOrder.OrderType = "Postpaid";
            db.AspNetNotesOrders.Add(NotesOrder);


            db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }


        // GET: AspNetOrderNotes/Create
        public ActionResult Create()
        {
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            return View();
        }

        // POST: AspNetOrderNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,SubjectID,CourseType,Price,CreationDate")] AspNetNote aspNetNote)
        {
            if (ModelState.IsValid)
            {
               // string EncrID = aspNetNote.Id + aspNetNote.SubjectID + aspNetNote.Price.ToString();
              //  aspNetNote.EncryptedID = Encrpt.Encrypt(EncrID, true);

                db.AspNetNotes.Add(aspNetNote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetNote.SubjectID);
            return View(aspNetNote);
        }

        // GET: AspNetOrderNotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetNote aspNetNote = db.AspNetNotes.Find(id);
            if (aspNetNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetNote.SubjectID);
            return View(aspNetNote);
        }

        // POST: AspNetOrderNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,SubjectID,CourseType,Price,CreationDate")] AspNetNote aspNetNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetNote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetNote.SubjectID);
            return View(aspNetNote);
        }

        // GET: AspNetOrderNotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetNote aspNetNote = db.AspNetNotes.Find(id);
            if (aspNetNote == null)
            {
                return HttpNotFound();
            }
            return View(aspNetNote);
        }

        // POST: AspNetOrderNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetNote aspNetNote = db.AspNetNotes.Find(id);
            db.AspNetNotes.Remove(aspNetNote);
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
