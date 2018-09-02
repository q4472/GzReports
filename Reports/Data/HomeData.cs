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
                rqp.Command = "[dbo].[session_get_by_id]";
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
                rqp.Command = "[dbo].[oc_клиенты_select]";
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
                rqp.Command = "[dbo].[oc_сотрудники_select]";
                rqp.Parameters = new RequestParameter[] {
                new RequestParameter ("DESCR", filter)
            };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataTable GetMnnItems(String[] srcs)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage
                {
                    Command = "[dbo].[мнн]",
                };
                if (srcs != null)
                {
                    rqp.Parameters = new RequestParameter[srcs.Length];
                    for (int i = 0; i < Math.Min(6, srcs.Length); i++)
                    {
                        String name = "src" + (i + 1).ToString();
                        String value = srcs[i];
                        rqp.Parameters[i] = new RequestParameter(name, value);
                    }
                }
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataSet GetTnItems(Guid session_id, String[] srcs, String[] terms, String mnn, Boolean exactly, Boolean eRu, Int32 pageNumber)
            {
                DataSet ds = null;
                RequestPackage rqp = new RequestPackage
                {
                    Command = "[dbo].[тн]"
                };
                Int32 pi = 0;
                if (srcs != null)
                {
                    Int32 len =
                        1 +                                                 // session_id
                        Math.Min(6, ((srcs == null) ? 0 : srcs.Length)) +   // srcs
                        Math.Min(9, ((terms == null) ? 0 : terms.Length)) + // terms
                        ((mnn == null) ? 0 : 1) +                           // mnn
                        1 +                                                 // exactly
                        1 +                                                 // eRu
                        1;                                                  // pageNumber
                    rqp.Parameters = new RequestParameter[len];
                    rqp.Parameters[pi++] = new RequestParameter("session_id", session_id);
                    for (int i = 0; i < Math.Min(6, srcs.Length); i++)
                    {
                        String name = "src" + (i + 1).ToString();
                        String value = srcs[i];
                        rqp.Parameters[pi++] = new RequestParameter(name, value);
                    }
                }
                if (terms != null)
                {
                    for (int i = 0; i < Math.Min(9, terms.Length); i++)
                    {
                        String name = "term" + (i + 1).ToString();
                        String value = terms[i];
                        rqp.Parameters[pi++] = new RequestParameter(name, value);
                    }
                }
                if (mnn != null)
                {
                    rqp.Parameters[pi++] = new RequestParameter("mnn", mnn);
                }
                rqp.Parameters[pi++] = new RequestParameter("exactly", exactly);
                rqp.Parameters[pi++] = new RequestParameter("eRu", eRu);
                rqp.Parameters[pi++] = new RequestParameter("PageNumber", pageNumber);
                ds = Execute(rqp);
                return ds;
            }
            public static DataSet GetTnItemsLP(String rn)
            {
                DataSet ds = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[dbo].[тн_цены]";
                rqp.Parameters = new RequestParameter[1];
                rqp.Parameters[0] = new RequestParameter("rn", rn);
                ds = Execute(rqp);
                return ds;
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
                rqp.Command = "[dbo].[ОтчетПоОплатам]";
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
                rqp.Command = "[dbo].[судебные_документы_upsert]";
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
                rqp.Command = "[dbo].[судебные_документы_отчёт]";
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
                rqp.Command = "[dbo].[судебные_документы_отчёт_по_истории_документа]";
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
                rqp.Command = "[dbo].[судебные_документы_отчёт_по_истории]";
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
                rqp.Command = "[dbo].[судебный_статус_и_email_by_iddoc]";
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
        public class Settings
        {
            public static DataSet Get(Guid sessionId)
            {
                DataSet ds = null;
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = sessionId;
                rqp.Command = "[dbo].[settings_get]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter("session_id", sessionId)
                };
                ds = Execute(rqp);
                return ds;
            }
            public static void UpdateRow(Guid sessionId, Int32 id, String value)
            {
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = sessionId;
                rqp.Command = "[dbo].[settings_update_row]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter("session_id", sessionId),
                    new RequestParameter("id", id),
                    new RequestParameter("value", value)
                };
                Execute(rqp);
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
        public static DataTable GetFsInfoCommon(String fileId, String link, String type = null)
        {
            RequestPackage rqp = new RequestPackage();
            rqp.Command = "[dbo].[file_info_get]";
            rqp.Parameters = new RequestParameter[] {
                new RequestParameter("file_id", fileId ),
                new RequestParameter("link", link ),
                new RequestParameter("type", type )
            };
            DataTable dt = GetFirstTable(Execute(rqp));
            return dt;
        }
    }
}