using System.ComponentModel;

namespace BOC.Areas.E_Library.Models
{
    public class SearchResult
    {
        public int ID { get; set; }
        public string DocProfileID { get; set; }
        public string ISBN { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string DivisionCode { get; set; }
        public string PublishCode { get; set; }
     
         public string FromDate { get; set; }
         //[DisplayName("FromDate")]
        public string ToDate { get; set; }
        public string ReadStatus { get; set; }
        public string Version { get; set; }
        public string LastUpdate { get; set; }
        public string UserUpdate { get; set; }
       

    }
}


