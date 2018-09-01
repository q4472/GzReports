using MvcApplication2.Areas.Order.Models;
using Nskd;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Order.Controllers
{
    public class F2Controller : Controller
    {
        public Object Index()
        {
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            Int32 specId = 0;
            Int32.TryParse(rqp["specId"] as String, out specId);
            PartialViewResult pvr = PartialView("~/Areas/Order/Views/F2/Index.cshtml");
            pvr.TempData.Add("specId", specId);
            return pvr;
        }
        public Object ParseFile()
        {
            F2Model m = new F2Model();
            String msg = "MvcApplication2.Areas.Order.Controllers.Controller.ParseFile()";
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase f = Request.Files[0];
                String fileName = f.FileName;
                String fileContentType = f.ContentType;
                Stream s = f.InputStream;
                msg = m.ReadTables(fileName, fileContentType, s);
            }
            TempData["q"] = msg;
            return PartialView("~/Areas/Order/Views/F2/ParsingResult.cshtml", m);
        }
        public Object CreateOrderSpecTable()
        {
            String msg = "";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            msg = F2Model.CreateOrderSpecTable(rqp);
            return msg;
        }
    }
}
