using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class BagMissHs_Attached
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
}
