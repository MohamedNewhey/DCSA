using DCSA.Database;
using DCSA.Helpers;
using DCSA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [Authorize]
    public class ImagesController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/Images
        public ActionResult Index()
        {
            var model = db.Images.ToList();

            return View(model);
        }

        public ActionResult Manage(string ID)
        {
            if (ID == null)
            {
                var model = new Image();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                var model = db.Images.First(x => x.ID == WID);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ImageSaveModel model)
        {
            JsonValidationModel JV = new JsonValidationModel();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int UserID = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            try
            {
                if (model.ID == -1)
                {
                    if (model.No == 0 )
                    {
                        JV.FieldID = "No";
                        JV.ValidFieldID = "ValidNo";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل رقم الصورة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }


                    if (model.URL == null)
                    {
                        JV.FieldID = "CoverPhoto1";
                        JV.ValidFieldID = "ValidLogo";
                        JV.Step = -1;
                        JV.Message = "من فضلك قم بتحميل الصورة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    Image image = new Image();
                    image.No = model.No;
                    image.UserID = UserID;
                    image.Publish = true;

                    db.Images.Add(image);
                    db.SaveChanges();

                    var ID = image.ID;
                    string LogoPath = "~/UploadedFiles/ImageSlider/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                    if (model.URL != null)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                        var CoverPath = Path.Combine(Server.MapPath(LogoPath + "/"), model.URL.FileName);
                        model.URL.SaveAs(CoverPath);
                        image.URL = "/UploadedFiles/ImageSlider/" + ID + "/" + model.URL.FileName;
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
                    if (model.No == 0)
                    {
                        JV.FieldID = "No";
                        JV.ValidFieldID = "ValidNo";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل رقم الصورة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }



                    Image image = db.Images.First(x => x.ID == model.ID);
                    image.No = model.No;
                    image.UserID = UserID;

                    var ID = image.ID;

                    string LogoPath = "~/UploadedFiles/ImageSlider/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                    if (model.URL != null)
                    {
                        string fullPath = Request.MapPath(image.URL);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                        var CoverPath = Path.Combine(Server.MapPath(LogoPath + "/"), model.URL.FileName);
                        model.URL.SaveAs(CoverPath);
                        image.URL = "/UploadedFiles/ImageSlider/" + ID + "/" + model.URL.FileName;
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
        public ActionResult DeleteImage(int id)
        {
            JsonValidationModel JVM = new JsonValidationModel();
            try
            {

                Image image = db.Images.Find(id);
                db.Images.Remove(image);
                db.SaveChanges();

                if (image.URL != null)
                {
                    string fullPath = Request.MapPath(image.URL);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImagesFolder = "~/UploadedFiles/ImageSlider/" + image.ID;
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
                var model = db.Images.Find(id);

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
    }
}