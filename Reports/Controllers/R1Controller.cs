using System;
using System.Web.Mvc;
using MvcApplication2.Models;
using Nskd;
using System.Data;

namespace MvcApplication2.Controllers
{
    public class R1Controller : Controller
    {
        public Object Index()
        {
            Object v = null;
            var m = new R1Model();
            if (ControllerContext.HttpContext.IsDebuggingEnabled)
                v = View("~/Views/R1/Index.cshtml", m); // _ViewStart.cshtml
            else
                v = PartialView("~/Views/R1/Index.cshtml", m);
            return v;
        }
        public Object GetClientSelector(R1Model m)
        {
            m.Client.FillClientData();
            m.Client.ClientSelector = "-1";
            return PartialView("ClientSelector", m);
        }
        public Object GetManagerSelector(R1Model m)
        {
            m.Emploee.FillManagerData();
            m.Emploee.ManagerSelector = "-1";
            return PartialView("ManagerSelector", m);
        }
        public Object GetReport(R1Model m)
        {
            m.GetReport();
            return PartialView("Report", m);
        }
        public Object SaveDocsRetComm()
        {
            Object r = "Reports.Controllers.R1Controller.SaveDocsRetComm()\n";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            rqp.AddSessionIdToParameters();
            r += Nskd.JsonV3.ToString(rqp) + "\n";
            ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012/");
            DataTable dt = rsp.GetFirstTable();
            if (dt != null && dt.Columns.Contains("docs_ret_comm") && dt.Rows.Count > 0)
            {
                r = dt.Rows[0]["docs_ret_comm"] as String;
            }
            return r;
        }
    }
}
