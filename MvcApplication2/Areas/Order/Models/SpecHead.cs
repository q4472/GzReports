using FarmSib.Base.Data;
using FarmSib.Base.Models;
using System;
using Nskd;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;

namespace MvcApplication2.Areas.Order.Models
{
    public class SpecHead
    {
        private void addColumns()
        {
            GridView.Columns.Add(new DataGridColumn { Caption = "id", ColumnName = "id", DataType = typeof(Int32), IsVisible = false }); // 0

            GridView.Columns.Add(new DataGridColumn { Caption = "НМЦК", ColumnName = "сумма_лота", DataType = typeof(Double) }); // 1
            GridView.Columns.Add(new DataGridColumn { Caption = "Сумма выигрыша", ColumnName = "сумма_выигрыша", DataType = typeof(Double) }); // 2
            GridView.Columns.Add(new DataGridColumn { Caption = "Сумма закупки", ColumnName = "всего_по_закупке", DataType = typeof(Double) }); // 3
            GridView.Columns.Add(new DataGridColumn { Caption = "Начальная маржа", ColumnName = "начальная_маржа", DataType = typeof(Double) }); // 4
            GridView.Columns.Add(new DataGridColumn { Caption = "Итоговая маржа", ColumnName = "итоговая_маржа", DataType = typeof(Double) }); // 5
            GridView.Columns.Add(new DataGridColumn { Caption = "Статус аукциона", ColumnName = "статус_аукциона", DataType = typeof(String) }); // 6
            GridView.Columns.Add(new DataGridColumn { Caption = "Номер извещения аукциона", ColumnName = "номер_извещения_аукциона", DataType = typeof(String) }); // 7
            GridView.Columns.Add(new DataGridColumn { Caption = "Дата окончания подачи", ColumnName = "дата_окончания_подачи", DataType = typeof(DateTime) }); // 8
            GridView.Columns.Add(new DataGridColumn { Caption = "Дата проведения аукциона", ColumnName = "дата_проведения_аукциона", DataType = typeof(DateTime) }); // 9
            GridView.Columns.Add(new DataGridColumn { Caption = "Наименование заказчика", ColumnName = "наименование_заказчика", DataType = typeof(String) }); // 10
            GridView.Columns.Add(new DataGridColumn { Caption = "ИНН заказчика", ColumnName = "инн_заказчика", DataType = typeof(String) }); // 11
            GridView.Columns.Add(new DataGridColumn { Caption = "Регион", ColumnName = "регион", DataType = typeof(String) }); // 12

            GridView.Columns.Add(new DataGridColumn { Caption = "Менеджер", ColumnName = "менеджер", DataType = typeof(String) }); // 13
            GridView.Columns.Add(new DataGridColumn { Caption = "Номер контракта", ColumnName = "номер_контракта", DataType = typeof(String), IsVisible = false }); // 14
            GridView.Columns.Add(new DataGridColumn { Caption = "Дата контракта", ColumnName = "дата_контракта", DataType = typeof(DateTime), IsVisible = false }); // 15
            GridView.Columns.Add(new DataGridColumn { Caption = "Дата окончания действия контракта", ColumnName = "дата_окончания_действия_контракта", DataType = typeof(DateTime), IsVisible = false }); // 16
            GridView.Columns.Add(new DataGridColumn { Caption = "Код контракта в 1С", ColumnName = "код_контракта_в_1с", DataType = typeof(Int32), IsVisible = false }); // 17
            GridView.Columns.Add(new DataGridColumn { Caption = "График поставки", ColumnName = "график_поставки", DataType = typeof(String) }); // 18
            GridView.Columns.Add(new DataGridColumn { Caption = "Требования по сроку годности", ColumnName = "требования_по_сроку_годности", DataType = typeof(String) }); // 19
            GridView.Columns.Add(new DataGridColumn { Caption = "Дата первой поставки", ColumnName = "дата_первой_поставки", DataType = typeof(DateTime), IsVisible = false }); // 20
            GridView.Columns.Add(new DataGridColumn { Caption = "Количество поставок", ColumnName = "количество_поставок", DataType = typeof(Int32), IsVisible = false }); // 21
            GridView.Columns.Add(new DataGridColumn { Caption = "Период поставок", ColumnName = "период_поставок", DataType = typeof(Int32), IsVisible = false }); // 22
            GridView.Columns.Add(new DataGridColumn { Caption = "Срок исполнения (заявка => склад)", ColumnName = "срок_исполнения_заявка_склад", DataType = typeof(Int32), IsVisible = false }); // 23
            GridView.Columns.Add(new DataGridColumn { Caption = "Срок исполнения (склад => отгрузка)", ColumnName = "срок_исполнения_склад_отгрузка", DataType = typeof(Int32), IsVisible = false }); // 24
            GridView.Columns.Add(new DataGridColumn { Caption = "Срок исполнения (отгрузка => покупатель)", ColumnName = "срок_исполнения_отгрузка_покупатель", DataType = typeof(Int32), IsVisible = false }); // 25
            GridView.Columns.Add(new DataGridColumn { Caption = "type", ColumnName = "type", DataType = typeof(Int32), IsVisible = false }); // 26
            GridView.Columns.Add(new DataGridColumn { Caption = "Расстояние от склада", ColumnName = "расстояние_от_склада", DataType = typeof(Double), IsVisible = false }); // 27
            GridView.Columns.Add(new DataGridColumn { Caption = "Условия оплаты", ColumnName = "условия_оплаты", DataType = typeof(String) }); // 28
            GridView.Columns.Add(new DataGridColumn { Caption = "Сумма обеспечения заявки", ColumnName = "сумма_обеспечения_заявки", DataType = typeof(Double) }); // 29
            GridView.Columns.Add(new DataGridColumn { Caption = "Сумма обеспечения контракта", ColumnName = "сумма_обеспечения_контракта", DataType = typeof(Double) }); // 30
            GridView.Columns.Add(new DataGridColumn { Caption = "", ColumnName = "user_id", DataType = typeof(Int32), IsVisible = false }); // 31
        }
        public DataGridView GridView { get; private set; }
        //public DataTable Table { get; private set; }
        public SpecHead()
        {
            GridView = new DataGridView();
            addColumns();
        }
        public void GetById(Int32 id)
        {
            DataTable dt = HomeData.Spec.Head.GetHeadAndAuction(id);
            GridView.AddRows(dt);
            GridView.PrimaryKey.Add(GridView.Columns[0]);
        }
        public void addRows(Dictionary<String, String> pars)
        {
            DataGridRow row = GridView.NewRow();
            for (int ci = 0; ci < GridView.Columns.Count; ci++)
            {
                String name = GridView.Columns[ci].ColumnName;
                if (pars.ContainsKey(name)) row[ci] = pars[name];
            }
            row["type"] = 1;
            GridView.Rows.Add(row);
        }
        public void Save(Guid sessionId)
        {
            DataTable dt = GridView.ToDataTable();
            dt = HomeData.Spec.Head.Save(dt, sessionId);
            GridView.Rows.RemoveAt(0);
            GridView.AddRows(dt);
            GridView.PrimaryKey.Add(GridView.Columns[0]);
        }
        public static String Delete(Guid sessionId, Int32 specId)
        {
            String msg = "Record has been deleted.";
            HomeData.Spec.Head.Delete(sessionId, specId);
            return msg;
        }
        public static String CalcOutgoingPrices(RequestPackage rqp)
        {
            String msg = "Start MvcApplication2.Areas.Order.Models.SpecHead.CalcOutgoingPrices().";
            Guid sessionId = rqp.SessionId;
            Int32 specId = 0;
            if (Int32.TryParse(rqp["specId"] as String, out specId))
            {
                msg = HomeData.Spec.Head.CalcOutgoingPrices(sessionId, specId);
            }
            return msg;
        }
    }
}