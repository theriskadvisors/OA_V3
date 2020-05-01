using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public class TreeViewNodeVM
    {
        public TreeViewNodeVM()
        {
            ChildNode = new List<TreeViewNodeVM>();
        }

        public int Id { set; get; }
        public string Code { get; set; }
        public string Balance { set; get; }
        public string Type { set; get; }
        public string Name { get; set; }
        public double LedgerDebit { set; get; }
        public double LedgerCredit { set; get; }
        public string TotalBalance { get; set; }
        public string NodeName
        {
            get { return GetNodeName(); }
        }
        public IList<TreeViewNodeVM> ChildNode { get; set; }

        public string GetNodeName()
        {

            return this.Code + "  " + this.Name;
        }
    }

}