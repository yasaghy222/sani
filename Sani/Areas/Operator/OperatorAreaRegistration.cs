using System.Web.Mvc;

namespace Sani.Areas.Operator
{
    public class OperatorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Operator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "SS-OManage/Login",
              "SS-OManage/Login",
              new { action = "Login", Controller = "Account" }
          );

            context.MapRoute(
               "SS-OManage",
               "SS-OManage",
               new { action = "Index", Controller = "Dashboard" }
           );

            context.MapRoute(
                "Operator/Dashboard",
                "Operator/Dashboard",
                new { action = "Dashboard", Controller = "Dashboard" }
            );

            context.MapRoute(
                "Operator_default",
                "Operator/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}