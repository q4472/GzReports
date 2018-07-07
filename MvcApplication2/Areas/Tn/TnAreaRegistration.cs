using System.Web.Mvc;

namespace MvcApplication2.Areas.Tn
{
    public class TnAreaRegistration : AreaRegistration
    {
        public override string AreaName { get { return "Tn"; } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(null, "Tn/SrcSelector/{*pathInfo}", new { controller = "Tn", action = "SrcSelector" });
            context.MapRoute(null, "Tn/MnnSelector/{*pathInfo}", new { controller = "Tn", action = "MnnSelector" });
            context.MapRoute(null, "Tn/TnSelector/{*pathInfo}", new { controller = "Tn", action = "TnSelector" });
            context.MapRoute(null, "Tn/GetUData/{*pathInfo}", new { controller = "Tn", action = "GetUData" });
            context.MapRoute(null, "Tn/Get1cData/{*pathInfo}", new { controller = "Tn", action = "Get1cData" });
            context.MapRoute(null, "Tn/GetCopy/{*pathInfo}", new { controller = "Tn", action = "GetCopy" });
            context.MapRoute(null, "Tn/GetInstruction/{*pathInfo}", new { controller = "Tn", action = "GetInstructions" });
        }
    }
}
