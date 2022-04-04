using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace BOC.Models
{
    public class SaveLog
    {
        public static void WriteLog(object chuoi)
        {
            string noidung = DateTime.Now.ToString() + ":" + chuoi.ToString() + Environment.NewLine;
            File.AppendAllText(@"C:\Logs\Log.txt", noidung);



        }
    }
}
