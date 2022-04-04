using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace BOC.Controllers
{
    public class ForgotController : Controller
    {
       
        public IActionResult Index()
        {
            var model = new LoginModel();
            return View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(LoginModel model)
        {
                if (string.IsNullOrEmpty(model.YourEmail))
                {
                    model.ErrorMessage = "Your email entered cannot be left blank.";
                    return View(model);
                }
                else
                {
                    Url account = new Url();
                    string uri = account.Get("ResetPassword");
   
                    HttpClient Client = new HttpClient();
                    var nvc = new List<KeyValuePair<string, string>>();
                    nvc.Add(new KeyValuePair<string, string>("Email", model.YourEmail));
                    nvc.Add(new KeyValuePair<string, string>("BAV_ID", model.EmployeeId));
                    var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

                    string Content;
                    HttpResponseMessage res;
                    res = Client.SendAsync(req).Result;
                    Content = res.Content.ReadAsStringAsync().Result;
                    var oData = JObject.Parse(Content);

                    model.Result = oData["ResultCode"].ToString();
                    if (model.Result == "0")
                    {
                        model.ErrorMessage = "Password will send to your email a few minutes.Please keep seceret your password.Don't lost back";

                    }
                    else
                    {
                        model.ErrorMessage = "Your data input ircorrect.Please check back again!";
                    }
                }

            


            return View(model);

        }
    
    }
}
