using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FarmSib.Base.Data;
using FarmSib.Base.Models;

namespace MvcApplication2.Areas.Reports.Models
{
    public class R3Model
    {
        private DataTable dt;

        public DataGridView FilteredView { get; set; }

        public DataGridView CrossView { get; set; }

        public List<StageSelector> FilterStageSelector { get; set; }

        public R3Model()
        {
            dt = HomeData.Reports.GetR3DataList();

            FillFilterStageSelector();

            FillFilteredView();
            
            CrossView = new DataGridView();
            FillCrossView();
        }

        public R3Model(List<String> fs, List<Int32> sf)
        {
            dt = HomeData.Reports.GetR3DataList();
            ApplyFilter(fs, sf);

            FillFilterStageSelector();

            FillFilteredView();
        }

        public R3Model(DateTime d1, DateTime d2)
        {
            dt = HomeData.Reports.GetR3DataList(d1, d2);

            FillFilterStageSelector();

            FillFilteredView();
        }

        public class StageSelector
        {
            public String Name;
            public Int32 Value;
            public Boolean IsSelected;
            public Int32 Count;
            public Decimal Sum1;
            public Decimal Sum2;
            public StageSelector()
            {
                Name = null;
                Value = 0;
                IsSelected = true;
                Count = 0;
                Sum1 = 0;
                Sum2 = 0;
            }
        }

        private void ApplyFilter(List<String> fs, List<Int32> sf)
        {
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                int ri = 0;
                while (ri < dt.Rows.Count)
                {
                    DataRow dr = dt.Rows[ri];
                    Decimal s1 = (Decimal)dr["s1"];
                    Decimal s2 = (Decimal)dr["s2"];
                    String ds2 = (String)dr["d2"];
                    if (!((String)dr["agreement_descr"]).Contains(fs[0])) { dt.Rows.Remove(dr); continue; }
                    if (!((String)dr["cust_descr"]).Contains(fs[1])) { dt.Rows.Remove(dr); continue; }
                    if (!((String)dr["docno"] + " " + (String)dr["doc_date"]).Contains(fs[2])) { dt.Rows.Remove(dr); continue; }
                    if (!((String)dr["stage_descr"]).Contains(fs[3])) { dt.Rows.Remove(dr); continue; }
                    if (!((String)dr["sud_set_of_docs_descr"]).Contains(fs[4])) { dt.Rows.Remove(dr); continue; }
                    if (!((String)dr["sud_note"]).Contains(fs[5])) { dt.Rows.Remove(dr); continue; }
                    if (!(s1.ToString("n2")).Contains(fs[6])) { dt.Rows.Remove(dr); continue; }
                    if (!(s2.ToString("n2") + ((ds2 == "") ? "" : " на " + ds2)).Contains(fs[7])) { dt.Rows.Remove(dr); continue; }
                    Boolean result = false;
                    if ((sf != null) && (sf.Count > 0))
                    {
                        Int32 id = (Int32)dr["stage_id"];
                        foreach (Int32 v in sf)
                        {
                            result = (v == id);
                            if (result) break;
                        }
                    }
                    if (!result) { dt.Rows.Remove(dr); continue; }
                    ri++;
                }
            }
        }

        private void FillFilterStageSelector()
        {
            FilterStageSelector = new List<StageSelector>();

            FilterStageSelector.Add(new StageSelector { Name = "не судебные", Value = 0 });
            FilterStageSelector.Add(new StageSelector { Name = "возбуждение", Value = 1 });
            FilterStageSelector.Add(new StageSelector { Name = "подготовка", Value = 2 });
            FilterStageSelector.Add(new StageSelector { Name = "разбирательство", Value = 3 });
            FilterStageSelector.Add(new StageSelector { Name = "апелляция", Value = 4 });
            FilterStageSelector.Add(new StageSelector { Name = "кассация", Value = 5 });
            FilterStageSelector.Add(new StageSelector { Name = "надзор", Value = 6 });
            FilterStageSelector.Add(new StageSelector { Name = "пересмотр", Value = 7 });
            FilterStageSelector.Add(new StageSelector { Name = "исполнение", Value = 8 });
            FilterStageSelector.Add(new StageSelector { Name = "завершено", Value = 9 });
            foreach (DataRow dr in dt.Rows)
            {
                Int32 stage_id = (Int32)dr["stage_id"];
                if ((stage_id >= 0) && (stage_id <= 9))
                {
                    FilterStageSelector[stage_id].Count++;
                    FilterStageSelector[stage_id].Sum1 += (Decimal)dr["s1"];
                    FilterStageSelector[stage_id].Sum2 += (Decimal)dr["s2"];
                }
            }
            FilterStageSelector = FilterStageSelector.FindAll(r => (r.Count > 0));
        }

