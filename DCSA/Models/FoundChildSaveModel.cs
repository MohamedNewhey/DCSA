using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class FoundChildSaveModel
    {
        public string PersonName { get; set; }
        public List<char> NationalID { get; set; }
        public List<char> PhonNo { get; set; }
        public int GoverID { get; set; }
        public int DistrictID { get; set; }
        public string Address { get; set; }
        public string ChildName { get; set; }
        public int ChildNameSource { get; set; }
        public HttpPostedFileBase ChildImage { get; set; }

        public string Gender { get; set; }
        public List<int> HealthStat { get; set; }

        public List<int> DisTypeID { get; set; }

        public DateTime? Birthdate { get; set; }
        public int Age { get; set; }
        public int KnowType { get; set; }

        public int FoundGoverID { get; set; }

        public int FoundDistrictID { get; set; }
        public string FoundAddress { get; set; }
        public int ExistGoverID { get; set; }
        public int ExistDistrictID { get; set; }
        public string ExistAddress { get; set; }
        public int IsKnown { get; set; }
        public string ClothesDes { get; set; }
        public string SpecialClo { get; set; }

        public int ReportBit { get; set; }
        public string ReportNo { get; set; }
        public string PoliceStation { get; set; }
        public DateTime? ReportDate { get; set; }
        public HttpPostedFileBase ReportImage { get; set; }
        public int PP { get; set; }
        public List<string> AttachmentName { get; set; }
        public List<HttpPostedFileBase> AttachmentDoc { get; set; }
        public string RecapResponse { get; set; }

    }
}