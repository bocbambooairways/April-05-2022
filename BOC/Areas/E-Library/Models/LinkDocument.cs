using BOC.Areas.E_Library.Data;
using BOC.Areas.E_Library.Models;
using BOC.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
namespace BOC.Areas.E_Library.Models
{
    public class LinkDocument
    {
        public void LoginToLinkDoc(string DocProfileID,string Device)
        {

            switch (Device)
            {
                case "Desktop":

                    break;
                case "Mobile":
                    break;
            }



          
        }
    }
}
