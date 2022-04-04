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

namespace BOC.Controllers
{
    public class LoungeController : Controller
    {
        //public static List<string> InvalidJsonElements;
        [HttpGet]
        public IActionResult Airport(LoungeModel model)
        {
            //Get Session Token
            var token = HttpContext.Session.GetString("Token");
            //Access API with Header
            Url airport = new Url();
            string uri = airport.Get("AirportList");

            HttpClient Client = new HttpClient();

            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri);

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

        [HttpGet]
        public IActionResult Airport_FLC(LoungeModel model)
        {
            //Get Session Token
            var token = HttpContext.Session.GetString("Token");
            //Access API with Header
            Url airport = new Url();
            string uri = airport.Get("AirportList");

            HttpClient Client = new HttpClient();

            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri);

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
