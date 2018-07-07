using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarmSib.Base.Models;
using MvcApplication2.Areas.Reports.Models;

namespace MvcApplication2.Areas.Reports.Controllers
{
    public class R1Controller : Controller
    {
        public ActionResult Index()
        {
            R1Model m = new R1Model();
            return View(m);
        }
        public PartialViewResult GetClientSelector(R1Model m)
        {
            m.Client.FillClientData();
            m.Client.ClientSelector = "-1";
            return PartialView("ClientSelector", m);
        }
        public PartialViewResult GetManagerSelector(R1Model m)
        {
            m.Emploee.FillManagerData();
            m.Emploee.ManagerSelector = "-1";
            return PartialView("ManagerSelector", m);
        }
        public PartialViewResult GetReport(R1Model m)
        {
            m.GetReport();
            return PartialView("Report", m);
        }
    }
}
