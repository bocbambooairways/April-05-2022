using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.BaggageMiss.Models
{
    public class BagMissFileAttach
    {

        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public FileAttach Data { get; set; }

    }
    public class FileAttach
    {
        public int BagFiles_ID { get; set; }
        public int BagMiss_ID { get; set; }
        public string TableName { get; set; }
        public string FileName { get; set; }
        public string sysFileName { get; set; }
        public string Status { get; set; }
        public string ServerID { get; set; }
        public string FolderName { get; set; }
        public int FileLoc_ID { get; set; }
        public string LastUserUpdate { get; set; }
        public string LastUpdate { get; set; }


    }
    public class FileDisplay
    {
        public int FileLoc_ID { get; set; }
        public string FileName { get; set; }
        public string SysFileName { get; set; }
        public string LastUserUpdate { get; set; }
        public string LastUpdate { get; set; }


    }
}
