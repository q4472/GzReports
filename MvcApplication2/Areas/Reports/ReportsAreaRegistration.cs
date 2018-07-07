using System.Web.Mvc;

namespace MvcApplication2.Areas.Reports
{
    public class ReportsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // R1
            context.MapRoute(
                name: null,
                url: "Reports/R1/GetClientSelector/{*pathInfo}",
                defaults: new { controller = "R1", action = "GetClientSelector" }
                );

            context.MapRoute(
                name: null,
                url: "Reports/R1/GetManagerSelector/{*pathInfo}",
                defaults: new { controller = "R1", action = "GetManagerSelector" }
                );

            context.MapRoute(
                name: null,
                url: "Reports/R1/GetReport/{*pathInfo}",
                defaults: new { controller = "R1", action = "GetReport" }
                );

            context.MapRoute(
                name: null,
                url: "Reports/R1/{*pathInfo}",
                defaults: new { controller = "R1", action = "Index" }
                );

            // R2
            context.MapRoute(
                name: null,
                url: "Reports/R2/GetEmploeeSelector/{*pathInfo}",
                defaults: new { controller = "R2", action = "GetEmploeeSelector" }
                );

            context.MapRoute(
                name: null,
                url: "Reports/R2/GetReport/{*pathInfo}",
                defaults: new { controller = "R2", action = "GetReport" }
                );

            context.MapRoute(
                name: null,
                url: "Reports/R2/{*pathInfo}",
                defaults: new { controller = "R2", action = "Index" }
                );

            // Sud
            context.MapRoute(
                name: null,
                url: "Reports/Sud/UpsertSud/{*pathInfo}",
                defaults: new { controller = "Sud", action = "UpsertSud" }
                );

            // R3
            context.MapRoute(
                name: null,
                url: "Reports/R3/ApplyFilter/{*pathInfo}",
                defaults: new { controller = "R3", action = "ApplyFilter" }
                );

            context.MapRoute(
                name: null,
                url: "Reports/R3/ApplyPeriod/{*pathInfo}",
                defaults: new { controller = "R3", action = "ApplyPeriod" }
                );

            context.MapRoute(
                name: null,
                url: "Reports/R3/GetHistory/{*pathInfo}",
                defaults: new { controller = "R3", action = "GetHistory" }
                );

            context.MapRoute(
                name: null,
                url: "Reports/R3/{*pathInfo}",
                defaults: new { controller = "R3", action = "Index" }
                );
        }
    }
}
