using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.E_Library.Models
{
    public class Search
    {
        public string PublishID { get; set; }
        public string PublishCode { get; set; }
        public dynamic PublishName { get; set; }
        public string Status { get; set; }
        public string DocDivID { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string UnRead { get; set; }
        public string Newest { get; set; }
        public string isdn { get; set; }
        public string KeySearch { get; set; }
        public string Author { get; set; }

        public Boolean CheckRead { get; set; }
        public Boolean CheckNews { get; set; }
        public string ReceivedDate { get; set; }
        public string PublishDate { get; set; }
        public List<Search> lstData { get; set; }
    }
}
