using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class PDFLibrarySaveModel
    {
        public int ID { get; set; }
        public string Header { get; set; }
        public int PdfOrder { get; set; }
        public DateTime PageDate { get; set; }
        public int TypeID { get; set; }
        public HttpPostedFileBase PdfLink { get; set; }
        public HttpPostedFileBase PdfLink_En { get; set; }
        public HttpPostedFileBase CoverPhoto { get; set; }
    }
}