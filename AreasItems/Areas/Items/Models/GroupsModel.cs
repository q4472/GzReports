using AreasItems.Data;
using Nskd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AreasItems.Areas.Items.Models
{
    public class GroupsModel
    {
        public DataTable Groups = null;
        public DataTable GItems = null;
        public DataTable FItems = null;
        public void LoadGroups(RequestPackage rqp)
        {
            Groups = HomeData.Items.LoadGroups(rqp);
        }
        public void LoadItems(RequestPackage rqp) 
        {
            FItems = HomeData.Items.LoadItems(rqp);
        }
        public void UpsertGroup(RequestPackage rqp)
        {
            Groups = HomeData.Items.UpsertGroup(rqp);
        }
        public void SelectGroup(RequestPackage rqp)
        {
            GItems = HomeData.Items.SelectGroup(rqp);
        }
        public void CommitLog(RequestPackage rqp)
        {
            Object[] log = rqp["log"] as Object[];
            if (log != null)
            {
                foreach (Object o in log)
                {
                    Hashtable record = o as Hashtable;
                    try
                    {
                        String cmd = record["cmd"] as String;
                        Guid sessionId = rqp.SessionId;
                        Guid groupId = Guid.Parse(record["groupId"] as String);
                        Int32 srcId = Int32.Parse(record["srcId"] as String);
                        Int32 itemId = Int32.Parse(record["itemId"] as String);
                        switch (cmd)
                        {
                            case "add":
                                HomeData.Items.AddItemIntoGroup(sessionId, groupId, srcId, itemId);
                                break;
                            case "remove":
                                HomeData.Items.RemoveItemFromGroup(sessionId, groupId, srcId, itemId);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}