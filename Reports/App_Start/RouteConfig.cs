using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // R1
            {
                routes.MapRoute(
                    name: null,
                    url: "Reports/R1/GetClientSelector/{*pathInfo}",
                    defaults: new { controller = "R1", action = "GetClientSelector" }
                    );

                routes.MapRoute(
                    name: null,
                    url: "Reports/R1/GetManagerSelector/{*pathInfo}",
                    defaults: new { controller = "R1", action = "GetManagerSelector" }
                    );

                routes.MapRoute(
                    name: null,
                    url: "Reports/R1/GetReport/{*pathInfo}",
                    defaults: new { controller = "R1", action = "GetReport" }
                    );

                routes.MapRoute(
                    name: null,
                    url: "Reports/R1/{*pathInfo}",
                    defaults: new { controller = "R1", action = "Index" }
                    );
            }
            // R2
            {
                routes.MapRoute(
                    name: null,
                    url: "Reports/R2/GetEmploeeSelector/{*pathInfo}",
                    defaults: new { controller = "R2", action = "GetEmploeeSelector" }
                    );

                routes.MapRoute(
                    name: null,
                    url: "Reports/R2/GetReport/{*pathInfo}",
                    defaults: new { controller = "R2", action = "GetReport" }
                    );

                routes.MapRoute(
                    name: null,
                    url: "Reports/R2/{*pathInfo}",
                    defaults: new { controller = "R2", action = "Index" }
                    );

                // Sud
                routes.MapRoute(
                    name: null,
                    url: "Reports/Sud/UpsertSud/{*pathInfo}",
                    defaults: new { controller = "Sud", action = "UpsertSud" }
                    );
            }
            // R3
            {
                routes.MapRoute(
                    name: null,
                    url: "Reports/R3/ApplyFilter/{*pathInfo}",
                    defaults: new { controller = "R3", action = "ApplyFilter" }
                    );

                routes.MapRoute(
                    name: null,
                    url: "Reports/R3/ApplyPeriod/{*pathInfo}",
                    defaults: new { controller = "R3", action = "ApplyPeriod" }
                    );

                routes.MapRoute(
                    name: null,
                    url: "Reports/R3/GetHistory/{*pathInfo}",
                    defaults: new { controller = "R3", action = "GetHistory" }
                    );

                routes.MapRoute(
                    name: null,
                    url: "Reports/R3/{*pathInfo}",
                    defaults: new { controller = "R3", action = "Index" }
                    );
            }
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*pathInfo}");
        }
    }
}