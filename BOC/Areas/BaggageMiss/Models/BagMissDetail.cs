using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.BaggageMiss.Models
{
    public class BaggageMissDetail
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public List<BaggageMissDetailModel> Data { get; set; }
    }
    public class BaggageMissDetailModel
    {
        public int ID { get; set; }
        public List<BagDesc> BagDesc { get; set; }
        public int BagMissDetail_ID { get; set; }
        public int BagMiss_ID { get; set; }
        public string Item { get; set; }
        public string UseDate { get; set; }
        public int Qty { get; set; }
        public int TotalAmount { get; set; }
        public string Currency { get; set; }
        public string Remark { get; set; }
        public String RadioSelected { get; set; }
        public string Status { get; set; }
        public string RecDate { get; set; }
        public int RecUserID { get; set; }
    }
   

}
