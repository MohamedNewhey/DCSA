using Microsoft.AspNet.Identity;
using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DCSA.Helpers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MobileOnly : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.MobileAdmin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.Admin).ToString())))
                {

                }
                else
                {
                    var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminHome", action = "Index" }));
                    filterContext.Result = result;
                }
            }
            else
            {
                var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                filterContext.Result = result;

            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FormsAccess : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.MobileAdmin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.Admin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.FormsAdmin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.HelplineApproveOnly).ToString())))
                {

                }
                else
                {
                    var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminHome", action = "Index" }));
                    filterContext.Result = result;
                }
            }
            else
            {
                var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                filterContext.Result = result;

            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FormsAccessWithoutAppOnly : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.MobileAdmin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.Admin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.FormsAdmin).ToString())))
                {

                }
                else
                {
                    var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminHome", action = "Index" }));
                    filterContext.Result = result;
                }
            }
            else
            {
                var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                filterContext.Result = result;

            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WebsiteOnly : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.WebsiteAdmin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.Admin).ToString())))
                {

                }
                else
                {
                    var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminHome", action = "Index" }));
                    filterContext.Result = result;
                }
            }
            else
            {
                var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                filterContext.Result = result;

            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FormsOnly : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.FormsAdmin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.Admin).ToString())))
                {

                }
                else
                {
                    var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminHome", action = "Index" }));
                    filterContext.Result = result;
                }
            }
            else
            {
                var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                filterContext.Result = result;

            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApproveOnly : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.FormsAdmin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.Admin).ToString())) || filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.HelplineApproveOnly).ToString())))
                {

                }
                else
                {
                    var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminHome", action = "Index" }));
                    filterContext.Result = result;
                }
            }
            else
            {
                var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                filterContext.Result = result;

            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SuperAdminOnly : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.User.IsInRole((((int)SystemConstants.Roles.Admin).ToString())))
                {

                }
                else
                {
                    var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminHome", action = "Index" }));
                    filterContext.Result = result;
                }
            }
            else
            {
                var result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                filterContext.Result = result;

            }
        }
    }


    public static class SecurityRoles
    {
        public static bool IsMobileAdmin()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int RoleID = int.Parse(claims.FirstOrDefault(c => c.Type == "RoleID").Value);
            if (RoleID == (int)(SystemConstants.Roles.MobileAdmin) || RoleID == (int)(SystemConstants.Roles.Admin))
                return true;
            else
                return false;
        }

        public static bool IsSuperAdmin()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int RoleID = int.Parse(claims.FirstOrDefault(c => c.Type == "RoleID").Value);
            if (RoleID == (int)(SystemConstants.Roles.Admin))
                return true;
            else
                return false;
        }

        public static bool IsWebsiteAdmin()
        {

            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int RoleID = int.Parse(claims.FirstOrDefault(c => c.Type == "RoleID").Value);
            if (RoleID == (int)(SystemConstants.Roles.WebsiteAdmin) || RoleID == (int)(SystemConstants.Roles.Admin))
                return true;
            else
                return false;
        }

        public static bool IsFormsAdmin()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int RoleID = int.Parse(claims.FirstOrDefault(c => c.Type == "RoleID").Value);
            if (RoleID == (int)(SystemConstants.Roles.FormsAdmin) || RoleID == (int)(SystemConstants.Roles.Admin) )
                return true;
            else
                return false;
        }

        public static bool IsApproveOnly()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int RoleID = int.Parse(claims.FirstOrDefault(c => c.Type == "RoleID").Value);
            if (RoleID == (int)(SystemConstants.Roles.HelplineApproveOnly) || RoleID == (int)(SystemConstants.Roles.Admin))
                return true;
            else
                return false;
        }

        public static bool IsApproveOnlyLayOut()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int RoleID = int.Parse(claims.FirstOrDefault(c => c.Type == "RoleID").Value);
            if (RoleID == (int)(SystemConstants.Roles.HelplineApproveOnly))
                return true;
            else
                return false;
        }
    }
}