using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class GrandTotal
    {
        public int StudentFeeMonthId { get; set; }
        public string RecurrenceFee { get; set; }
        public string NonRecurrenceFee { get; set; }
        public string Discount { get; set; }
        public string PlentyFee { get; set; }
        public string Arrear { get; set; }
    }
}