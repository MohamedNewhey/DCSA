using DCSA.Database;
using DCSA.Helpers;
using DCSA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [WebsiteOnly]
    public class SecretariesController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/Secretaries
        public ActionResult Index(string Search, string searchValue)
        {
            var model = new List<Secretary>();

            ViewBag.Search = Search;

            if (searchValue == null || searchValue == "")
                ViewBag.SearchValue = "تـصنيــــف";
            else
                ViewBag.SearchValue = searchValue;

            if (Search != null && Search != "" && (searchValue == null || searchValue == "" || searchValue.Trim() == "تـصنيــــف"))
                model = db.Secretaries.Where(x => x.Name.Contains(Search) || x.Description.Contains(Search)).ToList();
            else if (Search != null && Search != "" && searchValue != null && searchValue != "" && searchValue.Trim() != "تـصنيــــف")
            {
                if (searchValue.Trim() == "الاسم")
                    model = db.Secretaries.Where(x => x.Name.Contains(Search)).ToList();
                if (searchValue.Trim() == "نبذة عن الامين")
                    model = db.Secretaries.Where(x => x.Description.Contains(Search)).ToList();
            }
            else
                model = db.Secretaries.ToList();

            return View(model);
        }

        public ActionResult Manage(string ID)
        {
            if (ID == null)
            {
                var model = new Secretary();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                var model = db.Secretaries.First(x => x.ID == WID);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(SecretariesSaveModel model)
        {
            JsonValidationModel JV = new JsonValidationModel();

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

                    if (model.Image == null )
                    {
                        JV.FieldID = "Image";
                        JV.ValidFieldID = "ValidImage";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل صورة الامين";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    if (model.Description == null || model.Description == "")
                    {
                        JV.FieldID = "exampleFormControlTextarea1";
                        JV.ValidFieldID = "ValidDes";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل نبذة عن الامين";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    Secretary S = new Secretary();
                    S.Name = model.Name;
                    S.Description = model.Description;

                    db.Secretaries.Add(S);
                    db.SaveChanges();

                    var ID = S.ID;
                    string ImagePath = "~/UploadedFiles/Secretaries/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(ImagePath));
                    if (model.Image != null)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(ImagePath));
                        var CoverPath = Path.Combine(Server.MapPath(ImagePath + "/"), model.Image.FileName);
                        model.Image.SaveAs(CoverPath);
                        S.Image = "/UploadedFiles/Secretaries/" + ID + "/" + model.Image.FileName;
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

                    if (model.Description == null || model.Description == "")
                    {
                        JV.FieldID = "exampleFormControlTextarea1";
                        JV.ValidFieldID = "ValidDes";
                        JV.Step = -1;
                        JV.Message = "من فضلك ادخل نبذة عن الامين";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    Secretary S = db.Secretaries.First(x => x.ID == model.ID);
                    S.Name = model.Name;
                    S.Description = model.Description;

                    var ID = S.ID;

                    string ImagePath = "~/UploadedFiles/Secretaries/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(ImagePath));
                    if (model.Image != null)
                    {
                        string fullPath = Request.MapPath(S.Image);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        System.IO.Directory.CreateDirectory(Server.MapPath(ImagePath));
                        var CoverPath = Path.Combine(Server.MapPath(ImagePath + "/"), model.Image.FileName);
                        model.Image.SaveAs(CoverPath);
                        S.Image = "/UploadedFiles/Secretaries/" + ID + "/" + model.Image.FileName;
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
        public ActionResult DeleteSecretary(int id)
        {
            JsonValidationModel JVM = new JsonValidationModel();
            try
            {

                Secretary s = db.Secretaries.Find(id);
                
               

                if (s.Image != null)
                {
                    string fullPath = Request.MapPath(s.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImagesFolder = "~/UploadedFiles/Secretaries/" + s.ID;
                    if (System.IO.Directory.Exists(Server.MapPath(ImagesFolder)))
                        System.IO.Directory.Delete(Server.MapPath(ImagesFolder));
                }

                db.Secretaries.Remove(s);
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