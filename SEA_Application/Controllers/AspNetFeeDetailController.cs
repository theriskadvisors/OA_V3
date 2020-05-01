using Microsoft.AspNet.Identity;
using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class AspNetFeeDetailController : Controller
    {
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        public ActionResult Index()
        {
            var CurrentUserId = User.Identity.GetUserId();

            var result1 = (from fee_mon in db.StudentFeeMonths
                           join std in db.AspNetStudents on fee_mon.StudentId equals std.Id
                           join usr in db.AspNetUsers on std.StudentID equals usr.Id
                           join stdclass in db.AspNetClasses on std.ClassID equals stdclass.Id
                           where stdclass.SessionID == SessionID && usr.Id == CurrentUserId
                           select new { usr.Name, usr.UserName, fee_mon.TotalFee, fee_mon.FeePayable, fee_mon.Months, fee_mon.Status, stdclass.ClassName, std.CourseType }).ToList();


            List<StdFeeDetail> listFeeDetail = new List<StdFeeDetail>();

            foreach (var result in result1)
            {
                StdFeeDetail stdFeeDetail = new StdFeeDetail();


                stdFeeDetail.ClassName = result.ClassName;
                stdFeeDetail.CourseType = result.CourseType;
                stdFeeDetail.TotalFee = result.TotalFee;
                stdFeeDetail.PayableFee = result.FeePayable;
                stdFeeDetail.Months = result.Months;
                stdFeeDetail.Name = result.Name;
                stdFeeDetail.UserName = result.UserName;
                stdFeeDetail.Status = result.Status;


                listFeeDetail.Add(stdFeeDetail); 


            }



            return View(listFeeDetail);
           // return (result1, JsonRequestBehavior.AllowGet);
            
        }
    }
}