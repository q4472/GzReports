using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using FarmSib.Base.Models;
using FarmSib.Base.Data;
using Nskd;
using System.Xml;

namespace MvcApplication2.Areas.Prep.Models
{
    public class F2Model
    {
        private struct RiCi
        {
            public Int32 RowIndex;
            public Int32 ColumnIndex;
        }
        private RiCi FindRowColumn(DataTable dt, Regex re)
        {
            Int32 maxCnt = 16; // количество первых строчек для поиска
            RiCi rici = new RiCi { RowIndex = -1, ColumnIndex = -1 };
            if (dt != null)
            {
                // ищем только в первых строчках пока не найдём.
                for (int ri = 0; (ri < Math.Min(dt.Rows.Count, maxCnt)) && (rici.RowIndex == -1); ri++)
                {
                    DataRow dr = dt.Rows[ri];
                    for (int ci = 0; ci < dt.Columns.Count; ci++)
                    {
                        DataColumn dc = dt.Columns[ci];
                        if ((dr[dc] != DBNull.Value) && (re.IsMatch((String)dr[dc])))
                        {
                            rici.RowIndex = ri;
                            rici.ColumnIndex = ci;
                            break;
                        }
                    }
                }
            }
            return rici;
        }
        public DataSet ParsingTables { get; set; }
        public String ReadTables(String fileName, String fileContentType, Stream fileStream)
        {
            String msg = fileName + " " + fileStream.Length + " ";
            Int32 len = fileName.Length;

            switch (fileContentType)
            {
                case "application/msword": // doc
                    try
                    {
                        ParsingTables = HomeData.Prep.GetDocTables(fileStream);
                    }
                    catch (Exception e) { msg += e.ToString(); }
                    break;
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document": // docx
                    try
                    {
                        ParsingTables = NskdOpenXml.WordTablesReader.FromStream(fileStream);
                    }
                    catch (Exception e) { msg += e.ToString(); }
                    break;
                case "application/vnd.ms-excel": // xls
                    try
                    {
                        ParsingTables = HomeData.Prep.GetXlsTables(fileStream);
                    }
                    catch (Exception e) { msg += e.ToString(); }
                    break;
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": // xlsx
                    try
                    {
                        ParsingTables = NskdOpenXml.ExcelTablesReader.FromStream(fileStream);
                    }
                    catch (Exception e) { msg += e.ToString(); }
                    break;
                default:
                    msg += "Не обрабатываемый формат файла: '" + fileContentType + "'";
                    break;
            }

            //text/plain txt
            //application/octet-stream rar
            //application/octet-stream msg
            //application/pdf pdf

            // все таблицы сохранены в ParsingTables пробуем их разобрать
            try
            {
                RemoveEmptyRowsAndColumns();
                FindNeededColumns();
                RemoveNotNeededHeadRows();
                AddNeededColumns();
            }
            catch (Exception e) { msg += e.ToString(); }

            return msg;
        }
        private void RemoveEmptyRowsAndColumns()
        {
            if (ParsingTables != null)
            {
                int ti = 0;
                while (ti < ParsingTables.Tables.Count)
                {
                    DataTable dt = ParsingTables.Tables[ti];
                    // Подготовка к удалению пустых столбцов
                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ExtendedProperties.Add("isColumnEmpty", true);
                    }
                    // Удаление пустых строк
                    int ri = 0;
                    while (ri < dt.Rows.Count)
                    {
                        DataRow row = dt.Rows[ri];
                        Boolean isRowEmpty = true;
                        foreach (DataColumn column in dt.Columns)
                        {
                            object v = row[column];
                            if (v != DBNull.Value && v != null && (v.GetType() != typeof(String) || !String.IsNullOrWhiteSpace(v as String)))
                            {
                                isRowEmpty = false;
                                column.ExtendedProperties["isColumnEmpty"] = false;
                            }
                        }
                        if (isRowEmpty)
                        {
                            dt.Rows.Remove(row);
                        }
                        else { ri++; }
                    }
                    // Удаление пустых столбцов
                    int ci = 0;
                    while (ci < dt.Columns.Count)
                    {
                        DataColumn column = dt.Columns[ci];
                        if ((Boolean)column.ExtendedProperties["isColumnEmpty"])
                        {
                            dt.Columns.Remove(column);
                        }
                        else { ci++; }
                    }
                    if (dt.Rows.Count == 0 || dt.Columns.Count == 0)
                    {
                        ParsingTables.Tables.RemoveAt(ti);
                    }
                    else { ti++; }
                }
            }
        }
        public static String CreatePrepSpecTable(RequestPackage rqp)
        {
            String msg = String.Empty;
            if (rqp != null)
            {
                if (Int32.TryParse(rqp["specId"] as String, out Int32 specId))
                {
                    msg = specId.ToString();
                    DataGridView table = new DataGridView();
                    AddTableColumns(table);
                    AddTableRows(table, rqp.Parameters, specId);
                    DataTable dt = table.ToDataTable();
                    for (int ri = 0; ri < dt.Rows.Count; ri++)
                    {
                        DataRow dr = dt.Rows[ri];
                        HomeData.Spec.Table.InsertRow(dr);
                    }
                }
            }
            return msg;
        }
        private void FindNeededColumns()
        {
            if (ParsingTables != null)
            {
                int ti = 0;
                while (ti < ParsingTables.Tables.Count)
                {
                    DataTable dt = ParsingTables.Tables[ti];
                    // в таблице должно быть не меньше 4-х столбцов
                    /*
                    if (dt.Columns.Count < 4)
                    {
                        ParsingTables.Tables.Remove(dt);
                        continue;
                    }
                    */

                    Boolean isFinded = false;

                    RiCi rici = FindRowColumn(dt, new Regex(@"(?i)(международное\s*непатентованное\s*наименование)|(мнн)|(фарм\.?\s*группа)"));
                    if (rici.RowIndex >= 0 && rici.ColumnIndex >= 0)
                    {
                        DataColumn column = dt.Columns[rici.ColumnIndex];
                        if (!column.ExtendedProperties.Contains("HeadRowIndex"))
                        {
                            column.ColumnName = "МНН";
                            column.ExtendedProperties.Add("HeadRowIndex", rici.RowIndex);
                        }
                        isFinded = true;
                    }
                    //rici = FindRowColumn(dt, new Regex(@"(?i)(торговое)|(наименование)"));
                    //if (rici.RowIndex >= 0 && rici.ColumnIndex >= 0)
                    //{
                    //    DataColumn column = dt.Columns[rici.ColumnIndex];
                    //    if (!column.ExtendedProperties.Contains("HeadRowIndex"))
                    //    {
                    //        column.ColumnName = "Наименование";
                    //        column.ExtendedProperties.Add("HeadRowIndex", rici.RowIndex);
                    //    }
                    //    isFinded = true;
                    //}
                    rici = FindRowColumn(dt, new Regex("([Оо]писание)|([Хх]арактеристика)|([Фф]орма)|([Дд]озировка)|([Уу]паковка)"));
                    if (rici.RowIndex >= 0 && rici.ColumnIndex >= 0)
                    {
                        DataColumn column = dt.Columns[rici.ColumnIndex];
                        if (!column.ExtendedProperties.Contains("HeadRowIndex"))
                        {
                            column.ColumnName = "Описание";
                            column.ExtendedProperties.Add("HeadRowIndex", rici.RowIndex);
                        }
                        isFinded = true;
                    }
                    rici = FindRowColumn(dt, new Regex("([Ее]д\\.)|([Ее]д\\.? *[Ии]зм.*)"));
                    if (rici.RowIndex >= 0 && rici.ColumnIndex >= 0)
                    {
                        DataColumn column = dt.Columns[rici.ColumnIndex];
                        if (!column.ExtendedProperties.Contains("HeadRowIndex"))
                        {
                            column.ColumnName = "Ед. изм.";
                            column.ExtendedProperties.Add("HeadRowIndex", rici.RowIndex);
                        }
                        isFinded = true;
                    }
                    rici = FindRowColumn(dt, new Regex("([Кк]ол *- *во)|([Кк]оличество)"));
                    if (rici.RowIndex >= 0 && rici.ColumnIndex >= 0)
                    {
                        DataColumn column = dt.Columns[rici.ColumnIndex];
                        if (!column.ExtendedProperties.Contains("HeadRowIndex"))
                        {
                            column.ColumnName = "Кол-во";
                            column.ExtendedProperties.Add("HeadRowIndex", rici.RowIndex);
                        }
                        isFinded = true;
                    }
                    //rici = FindRowColumn(dt, new Regex(@"(?i)(цена)"));
                    //if (rici.RowIndex >= 0 && rici.ColumnIndex >= 0)
                    //{
                    //    DataColumn column = dt.Columns[rici.ColumnIndex];
                    //    if (!column.ExtendedProperties.Contains("HeadRowIndex"))
                    //    {
                    //        column.ColumnName = "Цена с НДС";
                    //        column.ExtendedProperties.Add("HeadRowIndex", rici.RowIndex);
                    //    }
                    //    isFinded = true;
                    //}
                    //rici = FindRowColumn(dt, new Regex(@"(?i)(сумма)"));
                    //if (rici.RowIndex >= 0 && rici.ColumnIndex >= 0)
                    //{
                    //    DataColumn column = dt.Columns[rici.ColumnIndex];
                    //    if (!column.ExtendedProperties.Contains("HeadRowIndex"))
                    //    {
                    //        column.ColumnName = "Сумма с НДС";
                    //        column.ExtendedProperties.Add("HeadRowIndex", rici.RowIndex);
                    //    }
                    //    isFinded = true;
                    //}

                    if (!isFinded)
                    {
                        ParsingTables.Tables.Remove(dt);
                    }
                    else { ti++; }
                }
            }
        }
        private void RemoveNotNeededHeadRows()
        {
            if (ParsingTables != null)
            {
                int ti = 0;
                while (ti < ParsingTables.Tables.Count)
                {
                    DataTable dt = ParsingTables.Tables[ti];
                    Int32 firstRowIndex = 16;
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ExtendedProperties.ContainsKey("HeadRowIndex"))
                        {
                            firstRowIndex = Math.Min(firstRowIndex, (Int32)column.ExtendedProperties["HeadRowIndex"]);
                        }
                    }
                    // Удаляем 
                    for (int ri = 0; ri < firstRowIndex; ri++)
                    {
                        dt.Rows.RemoveAt(0);
                    }
                    ti++;
                }
            }
        }
        private void AddNeededColumns()
        {
            if (ParsingTables != null)
            {
                int ti = 0;
                while (ti < ParsingTables.Tables.Count)
                {
                    DataTable dt = ParsingTables.Tables[ti];
                    if (!dt.Columns.Contains("Ед. изм."))
                    {
                        DataColumn column = dt.Columns.Add("Ед. изм.", typeof(String));
                        foreach (DataRow row in dt.Rows)
                        {
                            row[column] = "уп.";
                        }
                    }
                    ti++;
                }
            }
        }
        private static void SpecTableSave(RequestParameter[] pars, Int32 headId)
        {
            DataGridView table = new DataGridView();
            AddTableColumns(table);
            AddTableRows(table, pars, headId);
            DataTable dt = table.ToDataTable();
            for (int ri = 0; ri < dt.Rows.Count; ri++)
            {
                DataRow dr = dt.Rows[ri];
                HomeData.Spec.Table.InsertRow(dr);
            }
        }
        private static void AddTableColumns(DataGridView table)
        {
            table.Columns.Add(new DataGridColumn { ColumnName = "международное_непатентованное_наименование", DataType = typeof(String) }); // 0
            table.Columns.Add(new DataGridColumn { ColumnName = "требование", DataType = typeof(String) }); // 1
            table.Columns.Add(new DataGridColumn { ColumnName = "ед_изм_в_требовании", DataType = typeof(String) }); // 2
            table.Columns.Add(new DataGridColumn { ColumnName = "количество_в_требовании", DataType = typeof(Double) }); // 3
            table.Columns.Add(new DataGridColumn { ColumnName = "ед_изм", DataType = typeof(String) }); // 4
            table.Columns.Add(new DataGridColumn { ColumnName = "количество", DataType = typeof(Double) }); // 5
            table.Columns.Add(new DataGridColumn { ColumnName = "код_спецификации", DataType = typeof(Int32) }); // 6
            table.Columns.Add(new DataGridColumn { ColumnName = "номер_строки", DataType = typeof(Int32) }); // 7
            table.Columns.Add(new DataGridColumn { ColumnName = "tp_id", DataType = typeof(Guid) }); // 8
            table.Columns.Add(new DataGridColumn { ColumnName = "лекарственная_форма_и_дозировка", DataType = typeof(String) }); // 9
            //table.Columns.Add(new DataGridColumn { ColumnName = "код_спецификации", DataType = typeof(Int32) });
            //table.Columns.Add(new DataGridColumn { ColumnName = "номер_строки", DataType = typeof(Int32) });
            //table.Columns.Add(new DataGridColumn { ColumnName = "tp_id", DataType = typeof(Guid) });
            //table.Columns.Add(new DataGridColumn { ColumnName = "международное_непатентованное_наименование", DataType = typeof(String) });
            //table.Columns.Add(new DataGridColumn { ColumnName = "наименование", DataType = typeof(String) });
            //table.Columns.Add(new DataGridColumn { ColumnName = "лекарственная_форма_и_дозировка", DataType = typeof(String) });
            //table.Columns.Add(new DataGridColumn { ColumnName = "ед_изм", DataType = typeof(String) });
            //table.Columns.Add(new DataGridColumn { ColumnName = "количество", DataType = typeof(Int32) });
            //table.Columns.Add(new DataGridColumn { ColumnName = "цена_по_спецификации", DataType = typeof(Double) });
        }
        private static void AddTableRows(DataGridView table, RequestParameter[] pars, Int32 headId)
        {
            Regex re = new Regex(@"^t\[(\d+)\]\[(\d+)\]$");
            // в pars имя данныx из табличной части начинается с 't'
            Int32 cellCount = 0;
            foreach (RequestParameter p in pars)
            {
                if (re.IsMatch(p.Name))
                {
                    cellCount++;
                }
            }
            // в каждой строке по 4 ячейки
            int rowCount = cellCount / 4;
            // добавляем строчки
            for (int ri = 0; ri < rowCount; ri++)
            {
                DataGridRow r = table.NewRow();
                r["код_спецификации"] = headId;
                r["номер_строки"] = ri + 1;
                r["tp_id"] = Guid.NewGuid();
                table.Rows.Add(r);
            }
            // заполняем данными из pars
            foreach (RequestParameter p in pars)
            {
                if (re.IsMatch(p.Name))
                {
                    Object v = p.Value;
                    Match m = re.Match(p.Name);
                    Int32 ri = Int32.Parse(m.Groups[1].Value);
                    Int32 ci = Int32.Parse(m.Groups[2].Value);
                    DataGridRow r = table.Rows[ri];
                    switch (ci)
                    {
                        case 0: // МНН
                            r["международное_непатентованное_наименование"] = v;
                            break;
                        case 1: // Описание
                            r["требование"] = v;
                            r["лекарственная_форма_и_дозировка"] = v;
                            break;
                        case 2: // Ед. изм.
                            r["ед_изм_в_требовании"] = v;
                            r["ед_изм"] = v;
                            break;
                        case 3: // Количество
                            r["количество_в_требовании"] = v; // 3
                            r["количество"] = v; // 3
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private static Guid parseAndSaveCommonInf(Guid sessionId, XmlNode cardHeader, XmlNode[] noticeTabBoxWrappers)
        {
            Object[][] md = new Object[][] { 
                    // заявка
                    new Object[] { "номер",                                         cardHeader, "./h1" },
                    new Object[] { "дата_размещения",                               cardHeader, "./div[@class='public']" },
                    new Object[] { "кооператив",                                    cardHeader, "./div[@class='public']/span[@class='cooperative']" },
                    // общая_информация_о_закупке
                    new Object[] { "способ_определения_поставщика",                 noticeTabBoxWrappers[0], "./table//tr[1]/td[2]" },
                    new Object[] { "наименование_электронной_площадки_в_интернете", noticeTabBoxWrappers[0], "./table//tr[2]/td[2]" },
                    new Object[] { "адрес_электронной_площадки_в_интернете",        noticeTabBoxWrappers[0], "./table//tr[3]/td[2]" },
                    new Object[] { "размещение_осуществляет",                       noticeTabBoxWrappers[0], "./table//tr[4]/td[2]" },
                    new Object[] { "объект_закупки",                                noticeTabBoxWrappers[0], "./table//tr[5]/td[2]" },
                    new Object[] { "этап_закупки",                                  noticeTabBoxWrappers[0], "./table//tr[6]/td[2]" },
                    new Object[] { "сведения_о_связи_с_позицией_плана_графика",     noticeTabBoxWrappers[0], "./table//tr[7]/td[2]" },
                    new Object[] { "номер_типового_контракта",                      noticeTabBoxWrappers[0], "./table//tr[8]/td[2]" },
                    // информация_об_организации_осуществляющей_определение_поставщика
                    new Object[] { "организация_осуществляющая_размещение",         noticeTabBoxWrappers[1], "./table//tr[1]/td[2]" },
                    new Object[] { "почтовый_адрес",                                noticeTabBoxWrappers[1], "./table//tr[2]/td[2]" },
                    new Object[] { "место_нахождения",                              noticeTabBoxWrappers[1], "./table//tr[3]/td[2]" },
                    new Object[] { "ответственное_должностное_лицо",                noticeTabBoxWrappers[1], "./table//tr[4]/td[2]" },
                    new Object[] { "адрес_электронной_почты",                       noticeTabBoxWrappers[1], "./table//tr[5]/td[2]" },
                    new Object[] { "номер_контактного_телефона",                    noticeTabBoxWrappers[1], "./table//tr[6]/td[2]" },
                    new Object[] { "факс",                                          noticeTabBoxWrappers[1], "./table//tr[7]/td[2]" },
                    new Object[] { "дополнительная_информация",                     noticeTabBoxWrappers[1], "./table//tr[8]/td[2]" },
                    // информация_о_процедуре_закупки
                    new Object[] { "дата_и_время_начала_подачи_заявок",             noticeTabBoxWrappers[2], "./table//tr[1]/td[2]" },
                    new Object[] { "дата_и_время_окончания_подачи_заявок",          noticeTabBoxWrappers[2], "./table//tr[2]/td[2]" },
                    new Object[] { "место_подачи_заявок",                           noticeTabBoxWrappers[2], "./table//tr[3]/td[2]" },
                    new Object[] { "порядок_подачи_заявок",                         noticeTabBoxWrappers[2], "./table//tr[4]/td[2]" },
                    new Object[] { "дата_окончания_срока_рассмотрения_первых_частей", noticeTabBoxWrappers[2], "./table//tr[5]/td[2]" },
                    new Object[] { "дата_проведения_аукциона_в_электронной_форме",  noticeTabBoxWrappers[2], "./table//tr[6]/td[2]" },
                    new Object[] { "время_проведения_аукциона",                     noticeTabBoxWrappers[2], "./table//tr[7]/td[2]" },
                    new Object[] { "дополнительная_информация2",                    noticeTabBoxWrappers[2], "./table//tr[8]/td[2]" },
                    // начальная_максимальная_цена_контракта
                    new Object[] { "начальная_максимальная_цена_контракта",         noticeTabBoxWrappers[3], "./table//tr[1]/td[2]" },
                    new Object[] { "валюта",                                        noticeTabBoxWrappers[3], "./table//tr[2]/td[2]" },
                    new Object[] { "источник_финансирования",                       noticeTabBoxWrappers[3], "./table//tr[3]/td[2]" },
                    new Object[] { "идентификационный_код_закупки",                 noticeTabBoxWrappers[3], "./table//tr[4]/td[2]" },
                    new Object[] { "оплата_исполнения_контракта_по_годам",          noticeTabBoxWrappers[3], "./div[contains(@class, 'addingTbl')]/table//tr[1]/td[2]" },
                    // информация_об_объекте_закупки
                    new Object[] { "условия_запреты_и_ограничения_допуска_товаров", noticeTabBoxWrappers[4], "./table//tr[1]/td[2]" },
                    new Object[] { "табличная_часть_в_формате_html",                noticeTabBoxWrappers[4], "./table//tr[2]/td[1]" },
                    // преимущества_требования_к_участникам
                    new Object[] { "преимущества",                                  noticeTabBoxWrappers[5], "./table//tr[1]/td[2]" },
                    new Object[] { "требования",                                    noticeTabBoxWrappers[5], "./table//tr[2]/td[2]" },
                    new Object[] { "ограничения",                                   noticeTabBoxWrappers[5], "./table//tr[3]/td[2]" }
                };

            Guid oUid = HomeData.Prep.SaveCommonInf(sessionId, md);

            return oUid;
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
    }
}
