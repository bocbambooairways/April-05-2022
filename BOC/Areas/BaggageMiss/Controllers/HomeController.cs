using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using BOC.Areas.Baggage.Models;
using BOC.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Nancy.Json;
using BOC.Areas.BaggageMiss.Models;
using Microsoft.AspNetCore.Authentication;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BOC.Areas.Baggage.Controllers
{

    [Area("BaggageMiss")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }




        [HttpGet]
        public IActionResult Index(string msg, string UserCurrent)
        {

            //Remove all Session
            //HttpContext.Session.Clear();
            var model = new CheckModel();
            var token = GetToken();
            if (!String.IsNullOrEmpty(msg))
            {
                model.Result = msg;
            }

            if (UserCurrent == "Yes")
            {

                //HttpContext.Session.SetString("Token", Token.ToString());
                return View(model);
            }
            else
            {

                HttpContext.Session.SetString("Token", token);
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

        [HttpPost]
        public async Task<IActionResult> IndexAsync(CheckModel model)
        {
            try
            {
                // Get  Session Token cho 2 trường hợp (Nhân viên quầy vé làm thay khách và khách tự làm)
                if (model.token != null)
                {
                    HttpContext.Session.SetString("Token", model.token);
                }
                else
                {
                    model.token = HttpContext.Session.GetString("Token");
                }


                if (string.IsNullOrEmpty(model.PNR) || string.IsNullOrEmpty(model.FltNo))
                {
                    model.ErrorMessage = "Data entered cannot be left blank.";
                    return View(model);
                }
                else
                {

                    //Get path url api
                    Url misbag = new Url();
                    string uri = misbag.Get("MisBagProfileCheck");
                    HttpClient ClientMiss = new HttpClient();
                    var nvc_ = new List<KeyValuePair<string, string>>();
                    nvc_.Add(new KeyValuePair<string, string>("PNR", model.PNR));
                    nvc_.Add(new KeyValuePair<string, string>("FltNo", model.FltNo.ToString()));
                    ClientMiss.DefaultRequestHeaders.Add("Authorization", model.token);
                    var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc_) };

                    string ContentMissBag;
                    HttpResponseMessage res;
                    res = await ClientMiss.SendAsync(req).ConfigureAwait(false); // SỬ DỤNG AWAIT ĐỂ ĐỢI DO SERVER PHẢN HỒI CHẬM 
                    ContentMissBag = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var oData = JObject.Parse(ContentMissBag);
                    model.Result = oData["ResultCode"].ToString();

                    if (model.Result == "0")
                    {

                        //Bind Json To List 
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        List<BagMissData> lst = ser.Deserialize<List<BagMissData>>(oData["Data"].ToString());//str is JSON string.

                        if (lst != null)
                        {

                            for (int i = 0; i < lst.Count; i++)
                            {
                                lst[i].ID = i + 1;
                                model.BagMiss_Id = lst[i].BagMiss_ID;
                                //Get danh sách hồ sơ
                                Url misbagprofile = new Url();
                                string url = misbag.Get("MisBagProfileGet");
                                HttpClient Client2 = new HttpClient();
                                var nvc = new List<KeyValuePair<string, string>>();
                                nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", model.BagMiss_Id.ToString()));
                                Client2.DefaultRequestHeaders.Add("Authorization", model.token);
                                var req_ = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

                                string Content_;
                                HttpResponseMessage res_;
                                res_ = Client2.SendAsync(req_).Result;
                                Content_ = res_.Content.ReadAsStringAsync().Result;
                                var pData = JObject.Parse(Content_);

                                model.ResultCode = pData["ResultCode"].ToString();
                                if (model.ResultCode == "0")
                                {
                                    string ProfileNo = pData["Data"]["ProfileNo"].ToString();
                                    lst[i].ProfileNo = ProfileNo;
                                }



                            }

                            // Save  Session TypeOfDevice
                            var typeofdevice = model.TypeOfDevice;
                            var widthofdevice = model.WidthOfDevice == null ? "0" : model.WidthOfDevice;
                            HttpContext.Session.SetString("TypeOfDevice", typeofdevice);
                            HttpContext.Session.SetString("WidthOfDevice", widthofdevice);
                            HttpContext.Session.SetString("PNR", model.PNR);
                            HttpContext.Session.SetString("FltNo", model.FltNo);
                            // Save  Session object ProfileDescription
                            SessionHelper.SetObjectAsJson(HttpContext.Session, "ProfileDescription", lst);
                            //_logger.LogInformation("MissBaggage Exists!");
                            if (typeofdevice == "MOBILE")
                            {
                                return RedirectToAction("Index", "MProfiles");
                            }
                            if (typeofdevice == "NONE")
                            {
                                return RedirectToAction("Index", "Profiles");
                            }
                        }


                    }
                    else
                    {
                        Url misbagprofile = new Url();
                        string url = misbag.Get("MisBagProfileGet");
                        HttpClient Client2 = new HttpClient();
                        var nvc = new List<KeyValuePair<string, string>>();
                        nvc.Add(new KeyValuePair<string, string>("PNR", model.PNR));
                        nvc.Add(new KeyValuePair<string, string>("FltNo", model.FltNo));
                        nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", "0"));
                        Client2.DefaultRequestHeaders.Add("Authorization", model.token);
                        var req_ = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

                        string Content_;
                        HttpResponseMessage res_;
                        res_ = Client2.SendAsync(req_).Result;
                        Content_ = res_.Content.ReadAsStringAsync().Result;
                        var pData = JObject.Parse(Content_);
                        model.Result = pData["ResultCode"].ToString();
                        if (model.Result == "0")
                        {

                            //Bind Json To List 
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            BagMissData lst = ser.Deserialize<BagMissData>(pData["Data"].ToString());//str is JSON string.
                            if (lst != null)
                            {
                                model.FltDate = lst.Date.ToString();
                                // Save  Session TypeOfDevice,PNR,FltNo,FltDate,WidthOfDevice
                                var typeofdevice = model.TypeOfDevice;
                                HttpContext.Session.SetString("TypeOfDevice", typeofdevice);
                                HttpContext.Session.SetString("PNR", model.PNR);
                                HttpContext.Session.SetString("FltNo", model.FltNo.ToString());
                                HttpContext.Session.SetString("FltDate", model.FltDate);
                                //// Save  Session object ProfileDescription
                                //SessionHelper.SetObjectAsJson(HttpContext.Session, "ProfileDescription", lst);
                                //return RedirectToAction("Index", "Profiles");
                                if (typeofdevice == "MOBILE")
                                {
                                    HttpContext.Session.SetString("WidthOfDevice", model.WidthOfDevice);
                                    return RedirectToAction("Index", "MPages");
                                }
                                if (typeofdevice == "NONE")
                                {
                                    return RedirectToAction("Index", "Pages");
                                }


                            }


                        }

                        if (model.Result == "99" && model.Message == "Not found")
                        {

                            // Save  Session TypeOfDevice,PNR,FltNo,FltDate
                            var typeofdevice = model.TypeOfDevice;
                            HttpContext.Session.SetString("TypeOfDevice", typeofdevice);
                            //HttpContext.Session.SetString("WidthOfDevice", model.WidthOfDevice);
                            HttpContext.Session.SetString("PNR", model.PNR);
                            HttpContext.Session.SetString("FltNo", model.FltNo);
                            HttpContext.Session.SetString("FltDate", model.FltDate);
                            //Redirect In Area
                            //return RedirectToAction("Index", "Pages", new { areaName = "BaggageMiss" });
                            if (typeofdevice == "MOBILE")
                            {
                                return RedirectToAction("Index", "MPages");
                            }
                            if (typeofdevice == "NONE")
                            {
                                return RedirectToAction("Index", "Pages");
                            }

                        }
                        else
                        {
                            //model.ErrorMessage = "Login fail.Your data input not be found!";
                            model.ErrorMessage = pData["Message"].ToString();
                        }
                    }


                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ex;
            }

        }

        [HttpPost]
        public async Task<IActionResult> Login(string PNR, string FltNo)
        {
            try
            {
                //DateTime localDate = DateTime.Now;
                //_logger.LogInformation("LOGIN:" + localDate.ToString());

                var token = GetToken();
                HttpContext.Session.SetString("Token", token);

                CheckModel model = new CheckModel();
                model.PNR = PNR;
                model.FltNo = FltNo;
                model.token = token;

                //Get path url api
                Url misbag = new Url();
                string uri = misbag.Get("MisBagProfileCheck");
                HttpClient ClientMiss = new HttpClient();
                var nvc_ = new List<KeyValuePair<string, string>>();
                nvc_.Add(new KeyValuePair<string, string>("PNR", model.PNR));
                nvc_.Add(new KeyValuePair<string, string>("FltNo", model.FltNo));
                ClientMiss.DefaultRequestHeaders.Add("Authorization", model.token);
                var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc_) };

                string ContentMissBag;
                HttpResponseMessage res;
                res = await ClientMiss.SendAsync(req).ConfigureAwait(false); // SỬ DỤNG AWAIT ĐỂ ĐỢI DO SERVER PHẢN HỒI CHẬM 
                ContentMissBag = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var oData = JObject.Parse(ContentMissBag);
                model.Result = oData["ResultCode"].ToString();

                if (model.Result == "0")
                {

                    //Bind Json To List 
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    List<BagMissData> lst = ser.Deserialize<List<BagMissData>>(oData["Data"].ToString());//str is JSON string.

                    if (lst != null)
                    {

                        for (int i = 0; i < lst.Count; i++)
                        {
                            lst[i].ID = i + 1;
                            model.BagMiss_Id = lst[i].BagMiss_ID;
                        }

                        // Save  Session PNR && FLTNo
                        HttpContext.Session.SetString("PNR", model.PNR);
                        HttpContext.Session.SetString("FltNo", model.FltNo);
                        // Save  Session object ProfileDescription
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "ProfileDescription", lst);
                        //_logger.LogInformation("MissBaggage Exists!");
                        return RedirectToAction("Index", "Profiles");
                    }


                }
                else
                {
                    Url misbagprofile = new Url();
                    string url = misbagprofile.Get("MisBagProfileGet");
                    HttpClient Client2 = new HttpClient();
                    var nvc = new List<KeyValuePair<string, string>>();
                    nvc.Add(new KeyValuePair<string, string>("PNR", model.PNR));
                    nvc.Add(new KeyValuePair<string, string>("FltNo", model.FltNo));
                    nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", "0"));
                    Client2.DefaultRequestHeaders.Add("Authorization", model.token);
                    var req_ = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

                    string Content_;
                    HttpResponseMessage res_;
                    res_ = Client2.SendAsync(req_).Result;
                    Content_ = res_.Content.ReadAsStringAsync().Result;
                    var pData = JObject.Parse(Content_);
                    model.Result = pData["ResultCode"].ToString();
                    if (model.Result == "0")
                    {

                        //Bind Json To List 
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        BagMissData lst = ser.Deserialize<BagMissData>(pData["Data"].ToString());//str is JSON string.
                        if (lst != null)
                        {
                            model.FltDate = lst.Date.ToString();
                            // Save  Session PNR,FltNo,FltDate
                            HttpContext.Session.SetString("PNR", model.PNR);
                            HttpContext.Session.SetString("FltNo", model.FltNo.ToString());
                            HttpContext.Session.SetString("FltDate", model.FltDate);
                            // Save  Session object ProfileDescription
                            SessionHelper.SetObjectAsJson(HttpContext.Session, "ProfileDescription", lst);
                            //Redirect In Area
                            //return RedirectToAction("Index", "Pages", new { areaName = "BaggageMiss" });
                            return RedirectToAction("Index", "Pages");

                        }


                    }

                    if (model.Result == "99" && model.Message == "Not found")
                    {

                        // Save  Session PNR,FltNo,FltDate
                        HttpContext.Session.SetString("PNR", model.PNR);
                        HttpContext.Session.SetString("FltNo", model.FltNo);
                        HttpContext.Session.SetString("FltDate", model.FltDate);
                        //Redirect In Area
                        //return RedirectToAction("Index", "Pages", new { areaName = "BaggageMiss" });
                        return RedirectToAction("Index", "Pages");


                    }
                    else
                    {
                        //model.ErrorMessage = "Login fail.Your data input not be found!";
                        model.ErrorMessage = pData["Message"].ToString();
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.ToString());
                throw ex;
            }

        }
    }
}