        private void FillFilteredView()
        {
            FilteredView = new DataGridView();

            FilteredView.Columns.Add(new DataGridColumn { Caption = "Договор", DataType = typeof(String), IsUsedInFilter = true });
            FilteredView.Columns.Add(new DataGridColumn { Caption = "Клиент", DataType = typeof(String), IsUsedInFilter = true });
            FilteredView.Columns.Add(new DataGridColumn { Caption = "Накладная", DataType = typeof(String), IsUsedInFilter = true });
            FilteredView.Columns.Add(new DataGridColumn { Caption = "Стадия производства", DataType = typeof(String), IsUsedInFilter = true });
            FilteredView.Columns.Add(new DataGridColumn { Caption = "Комплектация документов", DataType = typeof(String), IsUsedInFilter = true });
            FilteredView.Columns.Add(new DataGridColumn { Caption = "Комментарий", DataType = typeof(String), IsUsedInFilter = true });
            FilteredView.Columns.Add(new DataGridColumn { Caption = "Сумма по документу", DataType = typeof(String), IsUsedInFilter = true, DataCellStyle = "text-align: right;" });
            FilteredView.Columns.Add(new DataGridColumn { Caption = "Сумма по выпискам", DataType = typeof(String), IsUsedInFilter = true, DataCellStyle = "text-align: right;" });
            FilteredView.Columns.Add(new DataGridColumn { Caption = "История", DataType = typeof(String), IsUsedInFilter = false });

            Decimal s1 = 0;
            Decimal s2 = 0;
            String ds2;
            Decimal sum1 = 0;
            Decimal sum2 = 0;
            Int32 agrCount = 0;
            String agrOldId = String.Empty;
            if (dt != null)
            {
                DataGridRow r;
                foreach (DataRow dr in dt.Rows)
                {
                    s1 = (Decimal)dr["s1"];
                    s2 = (Decimal)dr["s2"];
                    ds2 = (String)dr["d2"];
                    sum1 += s1;
                    sum2 += s2;
                    String agrId = (String)dr["agreement_id"];
                    if (agrId != agrOldId)
                    {
                        agrCount++;
                        agrOldId = agrId;
                    }
                    r = FilteredView.NewRow();
                    r.Attributes.Add("data-sud-iddoc", (String)dr["iddoc"]);
                    r.Attributes.Add("data-sud-docno", (String)dr["docno"]);
                    r.Attributes.Add("data-sud-agreement-id", agrId);
                    r.Attributes.Add("data-sud-agreement-descr", (String)dr["agreement_descr"]);
                    r.Attributes.Add("data-sud-status-id", ((Int32)dr["status_id"]).ToString());
                    r.Attributes.Add("data-sud-stage-id", ((Int32)dr["stage_id"]).ToString());
                    r.Attributes.Add("data-sud-stage-descr", (String)dr["stage_descr"]);
                    r.Attributes.Add("data-sud-set-of-docs-id", ((Int32)dr["set_of_docs_id"]).ToString());
                    r.Attributes.Add("data-sud-payments-status-id", ((Int32)dr["payments_status_id"]).ToString());
                    if (dr.Table.Columns.Contains("Срок оплаты установленный вручную"))
                    {
                        r.Attributes.Add("data-sud-payments-date", ((String)dr["Срок оплаты установленный вручную"]));
                    } else
                    {
                        r.Attributes.Add("data-sud-payments-date", "");
                    }
                    r.Attributes.Add("data-sud-note", (String)dr["sud_note"]);
                    r[0] = ((String)dr["agreement_descr"]);
                    r[1] = ((String)dr["cust_descr"]);
                    r[2] = ((String)dr["docno"] + " " + (String)dr["doc_date"]);
                    r[3] = ((String)dr["stage_descr"]);
                    r[4] = ((String)dr["sud_set_of_docs_descr"]);
                    r[5] = ((String)dr["sud_note"]);
                    r[6] = (s1.ToString("n2"));
                    r[7] = (s2.ToString("n2") + ((ds2 == "") ? "" : " на " + ds2));
                    r[8] = (((String)dr["short_history"]).ToString());
                    if (((Decimal)dr["s1"]) == ((Decimal)dr["s2"]))
                    {
                        r.Style = "background-color: #cfc;";
                    }
                    FilteredView.Rows.Add(r);
                }
                r = FilteredView.NewRow();
                r[0] = "Всего договоров: " + agrCount.ToString();
                r[1] = String.Empty;
                r[2] = String.Empty;
                r[3] = String.Empty;
                r[4] = String.Empty;
                r[5] = String.Empty;
                r[6] = (sum1.ToString("n2"));
                r[7] = (sum2.ToString("n2"));
                r[8] = String.Empty;
                r.Style = "font-weight: bold;";
                FilteredView.Rows.Add(r);
            }
        }

