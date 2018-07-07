using System.Web.Mvc;

namespace MvcApplication2.Areas.Prep
{
    public class PrepAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Prep";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // F0
            context.MapRoute(
                name: null,
                url: "Prep/F0/InsertBlankSpec/{*pathInfo}",
                defaults: new { controller = "F0", action = "InsertBlankSpec" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F0/PassToTender/{*pathInfo}",
                defaults: new { controller = "F0", action = "PassToTender" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F0/LoadAuctionInf/{*pathInfo}",
                defaults: new { controller = "F0", action = "LoadAuctionInf" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F0/CalcOutgoingPrices/{*pathInfo}",
                defaults: new { controller = "F0", action = "CalcOutgoingPrices" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F0/Delete/{*pathInfo}",
                defaults: new { controller = "F0", action = "Delete" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F0/ApplyFilter/{*pathInfo}",
                defaults: new { controller = "F0", action = "ApplyFilter" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F0/DetailSave/{*pathInfo}",
                defaults: new { controller = "F0", action = "DetailSave" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F0/Detail/{*pathInfo}",
                defaults: new { controller = "F0", action = "Detail" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F0/{*pathInfo}",
                defaults: new { controller = "F0", action = "Index" }
            );

            // F1
            context.MapRoute(
                name: null,
                url: "Prep/F1/DownloadExcelFile/{*pathInfo}",
                defaults: new { controller = "F1", action = "DownloadExcelFile" }
            );

            context.MapRoute(
                name: null,
                url: "Prep/F1/SaveTableV2/{*pathInfo}",
                defaults: new { controller = "F1", action = "SaveTableV2" }
            );

            context.MapRoute(
                name: null,
                url: "Prep/F1/SaveTable/{*pathInfo}",
                defaults: new { controller = "F1", action = "SaveTable" }
            );

            context.MapRoute(
                name: null,
                url: "Prep/F1/SaveShedule/{*pathInfo}",
                defaults: new { controller = "F1", action = "SaveShedule" }
                );

            context.MapRoute(
                name: null,
                url: "Prep/F1/{*pathInfo}",
                defaults: new { controller = "F1", action = "Index" }
            );

            // F2
            context.MapRoute(
                name: null,
                url: "Prep/F2/CreatePrepSpecTable/{*pathInfo}",
                defaults: new { controller = "F2", action = "CreatePrepSpecTable" }
            );

            context.MapRoute(
                name: null,
                url: "Prep/F2/ParseFile/{*pathInfo}",
                defaults: new { controller = "F2", action = "ParseFile" }
            );

            context.MapRoute(
                name: null,
                url: "Prep/F2/{*pathInfo}",
                defaults: new { controller = "F2", action = "Index" }
            );

            // F4
            context.MapRoute(
                name: null,
                url: "Prep/F4/SendEmail/{*pathInfo}",
                defaults: new { controller = "F4", action = "SendEmail" }
            );

            context.MapRoute(
                name: null,
                url: "Prep/F4/LoadS/{*pathInfo}",
                defaults: new { controller = "F4", action = "LoadS" }
            );

            context.MapRoute(
                name: null,
                url: "Prep/F4/{*pathInfo}",
                defaults: new { controller = "F4", action = "Index" }
            );
        }
    }
}
