using MvcApplication2.Areas.Tn.Models;
using FarmSib.Base.Models;
using Nskd;
using System;
using System.Data;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Tn.Controllers
{
    public class TnController : Controller
    {
        public Object SrcSelector()
        {
            return PartialView("~/Areas/Tn/Views/SrcSelector.cshtml");
        }
        public Object MnnSelector()
        {
            return PartialView("~/Areas/Tn/Views/MnnSelector.cshtml");
        }
        public Object TnSelector()
        {
            return PartialView("~/Areas/Tn/Views/TnSelector.cshtml");
        }
        public Object GetInstructions()
        {
            Object r = null;
            String json = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if (rqp != null && rqp.Parameters != null && rqp.Parameters.Length >= 3)
            {
                Guid guid;
                if (Guid.TryParse(rqp.Parameters[0].Value as String, out guid))
                {
                    String nr = rqp.Parameters[1].Value as String;
                    String id = rqp.Parameters[2].Value as String;
                    json = TnModel.GetInstruction(guid, nr, id);
                }
            }
            if (!String.IsNullOrWhiteSpace(json))
            {
                r = PartialView("~/Areas/Tn/Views/TnInstructions.cshtml", json);
            }
            else
            {
                r = "GetInstructions: Error in request.";
            }
            return r;
        }
        public Object GetCopy(Int32 copyCode = 0)
        {
            Object result = null;
            String uri = TnModel.GetRegCopyUri(copyCode);
            Int32 index = uri.IndexOf('}');
            if (index >= 0)
            {
                String alias = uri.Substring(1, index - 1);
                String path = uri.Substring(index + 1, uri.Length - index - 1);
                FileData fd = FileData.GetFile(alias, path);
                if (fd != null && fd.Contents != null)
                {
                    result = File(fd.Contents, fd.ContentType, fd.Name);
                }
            }
            return result;
        }
        public Object Get1cData()
        {
            Object result = "MvcApplication2.Areas.Tn.Controllers.TnController.Get1cData()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            String rCode = rqp["rCode"] as String;
            String fCode = rqp["fCode"] as String;
            String uCode = rqp["uCode"] as String;
            Guid sessionId = rqp.SessionId;
            TnModel m = new TnModel();
            m.Get1cData(sessionId, rCode, fCode, uCode);
            result = PartialView("~/Areas/Tn/Views/1cData.cshtml", m);
            return result;
        }
        public Object GetUData()
        {
            Object result = "MvcApplication2.Areas.Tn.Controllers.TnController.GetUData()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            String fCode = rqp["fCode"] as String;
            Guid sessionId = rqp.SessionId;
            TnModel m = new TnModel();
            DataSet ds = m.GetUData(sessionId, fCode);
            result = Nskd.JsonV2.ToString(ds);
            //result = PartialView("~/Areas/Tn/Views/1cData.cshtml", m);
            return result;
        }
    }
}
