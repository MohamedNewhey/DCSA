using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace DCSA.Helpers
{
    public class GlobalHelper
    {
        public static string EntityToString(DbEntityEntry dbEntry)
        {
            string Res = "";
            if (dbEntry.State == EntityState.Added)
                foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                    Res += propertyName + " :" + dbEntry.CurrentValues.GetValue<object>(propertyName) + Environment.NewLine;
            else if (dbEntry.State == EntityState.Deleted)
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    Res += propertyName + " :" + dbEntry.OriginalValues.GetValue<object>(propertyName) + Environment.NewLine;

            return Res;
        }

        public static List<Caus> GetCauses()
        {
            DefaultConnection db = new DefaultConnection();

            //var y = db.Causes.Where(x => x.Publish.Value).Select(x => x.CauseOrder).Distinct().OrderByDescending(x => x).ToList();
            //var list = new List<Caus>();
            //foreach (var item in y)
            //{
            //    var l = db.Causes.Where(x => x.CauseOrder == item && x.Publish.Value).OrderByDescending(x => x.CauseDate).ToList();
            //    foreach (var i in l)
            //    {
            //        list.Add(i);
            //    }
            //}

            var list = db.Causes.Where(x=>x.Publish.Value).OrderBy(x => x.CauseOrder).ToList();
            return list;
        }



        public static List<VideosLibrary> OrderVideos()
        {
            DefaultConnection db = new DefaultConnection();

            var y = db.VideosLibraries.Where(x => x.Publish.Value).Select(x => x.VideoOrder).Distinct().OrderByDescending(x => x).ToList();
            var list = new List<VideosLibrary>();
            foreach (var item in y)
            {
                var l = db.VideosLibraries.Where(x => x.VideoOrder == item && x.Publish.Value).OrderByDescending(x => x.PublishDate).ToList();
                foreach (var i in l)
                {
                    list.Add(i);
                }
            }

            return list;
        }
    }
}