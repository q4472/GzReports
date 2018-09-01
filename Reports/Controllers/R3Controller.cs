using MvcApplication2.Models;
using FarmSib.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Controllers
{
    public class R3Controller : Controller
    {
        public ActionResult Index()
        {
            R3Model m = new R3Model();
            return View(m);
        }

        public PartialViewResult ApplyFilter(List<String> fs, List<Int32> sf)
        {
            R3Model m = new R3Model(fs, sf);
            return PartialView("~/Views/Shared/DataGridView/Show.cshtml", m.FilteredView);
        }

        public PartialViewResult ApplyPeriod(String p1, String p2)
        {
            DateTime d1;
            if (!DateTime.TryParse(p1, out d1)) d1 = DateTime.Now.AddDays(-3);

            DateTime d2;
            if (!DateTime.TryParse(p2, out d2)) d2 = DateTime.Now;

            R3Model m = new R3Model(d1, d2);
            return PartialView("~/Views/Shared/DataGridView/Show.cshtml", m.FilteredView);
        }

        public PartialViewResult GetHistory(String iddoc)
        {
            R3Model.History m = new R3Model.History(iddoc);
            return PartialView("~/Views/Shared/DataGridView/Show.cshtml", m.HistoryView);
        }

    }
}
