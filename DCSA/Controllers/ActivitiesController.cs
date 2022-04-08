using DCSA.Database;
using DCSA.Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class ActivitiesController : BaseController
    {
        private DefaultConnection db = new DefaultConnection();

        //[Route("انشطة-المجلس/{name}")]
        //public ActionResult GetActivity(string name)
        //{
        //    if (name == null)
        //        return RedirectToAction("index", "Home");
        //    var PageType = db.PageTypes.FirstOrDefault(x => x.URL.Contains(name));
        //    ViewBag.PageHeader = name;
        //    ViewBag.Header = PageType.Name;

        //    if (PageType == null)
        //        return RedirectToAction("index", "Home");

        //    IPagedList<Page> model = null;
         
        //    model = GlobalHelper.OrderPages(PageType.ID).ToPagedList(1, 6);

        //    ViewBag.More = false;
        //    if (db.Pages.Where(x => x.TypeID == PageType.ID && x.Publish.Value).ToList().ToPagedList(2, 6).Count() != 0)
        //        ViewBag.More = true;

        //    return View(model);
        //}

        //[HttpGet]
        //public ActionResult _LoadMoreActivities(int? page, string name)
        //{
        //    var PageType = db.PageTypes.FirstOrDefault(x => x.URL.Contains(name));
        //    ViewBag.PageHeader = name;
        //    ViewBag.Header = PageType.Name;

        //    IPagedList<Page> model = null;

        //    ViewBag.LoadMore = false;

           
        //    if (page == null)
        //    {
        //        if (db.Pages.Where(x => x.TypeID == PageType.ID && x.Publish == true).ToList().ToPagedList(2, 6).Count() != 0)
        //            ViewBag.LoadMore = true;

               
        //        model = GlobalHelper.OrderPages(PageType.ID).ToPagedList(1, 6);
        //    }

        //    else
        //    {
        //        if (db.Pages.Where(x => x.TypeID == PageType.ID && x.Publish == true).ToList().ToPagedList(page.Value + 1, 6).Count() != 0)
        //            ViewBag.LoadMore = true;

        //        model = GlobalHelper.OrderPages(PageType.ID).ToPagedList(page.Value, 6);
        //    }

        //    return PartialView(model);
        //}

        //[Route("انشطة-المجلس")]
        //public ActionResult Index()
        //{
        //    return View();
        //}


        //[Route("انشطة-المجلس/{name}/{itemurl}")]
        //public ActionResult DisplayPage(string name, string itemurl)
        //{
        //    if (name == null)
        //        return RedirectToAction("index", "Home");
        //    var PageType = db.PageTypes.FirstOrDefault(x => x.URL.Contains(name));
        //    ViewBag.PageHeader = name;
        //    ViewBag.Header = PageType.Name;

        //    if (PageType == null)
        //        return RedirectToAction("index", "Home");

        //    var model = db.Pages.FirstOrDefault(x => x.TypeID == PageType.ID && x.Publish.Value && x.URL.Contains(itemurl));
        //    if (model == null)
        //        return RedirectToAction("index", "Activities", new { name = name });

        //    return View(model);
        //}


        //[Route("اخبار-المجلس")]
        //public ActionResult News()
        //{
        //    string name = "اخبار-المجلس";
        //    IPagedList<Page> model = null;

        //    var PageType = db.PageTypes.FirstOrDefault(x => x.URL.Contains(name));
        //    ViewBag.PageHeader = name;
        //    ViewBag.Header = PageType.Name;

        //    if (PageType == null)
        //        return RedirectToAction("index", "Home");

        //    model = GlobalHelper.OrderPages(PageType.ID).ToPagedList(1, 6);

        //    ViewBag.More = false;
        //    if (db.Pages.Where(x => x.TypeID == PageType.ID && x.Publish.Value).ToList().ToPagedList(2, 6).Count() != 0)
        //        ViewBag.More = true;

        //    return View("GetActivity", model);
        //}
    }
}