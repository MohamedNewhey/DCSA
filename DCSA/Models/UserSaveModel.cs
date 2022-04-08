using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class UserSaveModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int RoleID { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}