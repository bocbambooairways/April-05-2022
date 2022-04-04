using System.ComponentModel;

namespace BOC.Areas.E_Library.Models
{
    public class Doc_Folder_Get
    {
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public int RowCount { get; }
        public int ID { get; set; }
        public int HeadID { get; set; }
        public string Description { get; set; }
        public string InputDivision { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        //[DisplayName("")]
        //public string Folder { get; set; }
    }
}
