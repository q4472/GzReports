using FarmSib.Base.Data;
using FarmSib.MvcApplication2;
using Nskd;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcApplication2.Areas.Prep.Models
{
    public class F1Model
    {
        public Int32 Id;
        public DataTable Head_dt;
        public DataTable Table_dt;
        public DataTable Shedule_dt;
        public String SpecDataSetAsJsonString;
        public String TnViewsSrcSelectorHtml;
        public String TnViewsMnnSelectorHtml;
        public String TnViewsTnSelectorHtml;

        public F1Model(Guid sessionId, Int32 specId, Uri requestUri = null)
        {
            Id = specId;
            // загружаем таблицы
            if (specId != 0)
            {
                DataSet ds = HomeData.Spec.GetById(sessionId, specId);
                SpecDataSetAsJsonString = Nskd.JsonV2.ToString(ds);
                if ((ds != null) && (ds.Tables.Count > 0))
                {
                    Head_dt = ds.Tables[0];
                    if (ds.Tables.Count > 1)
                    {
                        Table_dt = ds.Tables[1];
                        if (ds.Tables.Count > 2)
                        {
                            Shedule_dt = ds.Tables[2];
                        }
                    }
                }
            }
            // загружаем html
            if (requestUri != null)
            {
                // надо указать порт на котором proxy, а это у меня или 80 или 8080
                // как выбирать при публикации или автоматически пока не знаю. поэтому менять руками !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                String proxyPort = "80";
#if DEBUG
                proxyPort = "8080";
#endif
                String shp = requestUri.Scheme + "://" + requestUri.Host + ":" + proxyPort; // + requestUri.Port.ToString();
                TnViewsSrcSelectorHtml = GetResponse(shp + "/Tn/SrcSelector");
                TnViewsMnnSelectorHtml = GetResponse(shp + "/Tn/MnnSelector");
                TnViewsTnSelectorHtml = GetResponse(shp + "/Tn/TnSelector");
            }
        }
        private String GetResponse(String requestUriString)
        {
            String result = String.Empty;
            if (!String.IsNullOrWhiteSpace(requestUriString))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
                request.Method = "POST";
                request.ContentLength = 0;
                HttpWebResponse response = null;
                try { response = (HttpWebResponse)request.GetResponse(); }
                catch (Exception e) { result = e.Message + "\nrequestUriString: " + requestUriString; }
                if ((response != null) && (response.StatusCode == HttpStatusCode.OK))
                {
                    Stream receivedStream = response.GetResponseStream();
                    Encoding encoding = Encoding.GetEncoding(response.CharacterSet);
                    using (StreamReader readStream = new StreamReader(receivedStream, encoding))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }
            return result;
        }
        private static void AddSheduleColumns()
        {
            //Shedule.Columns.Add(new DataGridColumn { Caption = "id", ColumnName = "id", DataType = typeof(Int32) });
            //Shedule.Columns.Add(new DataGridColumn { Caption = "код_спецификации", ColumnName = "код_спецификации", DataType = typeof(Int32) });
            //Shedule.Columns.Add(new DataGridColumn { Caption = "tp_id", ColumnName = "tp_id", DataType = typeof(Guid) });
            //Shedule.Columns.Add(new DataGridColumn { Caption = "дата", ColumnName = "дата", DataType = typeof(DateTime) });
            //Shedule.Columns.Add(new DataGridColumn { Caption = "количество", ColumnName = "количество", DataType = typeof(Double) });
        }
        private static void AddSheduleRows(NameValueCollection nvc)
        {
            List<DateTime> dates = new List<DateTime>();
            Int32 c = 2; // колонки 0 и 1 пропускаем они скрыты. это номер_спецификации и tp_id
            String name = "c" + c++.ToString();
            while (nvc.AllKeys.Contains(name))
            {
                dates.Add(DateTime.Parse(nvc[name]));
                name = "c" + c++.ToString();
            }
            for (int ri = 0; true; ri++)
            {
                Int32 spec_id = 0;
                name = "Shedule.Rows[" + ri.ToString() + "][0]";
                if (nvc.AllKeys.Contains(name)) { spec_id = Int32.Parse(nvc[name]); }

                Guid tp_id = new Guid();
                name = "Shedule.Rows[" + ri.ToString() + "][1]";
                if (nvc.AllKeys.Contains(name)) { tp_id = Guid.Parse(nvc[name]); }

                for (int di = 0; di < dates.Count; di++)
                {
                    name = "Shedule.Rows[" + ri.ToString() + "][" + (di + 2).ToString() + "]";
                    if (nvc.AllKeys.Contains(name))
                    {
                        /*
                        DataGridRow r = Shedule.NewRow();
                        //r[0] = identity
                        r[1] = spec_id;
                        r[2] = tp_id;
                        r[3] = dates[di];
                        r[4] = nvc[name];
                        Shedule.Rows.Add(r);
                        */
                    }
                    else { return; }
                }
            }
        }
        public static String SaveTableV2(RequestPackage rqp)
        {
            // сюда заходим с пакетом в параметрах которого представлены строки таблицы
            // в формате [row_index][column_name]
            String msg = "MvcApplication2.Areas.Order.Models.F1Model.SaveTableV2(): \n";

            if (rqp == null) { throw new NullReferenceException(); }
            if (rqp.Parameters == null || rqp.Parameters.Length == 0) { return msg + "\tНет параметров для разбора.\n"; }

            // надо сформировать запросы на сохранение для каждой строки
            // начинаем со строки с индексом 0
            // заканчиваем когда не найдётся параметра со следующим индексом строки
            Int32 ri = 0;
            for (; ; ri++)
            {

                // создаём новый пакет для очередной строки (пока пустой)
                RequestPackage rqpR = new RequestPackage
                {
                    SessionId = rqp.SessionId,
                    Command = "[Pharm-Sib].[dbo].[спецификации_таблица_строка_исполнить]",
                    Parameters = new RequestParameter[0]
                };

                // среди всех параметров исходного пакета ищем праметры для этой строки
                // и копируем их в новый пакет
                Regex re = new Regex(String.Format(@"\[{0}\]\[(\w+)\]", ri));

                foreach (RequestParameter p in rqp.Parameters)
                {
                    GroupCollection groups = re.Match(p.Name).Groups;
                    if (groups.Count == 2)
                    {
                        Array.Resize<RequestParameter>(ref rqpR.Parameters, rqpR.Parameters.Length + 1);
                        rqpR.Parameters[rqpR.Parameters.Length - 1] = new RequestParameter { Name = groups[1].Value, Value = p.Value };
                    }
                }

                // если ничего не нашли, то заканчиваем цикл
                if (rqpR.Parameters.Length == 0) { break; }
                // если что-то есть - исполнить
                else
                {
                    rqpR.AddSessionIdToParameters();
                    rqpR.ConvertParametersToSqlCompatibleType(specTableRowStoredProcFields);
                    ResponsePackage rsp = rqpR.GetResponse("http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_SQL_PORT_V12 + "/");
                    msg += rsp.Status;
                }
            }
            return msg;
        }
        public static void SaveShedule(NameValueCollection nvc)
        {
            Int32 id = 0;
            Int32.TryParse(nvc["id"], out id);
            //Id = id;
            //Table = new DataGridView();
            //Shedule = new DataGridView();
            if (id != 0)
            {
                //SpecHead head = new SpecHead();
                //DataTable dt = null; //Head.ToDataTable();
                //Data.HomeData.Prep.F1UpdateHeadFromShedule(id, dt);

                AddSheduleColumns();
                AddSheduleRows(nvc);
                //DataTable shedule = Shedule.ToDataTable();
                //Data.HomeData.Prep.F1SaveShedule(id, shedule);
            }
        }
        private static RequestPackage.MdSqlParameter[] specTableRowStoredProcFields = new RequestPackage.MdSqlParameter[]
            {
                new RequestPackage.MdSqlParameter { Name = "session_id", Type = SqlDbType.UniqueIdentifier },
                new RequestPackage.MdSqlParameter { Name = "uid", Type = SqlDbType.UniqueIdentifier },
                new RequestPackage.MdSqlParameter { Name = "id", Type = SqlDbType.Int },
                new RequestPackage.MdSqlParameter { Name = "код_спецификации", Type = SqlDbType.Int },
                new RequestPackage.MdSqlParameter { Name = "номер_строки", Type = SqlDbType.Int },
                new RequestPackage.MdSqlParameter { Name = "международное_непатентованное_наименование", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "наименование", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "лекарственная_форма_и_дозировка", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "производитель", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "страна", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "ед_изм", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "начальная_максимальная_цена", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "рег_цена", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "количество", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "цена_закуп", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "цена_по_спецификации", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "tp_id", Type = SqlDbType.UniqueIdentifier },
                new RequestPackage.MdSqlParameter { Name = "номер_ру", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "требование", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "примечание", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "количество_в_требовании", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "ед_изм_в_требовании", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "вес", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "объём", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "bg_color", Type = SqlDbType.NVarChar },
                new RequestPackage.MdSqlParameter { Name = "предельная_оптовая_цена", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "предельная_розничная_цена", Type = SqlDbType.Decimal },

                new RequestPackage.MdSqlParameter { Name = "сумма_закуп", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "сумма_по_спецификации", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "предельная_оптовая_сумма", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "предельная_розничная_сумма", Type = SqlDbType.Decimal },

                new RequestPackage.MdSqlParameter { Name = "status", Type = SqlDbType.NVarChar },

                // данные для формирования 'tp'
                new RequestPackage.MdSqlParameter { Name = "gr_item_id", Type = SqlDbType.Int },
                new RequestPackage.MdSqlParameter { Name = "gr_mnft_id", Type = SqlDbType.Int },
                new RequestPackage.MdSqlParameter { Name = "fs_item_code", Type = SqlDbType.Int },

                new RequestPackage.MdSqlParameter { Name = "цеховая_упаковка_вес", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "цеховая_упаковка_объём", Type = SqlDbType.Decimal },
                new RequestPackage.MdSqlParameter { Name = "цеховая_упаковка_количество", Type = SqlDbType.Decimal }
            };
    }
}
