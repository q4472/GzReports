using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Excel = DocumentFormat.OpenXml.Spreadsheet;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;
using System.IO;

namespace MvcApplication2.Areas.Order.Models
{
    public class NskdOpenXml
    {
        public class WordTablesReader
        {
            public static DataSet FromStream(Stream s)
            {
                DataSet ds = new DataSet();
                try
                {
                    using (var document = WordprocessingDocument.Open(s, false))
                    {
                        var docPart = document.MainDocumentPart;
                        var doc = docPart.Document;

                        IEnumerable<Word.Table> tables = doc.Body.Elements<Word.Table>();
                        foreach (Word.Table table in tables)
                        {
                            DataTable dataTable = ReadWordTable(table);
                            ds.Tables.Add(dataTable);
                        }
                    }
                }
                catch (Exception e) { ds = null; throw new Exception("Ошибка при разборе потока docx.", e); }
                return ds;
            }
            /*
            static void Main(string[] args)
            {
                string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                string wordFile = appPath + "\\TestDoc.docx";

                DataTable dataTable = ReadWordTable(wordFile);

                if (dataTable != null)
                {
                    string sFile = appPath + "\\ExportExcel.xlsx";

                    ExportDataTableToExcel(dataTable, sFile);

                    Console.WriteLine("Contents of word table exported to excel spreadsheet");

                    Console.ReadKey();
                }
            }
            */
            /// <summary> 
            /// This method reads the contents of table using openxml sdk 
            /// </summary> 
            /// <param name="fileName"></param> 
            /// <returns></returns> 
            public static DataTable ReadWordTable(Word.Table myTable)
            {
                DataTable table;

                try
                {
                    List<List<string>> totalRows = new List<List<string>>();
                    int maxCol = 0;

                    foreach (TableRow row in myTable.Elements<TableRow>())
                    {
                        List<string> tempRowValues = new List<string>();
                        foreach (TableCell cell in row.Elements<TableCell>())
                        {
                            tempRowValues.Add(cell.InnerText);
                        }

                        maxCol = ProcessList(tempRowValues, totalRows, maxCol);
                    }

                    table = ConvertListListStringToDataTable(totalRows, maxCol);

                    return table;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }

            /// <summary> 
            /// Add each row to the totalRows. 
            /// </summary> 
            /// <param name="tempRows"></param> 
            /// <param name="totalRows"></param> 
            /// <param name="MaxCol">the max column number in rows of the totalRows</param> 
            /// <returns></returns> 
            private static int ProcessList(List<string> tempRows, List<List<string>> totalRows, int MaxCol)
            {
                if (tempRows.Count > MaxCol)
                {
                    MaxCol = tempRows.Count;
                }

                totalRows.Add(tempRows);
                return MaxCol;
            }

            /// <summary> 
            /// This method converts list data to a data table 
            /// </summary> 
            /// <param name="totalRows"></param> 
            /// <param name="maxCol"></param> 
            /// <returns>returns datatable object</returns> 
            private static DataTable ConvertListListStringToDataTable(List<List<string>> totalRows, int maxCol)
            {
                DataTable table = new DataTable();
                for (int i = 0; i < maxCol; i++)
                {
                    table.Columns.Add();
                }
                foreach (List<string> row in totalRows)
                {
                    while (row.Count < maxCol)
                    {
                        row.Add("");
                    }
                    table.Rows.Add(row.ToArray());
                }
                return table;
            }

            /// <summary> 
            /// This method exports datatable to a excel file 
            /// </summary> 
            /// <param name="table">DataTable</param> 
            /// <param name="exportFile">Excel file name</param> 
            private static void ExportDataTableToExcel(DataTable table, string exportFile)
            {
                try
                {
                    // Create a spreadsheet document by supplying the filepath. 
                    SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                        Create(exportFile, SpreadsheetDocumentType.Workbook);

                    // Add a WorkbookPart to the document. 
                    WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();

                    // Add a WorksheetPart to the WorkbookPart. 
                    WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    // Add Sheets to the Workbook. 
                    Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                        AppendChild<Sheets>(new Sheets());

                    // Append a new worksheet and associate it with the workbook. 
                    Sheet sheet = new Sheet()
                    {
                        Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "mySheet"
                    };
                    sheets.Append(sheet);

                    SheetData data = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    //add column names to the first row   
                    Row header = new Row();
                    header.RowIndex = (UInt32)1;

                    foreach (DataColumn column in table.Columns)
                    {
                        Cell headerCell = createTextCell(
                            table.Columns.IndexOf(column) + 1,
                            1,
                            column.ColumnName);

                        header.AppendChild(headerCell);
                    }
                    data.AppendChild(header);

                    //loop through each data row   
                    DataRow contentRow;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        contentRow = table.Rows[i];
                        data.AppendChild(createContentRow(contentRow, i + 2));
                    }

                    workbookpart.Workbook.Save();

                    // Close the document. 
                    spreadsheetDocument.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            /// <summary> 
            /// This method creates text cell 
            /// </summary> 
            /// <param name="columnIndex"></param> 
            /// <param name="rowIndex"></param> 
            /// <param name="cellValue"></param> 
            /// <returns></returns> 
            private static Cell createTextCell(int columnIndex, int rowIndex, object cellValue)
            {
                Cell cell = new Cell();

                cell.DataType = CellValues.InlineString;
                cell.CellReference = getColumnName(columnIndex) + rowIndex;

                InlineString inlineString = new InlineString();
                Excel.Text t = new Excel.Text();

                t.Text = cellValue.ToString();
                inlineString.AppendChild(t);
                cell.AppendChild(inlineString);

                return cell;
            }

            /// <summary> 
            /// This method creates content row 
            /// </summary> 
            /// <param name="dataRow"></param> 
            /// <param name="rowIndex"></param> 
            /// <returns></returns> 
            private static Row createContentRow(DataRow dataRow, int rowIndex)
            {
                Row row = new Row
                {
                    RowIndex = (UInt32)rowIndex
                };

                for (int i = 0; i < dataRow.Table.Columns.Count; i++)
                {
                    Cell dataCell = createTextCell(i + 1, rowIndex, dataRow[i]);
                    row.AppendChild(dataCell);
                }
                return row;
            }

            /// <summary> 
            /// Formates or gets column name 
            /// </summary> 
            /// <param name="columnIndex"></param> 
            /// <returns></returns> 
            private static string getColumnName(int columnIndex)
            {
                int dividend = columnIndex;
                string columnName = String.Empty;
                int modifier;

                while (dividend > 0)
                {
                    modifier = (dividend - 1) % 26;
                    columnName =
                        Convert.ToChar(65 + modifier).ToString() + columnName;
                    dividend = (int)((dividend - modifier) / 26);
                }

                return columnName;
            }
        }
        public class ExcelTablesReader
        {
            /// <summary> 
            ///  Считывание данных из потока xlsx в объект DataTable 
            /// </summary> 
            /// <param name="s">Stream xlsx</param> 
            /// <returns></returns> 
            public static DataSet FromStream(Stream s)
            {
                DataSet ds = new DataSet();
                try
                {
                    using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(s, false))
                    {
                        // Получение части книги SpreadSheetDocument 
                        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                        // Получение всех листов SpreadSheetDocument  
                        IEnumerable<Sheet> sheetcollection = spreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                        foreach (Sheet sheet in sheetcollection)
                        {
                            DataTable dt = new DataTable(sheet.Name);
                            ds.Tables.Add(dt);
                            // Получение идентификатора отношения 
                            string relationshipId = sheet.Id.Value;
                            // Получение части листа SpreadSheetDocument 
                            WorksheetPart worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);
                            // Получение данных листа
                            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                            IEnumerable<Row> rowcollection = sheetData.Elements<Row>();
                            if (rowcollection.Count() > 0)
                            {
                                // Добавление столбцов. Для начала 10. При заполнении строк столбцы могут быть добавлены
                                for (int ci = 0; ci < 10; ci++)
                                {
                                    dt.Columns.Add(String.Format("Column{0}", ci), typeof(String));
                                }
                                // Добавление строк в DataTable 
                                foreach (Row row in rowcollection)
                                {
                                    DataRow temprow = dt.NewRow();
                                    int ci = 0;
                                    foreach (Cell cell in row.Elements<Cell>())
                                    {
                                        // Получение индекса столбца ячейки 
                                        int cellColumnIndex = GetColumnIndex(GetColumnName(cell.CellReference));
                                        // если ячейки не подряд - добавляем пустые ячейки
                                        if (ci < cellColumnIndex)
                                        {
                                            do
                                            {
                                                temprow[ci] = string.Empty;
                                                ci++;
                                            }
                                            while (ci < cellColumnIndex);
                                        }
                                        // если ячейка из колонки которой ещё не - добавляем колонки
                                        if (ci >= dt.Columns.Count)
                                        {
                                            do
                                            {
                                                dt.Columns.Add(String.Format("Column{0}", ci), typeof(String));
                                            }
                                            while (ci >= dt.Columns.Count);
                                        }
                                        temprow[ci] = GetValueOfCell(spreadsheetDocument, cell);
                                        ci++;
                                    }
                                    // Добавление строки в DataTable 
                                    // строки содержат строку заголовка 
                                    dt.Rows.Add(temprow);
                                }
                            }
                        }
                    }
                }
                catch (Exception e) { ds = null; throw new Exception("Ошибка при разборе потока xlsx.", e); }
                return ds;
            }
            /// <summary> 
            /// Get Column Name From given cell name 
            /// </summary> 
            /// <param name="cellReference">Cell Name(For example,A1)</param> 
            /// <returns>Column Name(For example, A)</returns> 
            private static string GetColumnName(string cellReference)
            {
                // Create a regular expression to match the column name of cell 
                Regex regex = new Regex("[A-Za-z]+");
                Match match = regex.Match(cellReference);
                return match.Value;
            }
            /// <summary> 
            /// Get Index of Column from given column name 
            /// </summary> 
            /// <param name="columnName">Column Name(For Example,A or AA)</param> 
            /// <returns>Column Index</returns> 
            private static int GetColumnIndex(string columnName)
            {
                int columnIndex = 0;
                int factor = 1;

                // From right to left 
                for (int position = columnName.Length - 1; position >= 0; position--)
                {
                    // For letters 
                    if (Char.IsLetter(columnName[position]))
                    {
                        columnIndex += factor * ((columnName[position] - 'A') + 1) - 1;
                        factor *= 26;
                    }
                }

                return columnIndex;
            }
            /// <summary> 
            ///  Получение значения ячейки  
            /// </summary> 
            /// <param name="spreadsheetdocument">SpreadSheet Document Object</param> 
            /// <param name="cell">Cell Object</param> 
            /// <returns>The Value in Cell</returns> 
            private static string GetValueOfCell(SpreadsheetDocument spreadsheetdocument, Cell cell)
            {
                // Получение значения в ячейке 
                SharedStringTablePart sharedString = spreadsheetdocument.WorkbookPart.SharedStringTablePart;
                if (cell.CellValue == null)
                {
                    return string.Empty;
                }


                string cellValue = cell.CellValue.InnerText;

                // Если тип данных ячейки представлен SharedString 
                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    return sharedString.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText;
                }
                else
                {
                    return cellValue;
                }
            }
        }
    }
}