using System.Web.Mvc;

namespace MvcApplication2.Areas.Env
{
    public class EnvAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Env";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                null,
                "Env/Get",
                new { controller = "Env", action = "Get" }
            );
            context.MapRoute(
                null,
                "Env/Set",
                new { controller = "Env", action = "Set" }
            );
        }
    }
}
