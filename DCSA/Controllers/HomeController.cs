using DCSA.Database;
using DCSA.Helpers;
using DCSA.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class HomeController : BaseController
    {
        DefaultConnection db = new DefaultConnection();

        [Route("")]
        [Route("Home/Index")]
        [Route("Home")]
        public ActionResult Index()
        {

            ViewBag.Causes = GlobalHelper.GetCauses();
            return View();
        }
        public ActionResult CauseDetails(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            var Model = db.Causes.FirstOrDefault(x => x.ID == id);
            if (Model == null)
                return RedirectToAction("Index");

            return View(Model);

        }

        [HttpPost]
        public ActionResult AddToSession(CartItem model)
        {
            var Cart = Session["Cart"] as List<CartItem>;
            if(Cart==null)
                Cart = new List<CartItem>();
            Cart.Add(model);
            Session.Add("Cart", Cart);

            return Json(new { Count = Cart.Count }, JsonRequestBehavior.AllowGet) ;
        }

        [Route("نتائج-البحث/{SearchWord?}")]
        public ActionResult SearchResult(string SearchWord)
        {
            List<SearchResultModel> model = new List<SearchResultModel>();
            model.AddRange(db.Causes.Where(x => (x.Header.Contains(SearchWord) || x.CauseContent.Contains(SearchWord)) && x.Publish.Value).Select(x =>
             new SearchResultModel { ID = x.ID, Name = x.Header, TypeID = x.TypeID.Value ,URL = x.URL, date = x.CauseDate.Value, CoverURL = x.CoverPhoto }));

            ViewBag.SearchWord = SearchWord;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetSearchResults(string SearchWord)
        {
            List<SearchResultModel> model = new List<SearchResultModel>();

           model.AddRange(db.Causes.Where(x => (x.Header.Contains(SearchWord) || x.CauseContent.Contains(SearchWord))&&x.Publish.Value).Select(x =>
            new SearchResultModel { ID = x.ID, Name = x.Header, TypeID = x.TypeID.Value, URL = x.URL, date = x.CauseDate.Value,CoverURL=x.CoverPhoto }));

          //  model.AddRange(db.PDFLibraries.Where(x => x.Header.Contains(SearchWord) && x.Publish.Value).Select(x =>
          //new SearchResultModel { ID = x.ID, Name = x.Header, TypeID = x.TypeID.Value, CatName = x.PDFLibraryType.Name, date = x.PdfDate.Value }));


            return PartialView("_GetSearchResults",model);
        }

        [Route("Robots.txt")]
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var ShareURL = string.Format("{0}://{1}{2}{3}",
            System.Web.HttpContext.Current.Request.Url.Scheme,
            System.Web.HttpContext.Current.Request.Url.Host,
            System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port,
            System.Web.HttpContext.Current.Request.ApplicationPath);

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            stringBuilder.AppendLine("disallow: /adminpanel/");
            stringBuilder.Append("sitemap: ");
            stringBuilder.AppendLine(ShareURL + "sitemap.xml");

            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }

        [Route("sitemap.xml")]
        public ActionResult SitemapXml()
        {
            var SM = new SitemapNode();
            var sitemapNodes = SM.GetSitemapNodes(this.Url);
            string xml = SM.GetSitemapDocument(sitemapNodes);
            return this.Content(xml, "text/xml", Encoding.UTF8);
        }


    }
}