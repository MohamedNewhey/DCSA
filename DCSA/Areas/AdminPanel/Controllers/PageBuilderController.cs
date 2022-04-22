using DCSA.Database;
using DCSA.Helpers;
using DCSA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [WebsiteOnly]
    public class PageBuilderController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: PageBulder
        public ActionResult Index(string Search, string searchValue)
        {
            var model = new List<Caus>();
        
            ViewBag.Search = Search;

            if(searchValue == null || searchValue == "")
                ViewBag.SearchValue = "تـصنيــــف";
            else
                ViewBag.SearchValue = searchValue;

            model = db.Causes.ToList();
            return View(model);
        }


        public ActionResult Display(int id)
        {
            return View(db.Causes.First(x=>x.ID==id));
        }

        public ActionResult Publish(int id, int type)
        {
            JsonValidationModel JV = new JsonValidationModel();

            try
            {
                var model = db.Causes.Find(id);

              
                db.SaveChanges();

                JV.IsValid = true;
                if (type == 1)
                    JV.Message = "تم النشر بنجاح";
                else
                    JV.Message = "تم الايقاف بنجاح";

                return Json(JV, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                JV.IsValid = false;
                JV.Message = "حدث خطأ في النظام";

                return Json(JV, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult Manage(string ID)
        {
            ViewBag.Types = new SelectList(db.CauseTypes, "ID", "Name");

            if (ID == null)
            {
                Caus model = new Caus();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                var model = db.Causes.First(x => x.ID == WID);
                if (model.CauseContent.Contains("../../"))
                {
                    var newContent = model.CauseContent.Replace("../..", "../../..");
                    model.CauseContent = newContent;
                }
                return View(model);
            }

        }

        public ActionResult SaveFile() {
         
            string FileName = "";
            var ShareURL = string.Format("{0}://{1}{2}{3}",
          System.Web.HttpContext.Current.Request.Url.Scheme,
          System.Web.HttpContext.Current.Request.Url.Host,
          System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port,
          System.Web.HttpContext.Current.Request.ApplicationPath);
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];

                if (file != null && file.ContentLength > 0)
                {
                    string FilesPath = "~/UploadedFiles/FormsFiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                
                    System.IO.Directory.CreateDirectory(Server.MapPath(FilesPath));

                     FileName = Guid.NewGuid() + "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];

                    string path = Path.Combine(Server.MapPath(FilesPath + "/"), FileName);
                    file.SaveAs(path);
                }
            }
            return Json(new { FilePath = ShareURL+ "UploadedFiles/FormsFiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month +"/"+ FileName });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePage(PageSaveModel model)
        {
            JsonValidationModel JV = new JsonValidationModel();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int UserID = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            try
            {
                

                if (model.ID == -1)
                {
                    if (model.Header == null || model.Header == "")
                    {
                        JV.FieldID = "Header";
                        JV.ValidFieldID = "ValidHeader";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل عنوان الصفحة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }


                    if (model.CoverPhoto == null)
                    {
                        JV.FieldID = "file1";
                        JV.ValidFieldID = "ValidPhoto";
                        JV.Step = -1;
                        JV.Message = "من فضلك قم بتحميل صورة الغلاف";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    if (model.ShortDescription == null || model.ShortDescription == "")
                    {
                        JV.FieldID = "exampleFormControlTextarea1";
                        JV.ValidFieldID = "ValidDes";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل وصف الصفحة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    Caus page = new Caus();
                    page.TypeID = model.TypeID;
                    page.Header = model.Header;
                    page.CauseContent = model.Content;
                    page.StartPrice = model.StartPrice;
                    page.ShortDescription = model.ShortDescription;
                    page.CreationDate = DateTime.Now;
                    page.TargetMoney = model.TargetMoney;
                   // page.CauseDate = model.PageDate;

                    if (model.PageOrder == 0)
                        page.CauseOrder = 1;
                    else
                        page.CauseOrder = model.PageOrder;

                    Regex reg = new Regex("[&/\\#,+()$~%.'…“”:*?<>{};=*'\",_&#^@]");
                  //  var url = model.URL.Replace(' ', '-');
                  //  url = reg.Replace(url, string.Empty);
                   // page.URL = url;
                    page.Publish = true;
                    page.UserID = UserID;

                    db.Causes.Add(page);
                    db.SaveChanges();

                    var ID = page.ID;
                    string ReportPath = "~/UploadedFiles/Pages/" + ID;
                    string CoverPhoto = "~/UploadedFiles/Pages/" + ID + "/CoverPhoto";

                    System.IO.Directory.CreateDirectory(Server.MapPath(ReportPath));
                    if (model.CoverPhoto != null)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(CoverPhoto));
                        var CoverPath = Path.Combine(Server.MapPath(CoverPhoto + "/"), model.CoverPhoto.FileName);
                        model.CoverPhoto.SaveAs(CoverPath);
                        page.CoverPhoto = "/UploadedFiles/Pages/" + ID + "/CoverPhoto/" + model.CoverPhoto.FileName;
                    }

                    db.SaveChanges();
                }
                else
                {

                    if (model.Header == null || model.Header == "")
                    {
                        JV.FieldID = "Header";
                        JV.ValidFieldID = "ValidHeader";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل عنوان الصفحة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }
                    if (model.ShortDescription == null || model.ShortDescription == "")
                    {
                        JV.FieldID = "exampleFormControlTextarea1";
                        JV.ValidFieldID = "ValidDes";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل وصف الصفحة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    Caus page = db.Causes.Find(model.ID);
                    page.TypeID = model.TypeID;
                    page.Header = model.Header;
                    page.CauseContent = model.Content;
                    page.ShortDescription = model.ShortDescription;
                    page.CreationDate = DateTime.Now;
                    page.TargetMoney = model.TargetMoney;
                    page.StartPrice = model.StartPrice;
                    //  page.CauseDate = model.PageDate;
                    page.UserID = UserID;

                    if (model.PageOrder == 0)
                        page.CauseOrder = 1;
                    else
                        page.CauseOrder = model.PageOrder;

                    Regex reg = new Regex("[&/\\#,+()$~%.'…“”:*?<>{};=*'\",_&#^@]");
                   // var url = model.URL.Replace(' ', '-');
                  //  url = reg.Replace(url, string.Empty);
                 //   page.URL = url;
                    db.SaveChanges();

                    var ID = page.ID;
                    string ReportPath = "~/UploadedFiles/Pages/" + ID;
                    string CoverPhoto = "~/UploadedFiles/Pages/" + ID + "/CoverPhoto";

                    System.IO.Directory.CreateDirectory(Server.MapPath(ReportPath));
                    if (model.CoverPhoto != null)
                    {
                        string fullPath = Request.MapPath(page.CoverPhoto);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        System.IO.Directory.CreateDirectory(Server.MapPath(CoverPhoto));
                        var CoverPath = Path.Combine(Server.MapPath(CoverPhoto + "/"), model.CoverPhoto.FileName);
                        model.CoverPhoto.SaveAs(CoverPath);
                        page.CoverPhoto = "/UploadedFiles/Pages/" + ID + "/CoverPhoto/" + model.CoverPhoto.FileName;
                    }

                    db.SaveChanges();
                }

                JV.FieldID = "";
                JV.ValidFieldID = "";
                JV.Step = -1;
                JV.Message = "تم حفظ البيانات بنجاح";
                JV.IsValid = true;

                return Json(JV, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
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
        public ActionResult DeletePage(int ID) {

            JsonValidationModel JVM = new JsonValidationModel();
            try
            {


                Caus model = db.Causes.Find(ID);

                db.Causes.Remove(model);
                db.SaveChanges();

                if(model.CoverPhoto != null)
                {
                    string fullPath = Request.MapPath(model.CoverPhoto);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImagesFolder = "~/UploadedFiles/Pages/" + model.ID + "/CoverPhoto";
                    if(System.IO.Directory.Exists(Server.MapPath(ImagesFolder)))
                         System.IO.Directory.Delete(Server.MapPath(ImagesFolder));

                    string Folder = "~/UploadedFiles/Pages/" + model.ID;
                    if(System.IO.Directory.Exists(Server.MapPath(Folder)))
                        System.IO.Directory.Delete(Server.MapPath(Folder));
                }

                JVM.IsValid = true;
                JVM.Message ="تمت العملية بنجاح";
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