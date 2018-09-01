using System.Web.Mvc;

namespace MvcApplication2.Areas.Docs1c
{
    public class Docs1cAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Docs1c";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                null,
                "Docs1c/F0/Save/{*pathInfo}",
                new { controller = "F0", action = "Save" }
            );
            context.MapRoute(
                null,
                "Docs1c/F0/Detail/{*pathInfo}",
                new { controller = "F0", action = "Detail" }
            );
            context.MapRoute(
                null,
                "Docs1c/F0/Filter/{*pathInfo}",
                new { controller = "F0", action = "Filter" }
            );
            context.MapRoute(
                null,
                "Docs1c/F0/{*pathInfo}",
                new { controller = "F0", action = "Index" }
            );
        }
    }
}
