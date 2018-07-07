using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Items.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            ViewResult r = View("~/Areas/Items/Views/Search/Index.cshtml");
            return r;
        }
    }
}
