using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOC.Areas.BaggageMiss.Models;
using Microsoft.AspNetCore.Http;

namespace BOC.Areas.BaggageMiss.Models
{
    public class ViewModel
    {
        public IEnumerable<BaggageMissContactModel> MissContacts { get; set; }
        public IEnumerable<BaggageMissDetailModel> MissDetails { get; set; }
        public IEnumerable<BaggageMissDescModel> MissDesc { get; set; }
    }
    public class BagDesc
    {
        public int BagDesc_ID { get; set; }
        public int BagGroup_ID { get; set; }
        public string BagDetailCode { get; set; }
        public string Desc_EN { get; set; }
        public string Desc_VN { get; set; }
        public string BagGroupCode { get; set; }
        public string GroupName_EN { get; set; }
        public string GroupName_VN { get; set; }
        public string DatePicker { get; set; }
        public string Remark { get; set; }
        public List<IFormFile> files { get; set; }
        public string FileRemove { get; set; }
        public String RadioSelected { get; set; }
        public string DataType { get; set; }
        public string sysFileName { get; set; }
        public bool UserCheck { get; set; }


    }
}
