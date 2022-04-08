using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class SupportSaveModel
    {
        public string PersonName { get; set; }
        public List<char> PhonNo { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int PC { get; set; }
        public string FileNo { get; set; }
        public string RecapResponse { get; set; }
        public string ChildName { get; set; }
        public int Age { get; set; }
    }
}