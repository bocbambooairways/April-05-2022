using BOC.Areas.E_Library.Models;
using BOC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BOC.Areas.E_Library.Controllers
{
    [Area("E-Library")]//Declare Area
    public class FolderController : Controller
    {
        public UriConfig UriConfig { get; }
        public FolderController(Microsoft.Extensions.Options.IOptions<UriConfig> _UriConfig)
        {
            UriConfig = _UriConfig.Value;
        }
        public IActionResult Index()
        {
           return View(GetSearch_Doc_Folder_Get_Index());
        }
        public IActionResult M_Index()
        {
            return View(GetSearch_Doc_Folder_Get_Index());
        }

        List<Doc_Folder_Get> GetSearch_Doc_Folder_Get_Index()
        {
            string uri_GetSearch_Doc_Folder_Get = UriConfig.uri_GetSearch_Doc_Folder_Get;
            List<Doc_Folder_Get> GetSearch_Doc_Folder_Get = new DataAPI().GetListAPIWithoutParams<Doc_Folder_Get>(uri_GetSearch_Doc_Folder_Get,
                                      HttpContext.Session.GetString("Token"), HttpMethod.Post, false, "Data").Result;
            return GetSearch_Doc_Folder_Get;

        }

    }
}
