using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DefaultConnection db = new DefaultConnection();
            base.OnActionExecuting(filterContext);
        }
    }
}