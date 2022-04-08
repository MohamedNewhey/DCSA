using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class PartnerSaveModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public HttpPostedFileBase Logo { get; set; }
    }
}