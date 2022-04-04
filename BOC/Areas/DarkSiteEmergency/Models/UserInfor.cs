using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.DarkSiteEmergency.Models
{
    public class UserInfor
    {
        public string Token { get; set; }
        [Required(ErrorMessage = "Required.")]
        public string UserCode { get; set; }
        [Required(ErrorMessage = "Required.")]
        public string Password { get; set; }
        public string preview { get; set; }
        public Boolean Remember { get; set; }
        public string TypeOfDevice { get; set; }
    }
}
