using System;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class R2Controller : Controller
    {
        public Object Index()
        {
            Object v = null;
            var m = new R2Model();
            if (ControllerContext.HttpContext.IsDebuggingEnabled)
                v = View("~/Views/R2/Index.cshtml", m); // _ViewStart.cshtml
            else
                v = PartialView("~/Views/R2/Index.cshtml", m);
            return v;
        }
        public Object GetEmploeeSelector(R2Model m) //, String SessionId)
        {
            //NskdSessionLite session = NskdSessionLite.GetById(SessionId);
            m.Emploee.FillManagerData();
            m.Emploee.ManagerSelector = "-1";
            return PartialView("EmploeeSelector", m);
        }
        public Object GetReport(R2Model m)
        {
            m.GetReport();
            return PartialView("Report", m);
        }
    }
}
