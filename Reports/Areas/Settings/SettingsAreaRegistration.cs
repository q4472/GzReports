using System.Web.Mvc;

namespace MvcApplication2.Areas.Settings
{
    public class SettingsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Settings";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // F0
            context.MapRoute(
                name: null,
                url: "Settings/F0/Save/{*pathInfo}",
                defaults: new { controller = "F0", action = "Save" }
            );
            context.MapRoute(
                name: null,
                url: "Settings/F0/{*pathInfo}",
                defaults: new { controller = "F0", action = "Index" }
            );
        }
    }
}
