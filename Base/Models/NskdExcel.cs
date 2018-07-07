using FarmSib.Base.Data;
using FarmSib.Base.Models;
using System;
using System.Data;
using System.IO;

namespace FarmSib.Base.Models
{
    public static class NskdExcel
    {
        private static class Md
        {
            public class TableColumn
            {
                public String ColumnName;
                public String Caption;
                public Type DataType;
                public String Width;
            }
            public static TableColumn[] Table1Columns = new TableColumn[]
            {
                    new TableColumn { ColumnName = "номер_строки", Caption = "№", DataType = typeof(Int32), Width = "4" }, // 0 A
                    new TableColumn { ColumnName = "международное_непатентованное_наименование", Caption = "Международное непатентованное наименование", DataType = typeof(String), Width = "20" }, // 1 B
                    new TableColumn { ColumnName = "требование", Caption = "Требуемое значение", DataType = typeof(String), Width = "30" }, // 2 C
                    new TableColumn { ColumnName = "ед_изм_в_требовании", Caption = "Ед. изм. в требовании", DataType = typeof(String), Width = "8" }, // 3 D
                    new TableColumn { ColumnName = "количество_в_требовании", Caption = "Кол-во в требовании", DataType = typeof(String), Width = "8" }, // 4 E
                    new TableColumn { ColumnName = "наименование", Caption = "Наименование", DataType = typeof(String), Width = "20" }, // 5 F
                    new TableColumn { ColumnName = "лекарственная_форма_и_дозировка", Caption = "Лекарственная форма, дозировка, упаковка", DataType = typeof(String), Width = "30" }, // 6 G
                    new TableColumn { ColumnName = "производитель", Caption = "Производитель", DataType = typeof(String), Width = "20" }, // 7 H
                    new TableColumn { ColumnName = "страна", Caption = "Страна", DataType = typeof(String), Width = "12" }, // 8 I
                    new TableColumn { ColumnName = "ед_изм", Caption = "Ед. изм.", DataType = typeof(String), Width = "8" }, // 9 J
                    new TableColumn { ColumnName = "количество", Caption = "Кол-во", DataType = typeof(Int32), Width = "8" }, // 10 K
                    new TableColumn { ColumnName = "рег_цена", Caption = "Рег. цена", DataType = typeof(Decimal), Width = "8" }, // 11 L
                    new TableColumn { ColumnName = "цена_закуп", Caption = "Цена закуп.", DataType = typeof(Decimal), Width = "8" }, // 12 M
                    new TableColumn { ColumnName = "сумма_закуп", Caption = "Сумма закуп.", DataType = typeof(Decimal), Width = "10" }, // 13 N
                    new TableColumn { ColumnName = "примечание", Caption = "Примечание", DataType = typeof(String), Width = "14" }, // 14 O
                    new TableColumn { ColumnName = "номер_ру", Caption = "Регистрационное удостоверение", DataType = typeof(String), Width = "16" }, // 15 P
                    new TableColumn { ColumnName = "вес", Caption = "Вес", DataType = typeof(Decimal), Width = "8" }, // 16 Q
                    new TableColumn { ColumnName = "объём", Caption = "Объём", DataType = typeof(Decimal), Width = "8" }, // 17 R
                    new TableColumn { ColumnName = "предельная_оптовая_цена", Caption = "Предельная оптовая цена", DataType = typeof(Decimal), Width = "8" }, // 18 S
                    new TableColumn { ColumnName = "предельная_оптовая_сумма", Caption = "Предельная оптовая сумма", DataType = typeof(Decimal), Width = "10" }, // 19 T
                    new TableColumn { ColumnName = "цена_по_спецификации", Caption = "Цена продажи", DataType = typeof(Decimal), Width = "8" }, // 20 U
                    new TableColumn { ColumnName = "сумма_по_спецификации", Caption = "Сумма продажи", DataType = typeof(Decimal), Width = "10" } // 21 V
            };
            public static TableColumn[] Table2Columns = new TableColumn[]
            {
                    new TableColumn { ColumnName = "номер_строки", Caption = "№", DataType = typeof(Int32), Width = "4" }, // 0 A
                    new TableColumn { ColumnName = "наименование", Caption = "Наименование", DataType = typeof(String), Width = "20" }, // 1 B
                    new TableColumn { ColumnName = "лекарственная_форма_и_дозировка", Caption = "Лекарственная форма, дозировка, упаковка", DataType = typeof(String), Width = "30" }, // 1 C
                    new TableColumn { ColumnName = "производитель", Caption = "Производитель", DataType = typeof(String), Width = "20" }, // 3 D
                    new TableColumn { ColumnName = "страна", Caption = "Страна", DataType = typeof(String), Width = "12" }, // 4 E
                    new TableColumn { ColumnName = "ед_изм", Caption = "Ед. изм.", DataType = typeof(String), Width = "8" }, // 5 F
                    new TableColumn { ColumnName = "количество", Caption = "Кол-во", DataType = typeof(Int32), Width = "8" }, // 6 G
                    new TableColumn { ColumnName = "цена_закуп", Caption = "Цена закуп.", DataType = typeof(Decimal), Width = "8" }, // 7 H
                    new TableColumn { ColumnName = "сумма_закуп", Caption = "Сумма закуп.", DataType = typeof(Decimal), Width = "10" }, // 8 I
                    new TableColumn { ColumnName = "примечание", Caption = "Примечание", DataType = typeof(String), Width = "14" }, // 9 J
                    new TableColumn { ColumnName = "цена_по_спецификации", Caption = "Цена продажи", DataType = typeof(Decimal), Width = "8" }, // 10 K
                    new TableColumn { ColumnName = "сумма_по_спецификации", Caption = "Сумма продажи", DataType = typeof(Decimal), Width = "10" } // 11 L
            };
        }
        private static String GetColumnName(UInt32 index) // zero-based
        {
            const byte BASE = 'Z' - 'A' + 1;
            string name = String.Empty;
            do
            {
                name = System.Convert.ToChar('A' + index % BASE) + name;
                index = index / BASE;
            }
            while (index-- > 0);
            return name;
        }
        public static Byte[] ToExcel(Guid sessionId, DataTable t, DataTable h)
        {
            MemoryStream ms;
            UInt32 zoomScale = 100;
            String fontName = "Arial";
            Double fontSize = 9;
            DataSet ds = HomeData.Settings.Get(sessionId);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[2] as String == "[Масштаб для выгрузки в Excel].[Общий]")
                        {
                            UInt32.TryParse(dr[3] as String, out zoomScale);
                            continue;
                        }
                        if (dr[2] as String == "[Шрифт для выгрузки в Excel].[Наименование]")
                        {
                            fontName = dr[3] as String;
                            continue;
                        }
                        if (dr[2] as String == "[Шрифт для выгрузки в Excel].[Размер]")
                        {
                            Double.TryParse(dr[3] as String, out fontSize);
                            continue;
                        }
                    }
                }
            }
            using (Spreadsheet spreadsheet = new Spreadsheet(2)) // два листа
            {
                spreadsheet.SetSheetName(1, "Расчёт заявки");
                spreadsheet.SetSheetName(2, "Требование на закупку");
                spreadsheet.SetZoomScale(zoomScale);
                spreadsheet.SetFont(0, fontName, fontSize); // default font
                spreadsheet.SetFont(1, fontName, fontSize); // data font

                UInt32 dxfId = spreadsheet.AppendDifferentialFormat("font-weight: bold;");

                XlWorksheet[] wss = spreadsheet.Wss;

                GenerateColumns(wss[0], Md.Table1Columns);
                GenrateSheetData0(wss[0], t, h);
                //GenerateMergeCells(wss[0]);
                GenerateConditionalFormatting(wss[0], dxfId, t);
                GenerateBackgroundColor(wss[0], t);
                GeneratePageSetup(wss[0]);

                GenerateColumns(wss[1], Md.Table2Columns);
                GenrateSheetData1(wss[1], t, h);
                GenerateMergeCells(wss[1]);
                GeneratePageSetup(wss[1]);

                ms = spreadsheet.CreateDocument();
            }
            return ms.ToArray();
        }
        private static void GenerateColumns(XlWorksheet ws, Md.TableColumn[] cols)
        {
            uint cn = 1;
            for (int ci = 0; ci < cols.Length; ci++)
            {
                Md.TableColumn col = cols[ci];
                if (col.Width == null)
                {
                    ws.AppendColumn(cn, cn, true);
                }
                else
                {
                    Double width = 0;
                    Double.TryParse(col.Width, out width);
                    ws.AppendColumn(cn, cn, false, true, width);
                }
                cn++;
            }
        }
        public static void GenrateSheetData0(XlWorksheet ws, DataTable t, DataTable h)
        {
            UInt32 rowIndex = 0;
            UInt32 columnIndex = 0;

            // строка заголовка
            columnIndex = 0;
            foreach (Md.TableColumn column in Md.Table1Columns)
            {
                String cellValueText = column.Caption;
                ws.UpsertCell(rowIndex, columnIndex, 1, cellValueText); // CellValues.SharedString
                columnIndex++;
            }
            rowIndex++;

            // строки данных
            foreach (DataRow dr in t.Rows)
            {
                columnIndex = 0;
                foreach (Md.TableColumn column in Md.Table1Columns)
                {
                    String f;
                    switch (column.ColumnName)
                    {
                        case "сумма_закуп":
                            f = "=K" + (rowIndex + 1).ToString() + "*M" + (rowIndex + 1).ToString();
                            ws.UpsertCell(rowIndex, columnIndex, 3, null, f);
                            break;
                        case "предельная_оптовая_сумма":
                            f = "=K" + (rowIndex + 1).ToString() + "*S" + (rowIndex + 1).ToString();
                            ws.UpsertCell(rowIndex, columnIndex, 3, null, f);
                            break;
                        case "сумма_по_спецификации":
                            f = "=K" + (rowIndex + 1).ToString() + "*U" + (rowIndex + 1).ToString();
                            ws.UpsertCell(rowIndex, columnIndex, 3, null, f);
                            break;
                        default:
                            Object value = dr[column.ColumnName];
                            Type type = column.DataType;
                            AppendValueToSpreadsheetCell(ws, rowIndex, columnIndex, value, type);
                            break;
                    }
                    columnIndex++;
                }
                rowIndex++;
            }

            // сумма по таблице столбца "Сумма закуп."
            {
                String f = "=SUM(N2:N" + rowIndex.ToString() + ")";
                ws.UpsertCell(rowIndex, 13, 3, "0", f); // CellValues.Number
            }
            // сумма по таблице столбца "Вес"
            {
                String f = "=SUM(Q2:Q" + rowIndex.ToString() + ")";
                ws.UpsertCell(rowIndex, 16, 3, "0", f); // CellValues.Number
            }
            // сумма по таблице столбца "Объём"
            {
                String f = "=SUM(R2:R" + rowIndex.ToString() + ")";
                ws.UpsertCell(rowIndex, 17, 3, "0", f); // CellValues.Number
            }
            // сумма по таблице столбца "Предельная оптовая сумма"
            {
                String f = "=SUM(T2:T" + rowIndex.ToString() + ")";
                ws.UpsertCell(rowIndex, 19, 3, "0", f); // CellValues.Number
            }
            // сумма по таблице столбца "Сумма продажи"
            {
                String f = "=SUM(V2:V" + rowIndex.ToString() + ")";
                ws.UpsertCell(rowIndex, 21, 3, "0", f); // CellValues.Number
            }
            rowIndex++;
            // сумма по таблице столбца "Сумма закуп." если "Страна" == Россия
            {
                ws.UpsertCell(rowIndex, 12, 2, "Россия"); // CellValues.SharedString
                String f = String.Format(
                    "=SUMIF(I{0}:I{1},\"=Россия\",N{0}:N{1})" +
                    "+SUMIF(I{0}:I{1},\"=Республика Беларусь\",N{0}:N{1})" +
                    "+SUMIF(I{0}:I{1},\"=Беларусь\",N{0}:N{1})" +
                    "+SUMIF(I{0}:I{1},\"=Казахстан\",N{0}:N{1})" +
                    "+SUMIF(I{0}:I{1},\"=Армения\",N{0}:N{1})", 2, (rowIndex - 1));
                ws.UpsertCell(rowIndex, 13, 3, "0", f); // CellValues.Number
                f = "=(N" + (rowIndex + 1).ToString() + "/N" + rowIndex.ToString() + ")*100";
                ws.UpsertCell(rowIndex, 14, 3, "0", f); // CellValues.Number
            }
            rowIndex++;
            // две таблицы с итогами и данными из шапки
            {
                ws.UpsertCell(rowIndex, 1, 2, "НМЦК"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 2, 3, h.Rows[0]["сумма_лота"]); // CellValues.Number
                ws.UpsertCell(rowIndex, 7, 2, "Сумма по закупке (руб)"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 8, 3, 0D, "=N" + (rowIndex - 1).ToString()); // CellValues.Number
                rowIndex++;
                ws.UpsertCell(rowIndex, 1, 2, "График поставки"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 2, 2, h.Rows[0]["график_поставки"] as String); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 7, 2, "Наценка (%)"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 8, 3, 10D); // CellValues.Number
                rowIndex++;
                ws.UpsertCell(rowIndex, 1, 2, "Срок годности"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 2, 2, h.Rows[0]["требования_по_сроку_годности"] as String); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 7, 2, "Прибыль (руб)"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 8, 3, 0D, "=I" + (rowIndex - 1).ToString() + "*I" + rowIndex.ToString() + "/100"); // CellValues.Number
                rowIndex++;
                ws.UpsertCell(rowIndex, 7, 2, "Сумма с наценкой (руб)"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 8, 3, 0D, "=I" + (rowIndex - 2).ToString() + "+I" + rowIndex.ToString()); // CellValues.Number
                rowIndex++;
                ws.UpsertCell(rowIndex, 7, 2, "Транспорт (руб)"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 8, 3, 0D); // CellValues.Number
                rowIndex++;
                ws.UpsertCell(rowIndex, 7, 2, "Минимальная сумма (руб)"); // CellValues.SharedString
                ws.UpsertCell(rowIndex, 8, 3, 0D, "=I" + (rowIndex - 1).ToString() + "+I" + rowIndex.ToString()); // CellValues.Number
                rowIndex++;
            }
        }
        public static void GenrateSheetData1(XlWorksheet ws, DataTable t, DataTable h)
        {
            ws.SetRowHeight(0, 48);
            ws.UpsertCell(0, 0, 2, h.Rows[0]["наименование_заказчика"]);

            ws.SetRowHeight(1, 24);
            ws.UpsertCell(1, 0, 2, "Номер аукциона: " + h.Rows[0]["номер_извещения_аукциона"]);

            ws.SetRowHeight(2, 48);
            ws.UpsertCell(2, 0, 2, "Требования по сроку годности: " + h.Rows[0]["требования_по_сроку_годности"]);

            Md.TableColumn[] cols = Md.Table2Columns;
            uint tableRowShift = 3;
            String f;
            Object v;
            for (uint rowIndex = 0; rowIndex < (t.Rows.Count + 1); rowIndex++)
            {
                ws.SetRowHeight(rowIndex + tableRowShift, 36);

                uint colIndex = 0;

                // №
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!A" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 4), v, f);

                // Наименование
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!F" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 2), v, f);

                // Форма
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!G" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 2), v, f);

                // Производитель
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!H" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 2), v, f);

                // Страна
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!I" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 2), v, f);

                // Ед. изм.
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!J" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 2), v, f);

                // Кол-во
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!K" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 4), v, f);

                // Цена закуп.
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!M" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 3), v, f);

                // Сумма закуп.
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!N" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 3), v, f);

                // Примечание
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!O" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 2), v, f);

                // Цена продажи
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!U" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 3), v, f);

                // Сумма продажи
                v = (rowIndex == 0 ? cols[colIndex].Caption : t.Rows[(int)rowIndex - 1][cols[colIndex].ColumnName]);
                f = "='Расчёт заявки'!V" + (rowIndex + 1).ToString();
                ws.UpsertCell(rowIndex + tableRowShift, colIndex++, (uint)(rowIndex == 0 ? 1 : 3), v, f);
            }
        }
        private static void AppendValueToSpreadsheetCell(XlWorksheet ws, UInt32 rowIndex, UInt32 columnIndex, Object value, Type type)
        {
            String cellValueText = null;
            if (value != null && value != DBNull.Value)
            {
                switch (type.ToString())
                {
                    case "System.Decimal":
                        ws.UpsertCell(rowIndex, columnIndex, 3, value);
                        break;
                    case "System.Double":
                        ws.UpsertCell(rowIndex, columnIndex, 3, value);
                        break;
                    case "System.Int32":
                        ws.UpsertCell(rowIndex, columnIndex, 4, value);
                        break;
                    default:
                        if (value != null)
                        {
                            cellValueText = value.ToString();
                        }
                        ws.UpsertCell(rowIndex, columnIndex, 2, cellValueText);
                        break;
                }
            }
            else
            {
                ws.UpsertCell(rowIndex, columnIndex, 2, cellValueText);
            }
        }
        private static void GenerateMergeCells(XlWorksheet ws)
        {
            // объединение ячеек для заголовка
            ws.AppendMergeCell(
                new String[] { "A1:C1", "A2:C2", "A3:C3" }
            );
        }
        private static void GenerateConditionalFormatting(XlWorksheet ws, UInt32 dxfId, DataTable t)
        {
            ws.AppendConditionalFormatting(new String[] { "K3:K" + (t.Rows.Count + 2).ToString() }, dxfId, "Value(E3)<>K3");
            ws.AppendConditionalFormatting(new String[] { "J3:J" + (t.Rows.Count + 2).ToString() }, dxfId, "D3<>J3");
        }
        private static void GenerateBackgroundColor(XlWorksheet ws, DataTable t)
        {
            for (int ri = 0; ri < t.Rows.Count; ri++)
            {
                DataRow dr = t.Rows[ri];
                String bgColor = dr["bg_color"] as String;
                if (!String.IsNullOrWhiteSpace(bgColor))
                {
                    int ci = 0;
                    foreach (Md.TableColumn col in Md.Table1Columns)
                    {
                        ws.SetCellBackgroundColor((ri + 1), ci, bgColor); // !!! 1 - количество строк в шапке
                        ci++;
                    }
                }
            }
        }
        private static void GeneratePageSetup(XlWorksheet ws)
        {
            ws.SetPageOrientationLandscape();
            ws.SetPagePaperSizeA4();
            ws.SetPageFitTo(1);
        }
    }
}