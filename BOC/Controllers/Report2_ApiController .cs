using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOC.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Nancy.Json;
using DevExtreme.AspNet.Data;

namespace BOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Report2_ApiController : ControllerBase
    {
        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions,  string fromdate1, string todate1, string fromdate2, string todate2)
        {
            // Get Session Token
            var token = HttpContext.Session.GetString("Token");
            //Access API with Header
            Url flightloadfactor = new Url();
            string uri = flightloadfactor.Get("RouteLoadFactor");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("From_1", fromdate1));
            nvc.Add(new KeyValuePair<string, string>("To_1", todate1));
            nvc.Add(new KeyValuePair<string, string>("From_2", fromdate2));
            nvc.Add(new KeyValuePair<string, string>("To_2", todate2));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;

            JToken _Report1Item;
            JObject ser = JObject.Parse(Content);
            Int32 _Result = (int)ser.SelectToken("ResultCode");

            List<RPT> lst = new List<RPT>();
            var ser2 = ser.SelectToken("Data");
            List<JToken> data = new List<JToken>(ser2.Children());

            foreach (var item in data)
            {
                RPT lstrpt2 = new RPT();

                _Report1Item = item.SelectToken("Station");
                lstrpt2.Station = _Report1Item.ToString();

                _Report1Item = item.SelectToken("AirportName");
                lstrpt2.AirportName = _Report1Item.ToString();

                _Report1Item = item.SelectToken("FltKy1");
                lstrpt2.FltKy1 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("FltKy2");
                lstrpt2.FltKy2 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("PaxC_Ky1");
                lstrpt2.PaxC_Ky1 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("PaxC_Ky2");
                lstrpt2.PaxC_Ky2 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("PaxY_Ky1");
                lstrpt2.PaxY_Ky1 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("PaxY_Ky2");
                lstrpt2.PaxY_Ky2 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("Config_Ky1");
                lstrpt2.Config_Ky1 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("Config_Ky2");
                lstrpt2.Config_Ky2 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("Tong_Ky1");
                lstrpt2.Tong_Ky1 = Int32.Parse(_Report1Item.ToString());

                _Report1Item = item.SelectToken("Tong_Ky2");
                lstrpt2.Tong_Ky2 = Int32.Parse(_Report1Item.ToString());

                if (lstrpt2.Config_Ky1 == 0)
                {
                    
                    lstrpt2.Load_Factor1 =  "0%";
                }
                else
                {
                     int loadfactor1 = (int)Math.Round(((double)(100 * lstrpt2.Tong_Ky1) / lstrpt2.Config_Ky1),2);
                     lstrpt2.Load_Factor1 = loadfactor1.ToString() + '%';
                }
                if (lstrpt2.Config_Ky2 == 0)
                {

                    lstrpt2.Load_Factor2 = "0%";
                }
                else
                {
                    int loadfactor2 = (int)Math.Round(((double)(100 * lstrpt2.Tong_Ky2) / lstrpt2.Config_Ky2),2);
                    lstrpt2.Load_Factor2 = loadfactor2.ToString() + '%';
                }

                lst.Add(lstrpt2);
            }
            for (int i = 0; i < lst.Count; i++)
            {
                lst[i].ID = i + 1;
            }

      
             return DataSourceLoader.Load(lst, loadOptions);

            
        }
    }
}

