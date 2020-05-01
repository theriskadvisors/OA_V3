using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class VoucherList
    {
        public string Time { set; get; }
        public int Id { set; get; }
        public string VoucherDescription { set; get; }
        public string Credit { set; get; }
        public string Debit { set; get; }
    }
}