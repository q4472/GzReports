using MvcApplication2.Areas.Settings.Models;
using Nskd;
using System;
using System.IO;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Settings.Controllers
{
    public class F0Controller : Controller
    {
        public ActionResult Index(Guid sessionId)
        {
            String settingsDsAsJsonString = F0Model.Get(sessionId);
            return PartialView("~/Areas/Settings/Views/F0/Index.cshtml", settingsDsAsJsonString);
        }
        public Object Save()
        {
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            String settingsDsAsJsonString = F0Model.Set(rqp);
            return settingsDsAsJsonString;
        }
    }
}
