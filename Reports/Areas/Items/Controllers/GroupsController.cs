using FarmSib.MvcApplication2;
using MvcApplication2.Areas.Items.Models;
using Nskd;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace MvcApplication2.Areas.Items.Controllers
{
    public class GroupsController : Controller
    {
        public Object Index()
        {
            if (Request.Path != "/Items/Groups")
            {
                throw new ArgumentException();
            }
            Object result = "MvcApplication2.Areas.Items.Controllers.GroupsController.Index()";
            GroupsModel m = new GroupsModel();
            result = PartialView("~/Areas/Items/Views/Groups/Index.cshtml", m);
            return result;
        }
        public Object ApplyFilter()
        {
            Object result = "MvcApplication2.Areas.Items.Controllers.GroupsController.ApplyFilter()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            GroupsModel m = new GroupsModel();
            switch (rqp.Command)
            {
                case "Items.Groups.Index.GetTableForItemsOutGroup":
                    m.LoadItems(rqp);
                    result = PartialView("~/Areas/Items/Views/Groups/TableForItemsOutGroup.cshtml", m);
                    break;
                case "Items.Groups.Index.ApplyFilterForGroups":
                    m.LoadGroups(rqp);
                    result = PartialView("~/Areas/Items/Views/Groups/GroupPart.cshtml", m);
                    break;
                default:
                    break;
            }
            return result;
        }
        public Object UpsertGroup()
        {
            Object result = "MvcApplication2.Areas.Items.Controllers.GroupsController.UpsertGroup()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            GroupsModel m = new GroupsModel();
            m.UpsertGroup(rqp);
            result = PartialView("~/Areas/Items/Views/Groups/GroupPart.cshtml", m);
            return result;
        }
        public Object SelectGroup()
        {
            Object result = "MvcApplication2.Areas.Items.Controllers.GroupsController.SelectGroup()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            GroupsModel m = new GroupsModel();
            m.SelectGroup(rqp);
            result = PartialView("~/Areas/Items/Views/Groups/TableForItemsInGroup.cshtml", m);
            return result;
        }
        public Object CommitLog()
        {
            Object result = "MvcApplication2.Areas.Items.Controllers.GroupsController.CommitLog()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            GroupsModel m = new GroupsModel();
            m.CommitLog(rqp);
            return result;
        }
        public Object GetFieldValueList()
        {
            Object result = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            String fieldName = rqp["field"] as String;
            if (!String.IsNullOrWhiteSpace(fieldName))
            {
                String term = rqp["term"] as String;
                if (!String.IsNullOrWhiteSpace(term))
                {
                    rqp.Command = "Goods.dbo.get_field_value_list";
                    //rqp.AddSessionIdToParameters();
                    ResponsePackage rsp = rqp.GetResponse("http://" + NskdEnv.DATA_SERVICE_HOST + ":" + NskdEnv.DATA_SERVICE_HOST_SQL_PORT_V12 + "/");
                    DataTable dt = rsp.GetFirstTable();
                    if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 1 && dt.Columns[1].DataType == typeof(String))
                    {
                        String[] list = new String[dt.Rows.Count];
                        for (Int32 ri = 0; ri < dt.Rows.Count; ri++)
                        {
                            list[ri] = dt.Rows[ri][1] as String;
                        }
                        result = list;
                    }
                }
            }
            return Nskd.JsonV2.ToString(result);
        }
    }
}
