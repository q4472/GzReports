using FarmSib.AreasAgrs.Areas.Agrs.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace FarmSib.AreasAgrs.Areas.Agrs.Controllers
{
    public class F0Controller : Controller
    {
        public Object Index(String sessionId)
        {
            F0Model m = new F0Model(sessionId);            
            return View("~/Areas/Agrs/Views/F0/Index.cshtml", m);
        }

        public PartialViewResult GetDataForSelectorWithListBox(String tableName, String filter)
        {
            Object dt = F0Model.GetDataForSelectorWithListBox(tableName, filter);
            PartialViewResult r = PartialView("~/Areas/Agrs/Views/Shared/ListBox.cshtml", dt);
            return r;
        }

        public Object GetDataForFilteredView()
        {
            Object result = null;
            Dictionary<String, String> fs = new Dictionary<String, String>(Request.Form.Count);
            for (int i = 0; i < Request.Form.Count; i++)
            {
                fs.Add(Request.Form.Keys[i], Request.Form.GetValues(i)[0]);
            }
            F0Model m = new F0Model(fs);
            result = PartialView("~/Areas/Agrs/Views/F0/FilteredView.cshtml", m.workingData);
            return result;
        }

        public PartialViewResult GetDataForDetailSection()
        {
            Dictionary<String, String> fs = new Dictionary<String, String>(Request.Form.Count);
            for (int i = 0; i < Request.Form.Count; i++)
            {
                fs.Add(Request.Form.Keys[i], Request.Form.GetValues(i)[0]);
            }
            F0Model m = new F0Model(fs);
            m.LoadDictionariesAndFillPopups();
            return PartialView("~/Areas/Agrs/Views/F0/Detail.cshtml", m);
        }

        public String Save()
        {
            String status = null;
            Dictionary<String, String> pars = new Dictionary<String, String>(Request.Form.Count);
            for (int i = 0; i < Request.Form.Count; i++)
            {
                pars.Add(Request.Form.Keys[i], Request.Form.GetValues(i)[0]);
            }
            if ((pars != null) && pars.ContainsKey("cmd"))
            {
                switch (pars["cmd"])
                {
                    case "save":
                        status = F0Model.Upsert(pars);
                        break;
                    case "delete":
                        status = F0Model.Delete(pars);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                status = "Ошибка в формате команды.";
            }
            return status;
        }
    }
}
