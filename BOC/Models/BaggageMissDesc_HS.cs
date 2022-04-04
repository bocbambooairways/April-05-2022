using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class BaggageMissDesc_HS
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public BaggageMissDescModel_HS Data { get; set; }
        

    }
   

    public class BaggageMissDescModel_HS
    {
        public List<BagDesc_HS> BagDesc { get; set; }
        public int BagFound_ID { get; set; }
        public string Airport { get; set; }
        public string BrandName { get; set; }
        public string HS_No { get; set; }
        public string HS_Date { get; set; }
        public int Qty { get; set; }
        public int TotalAmount { get; set; }
        public string Currency { get; set; }
        public string Remark { get; set; }
        public List<IFormFile> Uploads { get; set; }
        public string Status { get; set; }
   
    }
    public class BagDesc_HS
    {
        public int BagDesc_ID { get; set; }
        public int BagGroup_ID { get; set; }
        public string BagDetailCode { get; set; }
        public string Desc_EN { get; set; }
        public string Desc_VN { get; set; }
        public List<IFormFile> files { get; set; }
        public string FileRemove { get; set; }
        public String RadioSelected { get; set; }
        public string BagGroupCode { get; set; }
        public string GroupName_EN { get; set; }
        public string GroupName_VN { get; set; }
        public string DataType { get; set; }
        public string sysFileName { get; set; }
        public bool UserCheck { get; set; }


    }
   



}
