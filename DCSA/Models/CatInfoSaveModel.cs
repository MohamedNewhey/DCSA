using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
  
    public class CatInfoSaveModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase Logo { get; set; }
    }
}