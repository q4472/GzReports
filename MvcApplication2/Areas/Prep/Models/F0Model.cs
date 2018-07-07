using MvcApplication2.Areas.Env.Models;
using FarmSib.Base.Data;
using Nskd;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using FarmSib.MvcApplication2;

namespace MvcApplication2.Areas.Prep.Models
{
    public class F0Model
    {
        public DataTable FilteredData; // таблица с данными полученная в результате применения фильтра
        public Dictionary<String, String> FilterFields; // словарь с данными по полям для фильтрации
        private void ApplyFilter(RequestPackage rqp)
        {
            rqp.Command = "[dbo].[спецификации_get_by_filter_v2]";
            rqp.SetDBNullValueForNullOrEmptyOrWhiteSpaceParameterValue();
            rqp.AddSessionIdToParameters();
            ResponsePackage rsp = rqp.GetResponse("http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_SQL_PORT_V12 + "/");
            FilteredData = rsp.GetFirstTable();
        }
        public F0Model(String sessionId) // конструктор для захода с другой страницы или из меню
        {
            // пытаемся восстановить значение фильтра из параметров среды
            // результат записываем в параметры запроса на обновление и в список полей для фильтра на странице
            RequestPackage rqp = new RequestPackage
            {
                SessionId = new Guid(sessionId),
                Parameters = new RequestParameter[] {
                    new RequestParameter { Name = "pattern", Value = @"^prep_f0_filter\..*" }
                }
            };
            ResponsePackage rsp = EnvModel.Get(rqp);
            if (rsp.Data != null && rsp.Data.Tables.Count > 0)
            {
                DataTable dt = rsp.Data.Tables[0];
                rqp.Parameters = new RequestParameter[dt.Rows.Count];
                FilterFields = new Dictionary<String, String>();
                Boolean existsParameterType = false;
                Int32 ri = 0;
                for (; ri < dt.Rows.Count; ri++)
                {
                    DataRow dr = dt.Rows[ri];
                    String name = dr["name"] as String;
                    if (!String.IsNullOrWhiteSpace(name))
                    {
                        name = name.Replace("prep_f0_filter.", "");
                        if (name == "type") { existsParameterType = true; }
                        String value = dr["value"] as String;
                        rqp.Parameters[ri] = new RequestParameter { Name = name, Value = value };
                        FilterFields.Add(name, value);
                    }
                }
                if (!existsParameterType)
                {
                    Array.Resize<RequestParameter>(ref rqp.Parameters, (rqp.Parameters.Length + 1));
                    rqp.Parameters[ri] = new RequestParameter { Name = "type", Value = "1" };
                }
            }
            ApplyFilter(rqp);
        }
        public F0Model(RequestPackage rqp) // конструктор для захода с этой страницы с данными о фильтре
        {
            ApplyFilter(rqp);
        }
        public static Int32 CreateSpec(Guid sessionId)
        {
            return HomeData.Order.CreateSpec(sessionId);
        }
    }
    public static class F0Command
    {
        private static void SaveAuctionInf(Guid sessionId, Guid aUid, Int32 specHeadType)
        {
            // добавить строки (по количеству заказчиков) в таблицу "спецификации_шапка" на основании записи в Auctions
            Int32 headType = 1; // 1 - заявка, 2 - спецификация
            HomeData.Prep.AddSpec(sessionId, aUid, headType);

            try
            {
                // добавляем каталог в файловую систему для хранения информации об аукционе
                AddDirectoryToFs(sessionId, aUid);
            }
            catch (Exception) { }
        }
        private static void parseAndSaveCustomerRequirement(Guid sessionId, Guid oUid, XmlNode[] noticeTabBoxWrappers, XmlNode[] noticeBoxExpands, XmlNode[] expandRows)
        {
            // здесь может быть два варианта
            // первый - когда списки expand пусты - сохраняем из основного блока
            // второй - сохраняем из списков

            Object[][] md = null;
            if (expandRows.Length == 0 || noticeBoxExpands.Length == 0)
            {
                md = new Object[][] {
                        // требования_заказчика
                        new Object[] { "наименование_заказчика",                        noticeTabBoxWrappers[1], "./table//tr[1]/td[2]" },
                        // условия_контракта
                        new Object[] { "место_доставки_товара",                         noticeTabBoxWrappers[6], "./table//tr[1]/td[2]" },
                        new Object[] { "сроки_поставки_товара",                         noticeTabBoxWrappers[6], "./table//tr[2]/td[2]" },
                        new Object[] { "сведения_о_связи_с_позицией_плана_графика",     noticeTabBoxWrappers[0], "./table//tr[7]/td[2]" },
                        new Object[] { "оплата_исполнения_контракта_по_годам",          noticeTabBoxWrappers[3], "./div[contains(@class, 'addingTbl')]/table//tr[1]/td[2]" },
                        // обеспечение_заявок
                        new Object[] { "размер_обеспечения",                            noticeTabBoxWrappers[7], "./table//tr[2]/td[2]" },
                        new Object[] { "порядок_внесения_денежных_средств",             noticeTabBoxWrappers[7], "./table//tr[3]/td[2]" },
                        new Object[] { "платежные_реквизиты",                           noticeTabBoxWrappers[7], "./table//tr[4]/td[2]" },
                        // обеспечение_исполнения_контракта
                        new Object[] { "размер_обеспечения_2",                          noticeTabBoxWrappers[8], "./table//tr[2]/td[2]" },
                        new Object[] { "порядок_предоставления_обеспечения",            noticeTabBoxWrappers[8], "./table//tr[3]/td[2]" },
                        new Object[] { "платежные_реквизиты_2",                         noticeTabBoxWrappers[8], "./table//tr[4]/td[2]" }
                    };

                HomeData.Prep.SaveCustomerRequirement(sessionId, oUid, md);
            }
            else // второй вариант - со списком поставщиков
            {
                for (int i = 0; i < noticeBoxExpands.Length; i++)
                {
                    md = new Object[][] {
                            // требования_заказчика
                            new Object[] { "наименование_заказчика",                        noticeBoxExpands[i], ".//h3" },
                            // условия_контракта
                            //new Object[] { "место_доставки_товара",                         noticeTabBoxWrappers[6], "table//tr[1]/td[2]" },
                            //new Object[] { "сроки_поставки_товара",                         noticeTabBoxWrappers[6], "table//tr[2]/td[2]" },
                            new Object[] { "сведения_о_связи_с_позицией_плана_графика",     expandRows[i], ".//div[contains(@class, 'noticeTabBoxWrapper')][1]/table//tr[1]/td[2]" },
                            new Object[] { "оплата_исполнения_контракта_по_годам",          expandRows[i], ".//div[contains(@class, 'noticeTabBoxWrapper')][2]/table//tr[1]/td[2]" },
                            // обеспечение_заявок
                            new Object[] { "размер_обеспечения",                            expandRows[i], ".//div[contains(@class, 'noticeTabBoxWrapper')][3]/table//tr[2]/td[2]" },
                            new Object[] { "порядок_внесения_денежных_средств",             expandRows[i], ".//div[contains(@class, 'noticeTabBoxWrapper')][3]/table//tr[3]/td[2]" },
                            new Object[] { "платежные_реквизиты",                           expandRows[i], ".//div[contains(@class, 'noticeTabBoxWrapper')][3]/table//tr[4]/td[2]" },
                            // обеспечение_исполнения_контракта
                            new Object[] { "размер_обеспечения_2",                          expandRows[i], ".//div[contains(@class, 'noticeTabBoxWrapper')][4]/table//tr[2]/td[2]" },
                            new Object[] { "порядок_предоставления_обеспечения",            expandRows[i], ".//div[contains(@class, 'noticeTabBoxWrapper')][4]/table//tr[3]/td[2]" },
                            new Object[] { "платежные_реквизиты_2",                         expandRows[i], ".//div[contains(@class, 'noticeTabBoxWrapper')][4]/table//tr[4]/td[2]" }
                        };
                    HomeData.Prep.SaveCustomerRequirement(sessionId, oUid, md);
                }
            }
        }
        private static void AddDirectoryToFs(Guid sessionId, Guid oUid)
        {
            HomeData.Prep.AddContractDirectory(sessionId, oUid);
        }
        public static Boolean ExistsAuctionInf(Guid sessionId, String auctionNumber)
        {
            Boolean r = HomeData.Prep.ExistsAuctionInf(sessionId, auctionNumber);
            return r;
        }
        public static String LoadAuctionInf(Guid sessionId, String auctionNumber, Int32 specHeadType)
        {
            Guid? aUid = null;
            if (!String.IsNullOrWhiteSpace(auctionNumber))
            {
                aUid = LoadAuction(sessionId, auctionNumber, true);
                if (aUid != null)
                {
                    // сохранить данные об аукционе
                    SaveAuctionInf(sessionId, (Guid)aUid, specHeadType);
                }
            }
            return ((aUid == null) ? "" : aUid.ToString());
        }
        public static String LoadByRegion(Guid sessionId, String regionNumber, String publishDate)
        {
            RequestPackage rqp = new RequestPackage
            {
                SessionId = sessionId,
                Command = "LoadAuctionNumbers",
                Parameters = new RequestParameter[]
                {
                            new RequestParameter { Name = "region_number", Value = regionNumber },
                            new RequestParameter { Name = "publish_date", Value = publishDate }
                }
            };

            ResponsePackage rsp = null;
            try
            {
                rsp = rqp.GetResponse("http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_INET_PORT + "/");
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            List<String> auctionNumbers = new List<String>(); //ZakupkiGovRu.GetAuctionNumbersByRegion(regionNumber, publishDate);

            if (rsp != null && rsp.Data != null && rsp.Data.Tables.Count > 0)
            {
                DataTable dt = rsp.Data.Tables[0];
                if (dt.Columns.Count > 0 && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        auctionNumbers.Add(dr[0] as String);
                    }
                }
            }

            auctionNumbers.Add(sessionId.ToString());
            Thread lbns = new Thread(ThreadLoadByNumbers);
            lbns.Start(auctionNumbers);

            return String.Format("Найдено аукционов: {0}.", auctionNumbers.Count - 1);
        }
        public static void ThreadLoadByNumbers(Object auctionNumbers)
        {
            List<String> ans = (List<String>)auctionNumbers;
            Guid sessionId = new Guid(ans[ans.Count - 1]);
            ans.RemoveAt(ans.Count - 1);
            foreach (String an in ans)
            {
                LoadAuctionInf(sessionId, an, 1);
                Thread.Sleep(10000);
            }
        }
        public static String PassToTender(Guid sessionId, String auctionNumber)
        {
            String r = HomeData.Prep.PassToTender(sessionId, auctionNumber);
            return r;
        }
        private static Guid? LoadAuction(Guid sessionId, String auctionNumber, Boolean overwrite)
        {
            Guid? aUid = null;

            RequestPackage rqp = new RequestPackage
            {
                SessionId = sessionId,
                Command = "LoadAuction",
                Parameters = new RequestParameter[]
                {
                    new RequestParameter { Name = "auction_number", Value = auctionNumber },
                    new RequestParameter { Name = "overwrite", Value = overwrite }
                }
            };

            ResponsePackage rsp = null;
            try
            {
                rsp = rqp.GetResponse("http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_INET_PORT + "/");
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            if (rsp != null &&
                rsp.Data != null &&
                rsp.Data.Tables.Count > 0 &&
                rsp.Data.Tables[0].Columns.Count > 0 &&
                rsp.Data.Tables[0].Rows.Count > 0)
            {
                Object o = rsp.Data.Tables[0].Rows[0][0];
                if (o.GetType() == typeof(Guid))
                {
                    aUid = (Guid)o;
                }
            }

            return aUid;
        }
    }
}
