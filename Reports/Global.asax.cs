using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcApplication2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
    public class NskdEnv
    {
        public const String DATA_SERVICE_HOST = "127.0.0.1";
        public const String DATA_SERVICE_HOST_SQL_PORT = "11002";
        public const String DATA_SERVICE_HOST_INET_PORT = "11008";
        public const String DATA_SERVICE_HOST_SQL_PORT_V12 = "11012";
    }
}