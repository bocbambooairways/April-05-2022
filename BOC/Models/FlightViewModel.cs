using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class FlightViewModel
    {
        public string Email { get; set; } 
        public string Baver_ID { get; set; }
        public string YourFleet { get; set; }
        public List<FlightViewInfo> FlightInfo { get; set; }
        public string ErrorMessage { get; set; }

    }
    public class FlightViewInfo
    {      
        public int ID { get; set; }    
        public int FODocReader_ID { get; set; } = 0;
        public int FODoc_ID { get; set; } = 0;
        public string Email { get; set; } = "";
        public string Baver_ID { get; set; } = "";
        public string FileName { get; set; } = "";
        public string ReadTime { get; set; } = "";
        public string Notify { get; set; } = "";
    }


}
