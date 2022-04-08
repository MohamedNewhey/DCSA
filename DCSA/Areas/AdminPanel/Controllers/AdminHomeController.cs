using DCSA.Areas.AdminPanel.Models;
using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [Authorize]
    public class AdminHomeController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/AdminHome
        public ActionResult Index()
        {
            var currentYear = DateTime.Now.Year;
            HomeDataModel model = new HomeDataModel();
          
            return View(model);
        }
    }
}