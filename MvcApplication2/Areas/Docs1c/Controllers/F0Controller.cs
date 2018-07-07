using MvcApplication2.Areas.Docs1c.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Docs1c.Controllers
{
    public class F0Controller : Controller
    {
        public ActionResult Index()
        {
            F0Model m = new F0Model();
            m.GetData(null);
            return View("Index", m);
        }
        public Object Filter()
        {
            Object result = null;
            Dictionary<String, String> fs = new Dictionary<String, String>(Request.Form.Count);
            for (int i = 0; i < Request.Form.Count; i++)
            {
                fs.Add(Request.Form.Keys[i], Request.Form.GetValues(i)[0]);
            }
            F0Model m = new F0Model();
            m.GetData(fs);
            result = PartialView("FilteredView", m.WorkingData);
            return result;
        }
        public Object Detail()
        {
            Object result = null;
            Dictionary<String, String> fs = new Dictionary<String, String>(Request.Form.Count);
            for (int i = 0; i < Request.Form.Count; i++)
            {
                fs.Add(Request.Form.Keys[i], Request.Form.GetValues(i)[0]);
            }
            F0Model m = new F0Model();
            m.GetDetail(fs);
            result = PartialView("Detail", m.DetailData);
            return result;
        }
        public Object Save()
        {
            Object result = null;
            Dictionary<String, String> fs = new Dictionary<String, String>(Request.Form.Count);
            for (int i = 0; i < Request.Form.Count; i++)
            {
                fs.Add(Request.Form.Keys[i], Request.Form.GetValues(i)[0]);
            }
            F0Model m = new F0Model();
            m.Save(fs);
            return result;
        }
    }
}
