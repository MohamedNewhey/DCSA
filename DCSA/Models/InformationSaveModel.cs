using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Models
{
    public class InformationSaveModel
    {
        public int ID { get; set; }
        public string Question { get; set; }
        [AllowHtml]
        public string Answer { get; set; }
        public int CategoryID { get; set; }
    }
}