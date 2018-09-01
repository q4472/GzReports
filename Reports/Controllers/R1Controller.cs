﻿using System;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class R1Controller : Controller
    {
        public Object Index()
        {
            Object v = null;
            R1Model m = new R1Model();
            if (ControllerContext.HttpContext.IsDebuggingEnabled)
                v = View("~/Views/R1/Index.cshtml", m); // _ViewStart.cshtml
            else
                v = PartialView("~/Views/R1/Index.cshtml", m);
            return v;
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
