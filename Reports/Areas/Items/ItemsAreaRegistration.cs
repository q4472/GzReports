using System.Web.Mvc;

namespace MvcApplication2.Areas.Items
{
    public class ItemsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Items";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Search
            context.MapRoute(
                null,
                "Items/Search/{*pathInfo}",
                new { controller = "Search", action = "Index" }
            );

            // Groups
            context.MapRoute(
                null,
                "Items/Groups/GetFieldValueList/{*pathInfo}",
                new { controller = "Groups", action = "GetFieldValueList" }
            );

            context.MapRoute(
                null,
                "Items/Groups/CommitLog/{*pathInfo}",
                new { controller = "Groups", action = "CommitLog" }
            );

            context.MapRoute(
                null,
                "Items/Groups/SelectGroup/{*pathInfo}",
                new { controller = "Groups", action = "SelectGroup" }
            );

            context.MapRoute(
                null,
                "Items/Groups/ApplyFilter/{*pathInfo}",
                new { controller = "Groups", action = "ApplyFilter" }
            );

            context.MapRoute(
                null,
                "Items/Groups/UpsertGroup/{*pathInfo}",
                new { controller = "Groups", action = "UpsertGroup" }
            );

            context.MapRoute(
                null,
                "Items/Groups/{*pathInfo}",
                new { controller = "Groups", action = "Index" }
            );
        }
    }
}
