using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel.Controllers
{
    public class AdminBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DefaultConnection db = new DefaultConnection();
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            ViewBag.ControllerName = controllerName;
            ViewBag.ActionName = actionName;

            if (controllerName.ToLower() == "adminhome")
            {
                ViewBag.TITLEee = "لوحة التحكم";
                ViewBag.LIST = null;
            }
            var HeaderList = new List<StaticPage>();

            ViewBag.LIST = HeaderList;
            base.OnActionExecuting(filterContext);
        }

    }
}