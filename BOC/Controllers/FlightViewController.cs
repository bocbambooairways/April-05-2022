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
    public class FlightViewController : Controller
    {
        public IActionResult BL()
        {
            LoungeModel model = new LoungeModel();
            //Get Session Airport
            model.AirportChoose = HttpContext.Session.GetString("Airport");
            //Get Session Token
            //var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTZWN0aW9uSUQiOiIxN2FjZDk1Yy1kNjcyLTQ4MGMtODFiOS00ZDdhYjk0YzdkZmMiLCJVc2VySUQiOjUxMiwiVXNlckNvZGUiOiJuaGF0bm1AYmFtYm9vYWlyd2F5cy5jb20iLCJVc2VyTmFtZSI6Im5oYXRubUBiYW1ib29haXJ3YXlzLmNvbSIsIkRldmljZSI6IkMxMUZDQzM3LTE2RDYtMTFFQi1CQURFLTAwMEMyOUQ5M0E0OSIsIkNyZWF0ZSI6IjIwMjEtMDItMThUMjE6MTA6NDAuNjk3MTI1OSswNzowMCIsIkV4cGlyZSI6IjIwMjEtMDItMTlUMjE6MTA6NDAuNjk3MTI1OSswNzowMCJ9.61145fea38d4b6d58faa1941c0f403418e96c6352cf595e11fbe17ebcc4f6f47";
            var token = HttpContext.Session.GetString("Token");
            //Access API with Header
            Url flight = new Url();
            string uri = flight.Get("FlightLounge");

            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("Airport", model.AirportChoose));
            nvc.Add(new KeyValuePair<string, string>("DomInt", "ALL"));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            var oData = JObject.Parse(Content);

            //Bind Json To List 
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<FlightLounge> lst = ser.Deserialize<List<FlightLounge>>(oData["Data"].ToString());//str is JSON string.
            model.ListFlightLounge = lst;

            //Get Session FlightLounge
            TempData["FlightLounge"] = HttpContext.Session.GetString("FlightLounge");
            return View(model);
        }
        [HttpPost]
        public IActionResult BL(string airport, string city)
        {
            LoungeModel model = new LoungeModel();
            model.AirportChoose = airport.Trim();
            model.CityChoose = city.Trim();
            if (String.IsNullOrEmpty(airport) || String.IsNullOrEmpty(city))
            {

                model.ErrorMessage = "Can't find information your flight.";
            }
            else
            {

                // Save  Session FlightLounge
                HttpContext.Session.SetString("FlightLounge", model.CityChoose);

                // Save  Session Airport
                HttpContext.Session.SetString("Airport", model.AirportChoose);

            }
            return View(model);
        }
        public IActionResult FLC()
        {
            LoungeModel model = new LoungeModel();
            //Get Session Airport
            model.AirportChoose = HttpContext.Session.GetString("Airport");
            //Get Session Token
            //var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTZWN0aW9uSUQiOiIxN2FjZDk1Yy1kNjcyLTQ4MGMtODFiOS00ZDdhYjk0YzdkZmMiLCJVc2VySUQiOjUxMiwiVXNlckNvZGUiOiJuaGF0bm1AYmFtYm9vYWlyd2F5cy5jb20iLCJVc2VyTmFtZSI6Im5oYXRubUBiYW1ib29haXJ3YXlzLmNvbSIsIkRldmljZSI6IkMxMUZDQzM3LTE2RDYtMTFFQi1CQURFLTAwMEMyOUQ5M0E0OSIsIkNyZWF0ZSI6IjIwMjEtMDItMThUMjE6MTA6NDAuNjk3MTI1OSswNzowMCIsIkV4cGlyZSI6IjIwMjEtMDItMTlUMjE6MTA6NDAuNjk3MTI1OSswNzowMCJ9.61145fea38d4b6d58faa1941c0f403418e96c6352cf595e11fbe17ebcc4f6f47";
            var token = HttpContext.Session.GetString("Token");
            //Access API with Header
            Url flight = new Url();
            string uri = flight.Get("FlightLounge");

            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("Airport", model.AirportChoose));
            nvc.Add(new KeyValuePair<string, string>("DomInt", "ALL"));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            var oData = JObject.Parse(Content);

            //Bind Json To List 
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<FlightLounge> lst = ser.Deserialize<List<FlightLounge>>(oData["Data"].ToString());//str is JSON string.
            model.ListFlightLounge = lst;

            //Get Session FlightLounge
            TempData["FlightLounge"] = HttpContext.Session.GetString("FlightLounge");
            return View(model);
        }
        [HttpPost]
        public ActionResult FLC(string airport, string city)
        {
            LoungeModel model = new LoungeModel();
            model.AirportChoose = airport.Trim();
            model.CityChoose = city.Trim();
            if (String.IsNullOrEmpty(airport) || String.IsNullOrEmpty(city))
            {

                model.ErrorMessage = "Can't find information your flight.";
            }
            else
            {

                // Save  Session FlightLounge
                HttpContext.Session.SetString("FlightLounge", model.CityChoose);

                // Save  Session Airport
                HttpContext.Session.SetString("Airport", model.AirportChoose);

            }
            return View(model);
        }
        public IActionResult FLC_Counter()
        {
            LoungeModel model = new LoungeModel();
            //Get Session Airport
            model.AirportChoose = HttpContext.Session.GetString("Airport");
            //Get Session Token
            //var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTZWN0aW9uSUQiOiIxN2FjZDk1Yy1kNjcyLTQ4MGMtODFiOS00ZDdhYjk0YzdkZmMiLCJVc2VySUQiOjUxMiwiVXNlckNvZGUiOiJuaGF0bm1AYmFtYm9vYWlyd2F5cy5jb20iLCJVc2VyTmFtZSI6Im5oYXRubUBiYW1ib29haXJ3YXlzLmNvbSIsIkRldmljZSI6IkMxMUZDQzM3LTE2RDYtMTFFQi1CQURFLTAwMEMyOUQ5M0E0OSIsIkNyZWF0ZSI6IjIwMjEtMDItMThUMjE6MTA6NDAuNjk3MTI1OSswNzowMCIsIkV4cGlyZSI6IjIwMjEtMDItMTlUMjE6MTA6NDAuNjk3MTI1OSswNzowMCJ9.61145fea38d4b6d58faa1941c0f403418e96c6352cf595e11fbe17ebcc4f6f47";
            var token = HttpContext.Session.GetString("Token");
            //Access API with Header
            Url flightstatus = new Url();
            string uri = flightstatus.Get("CheckInStatus");
            HttpClient Client = new HttpClient();

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("Airport", model.AirportChoose));
            nvc.Add(new KeyValuePair<string, string>("DomInt", "ALL"));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            var oData = JObject.Parse(Content);

            //Bind Json To List 
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<FlightRoute> lst = ser.Deserialize<List<FlightRoute>>(oData["Data"].ToString());//str is JSON string.
            model.ListFlightRoute = lst;

            //Get Session FlightLounge
            TempData["FlightLounge"] = HttpContext.Session.GetString("FlightLounge");

            return View(model);
        }
    }
}

