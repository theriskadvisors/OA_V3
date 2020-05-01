using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.Models.StudentAssessments
{
    public class AssessmentsQ_A
    {
        public int Id { get; set; }

        public Nullable<int> STAID { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Catageory { get; set; }

        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public List<AspNetStudent_Subject> sublistlist { get; set; }

        public List<AspNetStudent_Term_Assessments_Answers> questionlist { get; set; }
    }
}