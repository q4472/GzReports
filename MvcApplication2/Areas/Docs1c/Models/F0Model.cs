using FarmSib.Base.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MvcApplication2.Areas.Docs1c.Models
{
    public class F0Model
    {
        public DataTable WorkingData;
        public DataTable FilterData;
        public DataTable DetailData;
        public void GetData(Dictionary<String, String> fs)
        {
            WorkingData = HomeData.Docs1c.LoadDH355(fs);
            FilterData = new DataTable();
            foreach (Object[] fmd in Md.DH355ListTable)
            {
                DataColumn col = WorkingData.Columns[(String)fmd[7]];
                col.Caption = (String)fmd[1];
                col.ExtendedProperties.Add("width", fmd[3]);
                col.ExtendedProperties.Add("alignment", fmd[4]);
                col.ExtendedProperties.Add("readonly", fmd[5]);
                col.ExtendedProperties.Add("hidden", fmd[6]);
                col.ExtendedProperties.Add("dbName", fmd[7]);
                col.ExtendedProperties.Add("relTable", fmd[9]);

                col = FilterData.Columns.Add((String)fmd[7], Type.GetType("System." + (String)fmd[2]));
                col.Caption = (String)fmd[1];
                col.ExtendedProperties.Add("width", fmd[3]);
                col.ExtendedProperties.Add("alignment", fmd[4]);
                col.ExtendedProperties.Add("readonly", fmd[5]);
                col.ExtendedProperties.Add("hidden", fmd[6]);
                col.ExtendedProperties.Add("dbName", fmd[7]);
                col.ExtendedProperties.Add("relTable", fmd[9]);
            }
            FilterData.Rows.Add(FilterData.NewRow());
            FilterData.Rows.Add(FilterData.NewRow());
        }
        public void GetDetail(Dictionary<String, String> fs)
        {
            DetailData = HomeData.Docs1c.LoadDH355Detail(fs);
            foreach (Object[] fmd in Md.DH355DetailTable)
            {
                DataColumn col = DetailData.Columns[(String)fmd[7]];
                col.Caption = (String)fmd[1];
                col.ExtendedProperties.Add("width", fmd[3]);
                col.ExtendedProperties.Add("alignment", fmd[4]);
                col.ExtendedProperties.Add("readonly", fmd[5]);
                col.ExtendedProperties.Add("hidden", fmd[6]);
                col.ExtendedProperties.Add("dbName", fmd[7]);
                col.ExtendedProperties.Add("relTable", fmd[9]);
            }
        }
        public void Save(Dictionary<String, String> fs)
        {
            HomeData.Docs1c.Save(fs);
        }
    }
}