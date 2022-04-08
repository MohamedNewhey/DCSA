using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class JsonValidationModel
    {
        public string FieldID { get; set; }
        public string ValidFieldID { get; set; }
        public bool IsValid { get; set; }
        public int Step { get; set; }
        public string Message { get; set; }
    }
}