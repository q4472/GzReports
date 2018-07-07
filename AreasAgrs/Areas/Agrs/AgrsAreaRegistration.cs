using System.Web.Mvc;

namespace FarmSib.AreasAgrs.Areas.Agrs
{
    public class AgrsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Agrs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // F0
            context.MapRoute(
                null,
                "Agrs/F0/Save/{*pathInfo}",
                new { controller = "F0", action = "Save" }
            );

            context.MapRoute(
                null,
                "Agrs/F0/FilteredView/GetDataForDetailSection/{*pathInfo}",
                new { controller = "F0", action = "GetDataForDetailSection" }
            );

            context.MapRoute(
                null,
                "Agrs/F0/Filter/GetDataForFilteredView/{*pathInfo}",
                new { controller = "F0", action = "GetDataForFilteredView" }
            );

            context.MapRoute(
                null,
                "Agrs/F0/SelectorWithListBox/GetData/{*pathInfo}",
                new { controller = "F0", action = "GetDataForSelectorWithListBox" }
            );

            context.MapRoute(
                null,
                "Agrs/F0/{*pathInfo}",
                new { controller = "F0", action = "Index" }
            );

            // F1
            context.MapRoute(
                null,
                "Agrs/F1/Save/{*pathInfo}",
                new { controller = "F1", action = "Save" }
            );

            context.MapRoute(
                null,
                "Agrs/F1/FilteredView/GetDataForDetailSection/{*pathInfo}",
                new { controller = "F1", action = "GetDataForDetailSection" }
            );

            context.MapRoute(
                null,
                "Agrs/F1/Filter/GetDataForFilteredView/{*pathInfo}",
                new { controller = "F1", action = "GetDataForFilteredView" }
            );

            context.MapRoute(
                null,
                "Agrs/F1/SelectorWithListBox/GetData/{*pathInfo}",
                new { controller = "F1", action = "GetDataForSelectorWithListBox" }
            );

            context.MapRoute(
                null,
                "Agrs/F1/{*pathInfo}",
                new { controller = "F1", action = "Index" }
            );
        }
    }
}
