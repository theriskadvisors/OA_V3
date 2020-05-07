using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SEA_Application.Models;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using Hangfire;
using System.Net;
using System.Text;

namespace SEA_Application.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            //var abc = 0;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //public string DoStuff()
        //{
        //    var message = "";
        //    var currentdate = DateTime.Now.Date;
        //    var StudeeMonth = db.StudentFeeMonths.Where(x => x.Status == "Printed" && x.DueDate <= currentdate).ToList();
        //    foreach (var item in StudeeMonth)
        //    {
        //        var url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
        //        String result = "";

        //        var studentid = item.AspNetStudent.AspNetUser.Id;
        //        var parentid = db.AspNetParent_Child.Where(x => x.ChildID == studentid).Select(x => x.ParentID).FirstOrDefault();
        //        var number = db.AspNetUsers.Where(x => x.Id == parentid).Select(x => x.PhoneNumber).FirstOrDefault();
        //        message = "Fee Challan of " + item.AspNetStudent.AspNetUser.Name + " is dispached and its due date: " + item.DueDate + " and validity date is: " + item.ValildityDate + " Thanks";
        //        var newnum = "";
        //        if (number != null)
        //        {
        //            var num = number.Substring(1);
        //            newnum = "92" + num;
        //        }

        //        String messageer = HttpUtility.UrlEncode(message);

        //        String strPost = "id=ipcngsch&pass=ipc_ngs123&msg=" + messageer + "&to=" + newnum + "&mask=IPC-NGS&type=xml&lang=English";
        //        StreamWriter myWriter = null;
        //        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
        //        objRequest.Method = "POST";
        //        objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
        //        objRequest.ContentType = "application/x-www-form-urlencoded";
        //        try
        //        {
        //            myWriter = new StreamWriter(objRequest.GetRequestStream());
        //            myWriter.Write(strPost);
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //        finally
        //        {
        //            myWriter.Close();
        //        }
        //        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        //        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        //        {
        //            result = sr.ReadToEnd();
        //            // Close and clean up the StreamReader
        //            sr.Close();
        //        }
        //        var messge = result;
        //    }



        //    return message;
        //}
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

        
            //RecurringJob.AddOrUpdate(() => DoStuff(), Cron.Daily);

            var userID = User.Identity.GetUserId();
            //ViewBag.SessionID = new SelectList(db.AspNetSessions, "Id", "SessionName");

            ViewBag.SessionID = db.AspNetSessions.ToList().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.SessionName,
                    Selected = (x.Status == "Active")
                });

            try
            {
                if (userID != null)
                {
                    SessionIDStaticController.GlobalSessionID = db.AspNetSessions.Where(x => x.Status == "Active").FirstOrDefault().Id.ToString();

                    if (UserManager.IsInRole(userID, "Teacher"))
                    {
                        System.Web.HttpContext.Current.Session["TeacherID"] = userID;
                        return RedirectToAction("Dashboard", "Teacher_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Student"))
                    {
                        System.Web.HttpContext.Current.Session["StudentID"] = userID;
                        return RedirectToAction("Dashboard", "Student_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Supervisor"))
                    {
                        System.Web.HttpContext.Current.Session["SupervisorID"] = userID;
                        return RedirectToAction("Dashboard", "Supervisor_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Admin"))
                    {
                        System.Web.HttpContext.Current.Session["AdminID"] = userID;
                        return RedirectToAction("Dashboard", "Admin_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Principal"))
                    {
                        System.Web.HttpContext.Current.Session["PrincipalID"] = userID;
                        return RedirectToAction("Dashboard", "Principal_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Accountant"))
                    {
                        System.Web.HttpContext.Current.Session["AccountantID"] = userID;
                        return RedirectToAction("Index", "FinanceSummary");
                    }
                    //else if (UserManager.IsInRole(userID, "Parent"))
                    //{

                    //    System.Web.HttpContext.Current.Session["ParentID"] = userID;
                    //    return RedirectToAction("Dashboard", "Parent_Dashboard");
                    //}
                    else if (UserManager.IsInRole(userID, "Parent"))
                    {
                        System.Web.HttpContext.Current.Session["ParentID"] = userID;
                        if (returnUrl != "")
                        {

                            return RedirectToLocal(returnUrl);
                        }

                        return RedirectToAction("Dashboard", "Parent_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "SuperAdmin"))
                    {
                        System.Web.HttpContext.Current.Session["SuperAdminID"] = userID;
                        return RedirectToAction("Dashboard", "SuperAdmin_Dashboard");
                    }
                    else
                    {
                        ViewBag.ReturnUrl = returnUrl;
                        return View();
                    }
                }
            }
            catch
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }


            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Dashboard()
        {


            var userID = User.Identity.GetUserId();

            if (UserManager.IsInRole(userID, "Teacher"))
            {
                System.Web.HttpContext.Current.Session["TeacherID"] = userID;
                return RedirectToAction("Dashboard", "Teacher_Dashboard");
            }
            else if (UserManager.IsInRole(userID, "Student"))
            {
                System.Web.HttpContext.Current.Session["StudentID"] = userID;
                return RedirectToAction("Dashboard", "Student_Dashboard");
            }
            else if (UserManager.IsInRole(userID, "Admin"))
            {
                System.Web.HttpContext.Current.Session["AdminID"] = userID;
                return RedirectToAction("Dashboard", "Admin_Dashboard");
            }
            else if (UserManager.IsInRole(userID, "Supervisor"))
            {
                System.Web.HttpContext.Current.Session["SupervisorID"] = userID;
                return RedirectToAction("Dashboard", "Supervisor_Dashboard");
            }
            else if (UserManager.IsInRole(userID, "Principal"))
            {
                System.Web.HttpContext.Current.Session["PrincipalID"] = userID;
                return RedirectToAction("Dashboard", "Principal_Dashboard");
            }
            else if (UserManager.IsInRole(userID, "Accountant"))
            {
                System.Web.HttpContext.Current.Session["AccountantID"] = userID;
                return RedirectToAction("Index", "FinanceSummary");
            }
            else if (UserManager.IsInRole(userID, "Parent"))
            {
                System.Web.HttpContext.Current.Session["ParentID"] = userID;
                return RedirectToAction("Dashboard", "Parent_Dashboard");
            }
            else if (UserManager.IsInRole(userID, "SuperAdmin"))
            {
                System.Web.HttpContext.Current.Session["SuperAdminID"] = userID;
                return RedirectToAction("Dashboard", "SuperAdmin_Dashboard");
            }
            return null;
        }

        // POST: /Account/Login for ConfirmationEmail
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login_Confirm(LoginViewModel model, string returnUrl)
        {
            SEA_DatabaseEntities db = new SEA_DatabaseEntities();
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:

                    var userID = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
                    AspNetUser res = db.AspNetUsers.Where(x => x.Id == userID).Select(x => x).FirstOrDefault();
                    if (res.Status == "False")
                    {
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        // return RedirectToAction("Index", "Home");
                        ModelState.AddModelError("", "Admin has disabled your account.");
                        return View(model);
                    }
                    var startdate = DateTime.Now;
                    LogTime(startdate, userID);
                    

                    //else { 
                    //    }
                    if (UserManager.IsInRole(userID, "Teacher"))
                    {
                        System.Web.HttpContext.Current.Session["TeacherID"] = userID;
                        return RedirectToAction("Dashboard", "Teacher_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Student"))
                    {
                        System.Web.HttpContext.Current.Session["StudentID"] = userID;
                        return RedirectToAction("Dashboard", "Student_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Supervisor"))
                    {
                        System.Web.HttpContext.Current.Session["SupervisorID"] = userID;
                        return RedirectToAction("Dashboard", "Supervisor_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Principal"))
                    {
                        System.Web.HttpContext.Current.Session["PrincipalID"] = userID;
                        return RedirectToAction("Dashboard", "Principal_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Admin"))
                    {
                        System.Web.HttpContext.Current.Session["AdminID"] = userID;
                        return RedirectToAction("Dashboard", "Admin_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Accountant"))
                    {
                        System.Web.HttpContext.Current.Session["AccountantID"] = userID;
                        return RedirectToAction("Index", "FinanceSummary");
                    }
                    else if (UserManager.IsInRole(userID, "Parent"))
                    {
                        System.Web.HttpContext.Current.Session["ParentID"] = userID;
                        if (returnUrl != "")
                        {

                            return RedirectToLocal(returnUrl);
                        }

                        return RedirectToAction("Dashboard", "Parent_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "SuperAdmin"))
                    {
                        System.Web.HttpContext.Current.Session["SuperAdminID"] = userID;
                        return RedirectToAction("Dashboard", "SuperAdmin_Dashboard");
                    }

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
         //  GetSessionID =  model.SessionID;
            SEA_DatabaseEntities db = new SEA_DatabaseEntities();

           // var data2 = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Accountant") && x.Status != "False").ToList();
           
            
          AspNetUser asp =   db.AspNetUsers.Where(x => x.UserName == model.UserName).FirstOrDefault();
          var VerifyUser = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Student") && x.Id == asp.Id).FirstOrDefault();
          if (VerifyUser != null)
          {
              if (asp == null)
              {

                  ViewBag.SessionID = db.AspNetSessions.ToList().Select(x => new SelectListItem
                  {
                      Value = x.Id.ToString(),
                      Text = x.SessionName,
                      Selected = (x.Status == "Active")
                  });

                  return View(model);
              }
              AspNetUsers_Session aus = db.AspNetUsers_Session.Where(x => x.SessionID.ToString() == model.SessionID && x.UserID == asp.Id).FirstOrDefault();
              if (aus == null)
              {
                  ViewBag.SessionID = db.AspNetSessions.ToList().Select(x => new SelectListItem
                  {
                      Value = x.Id.ToString(),
                      Text = x.SessionName,
                      Selected = (x.Status == "Active")
                  });
                  ModelState.AddModelError("", "Kindly select your enrolled session to login.");
                  return View(model);
              }
          }
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:

                    var userID = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
                    AspNetUser res = db.AspNetUsers.Where(x => x.Id == userID).Select(x => x).FirstOrDefault();
                    if (res.Status == "False")
                    {
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        // return RedirectToAction("Index", "Home");
                        SessionIDStaticController.GlobalSessionID = model.SessionID;


                            ViewBag.SessionID = db.AspNetSessions.ToList().Select(x => new SelectListItem
                            {
                                Value = x.Id.ToString(),
                                Text = x.SessionName,
                                Selected = (x.Status == "Active")
                            });
                        ModelState.AddModelError("", "Admin has disabled your account.");
                        return View(model);
                    }
                    var startdate = DateTime.Now;
                    LogTime(startdate, userID);
                    SessionIDStaticController.GlobalSessionID = model.SessionID;

                    //else { 
                    //    }
                    if (UserManager.IsInRole(userID, "Teacher"))
                    {
                        System.Web.HttpContext.Current.Session["TeacherID"] = userID;
                        return RedirectToAction("Dashboard", "Teacher_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Student"))
                    {
                       System.Web.HttpContext.Current.Session["StudentID"] = userID;
                         return RedirectToAction("Dashboard", "Student_Dashboard");
                    
                    }
                    else if (UserManager.IsInRole(userID, "Supervisor"))
                    {
                        System.Web.HttpContext.Current.Session["SupervisorID"] = userID;
                        return RedirectToAction("Dashboard", "Supervisor_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Principal"))
                    {
                        System.Web.HttpContext.Current.Session["PrincipalID"] = userID;
                        return RedirectToAction("Dashboard", "Principal_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Admin"))
                    {
                        System.Web.HttpContext.Current.Session["AdminID"] = userID;
                        return RedirectToAction("Dashboard", "Admin_Dashboard");
                    }
                    else if (UserManager.IsInRole(userID, "Accountant"))
                    {
                        System.Web.HttpContext.Current.Session["AccountantID"] = userID;
                        return RedirectToAction("Index", "FinanceSummary");
                    }
                    else if (UserManager.IsInRole(userID, "Parent"))
                    {
                        System.Web.HttpContext.Current.Session["ParentID"] = userID;
                        if (returnUrl != "")
                        {

                        //    return RedirectToLocal(returnUrl);
                              return RedirectToAction("Dashboard", "Parent_Dashboard");
                        }

                        return RedirectToAction("Dashboard", "Parent_Dashboard");
                    }
                   else if (UserManager.IsInRole(userID, "SuperAdmin"))
                    {
                        System.Web.HttpContext.Current.Session["SuperAdminID"] = userID;
                        return RedirectToAction("Dashboard", "SuperAdmin_Dashboard");
                    }
                  
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ViewBag.SessionID = db.AspNetSessions.ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.SessionName,
                        Selected = (x.Status == "Active")
                    });
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }


                }
        public void LogTime(DateTime date, string userID)
        {
            var id = db.AspNetLoginTimes.Where(x => x.UserId == userID && x.StartTime.Day == date.Day).Select(x => x.ID).FirstOrDefault();
            if(id!=0)
            {
                db.AspNetLoginTimes.Where(x => x.ID == id).FirstOrDefault().StartTime = date;
                db.SaveChanges();
            }
            else
            {
                var username = db.AspNetUsers.Where(x => x.Id == userID).Select(x => x.Name).FirstOrDefault();
                AspNetLoginTime logintime = new AspNetLoginTime();
                logintime.UserName = username;
                logintime.UserId = userID;
                logintime.StartTime = date;
                db.AspNetLoginTimes.Add(logintime);
                db.SaveChanges();
            }        
            return;
        }
        public ActionResult LogDuration()
        {
            return View();
        }
        public JsonResult LogDurationDetail()
        {
            List<LoginDetails> logindetail = new List<LoginDetails>();
            var admin = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").FirstOrDefault();

                LoginDetails login = new LoginDetails();
                var count = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime.Month == DateTime.Now.Month).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == admin.Id).Select(x => x.LastPasswordChange).FirstOrDefault();
                login.LoginCount = count;
                login.Name = admin.Name;
                login.ChangePassword = passtime;
                login.LastLogin = lastlogin;
                logindetail.Add(login);
            
            var startdate = logindetail.OrderByDescending(x => x.LastLogin).Select(x => x.LastLogin).ToList().LastOrDefault();
            var enddate = logindetail.OrderByDescending(x => x.LastLogin).Select(x=>x.LastLogin).ToList().FirstOrDefault();
            var result = new{ logindetail, startdate, enddate };
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UserLoginRecord(string type)
        {
            List<LoginDetails> logindetails = new List<LoginDetails>();

            if (type == "Parents")
            {
                var list = db.AspNetParents.Where(x=>x.AspNetUser.Status!="False").ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID && x.StartTime.Month == DateTime.Now.Month).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserID).Select(x => x.LastPasswordChange).FirstOrDefault();

                    login.Name = item.AspNetUser.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
            }
            if (type == "Teacher")
            {
                var teacherlist = db.AspNetEmployees.Where(x => x.VirtualRoleId == 35 && x.AspNetUser.Status!="False").ToList();
                foreach (var item in teacherlist)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime.Month == DateTime.Now.Month).Count();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
            }
            if (type == "Student")
            {
                var list = db.AspNetStudents.Where(x=>x.AspNetUser.Status!="False").ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var username = db.AspNetStudents.Where(x => x.StudentID == item.StudentID).Select(x => x.AspNetUser.Name).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.StudentID).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID && x.StartTime.Month == DateTime.Now.Month).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = username;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
            }
            if (type == "Admin")
            {
                LoginDetails login = new LoginDetails();
                var admin = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime.Month == DateTime.Now.Month).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").Select(x => x.LastPasswordChange).FirstOrDefault();

                login.Name = admin.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);
            }

            if (type == "Principal")
            {
                LoginDetails login = new LoginDetails();
                var p = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == p.Id && x.StartTime.Month == DateTime.Now.Month).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == p.Id).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").Select(x => x.LastPasswordChange).FirstOrDefault();
                login.Name = p.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);


            }
            if (type == "Accountant")
            {
                var list = db.AspNetEmployees.Where(x => x.VirtualRoleId == 34 && x.AspNetUser.Status!="False").ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime.Month == DateTime.Now.Month).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
            }
            return Json(logindetails.OrderBy(x=>x.Name).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserLoginDateFilter(string type, DateTime start, DateTime end)
        {
            List<LoginDetails> logindetails = new List<LoginDetails>();

            if (type == "Parents")
            {
                var list = db.AspNetParents.Where(x=>x.AspNetUser.Status!="False").ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserID).Select(x => x.LastPasswordChange).FirstOrDefault();

                    login.Name = item.AspNetUser.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
           
            }
            if (type == "Teacher")
            {
                var teacherlist = db.AspNetEmployees.Where(x => x.VirtualRoleId == 35 && x.AspNetUser.Status!="False").ToList();
                foreach (var item in teacherlist)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).Count();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
                //var userlist = db.AspNetLoginTimes.Where(x => x.StartTime >= start && x.StartTime <= end).Select(x => x.UserId).Distinct().ToList();
                //if (userlist != null)
                //{
                //    foreach (var item in userlist)
                //    {
                //        var teacher = db.AspNetEmployees.Where(x => x.UserId == item && x.VirtualRoleId == 35).FirstOrDefault();
                //        if (teacher != null)
                //        {
                //            LoginDetails login = new LoginDetails();
                //            var usercount = db.AspNetLoginTimes.Where(x => x.UserId == teacher.UserId).Count();
                //            var passtime = db.AspNetUsers.Where(x => x.Id == teacher.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                //            var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == teacher.UserId).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                //            login.Name = teacher.Name;
                //            login.LastLogin = lastlogin;
                //            login.ChangePassword = passtime;
                //            login.LoginCount = usercount;
                //            logindetails.Add(login);
                //        }
                //    }
                //}
            }
            if (type == "Student")
            {
                var list = db.AspNetStudents.Where(x=>x.AspNetUser.Status!="False").ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var username = db.AspNetStudents.Where(x => x.StudentID == item.StudentID).Select(x => x.AspNetUser.Name).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.StudentID).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = username;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
               
            }
            if (type == "Admin")
            {
                LoginDetails login = new LoginDetails();
                var admin = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime >= start && x.StartTime <= end).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").Select(x => x.LastPasswordChange).FirstOrDefault();

                login.Name = admin.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);
               
            }

            if (type == "Principal")
            {
                LoginDetails login = new LoginDetails();
                var p = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == p.Id && x.StartTime >= start && x.StartTime <= end).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == p.Id && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").Select(x => x.LastPasswordChange).FirstOrDefault();
                login.Name = p.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);
                

            }
            if (type == "Accountant")
            {
                var list = db.AspNetEmployees.Where(x => x.VirtualRoleId == 34 && x.AspNetUser.Status!="False").ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
                
            }
            return Json(logindetails.OrderBy(x => x.Name).ToList(), JsonRequestBehavior.AllowGet);
        }
        ///////////////////////////////////Word Record////////////////////////////////////////////


        public ActionResult ExportToWord(string type, DateTime start, DateTime end)
        {

            List<LoginDetails> logindetails = new List<LoginDetails>();
            if (type == "Parents")
            {
                var list = db.AspNetParents.ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserID).Select(x => x.LastPasswordChange).FirstOrDefault();

                    login.Name = item.AspNetUser.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
            }
            if (type == "Teacher")
            {
                var teacherlist = db.AspNetEmployees.Where(x => x.VirtualRoleId == 35).ToList();
                foreach (var item in teacherlist)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).Count();
                   

                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
                //var userlist = db.AspNetLoginTimes.Where(x => x.StartTime >= start && x.StartTime <= end).Select(x => x.UserId).Distinct().ToList();
                //if (userlist != null)
                //{
                //    foreach (var item in userlist)
                //    {
                //        var teacher = db.AspNetEmployees.Where(x => x.UserId == item && x.VirtualRoleId == 35).FirstOrDefault();
                //        if (teacher != null)
                //        {
                //            LoginDetails login = new LoginDetails();
                //            var usercount = db.AspNetLoginTimes.Where(x => x.UserId == teacher.UserId).Count();
                //            var passtime = db.AspNetUsers.Where(x => x.Id == teacher.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                //            var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == teacher.UserId).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                //            login.Name = teacher.Name;
                //            login.LastLogin = lastlogin;
                //            login.ChangePassword = passtime;
                //            login.LoginCount = usercount;
                //            logindetails.Add(login);
                //        }
                //    }
                //}
            }
            if (type == "Student")
            {
                var list = db.AspNetStudents.ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var username = db.AspNetStudents.Where(x => x.StudentID == item.StudentID).Select(x => x.AspNetUser.Name).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.StudentID).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = username;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
            }
            if (type == "Admin")
            {
                LoginDetails login = new LoginDetails();
                var admin = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime >= start && x.StartTime <= end).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").Select(x => x.LastPasswordChange).FirstOrDefault();

                login.Name = admin.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);
            }

            if (type == "Principal")
            {
                LoginDetails login = new LoginDetails();
                var p = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == p.Id && x.StartTime >= start && x.StartTime <= end).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == p.Id && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").Select(x => x.LastPasswordChange).FirstOrDefault();
                login.Name = p.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);


            }
            if (type == "Accountant")
            {
                var list = db.AspNetEmployees.Where(x => x.VirtualRoleId == 34).ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
            }
            List<LoginDels> iuhjk = new List<LoginDels>();

            foreach(var t in logindetails)
            {
                LoginDels gyu = new LoginDels();
                gyu.LastLogin = t.LastLogin.ToString();
                gyu.Name = t.Name;
                gyu.ChangePassword = t.ChangePassword.ToString();
                gyu.LoginCount = t.LoginCount;


                if (t.LoginCount == 0)
                {
                    gyu.LastLogin = "-";
                }
                if(t.ChangePassword == null)
                {
                    gyu.ChangePassword = "-";
                }

                iuhjk.Add(gyu);
            }

            //  var data = db.AspNetEmployees.Where(x=>x.VirtualRoleId==35).ToList();

            GridView gridview = new GridView();
            gridview.DataSource = iuhjk;
            gridview.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename = WordLoginReport.doc");
            Response.ContentType = "application/ms-word";
            Response.Charset = "";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    gridview.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }

        ///////////////////////////////////////////Excel Record Date Filter///////////////////////////////////
        public void ExcelLoginReport(string type, DateTime start, DateTime end)
        {
            List<LoginDetails> logindetails = new List<LoginDetails>();
            if (type == "Parents")
            {
                var list = db.AspNetParents.ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();                  
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserID).Select(x => x.LastPasswordChange).FirstOrDefault();

                    login.Name = item.AspNetUser.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
            }
            if (type == "Teacher")
            {
                var teacherlist = db.AspNetEmployees.Where(x => x.VirtualRoleId == 35).ToList();
                foreach (var item in teacherlist)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).Count();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
                //var userlist = db.AspNetLoginTimes.Where(x => x.StartTime >= start && x.StartTime <= end).Select(x => x.UserId).Distinct().ToList();
                //if (userlist != null)
                //{
                //    foreach (var item in userlist)
                //    {
                //        var teacher = db.AspNetEmployees.Where(x => x.UserId == item && x.VirtualRoleId == 35).FirstOrDefault();
                //        if (teacher != null)
                //        {
                //            LoginDetails login = new LoginDetails();
                //            var usercount = db.AspNetLoginTimes.Where(x => x.UserId == teacher.UserId).Count();
                //            var passtime = db.AspNetUsers.Where(x => x.Id == teacher.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                //            var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == teacher.UserId).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                //            login.Name = teacher.Name;
                //            login.LastLogin = lastlogin;
                //            login.ChangePassword = passtime;
                //            login.LoginCount = usercount;
                //            logindetails.Add(login);
                //        }
                //    }
                //}
            }
            if (type == "Student")
            {
                var list = db.AspNetStudents.ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var username = db.AspNetStudents.Where(x => x.StudentID == item.StudentID).Select(x => x.AspNetUser.Name).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.StudentID).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = username;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
            }
            if (type == "Admin")
            {
                LoginDetails login = new LoginDetails();
                var admin = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime >= start && x.StartTime <= end).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").Select(x => x.LastPasswordChange).FirstOrDefault();

                login.Name = admin.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);
            }

            if (type == "Principal")
            {
                LoginDetails login = new LoginDetails();
                var p = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == p.Id && x.StartTime >= start && x.StartTime <= end).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == p.Id && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").Select(x => x.LastPasswordChange).FirstOrDefault();
                login.Name = p.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);


            }
            if (type == "Accountant")
            {
                var list = db.AspNetEmployees.Where(x => x.VirtualRoleId == 34).ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime >= start && x.StartTime <= end).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
            }

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Name";
            ws.Cells["B1"].Value = "Last Login";
            ws.Cells["C1"].Value = "Password Last Changed";
            ws.Cells["D1"].Value = "Login Count";
            int rowStart = 2;
            foreach (var item in logindetails)
            {
                var lastlogin = item.LastLogin.ToString();
                var changepass = item.ChangePassword.ToString();
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Name;
                if(item.LoginCount != 0)
                {
                    ws.Cells[string.Format("B{0}", rowStart)].Value = lastlogin;
                }
                else
                {
                    ws.Cells[string.Format("B{0}", rowStart)].Value = "-";
                }
                if (changepass != "")
                {
                    ws.Cells[string.Format("C{0}", rowStart)].Value = changepass;
                }
                else
                {
                    ws.Cells[string.Format("C{0}", rowStart)].Value = "-";

                }


                ws.Cells[string.Format("D{0}", rowStart)].Value = item.LoginCount;
                rowStart++;
            }
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment:filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

        }
        //////////////////////////////////Excel Login Report///////////////////////
        public void ExcelReport(string type)
        {
            List<LoginDetails> logindetails = new List<LoginDetails>();
            if (type == "Parents")
            {
                var list = db.AspNetParents.ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID && x.StartTime.Month == DateTime.Now.Month).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserID).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserID).Select(x => x.LastPasswordChange).FirstOrDefault();

                    login.Name = item.AspNetUser.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
            }
            if (type == "Teacher")
            {
                var teacherlist = db.AspNetEmployees.Where(x => x.VirtualRoleId == 35).ToList();
                foreach (var item in teacherlist)
                {
                    LoginDetails login = new LoginDetails();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime.Month == DateTime.Now.Month).Count();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);
                }
            }
            if (type == "Student")
            {
                var list = db.AspNetStudents.ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var username = db.AspNetStudents.Where(x => x.StudentID == item.StudentID).Select(x => x.AspNetUser.Name).FirstOrDefault();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.StudentID).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID && x.StartTime.Month == DateTime.Now.Month).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.StudentID).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = username;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
            }
            if (type == "Admin")
            {
                LoginDetails login = new LoginDetails();
                var admin = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id && x.StartTime.Month == DateTime.Now.Month).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == admin.Id).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "056b9214-8c22-496f-8722-ab40472a42ac").Select(x => x.LastPasswordChange).FirstOrDefault();

                login.Name = admin.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);
            }

            if (type == "Principal")
            {
                LoginDetails login = new LoginDetails();
                var p = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").FirstOrDefault();
                var usercount = db.AspNetLoginTimes.Where(x => x.UserId == p.Id && x.StartTime.Month == DateTime.Now.Month).Count();
                var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == p.Id).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                var passtime = db.AspNetUsers.Where(x => x.Id == "50e8f732-fc30-4c1b-9dac-fe110b288b38").Select(x => x.LastPasswordChange).FirstOrDefault();
                login.Name = p.Name;
                login.LastLogin = lastlogin;
                login.ChangePassword = passtime;
                login.LoginCount = usercount;
                logindetails.Add(login);


            }
            if (type == "Accountant")
            {
                var list = db.AspNetEmployees.Where(x => x.VirtualRoleId == 34).ToList();
                foreach (var item in list)
                {
                    LoginDetails login = new LoginDetails();
                    var passtime = db.AspNetUsers.Where(x => x.Id == item.UserId).Select(x => x.LastPasswordChange).FirstOrDefault();
                    var usercount = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId && x.StartTime.Month == DateTime.Now.Month).Count();
                    var lastlogin = db.AspNetLoginTimes.Where(x => x.UserId == item.UserId).OrderByDescending(x => x.StartTime).Select(x => x.StartTime).FirstOrDefault();
                    login.Name = item.Name;
                    login.LastLogin = lastlogin;
                    login.ChangePassword = passtime;
                    login.LoginCount = usercount;
                    logindetails.Add(login);

                }
            }

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Name";
            ws.Cells["B1"].Value = "Last Login";
            ws.Cells["C1"].Value = "Password Last Changed";
            ws.Cells["D1"].Value = "Login Count";
            int rowStart = 2;
            foreach (var item in logindetails)
            {
                var lastlogin = item.LastLogin.ToString();
                var changepass = item.ChangePassword.ToString();
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Name;
                if (item.LoginCount != 0)
                {
                    ws.Cells[string.Format("B{0}", rowStart)].Value = lastlogin;
                }
                else
                {
                    ws.Cells[string.Format("B{0}", rowStart)].Value = "-";
                }
                if (changepass != "")
                {
                    ws.Cells[string.Format("C{0}", rowStart)].Value = changepass;
                }
                else
                {
                    ws.Cells[string.Format("C{0}", rowStart)].Value = "-";

                }


                ws.Cells[string.Format("D{0}", rowStart)].Value = item.LoginCount;
                rowStart++;
            }
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformates-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment:filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

        }

        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //var endtime = DateTime.Now;
            //LogTime(endtime,"logout");
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public class LoginDetails
        {
            public string Name { set; get; }
            public DateTime LastLogin { set; get; }
            
            public DateTime? ChangePassword { set; get; }
            public int LoginCount { set; get; }

        }
        public class LoginDels
        {
            public string Name { set; get; }
            public string LastLogin { set; get; }

            public string ChangePassword { set; get; }
            public int LoginCount { set; get; }

        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
