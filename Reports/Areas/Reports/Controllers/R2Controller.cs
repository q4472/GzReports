using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Areas.Reports.Models;
using FarmSib.Base.Models;

namespace MvcApplication2.Areas.Reports.Controllers
{
    public class R2Controller : Controller
    {
        public ActionResult Index()
        {
            R2Model m = new R2Model();
            return View(m);
        }
        public PartialViewResult GetEmploeeSelector(R2Model m) //, String SessionId)
        {
            //NskdSessionLite session = NskdSessionLite.GetById(SessionId);
            m.Emploee.FillManagerData();
            m.Emploee.ManagerSelector = "-1";
            return PartialView("EmploeeSelector", m);
        }
        public PartialViewResult GetReport(R2Model m)
        {
            m.GetReport();
            return PartialView("Report", m);
        }
    }
}
