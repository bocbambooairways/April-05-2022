using BOC.Areas.Baggage.Models;
using BOC.Areas.BaggageMiss.Models;
using BOC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BOC.Areas.BaggageMiss.Controllers
{
    [Area("BaggageMiss")]
    public class MProfilesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            //Remove Session BagMissID
            HttpContext.Session.Remove("BagMiss_ID");
            //Get Session object ProfileDescription
            List<BagMissData> _ProfileDescList = SessionHelper.GetObjectFromJson<List<BagMissData>>(HttpContext.Session, "ProfileDescription");
            ViewBag.ProfileDescLst = _ProfileDescList;
            ViewBag.PNR = HttpContext.Session.GetString("PNR");
            ViewBag.FltNo = HttpContext.Session.GetString("FltNo");
            return View(ViewBag.ProfileDescLst);
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
        public IActionResult Index(string t_flag, string t_bagmiss_id, string t_action)
        {
            CheckModel model = new CheckModel();
            var token = GetToken();
            Url misbagprofile = new Url();
            string url = misbagprofile.Get("MisBagProfileGet");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", "0"));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            var pData = JObject.Parse(Content);
            model.ResultCode = pData["ResultCode"].ToString();
            model.FltDate = pData["FltDate"].ToString();
            if (model.ResultCode == "0")
            {

                //Bind Json To List 
                JavaScriptSerializer ser = new JavaScriptSerializer();
                BagMissData lst = ser.Deserialize<BagMissData>(pData["Data"].ToString());//str is JSON string.
                if (lst != null)
                {
                    model.FltDate = lst.Date.ToString();
                    // Save  Session PNR,FltNo,FltDate
                    HttpContext.Session.SetString("PNR", lst.PNR.ToString());
                    HttpContext.Session.SetString("FltNo", lst.FltNo.ToString());
                    HttpContext.Session.SetString("FltDate", model.FltDate);
                    // Save  Session object ProfileDescription
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "ProfileDescription", lst);


                }

            }
            return RedirectToAction("Index", "MPages", new { t_flag = t_flag, t_bagmiss_id = t_bagmiss_id });
        }
        public IActionResult SetLang(string id)
        {
            string culture = id == null ? "VN" : id;
            Response.Cookies.Append(
               CookieRequestCultureProvider.DefaultCookieName,
               CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
               new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
           );

            // Save  Session Language
            HttpContext.Session.SetString("Lang", culture);
            return RedirectToAction("Index");
        }

    }
}
