using System;
using System.Data;
using System.IO;

namespace TestWord
{
    class Program
    {
        private static String cnString = @"Data Source=.\sqlexpress;Initial Catalog=Pharm-Sib;Integrated Security=True";
        static void Main(string[] args)
        {
            DataTable dt = Db.ПолучитьТабличнуюЧастьСпецификации(cnString, 71659);
            using (FileStream file = File.Create(@"C:\Temp\TestWord.docx"))
            {
                MemoryStream ms = new GeneratedClass().CreatePackage(dt);
                ms.Position = 0;
                ms.CopyTo(file);
            }
        }
    }
}

