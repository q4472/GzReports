using FarmSib.Base.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmSib.Base.Models
{
    public class DgvModel
    {
        public static String Exec(Object[] log)
        {
            String msg = "";

            // убрать из списка команд 'add' если дальше есть 'delete' с тем же id
            // пробуем список замен
            Dictionary<String, Int32> exchangeList = new Dictionary<string, int>();

            foreach (Object o in log)
            {
                Hashtable ht = o as Hashtable;
                String fileId;
                String recordId;
                String columnIndex;
                String value;
                Int32 id = 0;
                if ((ht != null) && ht.Contains("cmd"))
                {
                    String cmd = ht["cmd"] as String;
                    switch (cmd)
                    {
                        case "add":
                            msg += " + " + cmd;
                            recordId = ht["recordId"] as String;
                            msg += " recordId: '" + recordId + "'";
                            fileId = ht["fileId"] as String;
                            msg += " fileId: '" + fileId + "'";
                            id = HomeData.Dgv.Add(fileId);
                            msg += " id: '" + id.ToString() + "'";
                            if (id != 0)
                            {
                                exchangeList.Add(recordId, id);
                            }
                            break;
                        case "delete":
                            msg += " + " + cmd;
                            recordId = ht["recordId"] as String;
                            msg += " recordId: '" + recordId + "'";
                            if (exchangeList.ContainsKey(recordId))
                            {
                                id = exchangeList[recordId];
                            }
                            else
                            {
                                Int32.TryParse(recordId, out id);
                            }
                            msg += " id: '" + id.ToString() + "'";
                            HomeData.Dgv.Delete(id);
                            break;
                        case "change":
                            msg += " + " + cmd;
                            recordId = ht["recordId"] as String;
                            msg += " recordId: '" + recordId + "'";
                            if (exchangeList.ContainsKey(recordId))
                            {
                                id = exchangeList[recordId];
                            }
                            else
                            {
                                Int32.TryParse(recordId, out id);
                            }
                            msg += " id: '" + id.ToString() + "'";
                            fileId = ht["fileId"] as String;
                            msg += " fileId: '" + fileId + "'";
                            columnIndex = ht["columnIndex"] as String;
                            msg += " columnIndex: '" + columnIndex + "'";
                            value = ht["value"] as String;
                            msg += " value: '" + value + "'";
                            if (columnIndex == "0")
                            {
                                HomeData.Dgv.ChangeName(id, value);
                            }
                            else
                            {
                                HomeData.Dgv.ChangeValue(id, value);
                            }
                            break;
                        default:
                            msg += " ? " + cmd;
                            break;
                    }
                }
            }
            return msg;
        }
    }
}