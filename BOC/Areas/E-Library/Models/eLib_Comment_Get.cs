using System.Collections.Generic;

namespace BOC.Areas.E_Library.Models
{
    public class eLib_Comment_Get
    {
        public string DocCommentID { get; set; }
        public string HeadID { get; set; }
        public string Comment { get; set; }
        public string LastUpdate { get; set; }
        public string UserUpdate { get; set; }

        public List<eLib_Comment_Get> ls_Reply { get; set; }
    }
}
