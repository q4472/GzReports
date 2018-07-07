using System;
using System.Data;
using System.Data.SqlClient;

namespace TestWord
{
    class Db
    {
        public static DataTable ПолучитьТабличнуюЧастьСпецификации(String cnString, Int32 КодСпецификации)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = @"
                    SELECT TOP (1000) 
                        [uid]
                      ,[id]
                      ,[код_спецификации]
                      ,[номер_строки]
                      ,[международное_непатентованное_наименование]
                      ,[наименование]
                      ,[лекарственная_форма_и_дозировка]
                      ,[производитель]
                      ,[страна]
                      ,[ед_изм]
                      ,[начальная_максимальная_цена]
                      ,[рег_цена]
                      ,[количество]
                      ,[цена_закуп]
                      ,[цена_по_спецификации]
                      ,[tp_id]
                      ,[номер_ру]
                      ,[требование]
                      ,[примечание]
                      ,[количество_в_требовании]
                      ,[ед_изм_в_требовании]
                      ,[вес]
                      ,[объём]
                      ,[bg_color]
                      ,[предельная_оптовая_цена]
                      ,[предельная_розничная_цена]
                      ,[цеховая_упаковка_вес]
                      ,[цеховая_упаковка_объём]
                      ,[цеховая_упаковка_количество]
                    FROM [Pharm-Sib].[dbo].[спецификации_таблица]
                    where [код_спецификации] = @КодСпецификации
                    ",
                CommandType = CommandType.Text,
                Connection = new SqlConnection(cnString),
            };
            cmd.Parameters.AddWithValue("КодСпецификации", КодСпецификации);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}
