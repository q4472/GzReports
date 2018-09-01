using FarmSib.Base.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MvcApplication2.Areas.Reports.Models
{
    public class SudModel
    {
        public String iddoc { get; set; }
        public Int32 status_id { get; set; }
        public Int32 stage_id { get; set; }
        public Int32 set_of_docs_id { get; set; }
        public Int32 payments_status_id { get; set; }
        public String payment_date { get; set; }
        public String note { get; set; }
        public String agreement_id { get; set; }
        public Boolean agreement_cascade { get; set; }
        public String bill_list { get; set; }
        public void UpsertSud(String sessionId)
        {
            DataTable dt = HomeData.Mail.GetSudAndEmail(iddoc);
            String[] iddocs = new String[] { iddoc };
            if ((agreement_cascade) && (!String.IsNullOrWhiteSpace(bill_list)))
            {
                iddocs = bill_list.Split(',');
            }
            foreach (String id in iddocs)
            {
                HomeData.Reports.UpsertSud(
                    id,
                    status_id,
                    stage_id,
                    set_of_docs_id,
                    payments_status_id,
                    (String.IsNullOrWhiteSpace(payment_date)) ? null : payment_date,
                    note,
                    (sessionId == "null") ? null : sessionId
                    );
            }
            // отправляем сообщение менеджеру
            try
            {
                //HomeData.Log.ConsoleWriteLine("test");
                if (status_id == 1)
                {
                    //HomeData.Log.ConsoleWriteLine("status_id == 1");
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        //HomeData.Log.ConsoleWriteLine("(dt != null) && (dt.Rows.Count > 0)");
                        DataRow dr = dt.Rows[0];
                        Int32 isSud = (Int32)dr["is_sud"];
                        if (isSud == 0)
                        {
                            //HomeData.Log.ConsoleWriteLine("isSud == 0");
                            String docNo = dr["doc_no"] as String;
                            String docDate = dr["doc_date"] as String;
                            String email = (dr["person_email"] as String) ?? "";
                            String all = (agreement_cascade) ? " и для всех накладных по этому договору" : "";
                            String msg = String.Format("Для расходной накладной № {0} от {1}{2} установлен судебный статус.", docNo, docDate, all);
                            if (!String.IsNullOrWhiteSpace(email))
                            {
                                HomeData.Mail.Send(email + ": " + msg, "grshanin@farmsib.ru");
                                try
                                {
                                    HomeData.Mail.Send(msg, email);
                                }
                                catch (Exception e)
                                {
                                    msg = String.Format("<font color='red'>Ошибка. Для расходной накладной № {0} от {1}</font>/Автоматическая рассылка/{2}", docNo, docDate, e.Message);
                                    HomeData.Mail.Send(msg, "grshanin@farmsib.ru");
                                }
                            }
                            else
                            {
                                msg = String.Format("<font color='red'>Ошибка. Для расходной накладной № {0} от {1}</font>/Автоматическая рассылка/Неопределён адрес менеджера.", docNo, docDate);
                                HomeData.Mail.Send(msg, "grshanin@farmsib.ru");
                            }
                        }
                    }
                }
            }
            catch (Exception e) { HomeData.Log.WriteToConsole(sessionId, e.ToString()); }
        }
    }
}