using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class ComplaintSaveModel
    {
        public string PersonName { get; set; }
        public List<char> PhonNo { get; set; }
        public string ChildName { get; set; }
        public float Age { get; set; }
        public string Place { get; set; }
        public string Complaint { get; set; }
        public int PC { get; set; }
        public string FileNo { get; set; }
        public int PP { get; set; }
        public List<string> AttachmentName { get; set; }
        public List<HttpPostedFileBase> AttachmentDoc { get; set; }
        public string RecapResponse { get; set; }

    }
}