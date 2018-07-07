using FarmSib.Base.Data;
using FarmSib.Base.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MvcApplication2.Areas.Reports.Models
{
    public class R1Model
    {
        public ClientData Client { get; set; }
        public ManagerData Emploee { get; set; }

        //[DisplayFormat(DataFormatString="{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime OnDate { get; set; }
        public Int32? Pr1 { get; set; }
        public Int32? Pr2 { get; set; }
        public Boolean IsSudChecked { get; set; }
        public Boolean IsOrdChecked { get; set; }
        public Boolean IsSecondCopyRecived { get; set; }
        public DataTable ReportData { get; set; }
        public String Msg { get; set; }

        public R1Model()
        {
            Client = new ClientData();
            Emploee = new ManagerData();
            OnDate = DateTime.Now;
            IsSudChecked = true;
            IsOrdChecked = true;
            IsSecondCopyRecived = false;
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

            ReportData = HomeData.Reports.GetReport1(
                Client.ClientFilter,
                Client.ClientSelector,
                Emploee.ManagerFilter,
                Emploee.ManagerSelector,
                managerMultiCode,
                Pr1,
                Pr2,
                OnDate,
                IsSudChecked,
                IsOrdChecked,
                IsSecondCopyRecived
                );
        }
    }
}
