//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SEA_Application.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetAssessment_Topic
    {
        public int Id { get; set; }
        public Nullable<int> AssessmentID { get; set; }
        public Nullable<int> TopicID { get; set; }
    
        public virtual AspNetAssessment AspNetAssessment { get; set; }
        public virtual AspNetTopic AspNetTopic { get; set; }
    }
}
