using Nskd;
using System;
using System.Data;

namespace FarmSib.Base.Data
{
    public class HomeData
    {
        public class Home
        {
            public static void CreateSession(Guid sessionId, String userHostAddress)
            {
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[phs_s].[dbo].[session_insert]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter ( "id", sessionId.ToString() ),
                    new RequestParameter ( "user_host_address", userHostAddress )
                };
                ExecuteInSql(rqp);
            }
            public static void UpdateSession(
                String userToken,
                Guid sessionId,
                String cryptKey
                )
            {
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[phs_s].[dbo].[session_update]";
                DataTable dt = new DataTable();
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter ( "user_token", userToken ),
                    new RequestParameter ( "session_id", sessionId.ToString() ),
                    new RequestParameter ( "crypt_key", cryptKey )
                };
                ExecuteInSql(rqp);
            }
            public static DataTable GetSessionById(Guid id)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[phs_s].[dbo].[session_get_by_id]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter ("id", id)
                };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataTable GetClientData(String filter)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[oc_клиенты_select]";
                rqp.Parameters = new RequestParameter[] {
                new RequestParameter ( "DESCR", filter)
            };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataTable GetManagerData(String filter)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[oc_сотрудники_select]";
                rqp.Parameters = new RequestParameter[] {
                new RequestParameter ("DESCR", filter)
            };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
        }
        public class Reports
        {
            public static DataTable GetReport1(
                String clientFilter = null,
                String clientCode = null,
                String managerFilter = null,
                String managerCode = null,
                String managerMultiCode = null,
                Int32? pr1 = null, Int32? pr2 = null,
                DateTime? onDate = null,
                Boolean sud = true,
                Boolean ord = true,
                Boolean sec = false
                )
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[Отчёт по неоплаченным отгрузкам]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter("client_filter", clientFilter),
                    new RequestParameter("client_code", clientCode),
                    new RequestParameter("manager_filter", managerFilter),
                    new RequestParameter("manager_code", managerCode),
                    new RequestParameter("emploee_multi_code", managerMultiCode),
                    new RequestParameter("pr1", pr1),
                    new RequestParameter("pr2", pr2),
                    new RequestParameter("date", onDate),
                    new RequestParameter("sud", sud),
                    new RequestParameter("ord", ord),
                    new RequestParameter("sec", sec)
                };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataTable GetReport2(
                String emploeeFilter = null,
                String emploeeCode = null,
                String emploeeMultiCode = null,
                Boolean marketer = true,
                Boolean manager = true,
                DateTime? beginDate = null,
                DateTime? endDate = null,
                Boolean sud = true,
                Boolean ord = true
                )
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[ОтчетПоОплатам]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter("НачПериода", beginDate),
                    new RequestParameter("КонПериода", endDate),
                    new RequestParameter("emploee_filter", emploeeFilter),
                    new RequestParameter("emploee_code", emploeeCode),
                    new RequestParameter("emploee_multi_code", emploeeMultiCode),
                    new RequestParameter("marketer", marketer),
                    new RequestParameter("manager", manager),
                    new RequestParameter("sud", sud),
                    new RequestParameter("ord", ord)
                };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static void UpsertSud(
                String iddoc,
                Int32 status_id,
                Int32 stage_id,
                Int32 set_of_docs_id,
                Int32 payments_status_id,
                String payment_date,
                String note,
                String sessionId)
            {
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[судебные_документы_upsert]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter("iddoc", iddoc),
                    new RequestParameter("status_id", status_id),
                    new RequestParameter("stage_id", stage_id),
                    new RequestParameter("set_of_docs_id", set_of_docs_id),
                    new RequestParameter("payments_status_id", payments_status_id),
                    new RequestParameter("payment_date", payment_date),
                    new RequestParameter("note", note),
                    new RequestParameter("session_id", sessionId)
                };
                Execute(rqp);
            }
            public static DataTable GetR3DataList(DateTime? d1 = null, DateTime? d2 = null)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[судебные_документы_отчёт]";
                rqp.Parameters = new RequestParameter[] {
                new RequestParameter("d1", d1),
                new RequestParameter("d2", d2)
            };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataTable GetR3HistoryList(String iddoc)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[судебные_документы_отчёт_по_истории_документа]";
                rqp.Parameters = new RequestParameter[] {
                new RequestParameter("iddoc", iddoc)
            };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataTable GetR3CrossTable()
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[судебные_документы_отчёт_по_истории]";
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
        }
        public class Mail
        {
            public static DataTable GetSudAndEmail(String iddoc)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[Garza].[dbo].[судебный_статус_и_email_by_iddoc]";
                rqp.Parameters = new RequestParameter[] {
                        new RequestParameter("iddoc", iddoc )
                    };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static void Send(String msg, String address)
            {
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "";
                rqp.Parameters = new RequestParameter[] {
                        new RequestParameter("msg", msg ),
                        new RequestParameter("address", address )
                };
                ExecuteInMail(rqp);
            }
        }
        public class Log
        {
            public static void WriteToConsole(String sessionId, String msg)
            {
                Guid sId;
                Guid.TryParse(sessionId, out sId);
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = sId;
                rqp.Command = "WriteToConsole";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter { Name = "msg", Value = msg }
                };
                ExecuteSssp(rqp);
            }
        }
        private static void AddSessionIdToParameters(RequestPackage rqp)
        {
            if (rqp == null) { throw new ArgumentNullException(); }

            if (rqp.Parameters == null)
            {
                rqp.Parameters = new RequestParameter[] {
                            new RequestParameter { Name = "session_id", Value = rqp.SessionId }
                        };
            }
            else
            {
                Boolean existsSessionId = false;
                foreach (RequestParameter p in rqp.Parameters)
                {
                    if (p.Name == "session_id")
                    {
                        existsSessionId = true;
                        break;
                    }
                }
                if (!existsSessionId)
                {
                    Array.Resize<RequestParameter>(ref rqp.Parameters, rqp.Parameters.Length + 1);
                    rqp.Parameters[rqp.Parameters.Length - 1] = new RequestParameter { Name = "session_id", Value = rqp.SessionId };
                }
            }
        }
        private static String dataServicesHost = "127.0.0.1"; // localhost
        public static ResponsePackage ExecuteInSql(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11002/");
            return rsp;
        }
        public static ResponsePackage ExecuteInXl(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11003/");
            return rsp;
        }
        public static ResponsePackage ExecuteIn1c(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11004/");
            return rsp;
        }
        public static ResponsePackage ExecuteIn1c1(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11014/");
            return rsp;
        }
        public static ResponsePackage ExecuteInFs(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11005/");
            return rsp;
        }
        public static ResponsePackage ExecuteInMail(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11007/");
            return rsp;
        }
        public static ResponsePackage ExecuteSssp(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11008/");
            return rsp;
        }
        public static DataSet Execute(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11002/");
            return rsp.Data;
        }
        public static DataTable GetFirstTable(DataSet ds)
        {
            DataTable dt = null;
            if ((ds != null) && (ds.Tables.Count > 0))
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        public static Object GetScalar(DataSet ds)
        {
            Object r = null;
            DataTable dt = GetFirstTable(ds);
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count >= 0)
            {
                r = dt.Rows[0][0];
            }
            return r;
        }
    }
}