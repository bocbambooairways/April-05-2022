using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BOC.Models;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using Nancy.Json;

namespace BOC.Areas.AirPort.Controllers
{
    [Area("STV")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index(LoungeModel model)
        {
            
            //Get path url api AUTO SYSTEM LOGIN AND GET TOKEN
            Url login = new Url();
            string uri = login.Get("Login");
            HttpClient SysLogin = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("UserCode", "Sys_UserSmart_TV"));
            nvc.Add(new KeyValuePair<string, string>("Password", "PeMku0sYxC"));
            nvc.Add(new KeyValuePair<string, string>("DeviceID", "C11FCC37-16D6-11EB-BADE-000C29D93A49"));
            var reqsys = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string ContentSys;
            HttpResponseMessage res_sys;
            res_sys = SysLogin.SendAsync(reqsys).Result;
            ContentSys = res_sys.Content.ReadAsStringAsync().Result;
            var oDataSys = JObject.Parse(ContentSys);
            // Save  Session Token
            var token = oDataSys["Data"]["Token"].ToString();
            HttpContext.Session.SetString("Token", token);
            

             //   var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTZWN0aW9uSUQiOiIxN2FjZDk1Yy1kNjcyLTQ4MGMtODFiOS00ZDdhYjk0YzdkZmMiLCJVc2VySUQiOjUxMiwiVXNlckNvZGUiOiJuaGF0bm1AYmFtYm9vYWlyd2F5cy5jb20iLCJVc2VyTmFtZSI6Im5oYXRubUBiYW1ib29haXJ3YXlzLmNvbSIsIkRldmljZSI6IkMxMUZDQzM3LTE2RDYtMTFFQi1CQURFLTAwMEMyOUQ5M0E0OSIsIkNyZWF0ZSI6IjIwMjEtMDItMThUMjE6MTA6NDAuNjk3MTI1OSswNzowMCIsIkV4cGlyZSI6IjIwMjEtMDItMTlUMjE6MTA6NDAuNjk3MTI1OSswNzowMCJ9.61145fea38d4b6d58faa1941c0f403418e96c6352cf595e11fbe17ebcc4f6f47";
            string url = "http://boc_api.bambooairways.com:8080/AirportList";
            HttpClient Client = new HttpClient();

            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, url);

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            var oData = JObject.Parse(Content);

            //Bind Json To List 
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<AirportList> lst = ser.Deserialize<List<AirportList>>(oData["Data"].ToString());//str is JSON string.
            model.ListAirport = lst;
            return View(model);
        }
    }
   
}
