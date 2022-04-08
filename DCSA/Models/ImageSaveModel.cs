using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class ImageSaveModel
    {
        public int ID { get; set; }
        public int No { get; set; }
        public HttpPostedFileBase URL { get; set; }
    }
}