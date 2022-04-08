using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Models
{
    public class StaticPagesSaveModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int PageOrder { get; set; }
        public DateTime PageDate { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string URL { get; set; }

        public int MainPage { get; set; }
    }
}