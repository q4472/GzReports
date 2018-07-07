using AreasItems.Areas.Items.Models;
using Nskd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AreasItems.Areas.Items.Controllers
{
    public class GroupsController : Controller
    {
        public Object Index()
        {
            if (Request.Path != "/Items/Groups")
            {
                throw new ArgumentException();
            }
            Object result = "AreasItems.Areas.Items.Controllers.GroupsController.Index()";
            GroupsModel m = new GroupsModel();
            result = PartialView("~/Areas/Items/Views/Groups/Index.cshtml", m);
            return result;
        }
        public Object ApplyFilter()
        {
            Object result = "AreasItems.Areas.Items.Controllers.GroupsController.ApplyFilter()";
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
            Object result = "AreasItems.Areas.Items.Controllers.GroupsController.UpsertGroup()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            GroupsModel m = new GroupsModel();
            m.UpsertGroup(rqp);
            result = PartialView("~/Areas/Items/Views/Groups/GroupPart.cshtml", m);
            return result;
        }
        public Object SelectGroup()
        {
            Object result = "AreasItems.Areas.Items.Controllers.GroupsController.SelectGroup()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            GroupsModel m = new GroupsModel();
            m.SelectGroup(rqp);
            result = PartialView("~/Areas/Items/Views/Groups/TableForItemsInGroup.cshtml", m);
            return result;
        }
        public Object CommitLog()
        {
            Object result = "AreasItems.Areas.Items.Controllers.GroupsController.CommitLog()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            GroupsModel m = new GroupsModel();
            m.CommitLog(rqp);
            return result;
        }
    }
}
