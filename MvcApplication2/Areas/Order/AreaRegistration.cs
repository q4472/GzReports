using System.Web.Mvc;

namespace MvcApplication2.Areas.Order
{
    public class OrderAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Order";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // F0
            context.MapRoute(
                name: null,
                url: "Order/F0/ImportFromPart1/{*pathInfo}",
                defaults: new { controller = "F0", action = "ImportFromPart1" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/CreateSpec/{*pathInfo}",
                defaults: new { controller = "F0", action = "CreateSpec" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/PassToTender/{*pathInfo}",
                defaults: new { controller = "F0", action = "PassToTender" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/LoadAuctionInf/{*pathInfo}",
                defaults: new { controller = "F0", action = "LoadAuctionInf" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/CalcOutgoingPrices/{*pathInfo}",
                defaults: new { controller = "F0", action = "CalcOutgoingPrices" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/Delete/{*pathInfo}",
                defaults: new { controller = "F0", action = "Delete" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/ApplyFilter/{*pathInfo}",
                defaults: new { controller = "F0", action = "ApplyFilter" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/DetailSave/{*pathInfo}",
                defaults: new { controller = "F0", action = "DetailSave" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/Detail/{*pathInfo}",
                defaults: new { controller = "F0", action = "Detail" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F0/{*pathInfo}",
                defaults: new { controller = "F0", action = "Index" }
            );

            // F1
            context.MapRoute(
                name: null,
                url: "Order/F1/DownloadWordFile/{*pathInfo}",
                defaults: new { controller = "F1", action = "DownloadWordFile" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F1/DownloadExcelFile/{*pathInfo}",
                defaults: new { controller = "F1", action = "DownloadExcelFile" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F1/SaveTableV2/{*pathInfo}",
                defaults: new { controller = "F1", action = "SaveTableV2" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F1/SaveTable/{*pathInfo}",
                defaults: new { controller = "F1", action = "SaveTable" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F1/SaveShedule/{*pathInfo}",
                defaults: new { controller = "F1", action = "SaveShedule" }
                );

            context.MapRoute(
                name: null,
                url: "Order/F1/{*pathInfo}",
                defaults: new { controller = "F1", action = "Index" }
            );

            // F2
            context.MapRoute(
                name: null,
                url: "Order/F2/CreateOrderSpecTable/{*pathInfo}",
                defaults: new { controller = "F2", action = "CreateOrderSpecTable" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F2/ParseFile/{*pathInfo}",
                defaults: new { controller = "F2", action = "ParseFile" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F2/{*pathInfo}",
                defaults: new { controller = "F2", action = "Index" }
            );

            // F3

            context.MapRoute(
                name: null,
                url: "Order/F3/Save2/{*pathInfo}",
                defaults: new { controller = "F3", action = "Save2" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F3/Save/{*pathInfo}",
                defaults: new { controller = "F3", action = "Save" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F3/{*pathInfo}",
                defaults: new { controller = "F3", action = "Index" }
            );

            // F4
            context.MapRoute(
                name: null,
                url: "Order/F4/SendEmail/{*pathInfo}",
                defaults: new { controller = "F4", action = "SendEmail" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F4/LoadS/{*pathInfo}",
                defaults: new { controller = "F4", action = "LoadS" }
            );

            context.MapRoute(
                name: null,
                url: "Order/F4/{*pathInfo}",
                defaults: new { controller = "F4", action = "Index" }
            );
        }
    }
}
