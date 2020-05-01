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
    
    public partial class GenericSubject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GenericSubject()
        {
            this.AspnetSubjectTopics = new HashSet<AspnetSubjectTopic>();
            this.Student_GenericSubjects = new HashSet<Student_GenericSubjects>();
            this.Teacher_GenericSubjects = new HashSet<Teacher_GenericSubjects>();
        }
    
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string SubjectType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspnetSubjectTopic> AspnetSubjectTopics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student_GenericSubjects> Student_GenericSubjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teacher_GenericSubjects> Teacher_GenericSubjects { get; set; }
    }
}
