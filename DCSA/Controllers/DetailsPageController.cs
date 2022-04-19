using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class DetailsPageController : Controller
    {
        // GET: DetailsPage
        public ActionResult DetailsPage()
        {
            return View();
        }

        public ActionResult CartPage()
        {
            return View();
        }

        public ActionResult StaticPage1()
        {
            return View();
        }
    }
}