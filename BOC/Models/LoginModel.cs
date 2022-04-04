using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class LoginModel
    {
        [Required]
        [RegularExpression("[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}")]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string YourEmail { get; set; }
        public string EmployeeId { get; set; }
        public string ErrorMessage { get; set; }
        public bool Remember { get; set; }
        public string Result { get; set; }
        public string RequestPath { get; set; }
        public string TypeOfDevice { get; set; }
        public string WidthOfDevice { get; set; }
        public string DocProfileID { get; set; }


    }

}

