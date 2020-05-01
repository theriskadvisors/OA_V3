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
    
    public partial class AspNetSalaryDetail
    {
        public int Id { get; set; }
        public Nullable<int> GrossSalary { get; set; }
        public Nullable<int> MedicalAllowance { get; set; }
        public Nullable<int> AccomodationAllowance { get; set; }
        public Nullable<int> Tax { get; set; }
        public Nullable<int> BasicSalary { get; set; }
        public Nullable<int> FineCut { get; set; }
        public Nullable<int> AfterCutSalary { get; set; }
        public Nullable<int> AdvancePf { get; set; }
        public Nullable<int> EmployeePF { get; set; }
        public Nullable<int> AdvanceSalary { get; set; }
        public Nullable<int> EmployeeEOP { get; set; }
        public Nullable<int> Total { get; set; }
        public Nullable<int> SchoolEOP { get; set; }
        public Nullable<int> BTaxSalary { get; set; }
        public Nullable<int> ATaxSalary { get; set; }
        public Nullable<int> Bonus { get; set; }
        public Nullable<int> SalaryHold { get; set; }
        public Nullable<int> TotalSalary { get; set; }
        public string Status { get; set; }
        public int EmployeeId { get; set; }
        public int SalaryId { get; set; }
    
        public virtual AspNetEmployee AspNetEmployee { get; set; }
        public virtual AspNetSalary AspNetSalary { get; set; }
    }
}
