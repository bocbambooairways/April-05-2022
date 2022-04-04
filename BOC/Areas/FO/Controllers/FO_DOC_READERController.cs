using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BOC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Nancy.Json;


namespace BOC.Areas.FO.Controllers
{
    [Area("FO")]
    public class FO_DOC_READERController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(FlightViewModel model)
        {
            //Get Session Fleet And Reset Value
            //var fleet = HttpContext.Session.GetString("Fleet");
            //if (model.YourFleet == fleet)
            //{
            //    model.YourFleet = null;
            //}
            var fleet = model.YourFleet == null ? HttpContext.Session.GetString("Fleet") : model.YourFleet;
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Baver_ID))
            {
                model.ErrorMessage = "Data entered must not be blank!";
                TempData["Feedback"] = "1";
                return View(model);
            }
            else
            {
                // Get Session Token
                var token = GetToken();
                //Access API with Header
                Url fo_reader = new Url();
                string uri = fo_reader.Get("FO_Reader");
                HttpClient Client = new HttpClient();

                var nvc = new List<KeyValuePair<string, string>>();
                nvc.Add(new KeyValuePair<string, string>("Email", model.Email));
                nvc.Add(new KeyValuePair<string, string>("BAV_ID", model.Baver_ID));
                nvc.Add(new KeyValuePair<string, string>("AirCraft", model.YourFleet));
                var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

                string Content;
                HttpResponseMessage res;
                res = Client.SendAsync(req).Result;
                Content = res.Content.ReadAsStringAsync().Result;
                var oData = JObject.Parse(Content);

                //  Bind Json To List
                JavaScriptSerializer ser = new JavaScriptSerializer();
                List<FlightViewInfo> lst = ser.Deserialize<List<FlightViewInfo>>(oData["Data"].ToString());//str is JSON string.
                for (int i = 0; i < lst.Count; i++)
                {
                    lst[i].ID = i + 1;
                }
                ViewBag.Name = model.Email.ToString();
                if (lst.Count == 0)
                {
                    //Save Session Fleet in case NULL and if user access again
                    HttpContext.Session.SetString("Fleet", model.YourFleet);
                }
                else
                {
                    model.FlightInfo = lst;
                }
                return View(model);
            }
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
    }
}
