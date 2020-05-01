using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using System.Globalization;

namespace SEA_Application.Controllers
{
    public class AspNetFinancePeriodsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetFinancePeriods
        public ActionResult Index()
        {
            return View(db.AspNetFinancePeriods.ToList());
        }

        // GET: AspNetFinancePeriods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinancePeriod aspNetFinancePeriod = db.AspNetFinancePeriods.Find(id);
            if (aspNetFinancePeriod == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFinancePeriod);
        }

        // GET: AspNetFinancePeriods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetFinancePeriods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Year")] AspNetFinancePeriod aspNetFinancePeriod)
        {
            if (ModelState.IsValid)
            {
                db.AspNetFinancePeriods.Add(aspNetFinancePeriod);
                db.SaveChanges();
                string[] Months = new string[] { "July", "August", "September", "October", "November", "December", "January", "Febuary", "March", "April", "May", "June" };

                for (int i = 1; i <= 12; i++)
                {
                    AspNetFinanceMonth month = new AspNetFinanceMonth();
                    month.Month = Months[i - 1];
                    string start = "01";
                    string end = "";
                    if (i != 4 && i != 5 && i !=6 )
                    {
                        if(i<10)
                            month.Name = aspNetFinancePeriod.Year + "-00" + i;
                        else
                            month.Name = aspNetFinancePeriod.Year + "-0" + i;
                        if (i<=6)
                            start = start + "/0" + (i+6) + "/" + aspNetFinancePeriod.Year;
                        else
                            start = start + "/0" + (i-6) + "/" + aspNetFinancePeriod.Year;
                    }
                    else
                    {
                        if (i < 10)
                            month.Name = aspNetFinancePeriod.Year + "-00" + i;
                        else
                            month.Name = aspNetFinancePeriod.Year + "-0" + i;
                        if (i <= 6)
                            start = start + "/" + (i+6) + "/" + aspNetFinancePeriod.Year;
                        else
                            start = start + "/" + (i - 6) + "/" + aspNetFinancePeriod.Year;
                    }

                    if (i == 1 || i == 2 || i == 4 || i == 6 || i == 7 || i == 9 || i == 11)
                    {
                        end = "31";
                        if ( i != 4 && i != 5 && i != 6 )
                            if (i<=6)
                                end = end + "/0" + (i+6) + "/" + aspNetFinancePeriod.Year;
                            else
                                end = end + "/0" + (i - 6) + "/" + aspNetFinancePeriod.Year;
                        else
                        {
                            if (i <= 6)
                                end = end + "/" + (i + 6) + "/" + aspNetFinancePeriod.Year;
                            else
                                end = end + "/" + (i - 6) + "/" + aspNetFinancePeriod.Year;
                        }

                    }
                    else if (i == 3 || i == 5 || i == 10 || i == 12)
                    {
                        end = "30";
                        if (i != 4 && i != 5 && i != 6)
                            if (i <= 6)
                                end = end + "/0" + (i + 6) + "/" + aspNetFinancePeriod.Year;
                            else
                                end = end + "/0" + (i - 6) + "/" + aspNetFinancePeriod.Year;
                        else
                        {
                            if (i <= 6)
                                end = end + "/" + (i + 6) + "/" + aspNetFinancePeriod.Year;
                            else
                                end = end + "/" + (i - 6) + "/" + aspNetFinancePeriod.Year;
                        }
                    }
                    else if (i == 8)
                    {
                        var remainder = Convert.ToInt32(aspNetFinancePeriod.Year) % 4;
                        if (remainder == 0)
                        {
                            end = "29";
                        }
                        else
                        {
                            end = "28";
                        }

                        end = end + "/0" + (i - 6) + "/" + aspNetFinancePeriod.Year;

                    }

                    month.StartData = DateTime.ParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                    month.EndDate = DateTime.ParseExact(end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    month.PeriodId = db.AspNetFinancePeriods.Select(x => x.Id).Max();
                    db.AspNetFinanceMonths.Add(month);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetFinancePeriod);
        }

        // GET: AspNetFinancePeriods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinancePeriod aspNetFinancePeriod = db.AspNetFinancePeriods.Find(id);
            if (aspNetFinancePeriod == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFinancePeriod);
        }

        // POST: AspNetFinancePeriods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Year")] AspNetFinancePeriod aspNetFinancePeriod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetFinancePeriod).State = EntityState.Modified;
                db.SaveChanges();
                string[] Months = new string[] { "July", "Augst", "September", "October", "November", "December", "January", "Febuary", "March", "April", "May", "June" };

                for (int i = 1; i <= 12; i++)
                {
                    AspNetFinanceMonth month = db.AspNetFinanceMonths.Where(x => x.Id == aspNetFinancePeriod.Id).Select(x => x).FirstOrDefault();
                    month.Month = Months[i - 1];
                    string start = "01";
                    string end = "";
                    if (i < 10)
                    {
                        month.Name = aspNetFinancePeriod.Year + "-00" + i;
                        start = start + "/0" + i + "/" + aspNetFinancePeriod.Year;
                    }
                    else
                    {
                        month.Name = aspNetFinancePeriod.Year + "-0" + i;
                        start = start + "/" + i + aspNetFinancePeriod.Year;
                    }

                    if (i == 1 || i == 2 || i == 4 || i == 6 || i == 7 || i == 9 || i == 11)
                    {
                        end = "31";
                        if (i < 10)
                            end = end + "/0" + i + "/" + aspNetFinancePeriod.Year;
                    }
                    else if (i == 3 || i == 5 || i == 10 || i == 12)
                    {
                        end = "30";
                        if (i < 10)
                            end = end + "/0" + i + "/" + aspNetFinancePeriod.Year;
                    }
                    else if (i == 8)
                    {
                        var remainder = Convert.ToInt32(aspNetFinancePeriod.Year) % 4;
                        if (remainder == 0)
                        {
                            end = "29";
                        }
                        else
                        {
                            end = "28";
                        }

                        if (i < 10)
                            end = end + "/0" + i + "/" + aspNetFinancePeriod.Year;
                    }

                    month.StartData = DateTime.ParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                    month.EndDate = DateTime.ParseExact(end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    month.PeriodId = aspNetFinancePeriod.Id;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetFinancePeriod);
        }

        // GET: AspNetFinancePeriods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinancePeriod aspNetFinancePeriod = db.AspNetFinancePeriods.Find(id);
            if (aspNetFinancePeriod == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFinancePeriod);
        }

        // POST: AspNetFinancePeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetFinancePeriod aspNetFinancePeriod = db.AspNetFinancePeriods.Find(id);
            db.AspNetFinancePeriods.Remove(aspNetFinancePeriod);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCnfm(int id)
        {
            AspNetFinancePeriod aspNetFinancePeriod = db.AspNetFinancePeriods.Find(id);
            try
            {
                db.AspNetFinancePeriods.Remove(aspNetFinancePeriod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "It can't be deleted";
                return View("Details", aspNetFinancePeriod);
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
