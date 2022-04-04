using BOC.Areas.DarkSiteEmergency.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using BOC.Models;

namespace BOC.Areas.DarkSiteEmergency.Controllers
{
    [Area("DarkSiteEmergency")]//Declare Area
    public class HomeController : Controller
    {
        public UriConfig UriConfig { get; }
        public HomeController(Microsoft.Extensions.Options.IOptions<UriConfig> _UriConfig)
        {
            UriConfig = _UriConfig.Value;
        } 
        //Get du lieu
        [HttpPost]
        public async Task<List<Data>> GetData(string uri, 
                                              string Token,
                                              string param1,
                                              string _param1,
                                              string param2,
                                              string _param2,
                                              string param3,
                                              string _param3)
        {
            using (HttpClient ClientMiss = new HttpClient())
            {
                ClientMiss.DefaultRequestHeaders.Add("Authorization", Token);
                var nvc = new List<KeyValuePair<string, string>>();
                nvc.Add(new KeyValuePair<string, string>(param1, _param1));
                nvc.Add(new KeyValuePair<string, string>(param2, _param2));
                nvc.Add(new KeyValuePair<string, string>(param3, _param3));
                var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };
                string ContentMissBag;
                HttpResponseMessage res;
                res = await ClientMiss.SendAsync(req).ConfigureAwait(false); // SỬ DỤNG AWAIT ĐỂ ĐỢI DO SERVER PHẢN HỒI CHẬM 
                ContentMissBag = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var oData = JObject.Parse(ContentMissBag);
                var ConvertToObject = JsonConvert.SerializeObject(oData);
                List<Data> deserializeData = JsonConvert.DeserializeObject<List<Data>>(oData["Data"].ToString());
                return deserializeData;
            }
        }  
        //Get Main
        public async Task<IActionResult> Index()
        {
            try
            {
                CheckSession();
                HttpContext.Session.SetString("action", "Index");
                string uri_Login = UriConfig.uri_Login;
                string uri_GetData = UriConfig.uri_GetData;
                string Token = PostToGetToken(uri_Login);
                var LstData = GetData(uri_GetData,
                                      Token,
                                      "Language",
                                      HttpContext.Session.GetString("lang"),
                                      "DataType",
                                      "MAIN",
                                      "Preview",
                                      HttpContext.Session.GetString("preview")).Result;

                var LstFooter = GetData(uri_GetData,
                          Token,
                          "Language",
                          HttpContext.Session.GetString("lang"),
                          "DataType",
                          "FOOTER",
                          "Preview",
                           HttpContext.Session.GetString("preview")).Result;
                var LstRightSide = GetData(uri_GetData,
                          Token,
                          "Language",
                          HttpContext.Session.GetString("lang"),
                          "DataType",
                          "CONTACTS",
                          "Preview",
                          HttpContext.Session.GetString("preview")).Result;
                List<object> listOject = new List<object>();
                listOject.Add(LstData);
                listOject.Add(LstFooter);
                listOject.Add(LstRightSide);
                ViewBag.Main = (HttpContext.Session.GetString("lang").ToString() == "EN" ? "HOME" : "TRANG CHỦ");
                ViewBag.News = (HttpContext.Session.GetString("lang").ToString() == "EN" ? "LATEST" : "MỚI NHẤT");
                ViewBag.Action = HttpContext.Session.GetString("action");
                ViewBag.USR = HttpContext.Session.GetString("Username");
                if(LstData.Count != 0 )
                return View(listOject);
                return RedirectToAction("ClosingPage",
                                         "Home",
                                         new { area = "DarkSiteEmergency" , 
                                         lang = HttpContext.Session.GetString("lang")
                                         });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Login", "Home");
            }
        }
        //Get Chi tiet
        public IActionResult Detail()
        {
            try
            {
                CheckSession();
                HttpContext.Session.SetString("action", "Detail");
                string uri_Login = UriConfig.uri_Login;
                string uri_GetData = UriConfig.uri_GetData;
                string Token = PostToGetToken(uri_Login);
                var LstNews = GetData(uri_GetData,
                                    Token,
                                    "Language",
                                    HttpContext.Session.GetString("lang"),
                                    "DataType",
                                    "NEWS",
                                    "Preview",
                                    HttpContext.Session.GetString("preview")).Result;
                var LstFooter = GetData(uri_GetData,
                          Token,
                          "Language",
                          HttpContext.Session.GetString("lang"),
                          "DataType",
                          "FOOTER",
                          "Preview",
                           HttpContext.Session.GetString("preview")).Result;
                var LstRightSide = GetData(uri_GetData,
                          Token,
                          "Language",
                          HttpContext.Session.GetString("lang"),
                          "DataType",
                          "CONTACTS",
                          "Preview",
                           HttpContext.Session.GetString("preview")).Result;
                List<object> listOject = new List<object>();
                listOject.Add(LstNews);
                listOject.Add(LstFooter);
                listOject.Add(LstRightSide);
                ViewBag.Main = (HttpContext.Session.GetString("lang").ToString() == "EN" ? "HOME" : "TRANG CHỦ");
                ViewBag.News = (HttpContext.Session.GetString("lang").ToString() == "EN" ? "LATEST" : "MỚI NHẤT");
                ViewBag.Action = HttpContext.Session.GetString("action");
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["DaskSiteID"]))
                    ViewBag.DaskSiteID = int.Parse(HttpContext.Request.Query["DaskSiteID"].ToString());
                return View(listOject);
            }
            catch(Exception ex)
            {
                return View("error");
            }
        }
        //Get Token
        public string PostToGetToken(string url)
        {
            using (HttpClient Client = new HttpClient())
            {
               
                    var nvc = new List<KeyValuePair<string, string>>();
                    nvc.Add(new KeyValuePair<string, string>("UserCode", HttpContext.Session.GetString("UserCode").ToString()));
                    nvc.Add(new KeyValuePair<string, string>("Password", HttpContext.Session.GetString("Password").ToString()));
                    nvc.Add(new KeyValuePair<string, string>("DeviceID", "C11FCC37-16D6-11EB-BADE-000C29D93A49"));
                    var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };
                    string Content;
                    HttpResponseMessage res;
                    res = Client.SendAsync(req).Result;
                    Content = res.Content.ReadAsStringAsync().Result;
                    var oData = JObject.Parse(Content);
                    var ConvertToObject = JsonConvert.SerializeObject(oData);
                    UserInfor deserializeData = JsonConvert.DeserializeObject<UserInfor>(oData["Data"].ToString());
                  
                    if(deserializeData is null)
                        return string.Empty; 
                    return deserializeData.Token.ToString();
            }
        }
        //Get Tin moi
        public IActionResult News()
        {
            try
            {
                CheckSession();
                HttpContext.Session.SetString("action", "News");
                string uri_Login = UriConfig.uri_Login;
                string uri_GetData = UriConfig.uri_GetData;
                string Token = PostToGetToken(uri_Login);
                var LstNews = GetData(uri_GetData,
                                    Token,
                                    "Language",
                                    HttpContext.Session.GetString("lang"),
                                    "DataType",
                                    "NEWS",
                                    "Preview",
                                    HttpContext.Session.GetString("preview")).Result;
                var LstFooter = GetData(uri_GetData,
                          Token,
                          "Language",
                          HttpContext.Session.GetString("lang"),
                          "DataType",
                          "FOOTER",
                          "Preview",
                           HttpContext.Session.GetString("preview")).Result;
                var LstRightSide = GetData(uri_GetData,
                          Token,
                          "Language",
                          HttpContext.Session.GetString("lang"),
                          "DataType",
                          "CONTACTS",
                          "Preview",
                           HttpContext.Session.GetString("preview")).Result;
                List<object> listOject = new List<object>();
                listOject.Add(LstNews);
                listOject.Add(LstFooter);
                listOject.Add(LstRightSide);
                ViewBag.Main = (HttpContext.Session.GetString("lang").ToString() == "EN" ? "HOME" : "TRANG CHỦ");
                ViewBag.News = (HttpContext.Session.GetString("lang").ToString() == "EN" ? "LATEST" : "MỚI NHẤT");
                ViewBag.Action = HttpContext.Session.GetString("action");
                ViewBag.KQ = HttpContext.Session.GetString("UserCode") + HttpContext.Session.GetString("Password") + HttpContext.Session.GetString("preview");
                return View(listOject);
            }
            catch(Exception ex)
            {
                return View("error");
            }
        }
        //Check Session
        public void CheckSession()
        {
            if (HttpContext.Session.GetString("lang") == null && string.IsNullOrEmpty(HttpContext.Request.Query["lang"]))
            {
                HttpContext.Session.SetString("lang", "VN");
            }
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["lang"]))
            {
                HttpContext.Session.SetString("lang", HttpContext.Request.Query["lang"]);
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserInfor usrInfor)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("UserCode",usrInfor.UserCode);
                HttpContext.Session.SetString("Password",usrInfor.Password);
                HttpContext.Session.SetString("preview", "YES");
                return RedirectToAction("Index", "Home", new { area = "DarkSiteEmergency" });
            }
            //return RedirectToAction("error", "Home", new { area = "DarkSiteEmergency" });
            return View();
        }
        //close page
        public IActionResult ClosingPage()
        {
            CheckSession();
            string uri_Login = UriConfig.uri_Login;
            string uri_GetData = UriConfig.uri_GetData;
            string Token = PostToGetToken(uri_Login);
            var ClosingPage = GetData(uri_GetData,
                                Token,
                                "Language",
                                HttpContext.Session.GetString("lang"),
                                "DataType",
                                "CLOSE",
                                "Preview",
                                HttpContext.Session.GetString("preview")).Result;
            List<object> listOject = new List<object>();
            listOject.Add(ClosingPage);
            return PartialView(listOject);
        }

    }
}
