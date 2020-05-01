using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SEA_Application.Models
{
    public class Utility
    {




        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        public void SMSToOffitialsa(string Message)
        {

            List<string> SMSList = new List<string>();

            SMSList.Add("923329687357");
            //SMSList.Add("923401562576");
            SMSList.Add("923354241669");//admin ngs
            //SMSList.Add("923009479542");//principle ngs
            //SMSList.Add("923224839049");


            foreach (var number in SMSList)
            {

                var url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
                String result = "";
                String messageer = HttpUtility.UrlEncode(Message);
                String strPost = "id=ipcngsch&pass=ipc_ngs123&msg=" + messageer + "&to=" + number + "&mask=IPC-NGS&type=xml&lang=English";
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
        public void SMSToOffitialsp(string Message)
        {

            List<string> SMSList = new List<string>();

            SMSList.Add("923329687357");
            //SMSList.Add("923401562576");
            //SMSList.Add("923354241669");//admin ngs
            SMSList.Add("923009479542");//principle ngs
            //SMSList.Add("923224839049");


            foreach (var number in SMSList)
            {

                var url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
                String result = "";
                String messageer = HttpUtility.UrlEncode(Message);
                String strPost = "id=ipcngsch&pass=ipc_ngs123&msg=" + messageer + "&to=" + number + "&mask=IPC-NGS&type=xml&lang=English";
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

        public void SendSMS(AspNetMessage message, List<string> SenderList)
        {

            List<string> SMSList = new List<string>();
            
            foreach (var sender in SenderList)
            {
                //db.AspNetUsers.Where(x => x.Id == sender).FirstOrDefault().PhoneNumber = "03209498765";
                //db.SaveChanges();
                var number = db.AspNetUsers.Where(x => x.Id == sender).Select(x => x.PhoneNumber).FirstOrDefault();
                if(number != null)
                {
                    var dfd = "92" + number.Substring(1);
                    SMSList.Add(dfd);
                }
                else
                {
                    //TODO
                    var ttio = "This Id don't have their phone number "+ sender +" NGS Portal";
                    messagetosupport(ttio);
                }
                
            }
             SMSList = new List<string>();
            SMSList.Add("923215008833");//abuzar
            SMSList.Add("923214518884");//arslan
            SMSList.Add("923214064254");//taha
            SMSList.Add("923401562576");//bilal
            //SMSList.Add("923009479542");//natasha

            foreach (var number in SMSList)
            {

                var url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
                String result = "";
                String messageer = HttpUtility.UrlEncode(message.Message);
                String strPost = "id=ipcngsch&pass=ipc_ngs123&msg=" + messageer + "&to=" + number + "&mask=IPC-NGS&type=xml&lang=English";
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

        public void messagetosupport(string message)
        {
            var url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
            String result = "";
            String messageer = HttpUtility.UrlEncode(message);
            String strPost = "id=ipcngsch&pass=ipc_ngs123&msg=" + messageer + "&to=923329687357&mask=IPC-NGS&type=xml&lang=English";
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

        public void datatt()
        {
            var students = db.AspNetStudents.ToList();
            foreach (var stu in students)
            {
                string parentId = "-";
                var studentid = stu.StudentID;
                var parentto = db.AspNetParent_Child.Where(p => p.ChildID == studentid).FirstOrDefault();
                if (parentto != null)
                {
                    parentId = parentto.ParentID;

                    var studentclass = db.AspNetClasses.Where(p => p.Id == stu.ClassID).FirstOrDefault().ClassName;
                    var section = db.AspNetClasses.Where(p => p.Id == stu.ClassID).FirstOrDefault().Section;

                    var parent = db.AspNetUsers.Where(p => p.Id == parentId).FirstOrDefault();
                    var student = db.AspNetUsers.Where(p => p.Id == studentid).FirstOrDefault();

                    ruffdata sds = new ruffdata();

                    sds.StudentName = student.Name;
                    sds.FatherName = parent.Name;
                    sds.FatherUserName = parent.UserName;
                    sds.StudentClassName = studentclass + " " + section;

                    var ty = parent.UserName.Split('1')[0].ToUpper();
                    var fty = ty.First();


                    string fullName = ty.ToLower();


                    char[] upper = fullName.ToCharArray();
                    string tuio = "";
                    upper[0] = char.ToUpper(upper[0]);
                    for (int i = 1; i < upper.Count(); i++)
                    {
                        tuio += upper[i];
                    }
                    string upperr = new string(upper) + " ";

                    var Password = fty + tuio + "@1234";
                    sds.FatherPassword = Password;

                    db.ruffdatas.Add(sds);
                    db.SaveChanges();
                }

            }

        }

    }
}