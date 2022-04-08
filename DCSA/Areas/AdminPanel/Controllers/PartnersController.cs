using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class PartnersController : AdminBaseController
    {
        private DefaultConnection db = new DefaultConnection();

      
        public ActionResult Index(string Search)
        {
            var model = new List<Partner>();
            ViewBag.Search = Search;

            if (Search != null && Search != "")
                model = db.Partners.Where(x => x.Name.Contains(Search)).ToList();

            else
                model = db.Partners.ToList();

            return View(model);
        }

       
        public ActionResult Manage(string ID)
        {
            if (ID == null)
            {
                var model = new Partner();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                var model = db.Partners.First(x => x.ID == WID);
                return View(model);
            }
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(PartnerSaveModel model)
        {
            JsonValidationModel JV = new JsonValidationModel();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int UserID = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            try
            {
                if (model.ID == -1)
                {
                    if(model.Name == null || model.Name == "")
                    {
                        JV.FieldID = "Name";
                        JV.ValidFieldID = "ValidName";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل الاسم";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    if(model.URL == null || model.URL == "")
                    {
                        JV.FieldID = "URL";
                        JV.ValidFieldID = "ValidURL";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل الرابط";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    if (model.Logo == null)
                    {
                        JV.FieldID = "CoverPhoto1";
                        JV.ValidFieldID = "ValidLogo";
                        JV.Step = -1;
                        JV.Message = "من فضلك قم بتحميل الصورة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }
                    Partner PTR = new Partner();
                    PTR.URL = model.URL;
                    PTR.Name = model.Name;
                    PTR.UserID = UserID;
                    PTR.Publish = true;

                    db.Partners.Add(PTR);
                    db.SaveChanges();

                    var ID = PTR.ID;
                    string LogoPath = "~/UploadedFiles/Partners/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                    if (model.Logo != null)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                        var CoverPath = Path.Combine(Server.MapPath(LogoPath + "/"), model.Logo.FileName);
                        model.Logo.SaveAs(CoverPath);
                        PTR.Logo = "/UploadedFiles/Partners/" + ID + "/" + model.Logo.FileName;
                    }
                   
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

                    if (model.URL == null || model.URL == "")
                    {
                        JV.FieldID = "URL";
                        JV.ValidFieldID = "ValidURL";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل الرابط";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    

                    Partner PTR = db.Partners.First(x => x.ID == model.ID);
                    PTR.URL = model.URL;
                    PTR.Name = model.Name;
                    PTR.UserID = UserID;

                    var ID = PTR.ID;

                    string LogoPath = "~/UploadedFiles/Partners/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                    if (model.Logo != null)
                    {
                        string fullPath = Request.MapPath(PTR.Logo);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                        var CoverPath = Path.Combine(Server.MapPath(LogoPath + "/"), model.Logo.FileName);
                        model.Logo.SaveAs(CoverPath);
                        PTR.Logo = "/UploadedFiles/Partners/" + ID + "/" + model.Logo.FileName;
                    }

             

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
        public ActionResult DeletePartner(int id)
        {
            JsonValidationModel JVM = new JsonValidationModel();
            try
            {

                Partner partner = db.Partners.Find(id);
                db.Partners.Remove(partner);
                db.SaveChanges();

                if (partner.Logo != null)
                {
                    string fullPath = Request.MapPath(partner.Logo);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImagesFolder = "~/UploadedFiles/Partners/" + partner.ID;
                    if (System.IO.Directory.Exists(Server.MapPath(ImagesFolder)))
                        System.IO.Directory.Delete(Server.MapPath(ImagesFolder));
                }

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

        public ActionResult Publish(int id, int type)
        {
            JsonValidationModel JV = new JsonValidationModel();

            try
            {
                var model = db.Partners.Find(id);

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
