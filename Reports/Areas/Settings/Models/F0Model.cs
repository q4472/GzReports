using FarmSib.MvcApplication2;
using Nskd;
using System;

namespace MvcApplication2.Areas.Settings.Models
{
    public class F0Model
    {
        public static String Get(Guid sessionId)
        {
            String json = "{}";
            RequestPackage rqp = new RequestPackage
            {
                SessionId = sessionId,
                Command = "[dbo].[settings_get]",
            };
            rqp.AddSessionIdToParameters();
            String uri = "http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_SQL_PORT + "/";
            ResponsePackage rsp = rqp.GetResponse(uri);
            if (rsp != null)
            {
                json = Nskd.JsonV2.ToString(rsp.Data);
            }
            return json;
        }
        public static String Set(RequestPackage rqp)
        {
            String json = "{}";
            if (rqp != null)
            {
                Guid sessionId = rqp.SessionId;
                if (rqp.Parameters != null)
                {
                    foreach (RequestParameter p in rqp.Parameters)
                    {
                        UpdateSettingRow(sessionId, p.Name, p.Value);
                    }
                }
                json = Get(sessionId);
            }
            return json;
        }
        private static void UpdateSettingRow(Guid sessionId, String name, Object value)
        {
            RequestPackage rqp = new RequestPackage
            {
                SessionId = sessionId,
                Command = "[dbo].[settings_update_row]",
                Parameters = new RequestParameter[] {
                    new RequestParameter("session_id", sessionId),
                    new RequestParameter("name", name),
                    new RequestParameter("value", value)
                }
            };
            String uri = "http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_SQL_PORT + "/";
            rqp.GetResponse(uri);
        }
    }
}
