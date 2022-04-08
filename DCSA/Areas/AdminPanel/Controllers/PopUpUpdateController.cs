using DCSA.Database;
using DCSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    public class PopUpUpdateController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/PopUpUpdate
        public ActionResult Index()
        {
            var popUpUpdate = db.PopUpUpdates.FirstOrDefault();

            if (popUpUpdate == null)
            {
                var model = new PopUpUpdate();
                model.ID = -1;
                return View(model);
            }
            else
            {
                var model = popUpUpdate;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PopUpUpdate model)
        {
            JsonValidationModel JV = new JsonValidationModel();

            try
            {
                if (model.Header == null || model.Header == "")
                {
                    JV.FieldID = "Header";
                    JV.ValidFieldID = "ValidHeader";
                    JV.Step = -1;
                    JV.Message = "من فضلك ادخل العنوان";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

                if (model.PopUpContent == null || model.PopUpContent == "")
                {
                    JV.FieldID = "PopUpContent";
                    JV.ValidFieldID = "ValidContent";
                    JV.Step = -1;
                    JV.Message = "من فضلك ادخل المحتوى";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

                if (model.Link == null || model.Link == "")
                {
                    JV.FieldID = "Link";
                    JV.ValidFieldID = "ValidLink";
                    JV.Step = -1;
                    JV.Message = "من فضلك ادخل الرايط";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

                if (model.ID == -1)
                {
                    db.PopUpUpdates.Add(model);
                }
                else
                {
                    var u = db.PopUpUpdates.Find(model.ID);
                    u.Header = model.Header;
                    u.PopUpContent = model.PopUpContent;
                    u.Link = model.Link;
                    u.IsActive = model.IsActive;
                   
                }

                db.SaveChanges();

                JV.FieldID = "";
                JV.ValidFieldID = "";
                JV.Step = -1;
                JV.Message = "تم حفظ البيانات بنجاح";
                JV.IsValid = true;

                return Json(JV, JsonRequestBehavior.AllowGet);
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
    }
}