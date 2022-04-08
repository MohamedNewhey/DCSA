using DCSA.Database;
using DCSA.Helpers;
using DCSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [WebsiteOnly]
    public class StaticPagesController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();
        // GET: AdminPanel/StaticPages
        public ActionResult Index(string Search, string searchValue)
        {
            var model = new List<StaticPage>();
            ViewBag.Search = Search;

            if (searchValue == null || searchValue == "")
                ViewBag.SearchValue = "تـصنيــــف";
            else
                ViewBag.SearchValue = searchValue;

            if (Search != null && Search != "" && (searchValue == null || searchValue == "" || searchValue.Trim() == "تـصنيــــف"))
                model = db.StaticPages.Where(x => x.MainPage == false && (x.Name.Contains(Search) || x.URL.Contains(Search) || x.PageContent.Contains(Search))).ToList();

            else if (Search != null && Search != "" && searchValue != null && searchValue != "" && searchValue.Trim() != "تـصنيــــف")
            {
                if (searchValue.Trim() == "العنوان")
                    model = db.StaticPages.Where(x => x.MainPage == false && x.Name.Contains(Search)).ToList();
                if (searchValue.Trim() == "الرابط")
                    model = db.StaticPages.Where(x => x.MainPage == false && x.URL.Contains(Search)).ToList();
                if (searchValue.Trim() == "المحتوى")
                    model = db.StaticPages.Where(x => x.MainPage == false && x.PageContent.Contains(Search)).ToList();
            }

            else
                model = db.StaticPages.Where(x => x.MainPage == false).ToList();

            return View(model);
        }

        public ActionResult Publish(int id, int type)
        {
            JsonValidationModel JV = new JsonValidationModel();

            try
            {
                var model = db.StaticPages.Find(id);

                if (type == 1)
                    model.Publish = true;
                else
                    model.Publish = false;

                db.SaveChanges();

                JV.IsValid = true;
                if (type == 1)
                    JV.Message = "تم النشر بنجاح";
                else
                    JV.Message = "تم الايقاف بنجاح";

                return Json(JV, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                JV.IsValid = false;
                JV.Message = "حدث خطأ في النظام";

                return Json(JV, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ManageMainPages(string ID)
        {
            ViewBag.ID = null;

            if (ID == null)
            {
                var model = new StaticPage();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                ViewBag.ID = WID;

                if (WID == 2)
                    ViewBag.URL = Url.Action("ContactUS", "DisplayPages", new { area = ""});
                if(WID == 3)
                    ViewBag.URL = Url.Action("History", "DisplayPages", new { area = "" });
                if (WID == 4)
                    ViewBag.URL = Url.Action("SecretaryWord", "DisplayPages", new { area = "" });
                if (WID == 9)
                    ViewBag.URL = Url.Action("Message", "DisplayPages", new { area = "" });
                if (WID == 10)
                    ViewBag.URL = Url.Action("Role", "DisplayPages", new { area = "" });

                var model = db.StaticPages.First(x => x.ID == WID);
                if (model.PageContent.Contains("../../"))
                {
                    var newContent = model.PageContent.Replace("../..", "../../..");
                    model.PageContent = newContent;
                }
                return View(model);
            }
        }

        public ActionResult ManageServicesPages(string ID)
        {
            ViewBag.ID = null;

            if (ID == null)
            {
                var model = new StaticPage();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                ViewBag.ID = WID;

                if (WID == 25)
                    ViewBag.URL = Url.Action("LegalServices", "Services", new { area = "" });
                if (WID == 26)
                    ViewBag.URL = Url.Action("PsychicServices", "Services", new { area = "" });
                

                var model = db.StaticPages.First(x => x.ID == WID);
                if (model.PageContent.Contains("../../"))
                {
                    var newContent = model.PageContent.Replace("../..", "../../..");
                    model.PageContent = newContent;
                }
                return View(model);
            }
        }

        public ActionResult Manage(string ID)
        {
            if (ID == null)
            {
                var model = new StaticPage();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                var model = db.StaticPages.First(x => x.ID == WID);
                if (model.PageContent.Contains("../../"))
                {
                    var newContent = model.PageContent.Replace("../..", "../../..");
                    model.PageContent = newContent;
                }
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(StaticPagesSaveModel model)
        {
            JsonValidationModel JV = new JsonValidationModel();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int UserID = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            try
            {

                if (model.ID == -1)
                {
                    if (model.Name == null || model.Name == "")
                    {
                        JV.FieldID = "Name";
                        JV.ValidFieldID = "ValidName";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل الاسم";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    if(model.MainPage != 1)
                    {
                        if (model.URL == null || model.URL == "")
                        {
                            JV.FieldID = "URL";
                            JV.ValidFieldID = "ValidURL";
                            JV.Step = -1;
                            JV.Message = "من فضلك ادخل ال URL";
                            JV.IsValid = false;

                            return Json(JV, JsonRequestBehavior.AllowGet);
                        }

                        var urlCheck = db.StaticPages.Where(x => x.URL.Contains(model.URL)).FirstOrDefault();
                        if (urlCheck != null)
                        {
                            JV.FieldID = "URL";
                            JV.ValidFieldID = "ValidURL";
                            JV.Step = -1;
                            JV.Message = "هذا ال URL موجود من قبل";
                            JV.IsValid = false;

                            return Json(JV, JsonRequestBehavior.AllowGet);
                        }
                    }

                   

                    if (model.PageDate == null || model.PageDate == DateTime.Parse("1/1/0001 12:00:00 AM"))
                    {
                        JV.FieldID = "PageDate";
                        JV.ValidFieldID = "ValidDate";
                        JV.Step = -1;
                        JV.Message = "من فضلك اختر التاريخ";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    if (model.Content == null || model.Content == "")
                    {
                        JV.FieldID = "Content";
                        JV.ValidFieldID = "ValidContent";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل محتوى الصفحة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    StaticPage SP = new StaticPage();
                    SP.PageContent = model.Content;
                    SP.Name = model.Name;
                    SP.URL = model.URL;
                    SP.Publish = true;
                    SP.UserID = UserID;

                    if (model.MainPage == 1)
                        SP.MainPage = true;
                    else
                        SP.MainPage = false;


                    SP.CreationDate = DateTime.Now;
                    SP.PageDate = model.PageDate;
                    if (model.PageOrder == 0)
                        SP.PageOrder = 1;
                    else
                        SP.PageOrder = model.PageOrder;


                    db.StaticPages.Add(SP);
                    db.SaveChanges();

                    JV.FieldID = "";
                    JV.ValidFieldID = "";
                    JV.Step = -1;
                    JV.Message = "تم حفظ البيانات بنجاح";
                    JV.IsValid = true;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (model.Name == null || model.Name == "")
                    {
                        JV.FieldID = "Name";
                        JV.ValidFieldID = "ValidName";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل الاسم";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    if(model.MainPage != 1)
                    {
                        if (model.URL == null || model.URL == "")
                        {
                            JV.FieldID = "URL";
                            JV.ValidFieldID = "ValidURL";
                            JV.Step = -1;
                            JV.Message = "من فضلك ادخل ال URL";
                            JV.IsValid = false;

                            return Json(JV, JsonRequestBehavior.AllowGet);
                        }

                        var urlCheck = db.StaticPages.Where(x => x.URL.Contains(model.URL) && x.ID != model.ID).FirstOrDefault();
                        if (urlCheck != null)
                        {
                            JV.FieldID = "URL";
                            JV.ValidFieldID = "ValidURL";
                            JV.Step = -1;
                            JV.Message = "هذا ال URL موجود من قبل";
                            JV.IsValid = false;

                            return Json(JV, JsonRequestBehavior.AllowGet);
                        }

                    }

                    if (model.PageDate == null || model.PageDate == DateTime.Parse("1/1/0001 12:00:00 AM"))
                    {
                        JV.FieldID = "PageDate";
                        JV.ValidFieldID = "ValidDate";
                        JV.Step = -1;
                        JV.Message = "من فضلك اختر التاريخ";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }
                   
                    if (model.Content == null || model.Content == "")
                    {
                        JV.FieldID = "Content";
                        JV.ValidFieldID = "ValidContent";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل محتوى الصفحة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    StaticPage SP = db.StaticPages.First(x => x.ID == model.ID);
                    SP.PageContent = model.Content;
                    SP.Name = model.Name;
                    SP.URL = model.URL;
                    SP.UserID = UserID;

                    if (model.MainPage == 1)
                        SP.MainPage = true;
                    else
                        SP.MainPage = false;

                    SP.CreationDate = DateTime.Now;
                    SP.PageDate = model.PageDate;
                    if (model.PageOrder == 0)
                        SP.PageOrder = 1;
                    else
                        SP.PageOrder = model.PageOrder;

                    db.SaveChanges();

                    JV.FieldID = "";
                    JV.ValidFieldID = "";
                    JV.Step = -1;
                    JV.Message = "تم حفظ البيانات بنجاح";
                    JV.IsValid = true;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                JV.FieldID = "";
                JV.ValidFieldID = "";
                JV.Step = -1;
                JV.Message = "حدث خطأ";
                JV.IsValid = false;

                return Json(JV, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult DeletePage(int id)
        {
            JsonValidationModel JVM = new JsonValidationModel();
            try
            {
                
                StaticPage staticPage = db.StaticPages.Find(id);
                if (staticPage.MainPage.Value)
                {
                    JVM.IsValid = false;
                    JVM.Message = "خطاء فى النطام";
                    return Json(JVM, JsonRequestBehavior.AllowGet);
                }
                db.StaticPages.Remove(staticPage);
                db.SaveChanges();

                JVM.IsValid = true;
                JVM.Message = "تمت العملية بنجاح";
                return Json(JVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JVM.IsValid = false;
                JVM.Message = "خطاء فى النظام";
                return Json(JVM, JsonRequestBehavior.AllowGet);

            }

        }

    }
}