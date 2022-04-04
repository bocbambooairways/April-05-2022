using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.Baggage.Models
{
    public class CheckModel
    {
        public string token { get; set; }
        public string PNR { get; set; }
        public string FltNo { get; set; }
        public string BBCNo { get; set; }       
        public string ErrorMessage { get; set; }
        public string Result { get; set; }
        public string TypeOfDevice { get; set; }
        public string WidthOfDevice { get; set; }
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public int BagMiss_Id { get; set; }
        public string FltDate { get; set; }
        public List<BagMissData> Data { get; set; }

    }
   
    public class BagMissData
    {
        public int ID { get; set; }
        public List<string> PassengerNameList { get; set; }
        public int BagMiss_ID { get; set; }
        public string ProfileNo { get; set; }
        public string ProfileDate { get; set; }
        public int FlightID { get; set; }
        public string FltNo { get; set; }
        public string PNR { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
        public string Remark { get; set; }
        public string CabinClass { get; set; }
        public string BBCNo { get; set; }
        public string Airport { get; set; }
        public string Status { get; set; }


    }



}
