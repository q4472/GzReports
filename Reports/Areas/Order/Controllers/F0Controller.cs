using FarmSib.MvcApplication2;
using MvcApplication2.Areas.Env.Models;
using MvcApplication2.Areas.Order.Models;
using Nskd;
using System;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Order.Controllers
{
    public class F0Controller : Controller
    {
        public Object Index(String sessionId)
        {
            F0Model m = new F0Model(sessionId);
            return PartialView("~/Areas/Order/Views/F0/Index.cshtml", m);
        }
        public Object ApplyFilter()
        {
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            
            // сохраняем новое значение фильтра в параметрах среды
            EnvModel.Set(rqp, "order_f0_filter.");

            F0Model m = new F0Model(rqp);
            return PartialView("~/Areas/Order/Views/F0/FilteredView.cshtml", m);
        }
        public Object Detail(Int32 id = 0)
        {
            Object r = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            rqp.AddSessionIdToParameters();
            rqp.Command = "[Pharm-Sib].[dbo].[спецификации_шапка_get_by_id]";
            ResponsePackage rsp = rqp.GetResponse("http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_SQL_PORT + "/");
            if (rsp != null)
            {
                r = PartialView("~/Areas/Order/Views/F0/Detail.cshtml", rsp.GetFirstTable());
            }
            return r;
        }
        public Object DetailSave()
        {
            Object r = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            rqp.AddSessionIdToParameters();
            rqp.Command = "[Pharm-Sib].[dbo].[спецификации_шапка_update_from_not_null_values]";
            ResponsePackage rsp = rqp.GetResponse("http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_SQL_PORT + "/");
            if (rsp != null)
            {
                r = PartialView("~/Areas/Order/Views/F0/Detail.cshtml", rsp.GetFirstTable());
            }
            return r;
        }
        public Object Delete()
        {
            String msg = "MvcApplication2.Areas.Order.Controllers.F0Controller.Delete(): ";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            Guid sessionId = rqp.SessionId;
            Int32 specId;
            if (Int32.TryParse(rqp["id"] as String, out specId))
            {
                msg += SpecHead.Delete(sessionId, specId);
            }
            else { msg += "Ошибка при разборе идентификатора спецификации."; }
            return msg;
        }
        public Object CalcOutgoingPrices()
        {
            String msg = "Start MvcApplication2.Areas.Order.Controllers.F0Controller.CalcOutgoingPrices().";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            msg = SpecHead.CalcOutgoingPrices(rqp);
            return msg;
        }
        public Object LoadAuctionInf()
        {
            Object r = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if (rqp != null)
            {
                Int32 specHeadType = 2;
                switch (rqp.Command)
                {
                    case "Order.F0.ExistsAuctionInf":
                        r = F0Command.ExistsAuctionInf(rqp.SessionId, rqp["auctionNumber"] as String);
                        break;
                    case "Order.F0.LoadAuctionInf":
                        r = F0Command.LoadAuctionInf(rqp.SessionId, rqp["auctionNumber"] as String, specHeadType);
                        break;
                    case "Order.F0.LoadByRegion":
                        r = F0Command.LoadByRegion(rqp.SessionId, rqp["regionNumber"] as String, rqp["publishDate"] as String);
                        break;
                    default:
                        r = String.Format("Ошибка: Неизвестная команда: '{0}'.", rqp.Command);
                        break;
                }
            }
            else { r = "Ошибка: Неверный формат запроса."; }
            return r;
        }
        public Object PassToTender()
        {
            Object r = "MvcApplication2.Areas.Order.Controllers.F0Controller.PassToTender()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            Guid sessionId = rqp.SessionId;
            String auctionNumber = rqp["auctionNumber"] as String;
            if (!String.IsNullOrWhiteSpace(auctionNumber))
            {
                r = F0Command.PassToTender(sessionId, auctionNumber);
            }
            return r;
        }
        public Object CreateSpec()
        {
            Int32 specId = 0;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if (rqp != null)
            {
                Guid sessionId = new Guid();
                Guid.TryParse(rqp["SessionId"] as String, out sessionId);
                specId = F0Model.CreateSpec(sessionId);
            }
            return specId.ToString();
        }
        public Object ImportFromPart1()
        {
            String msg = "MvcApplication2.Areas.Order.Controllers.F0Controller.ImportFromPart1()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            //String auctionNumber = rqp["auction_number"] as String;
            rqp.Command = "[Pharm-Sib].[dbo].[спецификации_create_order_from_prep]";
            rqp.AddSessionIdToParameters();
            ResponsePackage rsp = rqp.GetResponse("http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_SQL_PORT_V12 + "/");
            Object spec_id = rsp.GetScalar();
            return msg + Nskd.JsonV2.ToString(spec_id); // + " " + auctionNumber;
        }
    }
}
