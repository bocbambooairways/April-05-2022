using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.DarkSiteEmergency.Models
{
    public class Data
    {
        public int DaskSiteID { get; set; }
        public string DataType { get; set; }
        public string Status { get; set; }
        public string Html_Data { get; set; }
        public string Html_Short { get; set; }
        public List<Data> lstData { get; set; }
    }
}
