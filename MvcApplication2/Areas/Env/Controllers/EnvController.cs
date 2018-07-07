using MvcApplication2.Areas.Env.Models;
using Nskd;
using System;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Env.Controllers
{
    public class EnvController : Controller
    {
        public Object Get()
        {
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            ResponsePackage rsp = EnvModel.Get(rqp);
            String data = Nskd.JsonV2.ToString(rsp);
            return data;
        }
        public Object Set()
        {
            String msg = "EnvController.Set: ";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            msg += EnvModel.Set(rqp);
            return msg;
        }
    }
}
