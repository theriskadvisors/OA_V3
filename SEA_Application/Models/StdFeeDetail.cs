using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class StdFeeDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserName { get; set; }

        public double? InstalmentAmount { get; set; }

        public double? TotalFee { get; set; }
        public double? PayableFee { get; set; }
        public string Months { get; set; }

        public string Status { get; set; }

        public string ClassName { get; set; }
        public string CourseType { get; set; }

    }
}