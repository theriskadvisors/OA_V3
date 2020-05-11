using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class LessonViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Lesson Name")]
        public string LessonName { get; set; }
        [Display(Name = "Lesson Video Url")]
        public string LessonVideoURL { get; set; }
        [Display(Name = "Lesson Description")]

        public string LessonDescription { get; set; }

        [Display(Name = "Lesson Duration")]

        public decimal? LessonDuration { get; set; }

        [Display(Name = "Creation Date")]

        public DateTime CreationDate { get; set; }

        [Display(Name = "Select Topic")]

        public int TopicId { get; set; }


        public int OrderBy { get; set; }

        [Display(Name = "Assignment Name")]

        public string AssignmentName { get; set; }

        [Display(Name = "Assignment Description")]

        public string AssignmentDescription { get; set; }

        [Display(Name = "Assignment Due Date")]

        public DateTime? AssignmentDueDate { get; set; }


        [Display(Name = "Attachment Name-1")]
        public string AttachmentName1 { get; set; }
        [Display(Name = "Attachment Name-2")]

        public string AttachmentName2 { get; set; }
        [Display(Name = "Attachment Name-3")]


        public string AttachmentName3 { get; set; }

        [Display(Name = "Link Name")]

        public string LinkName1 { get; set; }
        [Display(Name = "Material Link 1")]

        public string LinkUrl1 { get; set; }
        [Display(Name = "Link Name")]

        public string LinkName2 { get; set; }

        [Display(Name = "Material Link 2")]

        public string LinkUrl2 { get; set; }
        [Display(Name = "Link Name")]

        public string LinkName3 { get; set; }
        [Display(Name = "Material Link 3")]

        public string LinkUrl3 { get; set; }

        [Display(Name = "Start Time")]

        public DateTime? StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "Add Link")]

        public string AddLink { get; set; }


        [Display(Name = "Is Active")]

        public bool IsActive { get; set; }



        [Display(Name = "Start Date")]

        public DateTime StartDate { get; set; }

        [Display(Name = "Due Date")]

        public DateTime DueDate { get; set; }

        [Display(Name = "Select Session")]

        public int SessionId { get; set; }

    }
}