using AreasPrep.Areas.Prep.Models;
using Nskd;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AreasPrep.Areas.Prep.Controllers
{
    public class F4Controller : Controller
    {
        public Object Index()
        {
            Object r = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if (rqp != null)
            {
                F4Model m = new F4Model();
                m.LoadSpec(rqp);
                if (m.PageData != null)
                {
                    r = View("~/Areas/Prep/Views/F4/Index.cshtml", m);
                }
                else { r = "MvcApplication2.Areas.Prep.Controllers.F4Controller.Index(): Ошибка в параметрах запроса."; }
            }
            else { r = "MvcApplication2.Areas.Prep.Controllers.F4Controller.Index(): Ошибка в формате запроса."; }
            return r;
        }
        public Object LoadS()
        {
            ResponsePackage rsp = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if (rqp != null)
            {
                rsp = Models.F4Model.LoadS(rqp);
            }
            return Nskd.JsonV2.ToString(rsp);
        }
        public Object SendEmail()
        {
            String status = "Ошибка при передаче e-mail.";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if (rqp != null)
            {
                ResponsePackage rsp = Models.F4Model.SendEmail(rqp);
                if (rsp != null)
                {
                    status = rsp.Status;
                }
            }
            return status; 
        }
    }
}
