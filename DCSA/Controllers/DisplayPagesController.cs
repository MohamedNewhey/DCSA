using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class DisplayPagesController : BaseController
    {
        DefaultConnection db = new DefaultConnection();
        // GET: DisplayPages

        [Route("الاتصال-بنا")]
        public ActionResult ContactUS()
        {
            var model = db.StaticPages.Where(x => x.ID == 2).FirstOrDefault();

            return View(model);
        }

        [Route("تاريخ-المجلس")]
        public ActionResult History()
        {
            var model = db.StaticPages.Where(x => x.ID == 3).FirstOrDefault();

            return View("ContactUS", model);
        }

        [Route("كلمة-الأمين-العام")]
        public ActionResult SecretaryWord()
        {
            var model = db.StaticPages.Where(x => x.ID == 4).FirstOrDefault();

            return View("ContactUS", model);
        }

        [Route("الرؤية-و-الرسالة")]
        public ActionResult Message()
        {
            var model = db.StaticPages.Where(x => x.ID == 9).FirstOrDefault();

            return View("ContactUS", model);
        }

        [Route("دور-المجلس")]
        public ActionResult Role()
        {
            var model = db.StaticPages.Where(x => x.ID == 10).FirstOrDefault();

            return View("ContactUS", model);
        }

        [Route("امناء-المجلس")]
        public ActionResult Secretaries()
        {
            var model = db.Secretaries.ToList();

            return View(model);
        }

        [Route("فريق-الدعوة-للحماية-ومناهضة-العنف")]
        public ActionResult AntiViolanceTeam()
        {
            var model = db.StaticPages.Where(x => x.ID == 12).FirstOrDefault();

            return View("ContactUS", model);
        }

        [Route("خط-نجدة-الطفل")]
        public ActionResult HelpLine()
        {
            var model = db.StaticPages.Where(x => x.ID == 13).FirstOrDefault();

            return View("ContactUS", model);
        }

        [Route("صفحات-الموقع/{name}")]
        public ActionResult DisplayStaticContent(string name)
        {
            var model = db.StaticPages.Where(x => x.URL.Contains(name)).FirstOrDefault();

            return View("ContactUS", model);
        }
    }
}