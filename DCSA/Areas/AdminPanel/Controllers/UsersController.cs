using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCSA.Database;
using DCSA.Helpers;
using DCSA.Models;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [SuperAdminOnly]
    public class UsersController : AdminBaseController
    {
        private DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/Users
        public ActionResult Index(string Search, string searchValue)
        {
            var model = new List<User>();
            ViewBag.Search = Search;

            if (searchValue == null || searchValue == "")
                ViewBag.SearchValue = "تـصنيــــف";
            else
                ViewBag.SearchValue = searchValue;

            if (Search != null && Search != "" && (searchValue == null || searchValue == "" || searchValue.Trim() == "تـصنيــــف"))
                model = db.Users.Where(x => x.Name.Contains(Search) || x.Email.Contains(Search)).ToList();

            else if (Search != null && Search != "" && searchValue != null && searchValue != "" && searchValue.Trim() != "تـصنيــــف")
            {
                if (searchValue.Trim() == "الاسم")
                    model = db.Users.Where(x => x.Name.Contains(Search)).ToList();
                if (searchValue.Trim() == "البريد الالكتروني")
                    model = db.Users.Where(x => x.Email.Contains(Search)).ToList();
            }

            else
                model = db.Users.ToList();

            return View(model);
        }

        // GET: AdminPanel/Users/Create
        public ActionResult Manage(string ID)
        {
            ViewBag.RoleID = new SelectList(db.UserRoles, "ID", "Name");
            if (ID == null)
            {
                var model = new User();
                model.ID = -1;
                return View(model);
            }
            else
            {
                int WID = int.Parse(ID);
                var model = db.Users.First(x => x.ID == WID);
                return View(model);
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(UserSaveModel user)
        {
            ViewBag.RoleID = new SelectList(db.UserRoles, "ID", "Name", user.RoleID);

            JsonValidationModel JV = new JsonValidationModel();

            try
            {
                if(user.Name == null || user.Name == "")
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

                

                if (user.RoleID == -1 )
                {
                    JV.FieldID = "RoleID";
                    JV.ValidFieldID = "ValidRole";
                    JV.Step = -1;
                    JV.Message = "من فضلك اختر المسمى الوظيفي";
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

                if (user.ID == -1)
                {

                    var emailCheck = db.Users.Where(x => x.Email == user.Email).FirstOrDefault();
                    if (emailCheck != null)
                    {
                        JV.FieldID = "Email";
                        JV.ValidFieldID = "ValidEmail";
                        JV.Step = -1;
                        JV.Message = "هذا البريد الالكتروني موجود من قبل";
                        JV.IsValid = false;

                        return Json(JV, JsonRequestBehavior.AllowGet);
                    }

                    var u = new User();

                    u.Name = user.Name;
                    u.Username = user.Username;
                    u.Password = user.Password;
                    u.Email = user.Email;
                    u.Gender = user.Gender;
                    u.RoleID = user.RoleID;
                    u.CreationDate = DateTime.Now;
                    u.Phone = user.Phone;
                    if (user.Status == 1)
                        u.Status = true;
                    else
                        u.Status = false;
                    

                    db.Users.Add(u);
                    db.SaveChanges();

                    var ID = u.ID;
                    string ImagePath = "~/UploadedFiles/Users/" + ID;
                    System.IO.Directory.CreateDirectory(Server.MapPath(ImagePath));
                    if (user.Photo != null)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(ImagePath));
                        var CoverPath = Path.Combine(Server.MapPath(ImagePath + "/"), user.Photo.FileName);
                        user.Photo.SaveAs(CoverPath);
                        u.Photo = "/UploadedFiles/Users/" + ID + "/" + user.Photo.FileName;
                    }
                }
                else
                {
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

                    var u = db.Users.Find(user.ID);

                    u.Name = user.Name;
                    u.Username = user.Username;
                    u.Password = user.Password;
                    u.Email = user.Email;
                    u.Gender = user.Gender;
                    u.RoleID = user.RoleID;
                    u.CreationDate = DateTime.Now;
                    u.Phone = user.Phone;
                    if (user.Status == 1)
                        u.Status = true;
                    else
                        u.Status = false;

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
                }

                db.SaveChanges();

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

        // POST: AdminPanel/Users/Delete/5
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            JsonValidationModel JVM = new JsonValidationModel();
            try
            {

                User user = db.Users.Find(id);

                if (user.Photo != null)
                {
                    string fullPath = Request.MapPath(user.Photo);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImagesFolder = "~/UploadedFiles/Users/" + user.ID;
                    if (System.IO.Directory.Exists(Server.MapPath(ImagesFolder)))
                        System.IO.Directory.Delete(Server.MapPath(ImagesFolder));
                }

                db.Users.Remove(user);
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
