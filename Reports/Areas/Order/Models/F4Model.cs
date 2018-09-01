using FarmSib.Base.Data;
using FarmSib.Base.Models;
using Nskd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace MvcApplication2.Areas.Order.Models
{
    public class F4Model
    {
        public String PageData; // DataSet as json string
        public void LoadSpec(RequestPackage rqp)
        {
            if (rqp != null && rqp.Parameters != null && rqp.Parameters.Length > 0)
            {
                Guid sessionId = rqp.SessionId;
                Int32 specId = 0;
                Int32 specRowId = 0;
                foreach (RequestParameter p in rqp.Parameters)
                {
                    if (p != null && p.Name == "specId")
                    {
                        Int32.TryParse(p.Value as String, out specId);
                    }
                    if (p != null && p.Name == "specRowId")
                    {
                        Int32.TryParse(p.Value as String, out specRowId);
                    }
                }
                if (specId > 0 && specRowId > 0)
                {
                    DataSet spec = HomeData.Order.LoadSpec(sessionId, specId, specRowId);
                    if (spec != null)
                    {
                        PageData = Nskd.JsonV2.ToString(spec);
                    }
                }
            }
        }
        public static ResponsePackage LoadS(RequestPackage rqp)
        {
            ResponsePackage rsp = HomeData.Order.LoadS(rqp);
            return rsp;
        }
        public static ResponsePackage SendEmail(RequestPackage rqp)
        {

            NskdSessionLite session = NskdSessionLite.GetById(rqp.SessionId);
            Int32 psLen = rqp.Parameters.Length;
            RequestParameter[] ps = new RequestParameter[psLen + 1];
            Int32 i = 0;
            foreach (RequestParameter p in rqp.Parameters)
            {
                if (p.Name == "subject")
                {
                    p.Value = String.Format("{0} Запрос. {1}", session.UserName, p.Value);
                }
                ps[i++] = p;
            }
            rqp.Parameters = ps;
            ResponsePackage rsp = HomeData.Order.SendEmail(rqp);
            return rsp;
        }
    }
}