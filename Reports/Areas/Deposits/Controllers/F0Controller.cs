using MvcApplication2.Areas.Deposits.Models;
using FarmSib.Base.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Deposits.Controllers
{
    public class F0Controller : Controller
    {
        public ActionResult Index()
        {
            //F0Model m = new F0Model();
            ActionResult r = View("~/Areas/Deposits/Views/F0/Index.cshtml", null);
            return r;
        }
        public PartialViewResult GetFiltredData(String sd, String ed, String cust, String trNum, String onRowSelect)
        {
            ViewData = new ViewDataDictionary() { 
                new KeyValuePair<String, Object>("OnRowSelect", onRowSelect) 
            };
            PartialViewResult r = PartialView(
                "~/Views/Shared/DataGridView/Show.cshtml",
                F0Model.GetFiltredData(
                    Utility.UnEscape(sd),
                    Utility.UnEscape(ed),
                    Utility.UnEscape(cust),
                    Utility.UnEscape(trNum)));
            return r;
        }
        public PartialViewResult GetCustData(String id, Boolean flag11, Boolean flag12, Boolean flag21, Boolean flag22, Boolean flag31, Boolean flag32)
        {
            Int32 _id = -1;
            id = Utility.UnEscape(id);
            id = (new Regex(@"[^\d]")).Replace(id, "");
            Int32.TryParse(id, out _id);

            F0Model m = new F0Model();
            m.GetDetailData(_id, flag11, flag12, flag21, flag22, flag31, flag32);
            PartialViewResult r = PartialView("~/Areas/Deposits/Views/F0/Detail.cshtml", m);

            return r;
        }
        public PartialViewResult GetEntriesData(String id, String trNum, String onRowSelect)
        {
            Int32 _id = -1;
            id = Utility.UnEscape(id);
            id = (new Regex(@"[^\d]")).Replace(id, "");
            Int32.TryParse(id, out _id);

            trNum = Utility.UnEscape(trNum);
            trNum = (new Regex(@"[^\d]")).Replace(trNum, "");

            ViewData = new ViewDataDictionary() { 
                new KeyValuePair<String, Object>("OnRowSelect", onRowSelect) 
            };
            PartialViewResult r = PartialView(
                "~/Views/Shared/DataGridView/Show.cshtml",
                F0Model.GetEntriesData(_id, trNum));

            return r;
        }
    }
}
