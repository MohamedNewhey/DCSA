using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Areas.AdminPanel.Models
{
    public class FormFilesSaveModel
    {
        public int ID { get; set; }
        public string FileEx { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}