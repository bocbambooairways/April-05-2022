using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.SeatMap.Models
{
    public class SeatMapModel
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public Seat Data { get; set; }
    }
   
    public class Seat
    {
        public List<SeatData> ls { get; set; }
        public int col { get; set; }
        public int row { get; set; }
        public string FltNo { get; set; }
        public string FltDate { get; set; }
        public string Route { get; set; }
        public string Aircraft { get; set; }
        public string PNR { get; set; }
        public int TotalPax { get; set; }
        public string Language { get; set; }



    }
    public class SeatData
    {
        public List<SeatRow> SeatRow { get; set; }
        public int RowNo { get; set; }
    }
    public class SeatRow
    {
        public int row { get; set; }
        public int col { get; set; }
        public string SeatNo { get; set; }
        public string Attr { get; set; }
        public string PNR { get; set; }
    }
}
