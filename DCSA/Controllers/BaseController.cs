using DCSA.Database;
using DCSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCSA.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DefaultConnection db = new DefaultConnection();
            var Cart = Session["Cart"] as List<CartItem>;
            if (Cart != null)
                ViewBag.CartItems = Cart.Count();
            else
                ViewBag.CartItems = 0;


            ViewBag.StaticPages = db.StaticPages.Where(x=>x.PagePlace==1).OrderBy(x=>x.PageOrder).ToList();

            ViewBag.FooterPages = db.StaticPages.Where(x => x.PagePlace == 2).OrderBy(x=>x.PageOrder).ToList();

            base.OnActionExecuting(filterContext);
        }
    }
}