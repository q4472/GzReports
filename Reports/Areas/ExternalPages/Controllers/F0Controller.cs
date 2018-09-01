using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace MvcApplication2.Areas.ExternalPages.Controllers
{
    public class F0Controller : Controller
    {
        public Object Index()
        {
            WebRequest request = WebRequest.Create("http://www.farmsib.ru/");
            WebResponse response = request.GetResponse();
            String html = null;
            Stream responseStream = response.GetResponseStream();
            using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(1251)))
            {
                html = streamReader.ReadToEnd();
            }
            Int32 i = html.IndexOf("<body");
            Int32 j = html.IndexOf("</body>");
            html = html.Substring(i, j - i + 7);
            return html;
        }

    }
}
