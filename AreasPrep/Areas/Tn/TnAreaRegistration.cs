using System.Web.Mvc;

namespace AreasPrep.Areas.Tn
{
    public class TnAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tn";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: null,
                url: "Tn/GetUData/{*pathInfo}",
                defaults: new { controller = "Tn", action = "GetUData" }
            );
            context.MapRoute(
                name: null,
                url: "Tn/Get1cData/{*pathInfo}",
                defaults: new { controller = "Tn", action = "Get1cData" }
            );
            context.MapRoute(
                name: null,
                url: "Tn/GetCopy/{*pathInfo}",
                defaults: new { controller = "Tn", action = "GetCopy" }
            );
            context.MapRoute(
                name: null,
                url: "Tn/GetInstruction/{*pathInfo}",
                defaults: new { controller = "Tn", action = "GetInstructions" }
            );
        }
    }
}
