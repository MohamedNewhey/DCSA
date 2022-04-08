using DCSA.Database;
using DCSA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class VideosController : BaseController
    {
      
        private DefaultConnection db = new DefaultConnection();
        [Route("مكتبة-الفيديوهات")]
        public ActionResult Index()
        {
            return View(GlobalHelper.OrderVideos());
        }
    }
}