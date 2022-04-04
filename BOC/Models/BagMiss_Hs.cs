using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class BagMiss_Hs
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public BagMissHS_Model Data { get; set; }
    }
    public class BagMissHS_Model
    {
        public string[] BagDesc ;
        public int BagFound_ID { get; set; }
        public string HS_Type { get; set; }
        public string HS_No { get; set; }
        public DateTime HS_Date { get; set; }
        public string Airport { get; set; }
        public string BrandName { get; set; }
        public string Remark { get; set; }
        public int Qty { get; set; }
        public int TotalAmount { get; set; }
        public string Currency { get; set; }
        public string UserCreated { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUserUpdate { get; set; }

    }
}
