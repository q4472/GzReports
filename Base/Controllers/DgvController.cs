using FarmSib.Base.Lib;
using FarmSib.Base.Models;
using Nskd;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace FarmSib.Base.Controllers
{
    public class DgvController : Controller
    {
        public Object Index()
        {
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            Object[] log = rqp["log"] as Object[];
            String msg = DgvModel.Exec(log);
            return msg;
        }
    }
}
