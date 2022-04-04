using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BOC.Models;
using BOC.Areas.SeatMap.Models;
using Newtonsoft.Json;
using System.Text;

namespace BOC.Areas.SeatMap.Controllers
{
    [Area("SeatMap")]
    public class HomeController : Controller
    {
        public IActionResult Index(string userAgent)
        {

            var token = GetToken();
            //Save Token
            HttpContext.Session.SetString("Token", token);
            string FlightID = HttpContext.Request.Query["FlightID"].ToString();
            ViewBag.FlightID = FlightID;
            string PNR = HttpContext.Request.Query["PNR"].ToString();
            ViewBag.PNR = PNR;
            string lang = HttpContext.Request.Query["Language"].ToString();
            //Save Session Lang
            HttpContext.Session.SetString("Language", lang);
            Url seat = new Url();
            string url = seat.Get("FS_SeatMap_Get");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("FlightID", FlightID));
            nvc.Add(new KeyValuePair<string, string>("PNR", PNR));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

            string ContentSeat;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            ContentSeat = res.Content.ReadAsStringAsync().Result;
            //var oDataSeat = JObject.Parse(ContentSeat);

            SeatMapModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<SeatMapModel>(ContentSeat);
        
            model.Message = model.Message.ToString() == "Thành công" ? "Success" : model.Message.ToString();


            return View(model);
        }
        public string GetToken()
        {
            //Get path url api AUTO SYSTEM LOGIN AND GET TOKEN
            Url login = new Url();
            string uri = login.Get("Login");
            HttpClient SysLogin = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("UserCode", "AUTO_WEB"));
            nvc.Add(new KeyValuePair<string, string>("Password", "-_5#4eT6AF'*B6ey78#P"));
            nvc.Add(new KeyValuePair<string, string>("DeviceID", "C11FCC37-16D6-11EB-BADE-000C29D93A49"));
            var reqsys = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string ContentSys;
            HttpResponseMessage res_sys;
            res_sys = SysLogin.SendAsync(reqsys).Result;
            ContentSys = res_sys.Content.ReadAsStringAsync().Result;
            var oDataSys = JObject.Parse(ContentSys);
            // Save  Session Token
            var token = oDataSys["Data"]["Token"].ToString();
            return token;
           
        }
       

        [HttpPost]
        public IActionResult SelectSeat(string t_Data,string t_SeatChoose,string t_Status)
        {
           
            //Loai bỏ dấu ngoặc kép ra khỏi chuỗi.
            t_SeatChoose =t_SeatChoose.Replace("\"", "");
            //Get Session 
            string token = HttpContext.Session.GetString("Token");
            //Phân tách FlightID và PNR
            String[] data = t_Data.Split(",");
            string FlightID = data[0];
            string PNR = data[1];

            Url seat = new Url();
            string url = seat.Get("FS_SeatMap_Book");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("FlightID", FlightID));
            nvc.Add(new KeyValuePair<string, string>("PNR", PNR));
            nvc.Add(new KeyValuePair<string, string>("SeatNo", t_SeatChoose.Trim()));
            nvc.Add(new KeyValuePair<string, string>("Status", t_Status));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            var lst = Newtonsoft.Json.JsonConvert.DeserializeObject<SeatMapModel>(Content);
            Int32 _Result = Int32.Parse(lst.ResultCode.ToString());
            string Message = lst.Message.ToString();
            return Json(new {result= _Result, mess = Message});
        }


        [HttpPost]
        public IActionResult DeleteSeat(string t_Data)
        {

          
            //Get Session 
            string token = HttpContext.Session.GetString("Token");
            //Phân tách FlightID và PNR
            String[] data = t_Data.Split(",");
            string FlightID = data[0];
            string PNR = data[1];

            Url seat = new Url();
            string url = seat.Get("FS_SeatMap_Clear");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("FlightID", FlightID));
            nvc.Add(new KeyValuePair<string, string>("PNR", PNR));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            var lst = Newtonsoft.Json.JsonConvert.DeserializeObject<SeatMapModel>(Content);
            Int32 _Result = Int32.Parse(lst.ResultCode.ToString());
            string Message = lst.Message.ToString();
            return Json(new { result = _Result, mess = Message });
        }
    }
}
