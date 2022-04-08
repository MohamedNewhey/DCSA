using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class ServicesController : BaseController
    {
        DefaultConnection db = new DefaultConnection();

        [Route("خدمات-الدعم-القانوني-والنفسى")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("خدمات-الدعم-القانوني-والنفسى/خدمات-الدعم-القانونى")]
        public ActionResult LegalServices()
        {
            var model = db.StaticPages.Find(25);
            return View(model);
        }

        [Route("خدمات-الدعم-القانوني-والنفسى/خدمات-الدعم-نفسى")]
        public ActionResult PsychicServices()
        {
            var model = db.StaticPages.Find(26);

            return View("LegalServices", model);
        }
    }
}