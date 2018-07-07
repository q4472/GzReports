using System.Web.Mvc;

namespace MvcApplication2.Areas.ExternalPages
{
    public class ExternalPagesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ExternalPages";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: null,
                url: "ExternalPages/{*pathInfo}",
                defaults: new { controller = "F0", action = "Index" }
            );
        }
    }
}
