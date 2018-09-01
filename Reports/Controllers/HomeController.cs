using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FarmSib.Base.Models;
using System.Data;
using System.IO;
using System.Text;

namespace FarmSib.Base.Controllers
{
    public class HomeController : Controller
    {
        // сюда идут все не перехваченные запросы
        public ActionResult Index()
        {
            ActionResult result = null;
            try
            {
                switch (Request.HttpMethod)
                {
                    case "GET":
                        result = IndexGet();
                        break;

                    case "POST":
                        result = IndexPost();
                        break;

                    default:
                        result = View("~/Views/Shared/Error.cshtml", "Неподдерживаемый http метод: " + Request.HttpMethod + ".");
                        break;
                }
            }
            catch (Exception e)
            {
                result = View("~/Views/Shared/Error.cshtml", e.ToString());
            }
            return result;
        }

        private ViewResult IndexGet()
        {
            // Это для отладки. 
            // Прямые GET запросы сюда не доходят.
            // Их перехватывает прокси или IIS.

            ViewResult result = null;
            CommonModel m = new CommonModel();
            m.UserMainMenu = new UserMainMenu();

            //String tmp;
            //using (StreamReader sr = new StreamReader(Request.InputStream, Encoding.UTF8))
            {
                //tmp = sr.ReadToEnd();
            }
            //if (!String.IsNullOrWhiteSpace(tmp))
            {
                //Int32 i = tmp.IndexOf("SessionId=");
                //if (i >= 0)
                {
                    m.Session = new NskdSessionLite("xxx.xxx.xxx.xxx");
                    m.Session.UpdateSession("9a3a8b8810e21b59334d1ea9510e3bc6", null); // Соколов
                    //m.Session.UpdateSession("509923e9825cb5e8e59999fd34de467d", null); // Сорокина
                    m.Session = NskdSessionLite.GetById(m.Session.SessionId);
                    m.UserMainMenu = new UserMainMenu(m.Session);
                    TempData["SessionId"] = m.Session.SessionId.ToString();
                }
            }
            if ((m.Session == null) || (m.Session.UserId < 0)) // 2
            {
                result = View("~/Views/Shared/Error.cshtml", (Object)"Нарушены условия доступа.");
            }
            else
            {
                result = View("~/Views/Home/Index.cshtml", m);
            }
            return result;
        }

        private ViewResult IndexPost()
        {
            ViewResult result = null;
            CommonModel m = new CommonModel();
            m.UserMainMenu = new UserMainMenu();

            String tmp;
            using (StreamReader sr = new StreamReader(Request.InputStream, Encoding.UTF8))
            {
                tmp = sr.ReadToEnd();
            }
            if (!String.IsNullOrWhiteSpace(tmp))
            {
                Int32 i = tmp.IndexOf("SessionId=");
                if (i < 0) { i = tmp.IndexOf("sessionId="); }
                if (i >= 0)
                {
                    m.Session = NskdSessionLite.GetById(new Guid(tmp.Substring(i + 10, 36)));
                    m.UserMainMenu = new UserMainMenu(m.Session);
                }
            }

            if ((m.Session == null) || (m.Session.UserId < 0)) // 2
            {
                result = View("~/Views/Shared/Error.cshtml", (Object)"Нарушены условия доступа.");
            }
            else
            {
                result = View("~/Views/Home/Index.cshtml", m);
            }
            return result;
        }
    }
}