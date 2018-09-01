using FarmSib.Base.Data;
using Nskd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Areas.Env.Models
{
    public class EnvModel
    {
        public static String Set(RequestPackage rqp, String prefix = "")
        {
            String msg = "EnvModel.Set: ";
            if (rqp != null)
            {
                // переделываем rqp в rqp1 для сохранения на Sql сервере как пары имя - значение
                RequestPackage rqp1 = new RequestPackage();
                rqp1.SessionId = rqp.SessionId;
                rqp1.Command = "[Pharm-Sib].[dbo].[env_set]";
                if (rqp.Parameters != null && rqp.Parameters.Length > 0)
                {
                    // переделываем запрос к http server на запрос к sql server
                    foreach (RequestParameter p in rqp.Parameters)
                    {
                        rqp1.Parameters = new RequestParameter[] 
                        { 
                            new RequestParameter("session_id", rqp.SessionId.ToString()),
                            new RequestParameter("name", (prefix ?? "") + p.Name),
                            new RequestParameter("value", p.Value as String),
                            new RequestParameter("value_type", "string")
                        };
                        msg += HomeData.Env.Set(rqp1);
                    }
                }
            }
            return msg;
        }
        public static ResponsePackage Get(RequestPackage rqp)
        {
            ResponsePackage rsp = new ResponsePackage();
            rsp.Status = "EnvModel.Get";
            if (rqp != null)
            {
                // переделываем запрос к http server на запрос к sql server
                RequestPackage rqp1 = new RequestPackage();
                rqp1.SessionId = rqp.SessionId;
                rqp1.Command = "[Pharm-Sib].[dbo].[env_get]";
                rqp1.Parameters = new RequestParameter[] 
                {
                    new RequestParameter("session_id", rqp.SessionId),
                    new RequestParameter("pattern", rqp["pattern"] as String)
                };
                rsp = HomeData.Env.Get(rqp1);
            }
            return rsp;
        }
    }
}