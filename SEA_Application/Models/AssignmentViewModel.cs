using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SEA_Application.Models
{
    public class AssignmentViewModel
    {

        public int AssignmentId { get; set; }

        [Display(Name = "Student Name")]
        public string NameOfStudent { get; set; }
        public string Section { get; set; }
        [Display(Name = "Course Type")]
        public string CourseType { get; set; }

        public string Topic { get; set; }
        public string Lesson { get; set; }
        [Display(Name = "Assignment")]

        public string AssignmentName { get; set; }

        [Display(Name = "Subject Name")]

        public string SubjectName { get; set; }

        [Display(Name = "Assignment Due Date")]
        public DateTime? AssignmnetDueDate { get; set; }

        [Display(Name = "Assignment Submitted Date")]

        public DateTime? AssignmentSubmittedDate { get; set; }

    }
}