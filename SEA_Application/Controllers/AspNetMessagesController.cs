using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Text.RegularExpressions;

namespace SEA_Application.Controllers
{
    public class AspNetMessagesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetMessages
        public ActionResult Inbox()
        {
            var UserID = User.Identity.GetUserId();
            List<AspNetMessage_Receiver> aspNetMessages = (from messageReceiver in db.AspNetMessage_Receiver
                                                           where messageReceiver.ReceiverID == UserID
                                                           select messageReceiver).ToList();
            //{messageReceiver.AspNetMessage.AspNetUser.Name, messageReceiver.AspNetMessage.AspNetUser.UserName, messageReceiver.AspNetMessage.Subject, messageReceiver.AspNetMessage.Message, messageReceiver.AspNetMessage.Time,messageReceiver.AspNetMessage.Id }
            return View(aspNetMessages.OrderByDescending(x => x.AspNetMessage.Time));
        }

        public ActionResult Sent()
        {
            var UserID = User.Identity.GetUserId();
            List<AspNetMessage> aspNetMessages = (from message in db.AspNetMessages
                                                  where message.SenderID == UserID
                                                  select message).ToList();


            return View(aspNetMessages.OrderByDescending(x => x.Time));
        }


        public ActionResult ReceiveMessageDetail(int messageID)
        {
            var UserID = User.Identity.GetUserId();
            AspNetMessage_Receiver messageReceiver = (from receiveMessage in db.AspNetMessage_Receiver
                                                      where receiveMessage.ReceiverID == UserID && receiveMessage.MessageID == messageID
                                                      select receiveMessage).FirstOrDefault();
            messageReceiver.Seen = "Seen";
            db.SaveChanges();
            return View(messageReceiver);
        }

        public ActionResult SentMessageDetail(int messageID)
        {
            var UserID = User.Identity.GetUserId();
            AspNetMessage messageSent = (from messages in db.AspNetMessages
                                         where messages.SenderID == UserID && messages.Id == messageID
                                         select messages).FirstOrDefault();

            List<string> ReceiverList = new List<string>();
            List<string> usernames = db.AspNetMessage_Receiver.Where(x => x.MessageID == messageSent.Id).Select(x => x.AspNetUser.UserName).ToList();
            string user = "";
            foreach (var username in usernames)
            {
                user = user + username + ",";
            }


            ViewBag.ReceiverList = user;
            return View(messageSent);
        }

