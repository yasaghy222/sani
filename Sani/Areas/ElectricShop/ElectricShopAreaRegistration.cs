using System.Web.Mvc;

namespace Sani.Areas.ElectricShop
{
    public class ElectricShopAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ElectricShop";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ElectricShop",
                "SS-EManage",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "ElectricShop_default",
                "ElectricShop/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}