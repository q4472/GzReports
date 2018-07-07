using MvcApplication2.Areas.Docs.Models;
using FarmSib.Base.Lib;
using FarmSib.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Docs.Controllers
{
    public class RdController : Controller
    {
        public Object Index()
        {
            Object result = null;
            String alias = "docs_rd";
            if (Request.Form.Count > 1) // session_id, cmd
            {
                Dictionary<String, String> pars = new Dictionary<String, String>(Request.Form.Count);
                for (int i = 0; i < Request.Form.Count; i++)
                {
                    pars.Add(Request.Form.Keys[i], Request.Form.GetValues(i)[0]);
                }
                String cmd = pars["cmd"];
                if (!String.IsNullOrWhiteSpace(cmd))
                {
                    switch (cmd)
                    {
                        case "GetFsInfo":
                            String path = Utility.UnEscape(pars["path"]);
                            String type = pars["type"]; // или "dir" или "file"
                            DataGridView fileInfo = RdModel.GetFsInfo(alias, path, type);
                            result = PartialView("~/Views/Shared/DataGridView/Edit.cshtml", fileInfo);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Guid sessionId = new Guid();
                TempData["fsTreeId"] = String.Format("_{0}_", Guid.NewGuid().ToString("N"));
                TempData["alias"] = alias;
                TempData["html"] = FileTree.RenderDirectoryTree(sessionId, alias, null);
                result = View("Index");
            }
            return result;
        }
    }
}
