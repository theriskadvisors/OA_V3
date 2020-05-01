using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SEA_Application.Models;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class GenralApiController : ApiController
    {

        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        public class Messages
        {
            public string Message { set; get; }
            public string Subject { set; get; }
            public string SenderName { set; get; }
            public DateTime Time { set; get; }
            public string Seen { set; get; }
            public int Id { set; get; }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string MessageInbox(string Id)
        {
            //var messages = db.AspNetMessage_Receiver.Where(x => x.ReceiverID == Id).Select(x => new { x.AspNetMessage.Message , x.AspNetMessage.AspNetUser.Name, x.AspNetMessage.Time, x.AspNetMessage.Subject }).ToList();

            var messages = (from x in db.AspNetMessage_Receiver
                            where x.ReceiverID == Id
                            select new
                            {
                                x.AspNetMessage.Message,
                                x.AspNetMessage.Subject,
                                x.AspNetMessage.Time,
                                SenderName = x.AspNetMessage.AspNetUser.Name,
                                x.Seen,
                                x.Id
                            }).ToList();

            List<Messages> Message = new List<Messages>();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            foreach (var item in messages)
            {
                Messages m = new Messages();
                m.Message = item.Message;
                m.Seen = item.Seen;
                m.Time = (DateTime)item.Time;
                m.Subject = item.Subject;
                m.SenderName = item.SenderName;
                m.Id = item.Id;
                Message.Add(m);
            }

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(Message);
            //string jsonString = javaScriptSerializer.Serialize(Message);
            return jsonString;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public void SeeMessages(int Id)
        {
            var message = db.AspNetMessage_Receiver.Where(x => x.Id == Id).Select(x => x).FirstOrDefault();
            message.Seen = "Seen";
            db.SaveChanges();
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string MessageSentbox(string Id)
        {
            var messages = db.AspNetMessages.Where(x => x.SenderID == Id).Select(x => new { x.Message, x.Time, x.Subject, Name = x.AspNetMessage_Receiver.Select(y => y.AspNetUser.Name) }).ToList();

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(messages);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string SendMessage(string senderId, string reciverId, string message, string Subject)
        {
            AspNetMessage Message = new AspNetMessage();
            AspNetMessage_Receiver Receiver = new AspNetMessage_Receiver();
            try
            {

                var ReciverEmail = db.AspNetUsers.Where(x => x.Id == reciverId).Select(x => x.Email).FirstOrDefault();
                var Name = db.AspNetUsers.Where(x => x.Id == reciverId).Select(x => x.Name).FirstOrDefault();

                Message.Message = message;
                Message.SenderID = senderId;
                Message.Subject = Subject;
                Message.Time = DateTime.Now;

                db.AspNetMessages.Add(Message);
                db.SaveChanges();

                Receiver.MessageID = Message.Id;
                Receiver.ReceiverID = reciverId;
                Receiver.Seen = "Not Seen";
                Receiver.SeenTime = DateTime.Now;

                db.AspNetMessage_Receiver.Add(Receiver);
                db.SaveChanges();

                //SendEmail(ReciverEmail, "Message from" + Name, message);

                return "message has been sent";
            }
            catch (Exception ex)
            {
                return "Message Can't be sent. Errror" + ex.Message;
            }

        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string GetMessageRecievers(string role)
        {
            if (role == "Parent")
            {
                var users = (from Users in db.AspNetUsers.Where(x => x.Status != "False")
                             where (Users.AspNetRoles.Select(y => y.Name).Contains("Admin") || Users.AspNetRoles.Select(y => y.Name).Contains("Principal"))
                             select new
                             {
                                 Users.Id,
                                 Users.Email,
                                 Users.UserName,
                                 Users.Name,
                             }).ToList();

                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(users);

                return jsonString;
            }
            else
            {
                return "Mail box for this role is under maintainence";
            }

        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
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
            catch (Exception ex)
            {
                return false;
            }

        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string profile(string Id)
        {

            var user = db.AspNetUsers.Where(x => x.Id == Id).Select(x => new { x.Name, x.UserName, x.PhoneNumber, x.Email, });

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(user);

            return jsonString;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string Notification(string Id)
        {
            try
            {
                AspNetUser currentUser = db.AspNetUsers.First(x => x.Id == Id);
                
                    var NotificationsList = (from notification in db.AspNetNotification_User
                                             where notification.UserID == currentUser.Id && notification.Seen == false
                                             select new { notification.Id, notification.AspNetNotification.Url ,notification.AspNetNotification.Subject, notification.AspNetNotification.Time, notification.AspNetNotification.Description, notification.AspNetNotification.SenderID }).ToList();

                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(NotificationsList);

                    return jsonString;
                
            }
            catch
            {
                return "";
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string SeeNotification(int Id)
        {
            string status = "error";
            AspNetNotification_User Notification = db.AspNetNotification_User.Where(x => x.Id == Id).Select(x => x).FirstOrDefault();
            if (Notification == null)
            {
                status = "no data found";
            }
            else
            {
                Notification.Seen = true;
                if (db.SaveChanges() > 0)
                {
                    status = "success";
                }
            }
           return status;
        }

        /////////////////////////////////////////////////  Class /////////////////////////////////////////////////////


        public class notifications
        {
            public int? Id { get; set; }
            public string Message { get; set; }
            public DateTime? Time { get; set; }
            public string UserID { get; set; }
        }
    }
}
