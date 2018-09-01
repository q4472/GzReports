using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FarmSib.Base.Models
{
    public class DataGridView
    {
        public String Id { get; private set; }
        public DataGridColumnCollection Columns { get; set; }
        public DataGridRowCollection Rows { get; set; }
        public DataGridColumnCollection PrimaryKey { get; set; }
        public DataGridView()
        {
            Id = String.Format("_{0}_", Guid.NewGuid().ToString("N"));
            Columns = new DataGridColumnCollection(this);
            Rows = new DataGridRowCollection(this);
            PrimaryKey = new DataGridColumnCollection(this);
        }
        public DataGridRow NewRow()
        {
            DataGridRow r = new DataGridRow(this);
            return r;
        }
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            for (int ci = 0; ci < Columns.Count; ci++)
            {
                DataGridColumn col = Columns[ci];
                if (!String.IsNullOrWhiteSpace(col.ColumnName))
                {
                    dt.Columns.Add(col.ColumnName, col.DataType);
                }
            }
            for (int ri = 0; ri < Rows.Count; ri++)
            {
                DataGridRow row = Rows[ri];
                DataRow dr = dt.NewRow();
                try
                {
                    for (int ci = 0; ci < Columns.Count; ci++)
                    {
                        DataGridColumn col = Columns[ci];
                        if (!String.IsNullOrWhiteSpace(col.ColumnName))
                        {
                            switch (col.DataType.ToString())
                            {
                                case "System.Decimal":
                                    Decimal d;
                                    if (Decimal.TryParse(row[ci] as String, out d)) { dr[ci] = d; }
                                    else { dr[ci] = DBNull.Value; }
                                    break;
                                case "System.Guid":
                                    dr[ci] = (row[ci] == null) ? (new Guid()) : row[ci];
                                    break;
                                default:
                                    dr[ci] = (row[ci] == null) ? DBNull.Value : row[ci];
                                    break;
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return dt;
        }
        public void AddRows(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                DataGridRow r = NewRow();
                if ((dt.PrimaryKey != null) && (dt.PrimaryKey.Length > 0))
                {
                    r.Attributes.Add("data-primarykey", dr[dt.PrimaryKey[0]].ToString());
                }
                for (int ci = 0; ci < Columns.Count; ci++)
                {
                    String dbName = Columns[ci].ColumnName;
                    if (!String.IsNullOrWhiteSpace(dbName) && dt.Columns.Contains(dbName))
                    {
                        r[ci] = dr[dbName];
                    }
                }
                Rows.Add(r);
            }
        }

    }
    public class DataGridColumnCollection
    {
        private List<DataGridColumn> columns;
        public DataGridView DataGridView { get; set; }
        public DataGridColumnCollection(DataGridView parent)
        {
            if (parent == null) { throw new ArgumentNullException(); }

            columns = new List<DataGridColumn>();
            DataGridView = parent;
        }
        public Int32 Count { get { return columns.Count; } }
        public void Add(DataGridColumn column)
        {
            columns.Add(column);
            column.DataGridView = DataGridView;
            foreach (DataGridRow r in DataGridView.Rows)
            {
                r.AddCell();
            }
        }
        public IEnumerator GetEnumerator()
        {
            return columns.GetEnumerator();
        }
        public DataGridColumn this[Int32 index]
        {
            get
            {
                if ((columns != null) && (index < columns.Count)) return columns[index];
                else throw new IndexOutOfRangeException();
            }
        }
        public void RemoveAt(Int32 index)
        {
            columns.RemoveAt(index);
        }
    }
    public class DataGridRowCollection
    {
        private List<DataGridRow> rows;
        public DataGridView DataGridView { get; set; }
        public DataGridRowCollection(DataGridView parent)
        {
            if (parent == null) { throw new ArgumentNullException(); }

            rows = new List<DataGridRow>();
            DataGridView = parent;
        }
        public Int32 Count { get { return rows.Count; } }
        public void Add(DataGridRow row)
        {
            rows.Add(row);
        }
        public IEnumerator GetEnumerator()
        {
            return rows.GetEnumerator();
        }
        public DataGridRowCollection FindAll(Predicate<DataGridRow> match)
        {
            rows = rows.FindAll(match);
            return this;
        }
        public DataGridRow this[Int32 index]
        {
            get
            {
                if ((rows != null) && (index < rows.Count)) return rows[index];
                else throw new IndexOutOfRangeException();
            }
        }
        public void RemoveAt(Int32 index)
        {
            rows.RemoveAt(index);
        }
    }
    public class DataGridColumn
    {
        public Guid Id { get; private set; }
        public DataGridView DataGridView { get; set; }
        public String Caption { get; set; }
        public String ColumnName { get; set; }
        public Type DataType { get; set; }
        public Boolean IsUsedInFilter { get; set; }
        public Boolean IsVisible { get; set; }
        public String HeaderCellStyle { get; set; }
        public String DataCellStyle { get; set; }
        public String Width { get; set; }
        public DataGridColumn()
        {
            Id = Guid.NewGuid();
            DataGridView = null;
            Caption = null;
            DataType = typeof(Object);
            IsUsedInFilter = false;
            IsVisible = true;
            HeaderCellStyle = null;
            DataCellStyle = null;
            Width = null;
        }
    }
    public class DataGridRow
    {
        private List<DataGridCell> cells { get; set; }
        public DataGridView DataGridView { get; set; }
        public Dictionary<String, String> Attributes { get; set; }
        public String Style { get; set; }
        public Int32 Number { get; set; }
        public String Status { get; set; }
        public DataGridRow(DataGridView parent)
        {
            if (parent == null) { throw new ArgumentNullException(); }

            cells = new List<DataGridCell>(parent.Columns.Count);
            for (int ci = 0; ci < parent.Columns.Count; ci++)
            {
                cells.Add(new DataGridCell());
            }
            DataGridView = parent;
            Attributes = new Dictionary<String, String>();
            Style = null;
            Number = 0;
            Status = null;
        }
        public String GetAttributesAsHtmlString()
        {
            StringBuilder sb = new StringBuilder();
            if (Attributes != null)
            {
                foreach (KeyValuePair<String, String> attr in Attributes)
                {
                    sb.AppendFormat(" {0}=\"{1}\"", attr.Key, attr.Value);
                }
            }
            return sb.ToString();
        }
        public Object this[Int32 index]
        {
            get
            {
                Object value = null;
                if ((cells != null) && (index >= 0) && (index < cells.Count))
                {
                    value = cells[index].Value;
                    if (value == DBNull.Value) value = null;
                }
                else throw new IndexOutOfRangeException();
                return value;
            }
            set
            {
                if ((cells != null) && (index >= 0) && (index < cells.Count))
                {
                    Type type = DataGridView.Columns[index].DataType;
                    cells[index].Value = convertToColumnsDataType(type, value);
                }
                else throw new IndexOutOfRangeException();
            }
        }
        public Object this[String columnName]
        {
            get
            {
                for (int ci = 0; ci < DataGridView.Columns.Count; ci++)
                {
                    if (DataGridView.Columns[ci].ColumnName == columnName)
                    {
                        return this[ci];
                    }
                }
                throw new KeyNotFoundException();
            }
            set
            {
                for (int ci = 0; ci < DataGridView.Columns.Count; ci++)
                {
                    if (DataGridView.Columns[ci].ColumnName == columnName)
                    {
                        this[ci] = value;
                        return;
                    }
                }
                throw new KeyNotFoundException();
            }
        }
        public void AddCell()
        {
            cells.Add(new DataGridCell());
        }
        public Object[] ItemArray
        {
            get { return cells.ToArray(); }
            private set { }
        }
        private Object convertToColumnsDataType(Type targetType, Object value)
        {
            Object result = null;
            //if (value != null)
            {
                switch (targetType.ToString())
                {
                    case "System.Byte[]":
                        result = convertToColByteArray(value);
                        break;
                    case "System.DateTime":
                        result = convertToColDateTime(value);
                        break;
                        /*
                    case "System.Decimal":
                        result = convertToColDecimal(value);
                        break;
                         */
                    case "System.Double":
                        result = convertToColDouble(value);
                        break;
                    case "System.Int32":
                        result = convertToColInt32(value);
                        break;
                    case "System.String":
                        result = Convert.ToString(value, CultureInfo.InvariantCulture);
                        break;
                    default:
                        result = value;
                        break;
                }
            }
            return result;
        }
        private Object convertToColByteArray(Object value)
        {
            Object result = null;
            String temp = value as String;
            if ((value == null) || (value == DBNull.Value)) return null;
            if (temp != null)
            {
                if (String.IsNullOrWhiteSpace(temp)) return null;
                temp = temp.Replace("[", "").Replace("]", "");
                String[] ss = temp.Split(',');
                Byte[] bs = new Byte[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    Byte b = 0;
                    Byte.TryParse(ss[i], out b);
                    bs[i] = b;
                }
                result = bs;
            }
            return result;
        }
        private Object convertToColDateTime(Object value)
        {
            Object result = null;
            if ((value == null) || (value == DBNull.Value)) return null;
            String temp = value as String;
            if (temp != null)
            {
                if (String.IsNullOrWhiteSpace(temp)) return null;
                // убрать все пробелы
                value = (new Regex("\\s")).Replace(temp, "");
            }
            try
            {
                result = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            return result;
        }
        private Object convertToColDecimal(Object value)
        {
            Object result = null;
            try
            {
                if (value.GetType() == typeof(String))
                {
                    String temp = value as String;
                    if (!String.IsNullOrWhiteSpace(temp))
                    {
                        // убрать всё лишнее
                        temp = (new Regex(@"[^\d\.\,\+\-]")).Replace(temp, "");
                        if (temp.IndexOf('.') < 0)
                        {
                            // точки нет - значит запятая это разделитель дробной части и её надо заменить на точку
                            value = temp.Replace(',', '.');
                        }
                        else
                        {
                            // точка уже есть - значит запятые это разделители груп и их надо убрать
                            value = temp.Replace(",", "");
                        }
                        result = System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    result = System.Convert.ToDecimal(value);
                }
            }
            catch (Exception) { }
            return result;
        }
        private Object convertToColDouble(Object value)
        {
            Object result = null;
            if ((value == null) || (value == DBNull.Value)) return null;
            String temp = value as String;
            if (temp != null)
            {
                if (String.IsNullOrWhiteSpace(temp)) return null;
                // убрать всё лишнее
                temp = (new Regex(@"[^\d\.\,\+\-eE]")).Replace(temp, "");
                if (temp.IndexOf('.') < 0)
                {
                    // точки нет - значит запятая это разделитель дробной части и её надо заменить на точку
                    value = temp.Replace(',', '.');
                }
                else
                {
                    // точка уже есть - значит запятые это разделители груп и их надо убрать
                    value = temp.Replace(",", "");
                }
            }
            try
            {
                result = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch (Exception) { }

            return result;
        }
        private Object convertToColInt32(Object value)
        {
            Object result = null;
            if ((value == null) || (value == DBNull.Value)) return null;
            String temp = value as String;
            if (temp != null)
            {
                if (String.IsNullOrWhiteSpace(temp)) return null;
                // убрать всё лишнее
                value = (new Regex(@"[^\d\.\,\+\-eE]")).Replace(temp, "");
            }
            try
            {
                result = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            return result;
        }
    }
    public class DataGridCell
    {
        public Object Value { get; set; }
        public override String ToString()
        {
            String result = "";
            if (Value != null)
            {
                switch (Value.GetType().ToString())
                {
                    case "System.Byte[]":
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[");
                        foreach (Byte b in (Byte[])Value) { sb.Append(b); sb.Append(","); }
                        if (sb.Length > 1) sb.Length -= 1;
                        sb.Append("]");
                        result = sb.ToString();
                        break;
                    case "System.DateTime":
                        result = ((DateTime)Value).ToString("dd.MM.yyyy");
                        break;
                    case "System.Decimal":
                        result = ((Decimal)Value).ToString("f2");
                        break;
                    case "System.Double":
                        result = ((Double)Value).ToString("f2");
                        break;
                    default:
                        result = Value.ToString();
                        break;
                }
            }
            return result; //base.ToString();
        }
        public String ToFString(String format)
        {
            String result = "";
            if (Value != null)
            {
                switch (Value.GetType().ToString())
                {
                    case "System.Byte[]":
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[");
                        foreach (Byte b in (Byte[])Value) { sb.Append(b); sb.Append(","); }
                        if (sb.Length > 1) sb.Length -= 1;
                        sb.Append("]");
                        result = sb.ToString();
                        break;
                    case "System.DateTime":
                        result = ((DateTime)Value).ToString("yyyy-MM-dd");
                        break;
                    case "System.Decimal":
                        result = ((Decimal)Value).ToString(format ?? "f2");
                        break;
                    case "System.Double":
                        result = ((Double)Value).ToString(format ?? "f2");
                        break;
                    default:
                        result = Value.ToString();
                        break;
                }
            }
            return result;
        }
        public static implicit operator String(DataGridCell c) { return c.ToString(); }
        public String ToHtmlString()
        {
            return (new HtmlString(this.ToString())).ToHtmlString();
        }
    }
}
