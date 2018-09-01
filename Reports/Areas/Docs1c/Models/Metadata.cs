using System;

namespace MvcApplication2.Areas.Docs1c.Models
{
    public class Md
    {
        public static Object[] DH355ListTable = new Object[] { 
            //             0 field,  1 header,     2 type, 3 width, 4 alignment, 5 readonly, 6 hidden, 7 dbName,  8 dbType, 9 relTable
            new Object[] {    "Id",      "id",   "String",       0,      "Left",       true,     true,     "id",  "String",       null},
            new Object[] {  "Date",    "Дата", "DateTime",     100,     "Right",      false,    false,   "date",  "String",       null},
            new Object[] {"Number",   "Номер",   "String",     100,      "Left",      false,    false, "number",  "String",       null},
            new Object[] {   "Sum",   "Сумма",   "Double",     100,     "Right",      false,    false,    "sum", "Decimal",       null}
        };
        public static Object[] DH355DetailTable = new Object[] { 
            //                    0 field,         1 header,     2 type, 3 width, 4 alignment, 5 readonly, 6 hidden,       7 dbName,   8 dbType, 9 relTable
            new Object[] {           "Id",             "id",   "String",       0,      "Left",       true,     true,           "id",   "String",       null},
            new Object[] {         "Date",           "Дата", "DateTime",     100,     "Right",       true,    false,         "date",   "String",       null},
            new Object[] {       "Number",          "Номер",   "String",     100,      "Left",       true,    false,       "number",   "String",       null},
            new Object[] {          "Sum",          "Сумма",   "Double",     100,     "Right",       true,    false,          "sum",  "Decimal",       null},
            new Object[] {     "Tracking",        "Трекинг",   "String",     100,      "Left",      false,    false,     "tracking",   "String",       null},
            new Object[] {    "Sent_date",  "Дата отправки", "DateTime",     100,     "Right",      false,    false,    "sent_date", "DateTime",       null},
            new Object[] { "Recived_date", "Дата получения", "DateTime",     100,     "Right",      false,    false, "recived_date", "DateTime",       null}
        };
    }
}
