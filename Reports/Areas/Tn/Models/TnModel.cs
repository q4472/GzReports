using FarmSib.Base.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace MvcApplication2.Areas.Tn.Models
{
    public class TnModel
    {
        public DataSet FarmSib1cData ;
        public static String GetRegCopyUri(Int32 copyCode)
        {
            String uri = HomeData.Tn.GetRegCopyUri(copyCode);
            return uri;
        }
        public static String GetInstruction(Guid guid, String nr, String id)
        {
            String json = GrlsRosminzdravRu.GetSearchResult(guid, nr, id);
            return json;
        }
        public void Get1cData(Guid sessionId, String rCode, String fCode, String uCode)
        {
            FarmSib1cData = HomeData.Tn.Get1cData(sessionId, rCode, fCode, uCode);
        }
        public DataSet GetUData(Guid sessionId, String fCode)
        {
            return HomeData.Tn.GetUData(sessionId, fCode);
        }
    }
    public class GrlsRosminzdravRu
    {
        public static String GetSearchResult(Guid guid, String nr, String id)
        {
            String r = null;
            String regNr = null;
            String hfIdReg = null;
            UriBuilder b = new UriBuilder();
            b.Scheme = "http";
            b.Host = "grls.rosminzdrav.ru";

            // для запроса инструкций нужно знать два параметра regNr и hfIdReg
            // они могут быть в nr и id
            // или их можно найти или базе данных по guid или на сайте

            // для начала проверяем nr и id
            Int32 idAsInt;
            if (!String.IsNullOrWhiteSpace(nr) && !String.IsNullOrWhiteSpace(id) && Int32.TryParse(id, out idAsInt))
            {
                regNr = nr;
                hfIdReg = id;
            }
            else // если нет - пробуем базу
            {
                String[] iLinks = HomeData.Tn.GetInstructionsLinks(guid);
                regNr = iLinks[0];
                hfIdReg = iLinks[1];
            }
            // если парметров в базе нет, то пробуем получить их на сайте
            if (String.IsNullOrWhiteSpace(regNr) || String.IsNullOrWhiteSpace(hfIdReg))
            {
                b.Path = "/Grls_View_v2.aspx";
                b.Query = String.Format("routingGuid={0}", guid);
                String receivedString = GetResponse(b.Uri);
                if (!String.IsNullOrWhiteSpace(receivedString))
                {
                    Int32 i1 = receivedString.IndexOf("ctl00$plate$RegNr") + 19;
                    if (i1 > 0)
                    {
                        Int32 i2 = receivedString.IndexOf("/>", i1) - 2;
                        if (i2 > i1)
                        {
                            Regex re = new Regex("value=\"([^\"]*)\"");
                            Match m = re.Match(receivedString, i1, (i2 - i1 + 1));
                            if (m.Groups.Count > 1)
                            {
                                regNr = m.Groups[1].Value;
                            }
                        }
                    }
                    i1 = receivedString.IndexOf("ctl00$plate$hfIdReg") + 21;
                    if (i1 > 0)
                    {
                        Int32 i2 = receivedString.IndexOf("/>", i1) - 2;
                        if (i2 > i1)
                        {
                            Regex re = new Regex("value=\"([^\"]*)\"");
                            Match m = re.Match(receivedString, i1, (i2 - i1 + 1));
                            if (m.Groups.Count > 1)
                            {
                                hfIdReg = m.Groups[1].Value;
                            }
                        }
                    }
                }
            }
            // если параметры нашлись - грузим ссылки на инструкции с сайта
            if (!String.IsNullOrWhiteSpace(regNr) && !String.IsNullOrWhiteSpace(hfIdReg))
            {
                b.Path = "/GRLS_View_V2.aspx/AddInstrImg";
                b.Query = "";
                String body = "{regNumber: '" + regNr + "'" + ", idReg: '" + hfIdReg + "'}";
                r = GetResponse(b.Uri, "POST", body);
            }
            return r;
        }
        private static String GetResponse(Uri uri, String method = "GET", String body = null)
        {
            String result = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Accept = "text/html";
            request.UserAgent = "Mozilla/5.0";
            request.UseDefaultCredentials = true;
            request.Timeout = 5000; // 5 sec.
            request.Method = method;

            switch (method)
            {
                case "POST":
                    request.ContentType = "application/json; charset=utf-8";
                    if (!String.IsNullOrWhiteSpace(body))
                    {
                        Stream rStream = request.GetRequestStream();
                        using (StreamWriter w = new StreamWriter(rStream, Encoding.UTF8))
                        {
                            w.Write(body);
                        }
                    }
                    break;
                case "GET":
                    break;
                default:
                    break;
            }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e) { result = e.ToString(); }
            if ((response != null) && (response.StatusCode == HttpStatusCode.OK))
            {
                Stream receivedStream = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader readStream = new StreamReader(receivedStream, encoding))
                {
                    result = readStream.ReadToEnd();
                }
            }
            return result;
        }

    }
}