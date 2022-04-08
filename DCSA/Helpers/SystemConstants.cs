using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Helpers
{
    public class SystemConstants
    {
        public enum Roles
        {
            Admin = 1,
            MobileAdmin = 2,
            WebsiteAdmin = 3,
            FormsAdmin = 4,
            HelplineApproveOnly = 5
        }
    }
}