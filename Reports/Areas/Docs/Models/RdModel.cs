using FarmSib.Base.Data;
using FarmSib.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Docs.Models
{
    public class RdModel
    {
        public static DataGridView GetFsInfo(String alias, String path = null, String type = null)
        {
            String link = "{" + alias + "}" + path;
            String id;
            Byte[] hash = (MD5.Create()).ComputeHash(Encoding.Unicode.GetBytes(link));
            StringBuilder sb = new StringBuilder();
            foreach (Byte b in hash) { sb.AppendFormat("{0:x2}", b); }
            id = sb.ToString();

            DataTable dt = HomeData.Docs.Rd.GetFsInfo(id, link, type);
            DataGridView infoView = CreateInfoView(dt);
            return infoView;
        }
        private static DataGridView CreateInfoView(DataTable dt)
        {
            DataGridView fileInfoView = new DataGridView();

            //fileInfoView.Columns.Add(new DataGridColumn { Caption = "Код записи", DataType = typeof(Int32) });
            //fileInfoView.Columns.Add(new DataGridColumn { Caption = "Код файла", DataType = typeof(String) });
            //fileInfoView.Columns.Add(new DataGridColumn { Caption = "Код значения", DataType = typeof(Int32) });
            fileInfoView.Columns.Add(new DataGridColumn { Caption = "Наименование", DataType = typeof(String), DataCellStyle = "min-width: 200px;" });
            //fileInfoView.Columns.Add(new DataGridColumn { Caption = "Тип значения", DataType = typeof(String), DataCellStyle = "text-align: right;" });
            fileInfoView.Columns.Add(new DataGridColumn { Caption = "Значение", DataType = typeof(String), DataCellStyle = "min-width: 450px;" });

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    String name = (String)dr["name"];
                    String value = dr["value"] as String;
                    value = value ?? "";

                    DataGridRow r = fileInfoView.NewRow();
                    //r.Style = "font-weight: bold;";
                    r.Attributes.Add("data-record-id", ((Int32)dr["id"]).ToString());
                    r.Attributes.Add("data-file-id", dr["file_id"] as String);
                    r.Attributes.Add("data-nvp-id", ((Int32)dr["nvp_id"]).ToString());
                    r.Attributes.Add("data-value-type", dr["value_type"] as String);
                    r[0] = name;
                    r[1] = value;
                    fileInfoView.Rows.Add(r);
                }
            }
            return fileInfoView;
        }
    }
}