using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BOC.Models;
using Microsoft.AspNetCore.Http;

namespace BOC.Models
{
    public class ViewModelHS
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string BagFound_ID { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public string TotalAmount { get; set; }
        public string Currency { get; set; }
        public string Station { get; set; }
        public string DatePicker { get; set; }
        public string Remark { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<FileAttach_HS> FileAttach_HS { get; set; }
        public List<string> FileLoc_ID { get; set; }
        public string FileRemove { get; set; }
        public String RadioSelected { get; set; }
        public string KeySearch { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<BagMissHS_Model> Table_HS { get; set; }
        public IEnumerable<BaggageMissDescModel_HS> BagDesc { get; set; }
    }
    public class FileAttach_HS
    {
        public int BagMiss_ID { get; set; } = 0;
        public int FileLoc_ID { get; set; } = 0;
        public string FileName { get; set; } = "";
        public string sysFileName { get; set; } = "";
        public string Status { get; set; } = ""; //OK,XX,Empty


    }



}
