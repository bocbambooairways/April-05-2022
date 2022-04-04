using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class FlightModel
    {

        public List<FlightViewInfo> FlightInfo { get; set; }
        public string ErrorMessage { get; set; }
        public List<AirportLists> ListAirport { get; set; }
        public List<Flight> Ds { get; set; } = new List<Flight>();
        public string AirportChoose { get; set; }
        public string SelectedRouting { get; set; }
        public string Date { get; set; }
        public string Key { get; set; }
        public string TimeZone { get; set; }
        public string ViewType { get; set; }
        public bool AutoHide { get; set; }
        public string CityChoose { get; set; }
        public int Success { get; set; }
    }

    public class AirportLists
    {
        public int ID { get; set; }
        public string Airport { get; set; }
        public string AirportName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }
    public class Flight
    {
        public int ID { get; set; } = 0;
        public int FlightID { get; set; }
        public string? TimeKey { get; set; }

        public string Date { get; set; } = "";
        public string Date_Color { get; set; } = "";

        public string FltNo { get; set; } = "";
        public string FltNo_Color { get; set; } = "";
        public string RegisterNo { get; set; } = "";
        public string RegisterNo_Color { get; set; } = "";
        public string Aircraft { get; set; } = "";
        public string Aircraft_Color { get; set; } = "";
        public string Route { get; set; } = "";
        public string Route_Color { get; set; } = "";

        public string DateTime_ATA { get; set; } = "";
        public string DateTime_ATA_Color { get; set; } = "";
        public string STD { get; set; } = "";
        public string STD_Color { get; set; } = "";
        public string ETD { get; set; } = "";
        public string ETD_Color { get; set; } = "";
        public string BDT { get; set; } = "";
        public string BDT_Color { get; set; } = "";
        public string DoorClose { get; set; } = "";
        public string DoorClose_Color { get; set; } = "";
        public string TOff { get; set; } = "";
        public string TOff_Color { get; set; } = "";
        public string STA { get; set; } = "";
        public string STA_Color { get; set; } = "";
        public string ETA { get; set; } = "";
        public string ETA_Color { get; set; } = "";
        public string TDown { get; set; } = "";
        public string TDown_Color { get; set; } = "";
        public string ATD { get; set; } = "";
        public string ATD_Color { get; set; } = "";
        public string ATA { get; set; } = "";
        public string ATA_Color { get; set; } = "";
        public string Terminal { get; set; } = "";
        public string Terminal_Color { get; set; } = "";
        public string Gate { get; set; } = "";
        public string Gate_Color { get; set; } = "";
        public string Belt_Dep { get; set; } = "";
        public string Belt_Dep_Color { get; set; } = "";
        public string Bay_Dep { get; set; } = "";
        public string Bay_Dep_Color { get; set; } = "";
        public string Bay_Arr { get; set; } = "";
        public string Bay_Arr_Color { get; set; } = "";

    } 



}
