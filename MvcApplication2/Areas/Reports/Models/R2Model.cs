using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using FarmSib.Base.Models;
using FarmSib.Base.Data;
using System.Text;

namespace MvcApplication2.Areas.Reports.Models
{
    public class R2Model
    {
        public ManagerData Emploee { get; set; }
        public Boolean Marketer { get; set; }
        public Boolean Manager { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public Boolean IsSudChecked { get; set; }
        public Boolean IsOrdChecked { get; set; }
        public DataTable ReportData { get; set; }

        public R2Model()
        {
            DateTime now = DateTime.Now;
            //Int32 y = (now.Month == 1) ? (now.Year - 1) : now.Year;
            Emploee = new ManagerData();
            Marketer = true;
            Manager = true;
            //BeginDate = new DateTime(y, ((now.Month == 1) ? 12 : now.Month - 1), 1);
            //EndDate = (new DateTime(y, now.Month, 1)).AddDays(-1);
            BeginDate = now.AddDays(-1);
            EndDate = BeginDate;
            IsSudChecked = true;
            IsOrdChecked = true;
            ReportData = null;
        }
        public void GetReport()
        {
            StringBuilder sb = new StringBuilder();
            if ((Emploee != null) && (Emploee.ManagerMultiSelector != null) && (Emploee.ManagerMultiSelector.Length > 0))
            {
                foreach (String s in Emploee.ManagerMultiSelector)
                {
                    sb.Append("'");
                    sb.Append(s);
                    sb.Append("', ");
                }
                sb.Length -= 2;
            }
            String managerMultiCode = sb.ToString();
            if (String.IsNullOrWhiteSpace(managerMultiCode)) { managerMultiCode = null; }

            DataTable dt = HomeData.Reports.GetReport2(
                Emploee.ManagerFilter,
                Emploee.ManagerSelector,
                managerMultiCode,
                Marketer,
                Manager,
                BeginDate,
                EndDate,
                IsSudChecked,
                IsOrdChecked
                );

            dt.Columns.Add("level", typeof(Int32));

            String g0Name = String.Empty;
            DataRow g0Row = null;

            String g1Name = String.Empty;
            DataRow g1Row = null;

            String g2Name = String.Empty;
            DataRow g2Row = null;

            Int32 ri = 0;

            g0Name = "Общий итог";
            g0Row = dt.NewRow();
            g0Row["Маркетолог"] = g0Name;
            g0Row["Менеджер"] = String.Empty;
            g0Row["Выписка"] = String.Empty;
            g0Row["Счет/Реализация"] = String.Empty;
            g0Row["Клиент"] = String.Empty;
            g0Row["Сумма оплаты"] = 0;
            g0Row["Наценка"] = 0;
            g0Row["НДС"] = 0;
            g0Row["Транспортные расходы"] = 0;
            g0Row["Коммерческие расходы"] = 0;
            g0Row["Прибыль"] = 0;
            g0Row["level"] = 0;
            dt.Rows.InsertAt(g0Row, ri++);

            for (; ri < dt.Rows.Count; ri++)
            {
                DataRow dr = dt.Rows[ri];

                if (g1Name != (String)dr["Маркетолог"]) // начинаем нового Маркетолога
                {
                    g1Name = (String)dr["Маркетолог"];
                    g1Row = dt.NewRow();
                    g1Row["Маркетолог"] = g1Name;
                    g1Row["Менеджер"] = String.Empty;
                    g1Row["Выписка"] = String.Empty;
                    g1Row["Счет/Реализация"] = String.Empty;
                    g1Row["Клиент"] = String.Empty;
                    g1Row["Сумма оплаты"] = 0;
                    g1Row["Наценка"] = 0;
                    g1Row["НДС"] = 0;
                    g1Row["Транспортные расходы"] = 0;
                    g1Row["Коммерческие расходы"] = 0;
                    g1Row["Прибыль"] = 0;
                    g1Row["level"] = 1;
                    dt.Rows.InsertAt(g1Row, ri++); // вставить строку итогов для Маркетолога
                    dr = dt.Rows[ri]; // рабочую строку оставить прежней

                    g2Name = String.Empty;
                    g2Row = null;
                }
                if (g2Name != (String)dr["Менеджер"]) // начинаем нового Менеджера
                {
                    g2Name = (String)dr["Менеджер"];
                    g2Row = dt.NewRow();
                    g2Row["Маркетолог"] = String.Empty;
                    g2Row["Менеджер"] = g2Name;
                    g2Row["Выписка"] = String.Empty;
                    g2Row["Счет/Реализация"] = String.Empty;
                    g2Row["Клиент"] = String.Empty;
                    g2Row["Сумма оплаты"] = 0;
                    g2Row["Наценка"] = 0;
                    g2Row["НДС"] = 0;
                    g2Row["Транспортные расходы"] = 0;
                    g2Row["Коммерческие расходы"] = 0;
                    g2Row["Прибыль"] = 0;
                    g2Row["level"] = 2;
                    dt.Rows.InsertAt(g2Row, ri++); // вставить строку итогов для Менеджера
                    dr = dt.Rows[ri]; // рабочую строку оставить прежней
                }
                // расчёт итогов
                Double v1 = Convert.ToDouble(dr["Сумма оплаты"]);
                Double v2 = Convert.ToDouble(dr["Наценка"]);
                Double v3 = Convert.ToDouble(dr["НДС"]);
                Double v4 = Convert.ToDouble(dr["Транспортные расходы"]);
                Double v5 = Convert.ToDouble(dr["Коммерческие расходы"]);
                Double v6 = Convert.ToDouble(dr["Прибыль"]);

                g2Row["Сумма оплаты"] = Convert.ToDouble(g2Row["Сумма оплаты"]) + v1;
                g2Row["Наценка"] = Convert.ToDouble(g2Row["Наценка"]) + v2;
                g2Row["НДС"] = Convert.ToDouble(g2Row["НДС"]) + v3;
                g2Row["Транспортные расходы"] = Convert.ToDouble(g2Row["Транспортные расходы"]) + v4;
                g2Row["Коммерческие расходы"] = Convert.ToDouble(g2Row["Коммерческие расходы"]) + v5;
                g2Row["Прибыль"] = Convert.ToDouble(g2Row["Прибыль"]) + v6;

                g1Row["Сумма оплаты"] = Convert.ToDouble(g1Row["Сумма оплаты"]) + v1;
                g1Row["Наценка"] = Convert.ToDouble(g1Row["Наценка"]) + v2;
                g1Row["НДС"] = Convert.ToDouble(g1Row["НДС"]) + v3;
                g1Row["Транспортные расходы"] = Convert.ToDouble(g1Row["Транспортные расходы"]) + v4;
                g1Row["Коммерческие расходы"] = Convert.ToDouble(g1Row["Коммерческие расходы"]) + v5;
                g1Row["Прибыль"] = Convert.ToDouble(g1Row["Прибыль"]) + v6;

                g0Row["Сумма оплаты"] = Convert.ToDouble(g0Row["Сумма оплаты"]) + v1;
                g0Row["Наценка"] = Convert.ToDouble(g0Row["Наценка"]) + v2;
                g0Row["НДС"] = Convert.ToDouble(g0Row["НДС"]) + v3;
                g0Row["Транспортные расходы"] = Convert.ToDouble(g0Row["Транспортные расходы"]) + v4;
                g0Row["Коммерческие расходы"] = Convert.ToDouble(g0Row["Коммерческие расходы"]) + v5;
                g0Row["Прибыль"] = Convert.ToDouble(g0Row["Прибыль"]) + v6;

                // убрать Маркетолога и Менеджера - они есть в итогах
                dr["Маркетолог"] = String.Empty;
                dr["Менеджер"] = String.Empty;

                dr["level"] = 3;
            }

            ReportData = dt;
        }
    }
}