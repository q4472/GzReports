using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Areas.Reports.Models;

namespace MvcApplication2.Areas.Reports.Controllers
{
    public class SudController : Controller
    {
        public void UpsertSud(SudModel m, String SessionId)
        {
            m.UpsertSud(SessionId);
        }
    }
}