        public ActionResult NewMessage()
        {
            List<string> numbers = new List<string>();
            numbers.Add("923224839049");
            numbers.Add("923401562576");
            //Utility ttuy = new Utility();
            //ttuy.datatt();
            AspNetMessage tyu = new AspNetMessage();

            SendSMS(tyu, numbers);
            ViewBag.ReceiverList = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Admin")).ToList(), "Id", "UserName");
            //ViewBag. = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.UsersLists = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult NewMessage(AspNetMessage message)
        {
            message = new AspNetMessage();
            var Receiver = Request.Form["ReceiverList"];
            Receiver=Receiver + ",056b9214-8c22-496f-8722-ab40472a42ac" + ",50e8f732-fc30-4c1b-9dac-fe110b288b38";
            List<string> SenderList = Receiver.Split(',').ToList();
            var tempMessage = Request.Unvalidated["Message"];
            var tempSubject = Request.Form["Subject"];
            var tempIsEmail = Request.Form["IsEmail"];
            var tempIsText = Request.Form["IsText"];


            string result;

            result = tempMessage.Replace("\r", " ");

            result = result.Replace("\n", " ");
            result = result.Replace("\t", string.Empty);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*head([^>])*>", "<head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*head( )*>)", "</head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(<head>).*(</head>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*script([^>])*>", "<script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*script( )*>)", "</script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<script>).*(</script>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*style([^>])*>", "<style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*style( )*>)", "</style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(<style>).*(</style>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*td([^>])*>", "\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*br( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*li( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*div([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*tr([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*p([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<[^>]*>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @" ", " ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&bull;", " * ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&lsaquo;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&rsaquo;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&trade;", "(tm)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&frasl;", "/",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&lt;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&gt;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&copy;", "(c)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&reg;", "(r)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // http://hotwired.lycos.com/webmonkey/reference/special_characters/
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&(.{2,6});", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = result.Replace("\n", "\r");

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\t)", "\t\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\r)", "\t\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\t)", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string breaks = "\r\r\r";
            string tabs = "\t\t\t\t\t";
            for (int index = 0; index < result.Length; index++)
            {
                result = result.Replace(breaks, "\r\r");
                result = result.Replace(tabs, "\t\t\t\t");
                breaks = breaks + "\r";
                tabs = tabs + "\t";
            }
            message.Message = result;
            message.Subject = tempSubject.ToString();
            if (tempIsEmail == "on")
            {
                message.IsEmail = true;

            }
            else if (tempIsEmail == null)
            {
                message.IsEmail = false;
            }
            if (tempIsText == "on")
            {
                message.IsText = true;

            }
            else if (tempIsText == null)
            {
                message.IsText = false;
            }


            message.Time = DateTime.Now;
            message.SenderID = User.Identity.GetUserId();
            db.AspNetMessages.Add(message);
            db.SaveChanges();
            var MessageID = db.AspNetMessages.Max(x => x.Id);

            foreach (var sender in SenderList)
            {
                var messageReceive = new AspNetMessage_Receiver();
                messageReceive.MessageID = MessageID;
                messageReceive.ReceiverID = sender;
                messageReceive.Seen = "Not Seen";
                messageReceive.SeenTime = DateTime.Now;
                db.AspNetMessage_Receiver.Add(messageReceive);
                db.SaveChanges();
            }

            if (message.IsEmail == true)
            {
                SendEmail(message, SenderList);
            }
            if (message.IsText == true)
            {
                SendSMS(message, SenderList);
            }
            return RedirectToAction("Sent");

        }


        public void SendEmail(AspNetMessage message, List<string> SenderList)
        {
            List<string> EmailList = new List<string>();
            foreach (var sender in SenderList)
            {
                EmailList.Add(db.AspNetUsers.Where(x => x.Id == sender).Select(x => x.Email).FirstOrDefault());
            }
            foreach (var toEmail in EmailList)
            {
                try
                {
                    string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                    string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();
                    SmtpClient client = new SmtpClient();
                  //  SmtpClient client = new SmtpClient("smtpout.secureserver.net", 25);
                    client.Port = 25; // You can use Port 25 if 587 is blocked (mine is!)
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = false;
                    client.Timeout = 100000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    MailMessage mailMessage = new MailMessage(senderEmail, toEmail, message.Subject, message.Message);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {

                }
            }

        }
        public ActionResult SendEmailViaPrincipalDashboard()
        {
            AspNetMessage obj = new AspNetMessage();
            obj.IsEmail = true;
            obj.IsText = false;
            obj.Subject = Request.Form["subject"];
            obj.Message = Request.Form["message"];
            obj.Time = DateTime.Now;
            obj.SenderID = User.Identity.GetUserId();
            string toEmail = Request.Form["emailto"];


            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtpout.secureserver.net", 25);
                client.EnableSsl = false;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, obj.Subject, obj.Message);

                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Dashboard", "Principal_Dashboard");

        }

        public ActionResult SendEmailViaAdmin_Dashboard()
        {
            AspNetMessage obj = new AspNetMessage();
            obj.IsEmail = true;
            obj.IsText = false;
            obj.Subject = Request.Form["subject"];
            obj.Message = Request.Form["message"];
            obj.Time = DateTime.Now;
            obj.SenderID = User.Identity.GetUserId();
            string toEmail = Request.Form["emailto"];


            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtpout.secureserver.net", 25);
                client.EnableSsl = false;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, obj.Subject, obj.Message);

                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Dashboard", "Admin_Dashboard");

        }

        public void SendSMS(AspNetMessage message, List<string> SenderList)
        {
            List<string> SMSList = new List<string>();
            //SMSList = SenderList;
            foreach (var sender in SenderList)
            {
                SMSList.Add(db.AspNetUsers.Where(x => x.Id == sender).Select(x => x.PhoneNumber).FirstOrDefault());
            }

            //var responseType = "xml";
            //var id = "92300xxxxxxx";
            //var pass = "xxxxxxxxxxxx";
            //var lang = "English";
            //var mask = "Outreach";

            //message.Message = "Please tel at this number 03329687357 that you receive that message";

            //var Message = Server.UrlEncode(message.Message);

            //var address = "http://www.outreach.pk/api/sendsms.php/sendsms/url";


            foreach (var number in SMSList)
            {
                //var url = address + "?id=" + id + "&pass=" + pass + "&msg=" + Message + "&to=" + number + "&mask=" + mask + "&lang=" + lang + "&type=" + responseType;
                //var url1 = "http://outreach.pk/api/sendsms.php/sendsms/url?id=ipcngsch&pass=ipc_ngs123&mask=Outreach&to=923329687357,923007583659&lang=English&msg=This%20is%20test%20message%20by%201st%204connect&type=xml";
                //HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri(url1);

                ////set xmlhttp = CreateObject("MSXML2.ServerXMLHTTP")
                ////xmlhttp.open "POST", url, false
                ////xmlhttp.send ""
                ////msg = xmlhttp.responseText
                ////response.write(msg)
                ////set xmlhttp = nothing

                //HttpResponseMessage response = client.GetAsync("api/Values").Result;

                var url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
                String result = "";
                var newnum = "";
                if (number != null)
                {
                    var num = number.Substring(1);
                    newnum = "92" + num;
                }
              
                String messageer = HttpUtility.UrlEncode(message.Message);
         
                String strPost = "id=ipcngsch&pass=ipc_ngs123&msg=" + messageer + "&to=" + newnum + "&mask=IPC-NGS&type=xml&lang=English";
                StreamWriter myWriter = null;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
                objRequest.Method = "POST";
                objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(strPost);
                }
                catch (Exception e)
                {

                }
                finally
                {
                    myWriter.Close();
                }
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                var messge = result;
            }
        }

        public JsonResult GetClasses()
        {
            var aspNetClasses = (from classes in db.AspNetClasses
                                 select new { classes.Id, classes.ClassName }).ToList();
            return Json(aspNetClasses, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudents()
        {
            var Students = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Student") && x.Status != "False").Select(x => new { x.Id, x.Name, x.UserName }).ToList();
            return Json(Students, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeachers()
        {
            var Teachers = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Teacher") && x.Status != "False").Select(x => new { x.Id, x.Name, x.UserName }).ToList();
            return Json(Teachers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParents()
        {
            var Parents = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Parent") && x.Status != "False").Select(x => new { x.Id, x.Name, x.UserName }).ToList();
            return Json(Parents, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStudentsByClass(string Classes)
        {

            if (Classes != "null")
            {
                List<string> selectClassString = Classes.Split(',').ToList();
                List<int?> selectClass = new List<int?>();
                foreach (var item in selectClassString)
                {
                    selectClass.Add(Int32.Parse(item));
                }
                var Students = (from students in db.AspNetStudents
                                where selectClass.Contains(students.ClassID) && students.AspNetUser.Status != "False"
                                select new { students.AspNetUser.Id, students.AspNetUser.Name, students.AspNetUser.UserName }).ToList();
                return Json(Students, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult AllUsers()
        {
            var Users = db.AspNetUsers.Where(x => x.Status != "False").Select(x => new { x.Id, x.Name, x.UserName }).ToList();
            return Json(Users, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMessages()
        {
            try
            {
                var UserNameLog = User.Identity.Name;
                AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                //var NotificationsList = db.AspNetPushNotifications.Where(x => x.UserID == currentUser.Id && x.IsOpen == false).ToList();
                var MessagesList = (from recevicemessage in db.AspNetMessage_Receiver
                                    where recevicemessage.ReceiverID == currentUser.Id && recevicemessage.Seen == "Not Seen"
                                    select new { recevicemessage.AspNetMessage.Id, recevicemessage.AspNetMessage.Subject, recevicemessage.AspNetMessage.Message, recevicemessage.AspNetMessage.Time, recevicemessage.AspNetMessage.AspNetUser.UserName }).ToList();

                return Json(MessagesList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception) { return null; }

        }


        /*
        [Authorize(Roles = "Parent,Admin,Teacher,Principal")]
        public ActionResult SentIndex()
        {
            var aspNetMessages = db.AspNetMessages.Include(a => a.AspNetUser).Include(a => a.AspNetUser1);


            var UserNameLog = User.Identity.Name;
            AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);

            return View(aspNetMessages.ToList().Where(x => x.SenderID == currentUser.Id).OrderByDescending(x => x.Time));
        }

        // GET: AspNetMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var UserNameLog = User.Identity.Name;
            AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);

            AspNetMessage aspNetMessage = db.AspNetMessages.Find(id);
            if (aspNetMessage.RecieverID == currentUser.Id)
            {
                aspNetMessage.IsOpen = true;
                db.SaveChanges();
            }
            if (aspNetMessage == null)
            {
                return HttpNotFound();
            }
            return View(aspNetMessage);
        }

        // GET: AspNetMessages/Create
        public ActionResult Create()
        {
            ViewBag.RecieverID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Message,Time,IsOpen,SenderID,RecieverID")] AspNetMessage aspNetMessage)
        {
            if (ModelState.IsValid)
            {

                var UserNameLog = User.Identity.Name;
                AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);

                AspNetMessage NewMessage = new AspNetMessage();
                NewMessage.Message = aspNetMessage.Message;
                NewMessage.SenderID = currentUser.Id;
                NewMessage.RecieverID = aspNetMessage.RecieverID;
                NewMessage.IsOpen = false;
                NewMessage.Time = DateTime.Now;
                db.AspNetMessages.Add(NewMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RecieverID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetMessage.RecieverID);
            ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetMessage.SenderID);
            return View(aspNetMessage);
        }

        /*************************************************************************************************************************************************/
        /*
        [HttpGet]
        public JsonResult StudentsByClass(int id)
        {
            var students = (from student in db.AspNetUsers
                            join student_subject in db.AspNetStudent_Subject on student.Id equals student_subject.StudentID
                            join subject in db.AspNetSubjects on student_subject.SubjectID equals subject.Id
                            where subject.ClassID == id && subject.AspNetUser.Status != "False"
                            select new { student.Id, student.UserName, student.Name }).Distinct().ToList();
            return Json(students, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Parent,Admin,Teacher,Principal")]
        public ActionResult NewMessage()
        {
            ViewBag.RecieverID = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Admin")).ToList(), "Id", "UserName");
            //ViewBag. = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.UsersLists = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }

        public JsonResult UsersByList(string ListName)
        {

            if (ListName == "Teachers")
            {
                var UserList = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Teacher") && x.Status != "False").ToList(), "Id", "UserName");
                return Json(UserList, JsonRequestBehavior.AllowGet);

            }
            else if (ListName == "Parents")
            {
                var UserList = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Parent") && x.Status != "False").ToList(), "Id", "UserName");
                return Json(UserList, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var UserList = new SelectList(db.AspNetUsers.Where(x => x.Status != "False"), "Id", "Email");
                return Json(UserList, JsonRequestBehavior.AllowGet);
            }


        }

        public bool SendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                //var senderEmail = "azeemazeem187@gmail.com";
                //var senderPassword = "*********";

                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mailMessage);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(AspNetMessage aspNetMessage)
        {

            if (ModelState.IsValid)
            {
                var UserNameLog = User.Identity.Name;
                AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);

                var description = Request.Form["Description"];

                IEnumerable<string> selectedReciever = Request.Form["RecieverID"].Split(',');

                if (this.User.IsInRole("Parent"))
                {
                    try
                    {
                        
                        AspNetMessage NewMessage = new AspNetMessage();
                        NewMessage.Message = aspNetMessage.Message;
                        NewMessage.SenderID = currentUser.Id;
                        NewMessage.RecieverID = aspNetMessage.RecieverID;
                        NewMessage.IsOpen = false;
                        NewMessage.Time = DateTime.Now;
                        db.AspNetMessages.Add(NewMessage);
                        db.SaveChanges();
                        var Name = db.AspNetUsers.Where(x => x.Id == currentUser.Id).Select(x => x.Name).FirstOrDefault();
                        var ReciverEmail = db.AspNetUsers.Where(x => x.Id == aspNetMessage.RecieverID).Select(x => x.Email).FirstOrDefault();
                        SendEmail(ReciverEmail, "Message from" + Name, aspNetMessage.Message);
                    }
                    catch (Exception)
                    {
                        AspNetMessage errorMessage = new AspNetMessage();
                        errorMessage.Message = "An error occure while sending message: " + aspNetMessage.Message + " to Reciever: " + aspNetMessage.RecieverID;
                        errorMessage.SenderID = currentUser.Id;
                        errorMessage.RecieverID = currentUser.Id;
                        errorMessage.IsOpen = false;
                        errorMessage.Time = DateTime.Now;
                        db.AspNetMessages.Add(errorMessage);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var value = Request.Form["RecieverIDText"];
                    IEnumerable<string> enteredReciever = Request.Form["RecieverIDText"].Split(',');

                    foreach (var item in selectedReciever)
                    {
                        try
                        {
                            AspNetMessage NewMessage = new AspNetMessage();
                            NewMessage.Message = aspNetMessage.Message;
                            NewMessage.SenderID = currentUser.Id;
                            NewMessage.RecieverID = item;
                            NewMessage.IsOpen = false;
                            NewMessage.Time = DateTime.Now;
                            db.AspNetMessages.Add(NewMessage);
                            db.SaveChanges();
                            var Name = db.AspNetUsers.Where(x => x.Id == currentUser.Id).Select(x => x.Name).FirstOrDefault();
                            var ReciverEmail = db.AspNetUsers.Where(x => x.Id == item).Select(x => x.Email).FirstOrDefault();
                            SendEmail(ReciverEmail, "Message from" + Name, aspNetMessage.Message);
                        }
                        catch (Exception)
                        {
                            AspNetMessage errorMessage = new AspNetMessage();
                            errorMessage.Message = "An error occure while sending message: " + aspNetMessage.Message + " to Reciever: " + item;
                            errorMessage.SenderID = currentUser.Id;
                            errorMessage.RecieverID = currentUser.Id;
                            errorMessage.IsOpen = false;
                            errorMessage.Time = DateTime.Now;
                            db.AspNetMessages.Add(errorMessage);
                            db.SaveChanges();
                        }
                    }
                    foreach (var item in enteredReciever)
                    {
                        if (item != "")
                        {
                            try
                            {
                                AspNetUser reciever = db.AspNetUsers.FirstOrDefault(s => s.UserName == item);
                                AspNetMessage NewMessage = new AspNetMessage();
                                NewMessage.Message = aspNetMessage.Message;
                                NewMessage.SenderID = currentUser.Id;
                                NewMessage.RecieverID = reciever.Id;
                                NewMessage.IsOpen = false;
                                NewMessage.Time = DateTime.Now;
                                db.AspNetMessages.Add(NewMessage);
                                db.SaveChanges();
                                var Name = db.AspNetUsers.Where(x => x.Id == currentUser.Id).Select(x => x.Name).FirstOrDefault();
                                var ReciverEmail = db.AspNetUsers.Where(x => x.Id == reciever.Id).Select(x => x.Email).FirstOrDefault();
                                SendEmail(ReciverEmail, "Message from" + Name, aspNetMessage.Message);
                            }
                            catch (Exception)
                            {
                                AspNetMessage errorMessage = new AspNetMessage();
                                errorMessage.Message = "An error occure while sending message: " + aspNetMessage.Message + " to Reciever: " + item;
                                errorMessage.SenderID = currentUser.Id;
                                errorMessage.RecieverID = currentUser.Id;
                                errorMessage.IsOpen = false;
                                errorMessage.Time = DateTime.Now;
                                db.AspNetMessages.Add(errorMessage);
                                db.SaveChanges();

                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        /************************************************************************************************************************************************/


        /*
                // GET: AspNetMessages/Edit/5
                public ActionResult Edit(int? id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    AspNetMessage aspNetMessage = db.AspNetMessages.Find(id);
                    if (aspNetMessage == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.RecieverID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetMessage.RecieverID);
                    ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetMessage.SenderID);
                    return View(aspNetMessage);
                }

                // POST: AspNetMessages/Edit/5
                // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
                // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult Edit([Bind(Include = "Id,Message,Time,IsOpen,SenderID,RecieverID")] AspNetMessage aspNetMessage)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(aspNetMessage).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.RecieverID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetMessage.RecieverID);
                    ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetMessage.SenderID);
                    return View(aspNetMessage);
                }

                // GET: AspNetMessages/Delete/5
                public ActionResult Delete(int? id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    AspNetMessage aspNetMessage = db.AspNetMessages.Find(id);
                    if (aspNetMessage == null)
                    {
                        return HttpNotFound();
                    }
                    return View(aspNetMessage);
                }

                // POST: AspNetMessages/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public ActionResult DeleteConfirmed(int id)
                {
                    AspNetMessage aspNetMessage = db.AspNetMessages.Find(id);
                    db.AspNetMessages.Remove(aspNetMessage);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                /***********************************************************************************************************************************************************/
        /*
        public JsonResult GetMessages()
        {
            try
            {
                var UserNameLog = User.Identity.Name;
                AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                //var NotificationsList = db.AspNetPushNotifications.Where(x => x.UserID == currentUser.Id && x.IsOpen == false).ToList();
                var MessagesList = (from message in db.AspNetMessages
                                    where message.RecieverID == currentUser.Id && message.IsOpen == false
                                    select new { message.Id, message.Message, message.Time, message.AspNetUser1.UserName }).ToList();

                return Json(MessagesList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception) { return null; }

        }

        /***********************************************************************************************************************************************************/

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
