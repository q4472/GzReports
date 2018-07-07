using FarmSib.Base.Lib;
using FarmSib.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FarmSib.Base.Controllers
{
    public class FsController : Controller
    {
        public Object Index(String alias)
        {
            Object result = null;
            if (Request.Form.Count > 0)
            {
                Dictionary<String, String> pars = new Dictionary<String, String>(Request.Form.Count);
                for (int i = 0; i < Request.Form.Count; i++)
                {
                    pars.Add(Request.Form.Keys[i], Request.Form.GetValues(i)[0]);
                }
                if (pars.ContainsKey("cmd"))
                {
                    String html;
                    String path = Utility.UnEscape(pars["path"]);

                    switch (pars["cmd"])
                    {
                        case "GetFileInfo":
                            html = "GetFileInfo: '" + path + "'";
                            //html = FileTree.GetFileInfo(alias, path);
                            result = html;
                            break;
                        case "GetDirectoryInfo":
                            Guid sessionId = new Guid();
                            html = FileTree.RenderDirectoryTree(sessionId, alias, path);
                            result = html;
                            break;
                        case "DownloadFile":
                            FileData fd = FileData.GetFile(alias, path);
                            result = File(fd.Contents, fd.ContentType, fd.Name);
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }
    }
}
