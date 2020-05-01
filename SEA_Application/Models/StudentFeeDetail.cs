using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class Previous
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
    public class StudentFeeDetail
    {
        public string StudentId { get; set; }
        public string ChallanNo { get; set; }
        public string StudentName { get; set; }
        public string StudentRegNo { get; set; }
        public List<AspNetClass_FeeType> FeeBreakdown { get; set; }
        public List<Previous> PreviousFee { get; set; }
        public int Fine { get; set; }
        public int Discount { get; set; }
        public int GrossFee { get; set; }
        public int TotalFee { get; set; }
    }
}