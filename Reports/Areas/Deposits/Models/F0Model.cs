using FarmSib.Base.Data;
using FarmSib.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MvcApplication2.Areas.Deposits.Models
{
    public class F0Model
    {
        public Detail DetailData;
        public class Detail
        {
            public Int32 CustId;
            public String CustInn;
            public String CustKpp;
            public String CustDescr;
            public String CustFullDescr;
            public String CustAddress;
            public DataGridView TurnoverByTradeNumber;
        }
        public static DataGridView GetFiltredData(String sd, String ed, String cust, String trNum)
        {
            DataGridView filtredData = new DataGridView();

            filtredData.Columns.Add(new DataGridColumn() { Caption = "Контрагент_id", DataType = typeof(String), HeaderCellStyle = "display: none;", DataCellStyle = "display: none;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Контрагент", DataType = typeof(String) });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Долг на начало периода", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Обеспечение", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Возврат", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Долг на конец периода", DataType = typeof(String), DataCellStyle = "text-align: right;" });

            DataTable dt = HomeData.Deposits.F0.GetCustTotals(sd, ed, cust, trNum);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataGridRow row = filtredData.NewRow();
                    filtredData.Rows.Add(row);
                    row[0] = dr["РегистраторКонтрагент_id"];
                    row[1] = dr["КонтрагентНаименование"];
                    row[2] = ((Double)dr["Долг на начало периода"]).ToString("n2");
                    row[3] = ((Double)dr["Обеспечение"]).ToString("n2");
                    row[4] = ((Double)dr["Возврат"]).ToString("n2");
                    row[5] = ((Double)dr["Долг на конец периода"]).ToString("n2");
                }
            }

            return filtredData;
        }
        public static DataTable GetCustData(Int32 id)
        {
            DataTable dt = HomeData.Deposits.F0.GetCustData(id);
            return dt;
        }
        public static DataGridView GetTurnoverByTradeNumber(Int32 id, Boolean flag11, Boolean flag12, Boolean flag21, Boolean flag22, Boolean flag31, Boolean flag32)
        {
            DataGridView filtredData = new DataGridView();

            filtredData.Columns.Add(new DataGridColumn() { Caption = "Номер торгов", DataType = typeof(String) });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Обеспечение", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Возврат", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Долг", DataType = typeof(Double), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Код договора", DataType = typeof(String) });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Номер договора", DataType = typeof(String) });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Дата договора", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Сумма по договору", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Сумма по накладным", DataType = typeof(String), DataCellStyle = "text-align: right;" });

            DataTable dt = HomeData.Deposits.F0.GetTradeTotals(id, flag11, flag12, flag21, flag22, flag31, flag32);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataGridRow row = filtredData.NewRow();
                    filtredData.Rows.Add(row);
                    int i = 0;
                    row[i++] = dr["НомерТоргов"];
                    //row[i++] = dr["Долг на начало периода"];
                    row[i++] = ((Double)dr["Обеспечение"]).ToString("n2");
                    row[i++] = ((Double)dr["Возврат"]).ToString("n2");
                    row[i++] = ((Double)dr["Долг на конец периода"]).ToString("n2");
                    row[i++] = dr["ДоговорКод"];
                    row[i++] = dr["ДоговорНомер"];
                    row[i++] = ((DateTime)dr["ДоговорДата"]).ToString("yyyy-MM-dd");
                    row[i++] = ((Double)dr["ДоговорСумма"]).ToString("n2");
                    row[i++] = ((Double)dr["СуммаПоНакладным"]).ToString("n2");
                }
            }

            return filtredData;
        }
        public static DataGridView GetEntriesData(Int32 id, String trNum)
        {
            DataGridView filtredData = new DataGridView();

            filtredData.Columns.Add(new DataGridColumn() { Caption = "Дата", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Обеспечение", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Возврат", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Документ", DataType = typeof(String) });
            filtredData.Columns.Add(new DataGridColumn() { Caption = "Назначение", DataType = typeof(String) });

            DataTable dt = HomeData.Deposits.F0.GetEntriesData(id, trNum);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataGridRow row = filtredData.NewRow();
                    filtredData.Rows.Add(row);
                    Double o = (Double)dr["Обеспечение"];
                    Double v = (Double)dr["Возврат"];
                    int i = 0;
                    row[i++] = ((DateTime)dr["Дата"]).ToString("yyyy-MM-dd");
                    row[i++] = ((o == 0) ? "" : o.ToString("n2"));
                    row[i++] = ((v == 0) ? "" : v.ToString("n2"));
                    row[i++] = dr["РегистраторПредставление"];
                    row[i++] = dr["РегистраторНазначениеПлатежа"];
                }
            }

            return filtredData;
        }
        public void GetDetailData(Int32 id, Boolean flag11, Boolean flag12, Boolean flag21, Boolean flag22, Boolean flag31, Boolean flag32)
        {
            DetailData = new Detail();

            DataTable dt = GetCustData(id);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DetailData.CustId = id;
                DetailData.CustInn = dt.Rows[0]["ИНН"] as String;
                DetailData.CustKpp = dt.Rows[0]["КПП"] as String;
                DetailData.CustDescr = dt.Rows[0]["Наименование"] as String;
                DetailData.CustFullDescr = dt.Rows[0]["НаименованиеПолное"] as String;
                DetailData.CustAddress = dt.Rows[0]["Адрес"] as String;
            }
            DetailData.TurnoverByTradeNumber = GetTurnoverByTradeNumber(id, flag11, flag12, flag21, flag22, flag31, flag32);
        }
    }
}