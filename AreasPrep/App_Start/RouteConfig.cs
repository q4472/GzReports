using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FarmSib.AreasPrep
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "Items/GetLP/{*pathInfo}",
                defaults: new { controller = "Items", action = "GetLP" }
            );

            routes.MapRoute(
                name: null,
                url: "Items/GetTnItems/{*pathInfo}",
                defaults: new { controller = "Items", action = "GetTnItems" }
            );

            routes.MapRoute(
                name: null,
                url: "Dgv/{*pathInfo}",
                defaults: new { controller = "Dgv", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "Fs/{alias}/{*pathInfo}",
                defaults: new { controller = "Fs", action = "Index", alias = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: null,
                url: "{*pathInfo}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}