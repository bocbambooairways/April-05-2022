using System.Collections.Generic;

namespace BOC.Areas.E_Library.Models
{
    public class ProfileDetail
    {

        public string Author { get; set; }
        public string PublishDate { get; set; }
        public int AttachFileCount { get; set; }
        public int DocProfileID { get; set; }
        public string ISBN { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string DivisionCode { get; set; }

        public string PublishCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReadStatus { get; set; }
        public string Version { get; set; }
        public string LastUpdate { get; set; }
        public string UserUpdate { get; set; }
        public List<AttachedFiles> Attached_Files { get; set; }
    }
    public class AttachedFiles
    {
        public int ProfileDetailID { get; set; }
        public int FileLoc_ID { get; set; }
        public string Title { get; set; }
        public string DataType { get; set; }
        public string OriginalFileName { get; set; }
        public string sysFileName { get; set; }
        public string Status { get; set; }
        public string LastUpdate { get; set; }
        public string UserUpdate { get; set; }
       
        public string Folder { get; set; }
        public string ServerID { get; set; }

    }
}
