using DCSA.Database;
using DCSA.Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [SuperAdminOnly]
    public class SystemToolsController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/SystemTools
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DownLoadDatabase()
        {

            var db = new DefaultConnection();
            string dataTime = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH-mm");
            string directory = Server.MapPath("~/") + "/backups/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = directory + dataTime + ".bak";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);


            db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, "EXEC [dbo].[BackUp] @path = N'" + fileName + "'");
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "NCCM Database" + "_" + DateTime.Now.ToShortDateString() + ".bak");
        }


        public ActionResult DownloadAllResources()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {


                    string[] UploadedFiles = Directory.GetFileSystemEntries(Server.MapPath("~/UploadedFiles"), "*", SearchOption.TopDirectoryOnly);

                    foreach (var direc in UploadedFiles)
                    {
                        DirectoryInfo[] d = new DirectoryInfo(direc).GetDirectories();
                        var splited = direc.Split('\\');
                        var Foldername = splited[splited.Length - 1];
                        ziparchive.CreateEntry(Foldername + "/");
                        foreach (var di in d)
                        {

                            DirectoryInfo[] da = di.GetDirectories();
                            ziparchive.CreateEntry(Foldername + "/"+di.Name+"/");
                            if (da.Count() == 0)
                            {
                                FileInfo[] Files = di.GetFiles();

                                foreach (FileInfo file in Files)
                                {
                                    var path = file.FullName;
                                    byte[] b = System.IO.File.ReadAllBytes(path);
                                    var Filename = Path.GetFileName(file.Name);
                                    AddToArchive(ziparchive, Foldername + "/" + di.Name + "/"+Filename, b.ToArray());
                                }
                            }
                            else
                            {
                                foreach(var dir in da)
                                {
                                    FileInfo[] Files = dir.GetFiles();
                                    ziparchive.CreateEntry(Foldername + "/" + di.Name + "/" + dir.Name + "/");
                                    foreach (FileInfo file in Files)
                                    {
                                        var path = file.FullName;
                                        byte[] b = System.IO.File.ReadAllBytes(path);
                                        var Filename = Path.GetFileName(file.Name);
                                        AddToArchive(ziparchive, Foldername + "/" + di.Name + "/" + dir.Name + "/"+Filename, b.ToArray());
                                    }
                                }
                            }
                           
                        }
                        

                    }

                }
                return File(memoryStream.ToArray(), "application/zip", "NCCM Resources" + "_" + DateTime.Now.ToShortDateString() + ".zip");
            }
        }
        private void AddToArchive(ZipArchive ziparchive, string fileName, byte[] attach)
        {
            var zipEntry = ziparchive.CreateEntry(fileName);
            using (var zipStream = zipEntry.Open())
            using (var streamIn = new MemoryStream(attach))
            {
                streamIn.CopyTo(zipStream);
            }
        }

        public ActionResult Logger(int? page, string FromDate, string ToDate, string UserID)
        {
            IPagedList<AuditLogger> model = null; 
            var predicate = PredicateBuilder.True<AuditLogger>();
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.UserID = UserID;

            if(FromDate != null && FromDate != "" && ToDate != null && ToDate != "")
            {
                DateTime from;
                DateTime.TryParse(FromDate, out from);

                DateTime to;
                DateTime.TryParse(ToDate, out to);

                predicate = predicate.And(i => i.Time >= from && i.Time <= to);
            }

            if(UserID != null && UserID != "")
            {
                int user;
                Int32.TryParse(UserID, out user);

                predicate = predicate.And(i => i.UserID == user);
            }

            model = db.AuditLoggers.Where(predicate).OrderByDescending(x => x.Time).ToList().ToPagedList(page ?? 1,10);
            ViewBag.Count = db.AuditLoggers.Where(predicate).ToList().Count();

            return View(model);
        }
    }
}