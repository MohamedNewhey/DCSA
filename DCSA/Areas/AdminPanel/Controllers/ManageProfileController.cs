using DCSA.Database;
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
    public class ManageProfileController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/ManageProfile
        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int UserID = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var model = new User();

            model = db.Users.Find(UserID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserSaveModel user)
        {

            JsonValidationModel JV = new JsonValidationModel();

            try
            {
                if (user.Name == null || user.Name == "")
                {
                    JV.FieldID = "Name";
                    JV.ValidFieldID = "ValidName";
                    JV.Step = -1;
                    JV.Message = "من فضلك ادخل اسم المستخدم";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

                if (user.Email == null || user.Email == "")
                {
                    JV.FieldID = "Email";
                    JV.ValidFieldID = "ValidEmail";
                    JV.Step = -1;
                    JV.Message = "من فضلك ادخل البريد الالكتروني";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

                var emailCheck = db.Users.Where(x => x.Email == user.Email && x.ID != user.ID).FirstOrDefault();
                if (emailCheck != null)
                {
                    JV.FieldID = "Email";
                    JV.ValidFieldID = "ValidEmail";
                    JV.Step = -1;
                    JV.Message = "هذا البريد الالكتروني موجود من قبل";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

                if (user.Password == null || user.Password == "")
                {
                    JV.FieldID = "Password";
                    JV.ValidFieldID = "ValidPassword";
                    JV.Step = -1;
                    JV.Message = "من فضلك ادخل كلمة المرور";
                    JV.IsValid = false;

                    return Json(JV, JsonRequestBehavior.AllowGet);
                }

               
                var u = db.Users.Find(user.ID);

                u.Name = user.Name;
                u.Username = user.Username;
                u.Password = user.Password;
                u.Email = user.Email;
                u.Gender = user.Gender;
                u.CreationDate = DateTime.Now;
                u.Phone = user.Phone;
              

                var ID = u.ID;

                string ImagePath = "~/UploadedFiles/Users/" + ID;
                System.IO.Directory.CreateDirectory(Server.MapPath(ImagePath));
                if (user.Photo != null)
                {
                    string fullPath = Request.MapPath(u.Photo);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    System.IO.Directory.CreateDirectory(Server.MapPath(ImagePath));
                    var CoverPath = Path.Combine(Server.MapPath(ImagePath + "/"), user.Photo.FileName);
                    user.Photo.SaveAs(CoverPath);
                    u.Photo = "/UploadedFiles/Users/" + ID + "/" + user.Photo.FileName;
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