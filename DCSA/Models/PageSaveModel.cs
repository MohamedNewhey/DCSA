using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Models
{
    public class PageSaveModel
    {
        public int ID { get; set; }
        public int TypeID { get; set; }
        public string Header { get; set; }
        public int PageOrder { get; set; }
        public double TargetMoney { get; set; }
        public double StartPrice { get; set; }
        //public DateTime PageDate { get; set; }
        //   public string URL { get; set; }
        public string ShortDescription { get; set; }
        public HttpPostedFileBase CoverPhoto { get; set; }

        [AllowHtml]
        public string Content { get; set; }
    }
}