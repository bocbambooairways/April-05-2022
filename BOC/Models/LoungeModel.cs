using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class LoungeModel
    {
        public List<AirportList> ListAirport { get; set; }
        public List<FlightLounge> ListFlightLounge { get; set; }
        public List<FlightRoute> ListFlightRoute { get; set; }
        public string AirportChoose { get; set; }
        public string SelectedRouting { get; set; }
        public string Date { get; set; }
        public string Key { get; set; }
        public string TimeZone { get; set; }
        public string ViewType { get; set; }
        public bool AutoHide { get; set; }
        public string CityChoose { get; set; }
        public string ErrorMessage { get; set; }
        public int Success { get; set; }
    }

    public class AirportList
    {
        public int ID { get; set; }
        public string Airport { get; set; }
        public string AirportName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }
    public class FlightLounge
    {
        public string STD { get; set; }
        public string ETD { get; set; }
        public string CityName { get; set; }
        public string FlightNo { get; set; }
        public string AirportName { get; set; }
        public string Gate { get; set; }
        public string Remark { get; set; }
        public string FltStatus { get; set; }
    }
    public class FlightRoute
    {
        public string Info { get; set; }
 
    }
 


}
