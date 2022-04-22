using DCSA.Areas.AdminPanel.Models;
using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    [Authorize]
    public class AdminHomeController : AdminBaseController
    {
        DefaultConnection db = new DefaultConnection();

        // GET: AdminPanel/AdminHome
        public ActionResult Index()
        {
            var currentYear = DateTime.Now.Year;
            HomeDataModel model = new HomeDataModel();
            model.CausesNumber = db.Causes.Count();
            model.DonatersNumber = db.Donations.Count();
            model.TargetMoney = db.Causes.Sum(x => x.TargetMoney).Value;
            model.TargetRemain = model.TargetMoney - (db.Donations.Sum(x => x.Amount).HasValue? db.Donations.Sum(x => x.Amount).Value: 0);
            model.Causes = new List<CauseModel>();
            model.Causes = db.Causes.Select(x => new CauseModel
            {
                ID = x.ID,
                Name = x.Header,
                Target = x.TargetMoney.Value,
                Remain = x.TargetMoney.Value - (x.Donations.Sum(y => y.Amount).HasValue? x.Donations.Sum(y => y.Amount).Value : 0) + x.StartPrice.Value,
                Per = Math.Round((x.Donations.Sum(y => y.Amount).HasValue ? x.Donations.Sum(y => y.Amount).Value : 0) + x.StartPrice.Value / x.TargetMoney.Value * 100, 0)

            }).ToList();
            


            return View(model);
        }
    }
}