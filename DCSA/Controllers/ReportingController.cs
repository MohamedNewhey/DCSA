using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class ReportingController : BaseController
    {
        [Route("للابلاغ-والدعم-والمشورة")]
        public ActionResult Index()
        {
            return View();
        }

      

    }
}