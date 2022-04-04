using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class FlightOneModel
    {
        public Items PaxEst_Full { get; set; }
        public Items PaxEst_Total { get; set; }
        public Items Pax_Full { get; set; }
        public Items Pax_Total { get; set; }
        public Items Crew { get; set; }
        public Items Cargo_Full { get; set; }
        public Items Mail { get; set; }
        public Items Co_Mat_Full { get; set; }
        public Items Fuel_Remmain { get; set; }
        public Items Fuel_Topup { get; set; }
        public Items Fuel_Trip { get; set; }
        public string FlightID { get; set; }
        public string TimeKey { get; set; }
        public Items Date { get; set; }
        public Items FltNo { get; set; }
        public Items RegisterNo { get; set; }
        public Items Aircraft { get; set; }
        public Items Route { get; set; }
        public Items DateTime_ATD { get; set; }
        public Items DateTime_ATA { get; set; }
        public Items STD { get; set; }
        public Items ETD { get; set; }
        public Items BDT { get; set; }
        public Items DoorClose { get; set; }
        public Items TOff { get; set; }
        public Items STA { get; set; }
        public Items ETA { get; set; }
        public Items TDown { get; set; }
        public Items ATD { get; set; }
        public Items ATA { get; set; }
        public Items Terminal { get; set; }
        public Items Gate { get; set; }
        public Items Belt_Dep { get; set; }
        public Items Bay_Dep { get; set; }
        public Items Bay_Arr { get; set; }
        public List<Conference> ConferenceFlight { get; set; }
        public List<VIP> VipFlight { get; set; }

        public List<FlightDoc> FlightDoc { get; set; }

    }
    public class Conference
    {
        public int id { get; set; }
        public int FlightID { get; set; }
        public int ConferenceID { get; set; }
        public int parentId { get; set; }
        public string MessageType { get; set; }
        public string ScheduleDate { get; set; }
        public string DivisionName { get; set; }
        public string Remark { get; set; }
        public string Scope { get; set; }
        public string Airport { get; set; }
        public string RouteType { get; set; }
        public Items Status { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string DeleteDate { get; set; }
        public string DeleteUser { get; set; }

    }
    public class VIP
    {
        public string Name { get; set; }
        public string Duty { get; set; }
        public string Remark { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string RecieveNote { get; set; }
        public string RecieveUser { get; set; }
        public string RecieveDate { get; set; }
        public Items ReceiveStatus { get; set; }
    }
    public class Items
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int Len { get; set; }
        public string BackColor { get; set; }
        public string FontColor { get; set; }
        public bool Blink { get; set; }
        public bool ReadOnly { get; set; }
             
    }
    public class FlightDoc
    {
        
        public int FlightDocID { get; set; }
        public int FlightID { get; set; }
        public string Airport { get; set; }
        public string RouteType { get; set; }
        public string DocumentType { get; set; }
        public string Content { get; set; }
        public string UserUpdate { get; set; }
        public string RecDate { get; set; }
        public string FileName { get; set; }
        public int OddEvent { get; set; }
    }
   
  
   
  
  
}
