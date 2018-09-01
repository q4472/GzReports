using System.Web.Mvc;

namespace MvcApplication2.Areas.Docs
{
    public class DocsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Docs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                null,
                "Docs/Ct/{*pathInfo}",
                new { controller = "Ct", action = "Index" }
            );
            context.MapRoute(
                null,
                "Docs/Rd/{*pathInfo}",
                new { controller = "Rd", action = "Index" }
            );
        }
    }
}
