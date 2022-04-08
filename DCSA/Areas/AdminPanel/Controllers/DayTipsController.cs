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
    [MobileOnly]
    public class DayTipsController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/DayTips
        public ActionResult Index(string Search)
        {
            var model = new List<DayTip>();
            ViewBag.Search = Search;

            if (Search != null && Search != "")
                model = db.DayTips.Where(x => x.Name.Contains(Search)).ToList();

            else
                model = db.DayTips.ToList();

            return View(model);
        }

        public ActionResult Manage(string ID)
        {
            ViewBag.ID = ID;

            if (ID == null)
            {
                var model = new DayTip();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                var model = db.DayTips.First(x => x.ID == WID);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(DayTipSaveModel model)
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
                        JV.Message = "من فضلك ادخل المعلومة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    if (model.Image == null)
                    {
                        JV.FieldID = "CoverPhoto1";
                        JV.ValidFieldID = "ValidLogo";
                        JV.Step = -1;
                        JV.Message = "من فضلك اختر الصورة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    var fromCheck = db.DayTips.Where(x => x.FromDate == model.FromDate || (x.FromDate < model.FromDate && x.ToDate > model.FromDate) || x.ToDate == model.FromDate).FirstOrDefault();
                    var toCheck = db.DayTips.Where(x => x.FromDate == model.ToDate || (x.FromDate < model.ToDate && x.ToDate > model.ToDate) || x.ToDate == model.ToDate).FirstOrDefault();
                    if(fromCheck != null || toCheck != null)
                    {
                        JV.FieldID = "Date";
                        JV.ValidFieldID = "ValidPeriod";
                        JV.Step = -1;
                        JV.Message = "يوجد معلومة في نفس الفترة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    DayTip DT = new DayTip();
                    DT.Name = model.Name;
                    DT.UserID = UserID;
                    DT.Publish = true;
                    DT.FromDate = model.FromDate;
                    DT.ToDate = model.ToDate;

                    db.DayTips.Add(DT);
                    db.SaveChanges();

                    var ID = DT.ID;
                    string LogoPath = "~/UploadedFiles/DayTips/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                    if (model.Image != null)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                        var CoverPath = Path.Combine(Server.MapPath(LogoPath + "/"), model.Image.FileName);
                        model.Image.SaveAs(CoverPath);
                        DT.Image = "/UploadedFiles/DayTips/" + ID + "/" + model.Image.FileName;
                    }

                    var systemUpdate = db.SystemUpdates.FirstOrDefault();
                    if (systemUpdate != null)
                        systemUpdate.UpdateTime = DateTime.Now;
                    else
                    {
                        var Item = new SystemUpdate();
                        Item.UpdateTime = DateTime.Now;
                        db.SystemUpdates.Add(Item);
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
                        JV.Message = "من فضلك ادخل المعلومة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    var fromCheck = db.DayTips.Where(x => x.FromDate == model.FromDate || (x.FromDate < model.FromDate && x.ToDate > model.FromDate) || x.ToDate == model.FromDate).FirstOrDefault();
                    var toCheck = db.DayTips.Where(x => x.FromDate == model.ToDate || (x.FromDate < model.ToDate && x.ToDate > model.ToDate) || x.ToDate == model.ToDate).FirstOrDefault();
                    if (fromCheck != null || toCheck != null)
                    {
                        JV.FieldID = "Date";
                        JV.ValidFieldID = "ValidPeriod";
                        JV.Step = -1;
                        JV.Message = "يوجد معلومة في نفس الفترة";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    DayTip DT = db.DayTips.First(x => x.ID == model.ID);
                    DT.Name = model.Name;
                    DT.UserID = UserID;
                    DT.FromDate = model.FromDate;
                    DT.ToDate = model.ToDate;

                    var ID = DT.ID;

                    string LogoPath = "~/UploadedFiles/DayTips/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                    if (model.Image != null)
                    {
                        string fullPath = Request.MapPath(DT.Image);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        System.IO.Directory.CreateDirectory(Server.MapPath(LogoPath));
                        var CoverPath = Path.Combine(Server.MapPath(LogoPath + "/"), model.Image.FileName);
                        model.Image.SaveAs(CoverPath);
                        DT.Image = "/UploadedFiles/DayTips/" + ID + "/" + model.Image.FileName;
                    }

                    var systemUpdate = db.SystemUpdates.FirstOrDefault();
                    if (systemUpdate != null)
                        systemUpdate.UpdateTime = DateTime.Now;
                    else
                    {
                        var Item = new SystemUpdate();
                        Item.UpdateTime = DateTime.Now;
                        db.SystemUpdates.Add(Item);
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
        public ActionResult DeleteTip(int id)
        {
            JsonValidationModel JVM = new JsonValidationModel();
            try
            {

                DayTip DT = db.DayTips.Find(id);
                db.DayTips.Remove(DT);

                var systemUpdate = db.SystemUpdates.FirstOrDefault();
                if (systemUpdate != null)
                    systemUpdate.UpdateTime = DateTime.Now;
                else
                {
                    var Item = new SystemUpdate();
                    Item.UpdateTime = DateTime.Now;
                    db.SystemUpdates.Add(Item);
                }

                db.SaveChanges();

                if (DT.Image != null)
                {
                    string fullPath = Request.MapPath(DT.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImagesFolder = "~/UploadedFiles/DayTips/" + DT.ID;
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
                var model = db.DayTips.Find(id);

                if (type == 1)
                    model.Publish = true;
                else
                    model.Publish = false;

                var systemUpdate = db.SystemUpdates.FirstOrDefault();
                if (systemUpdate != null)
                    systemUpdate.UpdateTime = DateTime.Now;
                else
                {
                    var Item = new SystemUpdate();
                    Item.UpdateTime = DateTime.Now;
                    db.SystemUpdates.Add(Item);
                }

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