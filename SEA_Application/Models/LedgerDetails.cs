using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class LedgerDetails
    {
        [Key]
        public int Id { set; get; }
        public double credit { set; get; }
        public double debit { set; get; }
        public string description { set; get; }
        public string code { set; get; }
        public string Name { set; get; }
        public string time { set; get; }
        public string Balance { set; get; }
        public string Type { set; get; }
    }
}