using FarmSib.Base.Models;
using AreasPrep.Areas.Prep.Models;
using Nskd;
using System;
using System.Web.Mvc;

namespace AreasPrep.Areas.Prep.Controllers
{
    public class F1Controller : Controller
    {
        public Object Index()
        {
            Object result = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if(rqp != null &&
                Int32.TryParse(rqp["specId"] as String, out Int32 specId))
            {
                F1Model m = new F1Model(rqp.SessionId, specId, Request.Url); 
                result = PartialView("~/Areas/Prep/Views/F1/Index.cshtml", m);
            }
            return result;
        }
        public Object SaveTableV2()
        {
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            String msg = F1Model.SaveTableV2(rqp);
            return msg;// "Для проверки сохранения надо презагрузить таблицу.";
        }
        public FileContentResult DownloadExcelFile(Guid sessionId, Int32 specId)
        {
            F1Model m = new F1Model(sessionId, specId);
            Byte[] buff = NskdExcel.ToExcel(sessionId, m.Table_dt, m.Head_dt);
            String fileName = null;
            try
            {
                fileName = (String)m.Head_dt.Rows[0]["номер_извещения_аукциона"];
            }
            catch (Exception) { }
            if (String.IsNullOrWhiteSpace(fileName)) fileName = specId.ToString();
            if (String.IsNullOrWhiteSpace(fileName)) fileName = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");
            fileName += ".xlsx";
            FileContentResult f = File(buff, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            return f;
        }
        public String SaveShedule()
        {
            F1Model.SaveShedule(Request.Form);
            return "Сохранено";
        }
    }
}