        public class History
        {
            public DataGridView HistoryView { get; set; }

            public History(String iddoc)
            {
                HistoryView = new DataGridView();
                HistoryView.Columns.Add(new DataGridColumn { Caption = "Дата", DataType = typeof(String) });
                HistoryView.Columns.Add(new DataGridColumn { Caption = "Операция", DataType = typeof(String) });
                HistoryView.Columns.Add(new DataGridColumn { Caption = "Автор", DataType = typeof(String) });
                HistoryView.Columns.Add(new DataGridColumn { Caption = "Статус", DataType = typeof(String) });
                HistoryView.Columns.Add(new DataGridColumn { Caption = "Стадия", DataType = typeof(String) });
                HistoryView.Columns.Add(new DataGridColumn { Caption = "Комплектность", DataType = typeof(String) });
                HistoryView.Columns.Add(new DataGridColumn { Caption = "Оплата", DataType = typeof(String) });
                HistoryView.Columns.Add(new DataGridColumn { Caption = "Комментарий", DataType = typeof(String) });

                DataTable hdt = HomeData.Reports.GetR3HistoryList(iddoc);
                if ((hdt != null) && (hdt.Rows.Count > 0))
                {
                    foreach (DataRow dr in hdt.Rows)
                    {
                        DataGridRow r = HistoryView.NewRow();
                        r[0] = (((DateTime)dr["audit_datetime"]).ToString("yyyy-MM-dd HH:mm"));
                        r[1] = ((String)dr["audit_operation"]);
                        r[2] = ((String)dr["user_name"]);
                        r[3] = ((String)dr["status_descr"]);
                        r[4] = ((String)dr["stage_descr"]);
                        r[5] = (((Int32)dr["set_of_docs_id"]).ToString());
                        r[6] = (((Int32)dr["payments_status_id"]).ToString());
                        r[7] = ((String)dr["note"]);
                        HistoryView.Rows.Add(r);
                    }
                }
            }
        }

        public void FillCrossView()
        {
            CrossView.Columns.Add(new DataGridColumn { Caption = "Дата", DataType = typeof(String), IsUsedInFilter = false });
            CrossView.Columns.Add(new DataGridColumn { Caption = "Не судебный", DataType = typeof(String), IsUsedInFilter = false });
            //CrossView.Columns.Add(new DataGridColumn { Caption = "Возбуждение", DataType = typeof(String), IsUsedInFilter = false });
            CrossView.Columns.Add(new DataGridColumn { Caption = "Подготовка", DataType = typeof(String), IsUsedInFilter = false });
            CrossView.Columns.Add(new DataGridColumn { Caption = "Разбирательство", DataType = typeof(String), IsUsedInFilter = false });
            //CrossView.Columns.Add(new DataGridColumn { Caption = "Апелляция", DataType = typeof(String), IsUsedInFilter = false });
            //CrossView.Columns.Add(new DataGridColumn { Caption = "Кассация", DataType = typeof(String), IsUsedInFilter = false });
            //CrossView.Columns.Add(new DataGridColumn { Caption = "Надзор", DataType = typeof(String), IsUsedInFilter = false });
            //CrossView.Columns.Add(new DataGridColumn { Caption = "Пересмотр", DataType = typeof(String), IsUsedInFilter = false });
            CrossView.Columns.Add(new DataGridColumn { Caption = "Исполнение", DataType = typeof(String), IsUsedInFilter = false });
            CrossView.Columns.Add(new DataGridColumn { Caption = "Завершено", DataType = typeof(String), IsUsedInFilter = false });

            DataTable cdt = HomeData.Reports.GetR3CrossTable();
            if ((cdt != null) && (cdt.Rows.Count > 0))
            {
                foreach (DataRow dr in cdt.Rows)
                {
                    DataGridRow r = CrossView.NewRow();
                    //r.Attributes.Add("data-sud-iddoc", (String)dr["iddoc"]);
                    r[0] = (((DateTime)dr["date"]).ToString("yyyy-MM-dd"));
                    r[1] = (((Int32)dr["0"]).ToString());
                    //r.Cells.Add(((Int32)dr["1"]).ToString());
                    r[2] = (((Int32)dr["2"]).ToString());
                    r[3] = (((Int32)dr["3"]).ToString());
                    //r.Cells.Add(((Int32)dr["4"]).ToString());
                    //r.Cells.Add(((Int32)dr["5"]).ToString());
                    //r.Cells.Add(((Int32)dr["6"]).ToString());
                    //r.Cells.Add(((Int32)dr["7"]).ToString());
                    r[4] = (((Int32)dr["8"]).ToString());
                    r[5] = (((Int32)dr["9"]).ToString());
                    CrossView.Rows.Add(r);
                }
            }
        }
    }
}
