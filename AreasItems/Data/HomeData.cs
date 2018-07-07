using Nskd;
using System;
using System.Data;

namespace AreasItems.Data
{
    public class HomeData
    {
        public class Home
        {
            public static DataTable GetMnnItems(String[] srcs)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage
                {
                    Command = "[dbo].[мнн]",
                };
                if (srcs != null)
                {
                    rqp.Parameters = new RequestParameter[srcs.Length];
                    for (int i = 0; i < Math.Min(6, srcs.Length); i++)
                    {
                        String name = "src" + (i + 1).ToString();
                        String value = srcs[i];
                        rqp.Parameters[i] = new RequestParameter(name, value);
                    }
                }
                dt = FarmSib.Base.Data.HomeData.GetFirstTable(FarmSib.Base.Data.HomeData.Execute(rqp));
                return dt;
            }
            public static DataSet GetTnItems(String[] srcs, String[] terms, String mnn, Boolean exactly, Boolean eRu, Int32 pageNumber)
            {
                DataSet ds = null;
                RequestPackage rqp = new RequestPackage
                {
                    Command = "[dbo].[тн]"
                };
                Int32 pi = 0;
                if (srcs != null)
                {
                    Int32 len =
                        Math.Min(6, ((srcs == null) ? 0 : srcs.Length)) +   // srcs
                        Math.Min(9, ((terms == null) ? 0 : terms.Length)) + // terms
                        ((mnn == null) ? 0 : 1) +                           // mnn
                        1 +                                                 // exactly
                        1 +                                                 // eRu
                        1;                                                  // pageNumber
                    rqp.Parameters = new RequestParameter[len];
                    for (int i = 0; i < Math.Min(6, srcs.Length); i++)
                    {
                        String name = "src" + (i + 1).ToString();
                        String value = srcs[i];
                        rqp.Parameters[pi++] = new RequestParameter(name, value);
                    }
                }
                if (terms != null)
                {
                    for (int i = 0; i < Math.Min(9, terms.Length); i++)
                    {
                        String name = "term" + (i + 1).ToString();
                        String value = terms[i];
                        rqp.Parameters[pi++] = new RequestParameter(name, value);
                    }
                }
                if (mnn != null)
                {
                    rqp.Parameters[pi++] = new RequestParameter("mnn", mnn);
                }
                rqp.Parameters[pi++] = new RequestParameter("exactly", exactly);
                rqp.Parameters[pi++] = new RequestParameter("eRu", eRu);
                rqp.Parameters[pi++] = new RequestParameter("PageNumber", pageNumber);
                ds = FarmSib.Base.Data.HomeData.Execute(rqp);
                return ds;
            }
            public static DataSet GetTnItemsLP(String rn)
            {
                DataSet ds = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[dbo].[тн_цены]";
                rqp.Parameters = new RequestParameter[1];
                rqp.Parameters[0] = new RequestParameter("rn", rn);
                ds = FarmSib.Base.Data.HomeData.Execute(rqp);
                return ds;
            }
        }
        public class Items
        {
            public static DataTable LoadGroups(RequestPackage rqp)
            {
                DataTable dt = null;
                // меняем только команду
                rqp.Command = "[Goods].[dbo].[item_groups_get]";
                dt = FarmSib.Base.Data.HomeData.GetFirstTable(FarmSib.Base.Data.HomeData.Execute(rqp));
                return dt;
            }
            public static DataTable LoadItems(RequestPackage rqp)
            {
                DataTable dt = null;
                // меняем только команду
                rqp.Command = "[Goods].[dbo].[items_get]";
                dt = FarmSib.Base.Data.HomeData.GetFirstTable(FarmSib.Base.Data.HomeData.Execute(rqp));
                return dt;
            }
            public static DataTable UpsertGroup(RequestPackage rqp)
            {
                DataTable dt = null;
                /*
                switch (rqp.Command)
                {
                    case "Items.Groups.Index.InsertGroup":
                        // меняем только команду
                        rqp.Command = "[Pharm-Sib].[dbo].[item_groups_insert]";
                        dt = GetFirstTable(Execute(rqp));
                        break;
                    default:
                        break;
                }
                */
                return dt;
            }
            public static DataTable SelectGroup(RequestPackage rqp)
            {
                DataTable dt = null;
                switch (rqp.Command)
                {
                    case "Items.Groups.GroupPart.SelectGroup":
                        // меняем только команду
                        rqp.Command = "[Goods].[dbo].[item_groups_get_items_in_group]";
                        dt = FarmSib.Base.Data.HomeData.GetFirstTable(FarmSib.Base.Data.HomeData.Execute(rqp));
                        break;
                    default:
                        break;
                }
                return dt;
            }
            public static void AddItemIntoGroup(Guid sessionId, Guid groupId, Int32 srcId, Int32 itemId)
            {
                /*
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = sessionId;
                rqp.Command = "[Pharm-Sib].[dbo].[item_groups_insert_item]";
                rqp.Parameters = new RequestParameter[] { 
                    new RequestParameter { Name = "session_id", Value = sessionId },
                    new RequestParameter { Name = "group_id", Value = groupId },
                    new RequestParameter { Name = "src_id", Value = srcId },
                    new RequestParameter { Name = "item_id", Value = itemId }
                };
                ResponsePackage rsp = ExecuteInSql(rqp);
                */
            }
            public static void RemoveItemFromGroup(Guid sessionId, Guid groupId, Int32 srcId, Int32 itemId)
            {
                /*
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = sessionId;
                rqp.Command = "[Pharm-Sib].[dbo].[item_groups_remove_item]";
                rqp.Parameters = new RequestParameter[] { 
                    new RequestParameter { Name = "session_id", Value = sessionId },
                    new RequestParameter { Name = "group_id", Value = groupId },
                    new RequestParameter { Name = "src_id", Value = srcId },
                    new RequestParameter { Name = "item_id", Value = itemId }
                };
                ResponsePackage rsp = ExecuteInSql(rqp);
                */
            }
        }
        public class Tn
        {
            public static String GetRegCopyUri(Int32 copyCode)
            {
                String uri = "{q}/" + copyCode.ToString();
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = new Guid();
                rqp.Command = "[Grls].[dbo].[рз_получить_сслылку_на_копию_ру]";
                rqp.Parameters = new RequestParameter[] { new RequestParameter("id", copyCode) };
                ResponsePackage rsp = FarmSib.Base.Data.HomeData.ExecuteInSql(rqp);
                uri = FarmSib.Base.Data.HomeData.GetScalar(rsp.Data) as String;
                return uri;
            }
            public static String[] GetInstructionsLinks(Guid id)
            {
                String[] iLinks = new String[] { null, null };
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = new Guid();
                rqp.Command = "[Grls].[dbo].[рз_получить_сслылки_на_инструкции]";
                rqp.Parameters = new RequestParameter[] { new RequestParameter("Код_в_источнике", id) };
                ResponsePackage rsp = FarmSib.Base.Data.HomeData.ExecuteInSql(rqp);
                if (rsp != null)
                {
                    DataTable dt = FarmSib.Base.Data.HomeData.GetFirstTable(rsp.Data);
                    if (dt != null && dt.Rows.Count == 1 && dt.Columns.Count == 2)
                    {
                        iLinks[0] = dt.Rows[0][0] as String;
                        iLinks[1] = dt.Rows[0][1] as String;
                    }
                }
                return iLinks;
            }
            public static DataSet Get1cData(Guid sessionId, String rCode, String fCode, String uCode)
            {
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = sessionId;
                rqp.Command = "[Pharm-Sib].[dbo].[item_get_1c_data]";
                rqp.Parameters = new RequestParameter[] { 
                    new RequestParameter { Name = "session_id", Value = sessionId },
                    new RequestParameter { Name = "r_code", Value = rCode },
                    new RequestParameter { Name = "f_code", Value = fCode },
                    new RequestParameter { Name = "u_code", Value = uCode }
                };
                DataSet ds = FarmSib.Base.Data.HomeData.Execute(rqp);
                return ds;
            }
            public static DataSet GetUData(Guid sessionId, String fCode)
            {
                RequestPackage rqp = new RequestPackage();
                rqp.SessionId = sessionId;
                rqp.Command = "[Pharm-Sib].[dbo].[item_get_u_data]";
                rqp.Parameters = new RequestParameter[] { 
                    new RequestParameter { Name = "session_id", Value = sessionId },
                    new RequestParameter { Name = "f_code", Value = fCode }
                };
                DataSet ds = FarmSib.Base.Data.HomeData.Execute(rqp);
                return ds;
            }
        }
    }
}
