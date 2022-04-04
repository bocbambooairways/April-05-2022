using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOC.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;


namespace BOC.Controllers
{
    
    public class ChangePassController : Controller
    {
        public IActionResult Index()
        {
            var model = new ChangePassModel();
            return View(model);

        }
        [HttpPost]
        public ActionResult Index(ChangePassModel model)
        {
            //Get Session Token
            var token = HttpContext.Session.GetString("Token");

            //Get path url api
            Url login = new Url();
            string uri = login.Get("ChangePassword");

            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("Password", model.OldPass));
            nvc.Add(new KeyValuePair<string, string>("NewPassword", model.NewPass));
            nvc.Add(new KeyValuePair<string, string>("ConfirmPassword", model.ConfirmPass));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            var oData = JObject.Parse(Content);

            model.Success = Int32.Parse(oData["ResultCode"].ToString());
            if (model.Success == 0)
            {

                //Response.Cookies.Delete("Password", new CookieOptions()
                //{
                //    Secure = true,
                //});
                SetCookie("Password", string.Empty, -1);
                //model.ErrorMessage = "Your password changed successful!";
                //return RedirectToAction("Index","Home");
                return RedirectToAction("Index", "Home", new { msg = "1"  });
            }
            else { model.ErrorMessage = "Incorrect user input."; }

            return View(model);

        }
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }


    }
}
