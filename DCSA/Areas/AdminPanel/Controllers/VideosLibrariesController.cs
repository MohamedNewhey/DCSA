using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using DCSA.Database;
using DCSA.Helpers;
using DCSA.Models;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [WebsiteOnly]
    public class VideosLibrariesController : AdminBaseController
    {
        private DefaultConnection db = new DefaultConnection();

       
        public ActionResult Index()
        {
            return View(db.VideosLibraries.ToList());
        }

        
        public ActionResult Manage(string ID)
        {
            if (ID == null)
            {
                var model = new VideosLibrary();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                var model = db.VideosLibraries.FirstOrDefault(x=>x.ID==WID);
                return View(model);
            }
          
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(VideosLibrary videosLibrary)
        {
            JsonValidationModel JV = new JsonValidationModel();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int UserID = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            try
                {
                    if (videosLibrary.Name == null || videosLibrary.Name == "")
                    {
                        JV.FieldID = "Name";
                        JV.ValidFieldID = "ValidName";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل اسم الفيديو";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                if (videosLibrary.Description == null || videosLibrary.Description == "")
                {
                    JV.FieldID = "Description";
                    JV.ValidFieldID = "ValidDes";
                    JV.Step = -1;
                    JV.Message = "من فضلك ادخل وصف الفيديو";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

                if (videosLibrary.URL == null || videosLibrary.URL == "")
                {
                    JV.FieldID = "URL";
                    JV.ValidFieldID = "ValidURL";
                    JV.Step = -1;
                    JV.Message = "من فضلك ادخل رابط الفيديو";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

                if (videosLibrary.ID == -1)
                    {
                    videosLibrary.UserID = UserID;
                    videosLibrary.Publish = true;
                    videosLibrary.CreationDate = DateTime.Now;

                    db.VideosLibraries.Add(videosLibrary);
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
                        var model = db.VideosLibraries.FirstOrDefault(x => x.ID == videosLibrary.ID);
                        model.URL = videosLibrary.URL;
                        model.Name = videosLibrary.Name;
                        model.UserID = UserID;
                    model.CreationDate = DateTime.Now;
                    model.PublishDate = videosLibrary.PublishDate;
                    model.VideoOrder = videosLibrary.VideoOrder;

                        model.Description = videosLibrary.Description;
                        db.SaveChanges();

                    JV.FieldID = "";
                    JV.ValidFieldID = "";
                    JV.Step = -1;
                    JV.Message = "تم حفظ البيانات بنجاح";
                    JV.IsValid = true;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }
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
     
        public ActionResult DeleteVideo(int id)
        {
            JsonValidationModel JV = new JsonValidationModel();


            try
            {
                VideosLibrary videosLibrary = db.VideosLibraries.Find(id);
                db.VideosLibraries.Remove(videosLibrary);
                db.SaveChanges();

                JV.FieldID = "";
                JV.ValidFieldID = "";
                JV.Step = -1;
                JV.Message = "تمت العملية بنجاح";
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

        public ActionResult Publish(int id, int type)
        {
            JsonValidationModel JV = new JsonValidationModel();

            try
            {
                var model = db.VideosLibraries.Find(id);

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
