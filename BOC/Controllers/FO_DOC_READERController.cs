using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BOC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Nancy.Json;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BOC.Controllers
{
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
            var fleet = HttpContext.Session.GetString("Fleet");
            if(model.YourFleet==fleet)
            {
                model.YourFleet = null;
            }
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Baver_ID))
            {
                model.ErrorMessage = "Data entered must not be blank!";
                TempData["Feedback"] = "1";
                return View(model);
            }
            else
            {
                // Get Session Token
                var token = HttpContext.Session.GetString("Token");
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
                    lst[i].ID = i+1;
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
    }
}
