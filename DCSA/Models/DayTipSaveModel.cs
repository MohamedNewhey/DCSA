using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class DayTipSaveModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}