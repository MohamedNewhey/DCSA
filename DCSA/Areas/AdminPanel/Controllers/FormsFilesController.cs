using DCSA.Areas.AdminPanel.Models;
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
    public class FormsFilesController : AdminBaseController
    {
        // GET: AdminPanel/FormsFiles
        public ActionResult Index()
        {
            var model = new List<FormFilesSaveModel>();

            DirectoryInfo dir = new DirectoryInfo(Request.MapPath("/UploadedFiles/FormsFiles"));
            DirectoryInfo[] directory = dir.GetDirectories();

            int I = 1;
            foreach(var Dir in directory)
            {
                DirectoryInfo[] subDir = Dir.GetDirectories();

                foreach(var sDir in subDir)
                {
                    FileInfo[] files = sDir.GetFiles();

                    foreach(var file in files)
                    {
                        FormFilesSaveModel item = new FormFilesSaveModel();
                        item.ID = I;
                        item.FileName = file.Name;
                        item.FileEx = file.Extension;

                     

                        var path = file.FullName.Split(new string[] { "UploadedFiles" }, StringSplitOptions.None)[1];
                        item.FilePath = "\\UploadedFiles" + path;

                        model.Add(item);

                        I++;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteFile(string path)
        {
            JsonValidationModel JVM = new JsonValidationModel();
            try
            {
                string fullPath = Request.MapPath(path);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
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

    }
}