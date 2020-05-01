using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class DailyHomework
    {
        public int HomeworkID { get; set; }
        public int ClassID { get; set; }
        public DateTime Date { get; set; }

        public List<AspNetSubject_Homework> subjectHomework { get; set; }
    }
}