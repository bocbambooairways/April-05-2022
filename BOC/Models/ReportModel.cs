using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class ReportModel
    {
        public string FromDate1 { get; set; }
        public string ToDate1 { get; set; }
        public string FromDate2 { get; set; }
        public string ToDate2 { get; set; }
        public string ErrorMessage { get; set; }
        public List<RPT> Report1 { get; set; }

    }
    public class RPT
    {
        public int ID { get; set; }
        public string Station { get; set; }
        public string AirportName { get; set; }
        public int FltKy1 { get; set; }
        public int FltKy2 { get; set; }
        public int PaxC_Ky1 { get; set; }
        public int PaxC_Ky2 { get; set; }
        public int PaxY_Ky1 { get; set; }
        public int PaxY_Ky2 { get; set; }
        public int Config_Ky1 { get; set; }
        public int Config_Ky2 { get; set; }
        public int Tong_Ky1 { get; set; }
        public int Tong_Ky2 { get; set; }
        public string Load_Factor1 { get; set; }
        public string Load_Factor2 { get; set; }
    }
}
