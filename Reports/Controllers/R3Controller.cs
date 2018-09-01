using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class R3Controller : Controller
    {
        public Object Index()
        {
            Object v = null;
            var m = new R3Model();
            if (ControllerContext.HttpContext.IsDebuggingEnabled)
                v = View("~/Views/R3/Index.cshtml", m); // _ViewStart.cshtml
            else
                v = PartialView("~/Views/R3/Index.cshtml", m);
            return v;
        }
        public Object ApplyFilter(List<String> fs, List<Int32> sf)
        {
            R3Model m = new R3Model(fs, sf);
            return PartialView("~/Views/Shared/DataGridView/Show.cshtml", m.FilteredView);
        }
        public Object ApplyPeriod(String p1, String p2)
        {
            DateTime d1;
            if (!DateTime.TryParse(p1, out d1)) d1 = DateTime.Now.AddDays(-3);

            DateTime d2;
            if (!DateTime.TryParse(p2, out d2)) d2 = DateTime.Now;

            R3Model m = new R3Model(d1, d2);
            return PartialView("~/Views/Shared/DataGridView/Show.cshtml", m.FilteredView);
        }
        public Object GetHistory(String iddoc)
        {
            R3Model.History m = new R3Model.History(iddoc);
            return PartialView("~/Views/Shared/DataGridView/Show.cshtml", m.HistoryView);
        }

    }
}
