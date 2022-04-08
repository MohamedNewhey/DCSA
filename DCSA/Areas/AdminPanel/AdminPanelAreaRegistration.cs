using System.Web.Mvc;

namespace DCSA.Areas.AdminPanel
{
    public class AdminPanelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminPanel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AdminPanel_defaultRouting",
                "AdminPanel/{controller}/{action}/{id}",
                new { action = "Index", controller = "AdminHome", id = UrlParameter.Optional }
            );
        }
    }
}