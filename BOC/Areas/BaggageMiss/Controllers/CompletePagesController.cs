using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Cache;
using BOC.Models;
using System;

namespace BOC.Areas.BaggageMiss.Controllers
{
    [Area("BaggageMiss")]
    public class CompletePagesController : Controller
    {
        public IActionResult Index()
        {
            //SaveLog.WriteLog("Start");
            string lang = HttpContext.Request.Query["t_flag"].ToString();
            //SaveLog.WriteLog(lang);
            string bagmiss_id = HttpContext.Request.Query["t_bagmiss_id"].ToString();
            //SaveLog.WriteLog(bagmiss_id);
            var deviceName = HttpContext.Items["DeviceName"].ToString();
           // SaveLog.WriteLog(deviceName.ToString());
            var isMobile = Convert.ToBoolean(HttpContext.Items["isMobile"]);
           // SaveLog.WriteLog(isMobile.ToString());
            if (deviceName == "smartphone" && isMobile==true)
            {
                return RedirectToAction("Index", "MPages", new { area = "BaggageMiss", @t_flag = lang, @t_bagmiss_id = bagmiss_id });

            }
            else
            {
                return RedirectToAction("Index", "Pages", new { area = "BaggageMiss", @t_flag = lang, @t_bagmiss_id = bagmiss_id });
            }

        }
    }
}
