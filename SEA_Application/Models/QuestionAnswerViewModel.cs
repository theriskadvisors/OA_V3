using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class QuestionAnswerViewModel
    {

        public int Id { get; set; }


        [Display(Name = "Question Name")]

        public string QuestionName { get; set; }
        
        [Display(Name="Lesson Name ")]
        public int LessonId { get; set; }
        [Display(Name="Created By")]

        public string QuestionnCreatedBy { get; set; }

        [Display(Name = "Is Quiz")]
        public bool QuestionIsQuiz { get; set; }

        [Display(Name = "Is Active")]

        public bool QuestionIsActive { get; set; }

        [Display(Name = "Question Type")]

        public string QuestionType { get; set; }

        [Display(Name = "Option Name (a)")]

        public string OptionNameOne { get; set; }

        [Display(Name = "Option Name (b)")]

        public string QuestionNameTwo { get; set; }

        [Display(Name = "Option Name (c)")]

        public string QuestionNameThree { get; set; }

        [Display(Name = "Option Name (d)")]

        public string QuesitonNameFour { get; set; }
       
        [Display(Name = "Select Answer")]

        public string Answer { get; set; }

        public String FillAnswer { get; set; }

    }
}