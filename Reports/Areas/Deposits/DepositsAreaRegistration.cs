using System.Web.Mvc;

namespace MvcApplication2.Areas.Deposits
{
    public class DepositsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Deposits";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                null,
                "Deposits/F0/GetEntriesData/{*pathInfo}",
                new { controller = "F0", action = "GetEntriesData" }
            );
            context.MapRoute(
                null,
                "Deposits/F0/GetCustData/{*pathInfo}",
                new { controller = "F0", action = "GetCustData" }
            );
            context.MapRoute(
                null,
                "Deposits/F0/GetFiltredData/{*pathInfo}",
                new { controller = "F0", action = "GetFiltredData" }
            );
            context.MapRoute(
                null,
                "Deposits/F0/{*pathInfo}",
                new { controller = "F0", action = "Index" }
            );
            context.MapRoute(
                null,
                "Deposits/F1/{*pathInfo}",
                new { controller = "F1", action = "Index" }
            );
        }
    }
}
