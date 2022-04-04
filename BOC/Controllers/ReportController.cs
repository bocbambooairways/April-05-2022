using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Nancy.Json;
using System.Globalization;

namespace BOC.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Report1()
        {
        
            var model = new ReportModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Report1(ReportModel model)
        {

            if (string.IsNullOrEmpty(model.FromDate1) || string.IsNullOrEmpty(model.ToDate1) || string.IsNullOrEmpty(model.FromDate2) || string.IsNullOrEmpty(model.ToDate2))
            {
                model.ErrorMessage = "Data entered must not be blank!";
                return View(model);
            }
            else
            {
                var ffd1 = model.FromDate1;
                DateTime frd1 = DateTime.ParseExact(ffd1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ttd1 = model.ToDate1;
                DateTime trd1 = DateTime.ParseExact(ttd1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ffd2 = model.FromDate2;
                DateTime frd2 = DateTime.ParseExact(ffd2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ttd2 = model.ToDate2;
                DateTime trd2 = DateTime.ParseExact(ttd2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                // Format Datetime For (FromDate And ToDate)
                string fd_1 = model.FromDate1;
                string[] fd1 = fd_1.Split('/');
                string fromdate1 = fd1[2] + '-' + fd1[1] + '-' + fd1[0];
                ViewData["FromDate1"] = fromdate1;

                string fd_2 = model.FromDate2;
                string[] fd2 = fd_2.Split('/');
                string fromdate2 = fd2[2] + '-' + fd2[1] + '-' + fd2[0];
                ViewData["FromDate2"] = fromdate2;

                string td_1 = model.ToDate1;
                string[] td1 = td_1.Split('/');
                string todate1 = td1[2] + '-' + td1[1] + '-' + td1[0];
                ViewData["ToDate1"] = todate1;

                string td_2 = model.ToDate2;
                string[] td2 = td_2.Split('/');
                string todate2 = td2[2] + '-' + td2[1] + '-' + td2[0];
                ViewData["ToDate2"] = todate2;

       
                if (frd1 > trd1 || frd2 > trd2)
                {
                    model.ErrorMessage = "Todate must be larger than Fromdate!";
                    
                }
                return View(model);

            }
        }
         public IActionResult Report2()
         {
            var model = new ReportModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Report2(ReportModel model)
        {
           

            if (string.IsNullOrEmpty(model.FromDate1) || string.IsNullOrEmpty(model.ToDate1) || string.IsNullOrEmpty(model.FromDate2) || string.IsNullOrEmpty(model.ToDate2))
            {
                model.ErrorMessage = "Data entered must not be blank!";
                return View(model);
            }
            else
            {
                var ffd1 = model.FromDate1;
                DateTime frd1 = DateTime.ParseExact(ffd1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ttd1 = model.ToDate1;
                DateTime trd1 = DateTime.ParseExact(ttd1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ffd2 = model.FromDate2;
                DateTime frd2 = DateTime.ParseExact(ffd2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ttd2 = model.ToDate2;
                DateTime trd2 = DateTime.ParseExact(ttd2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                // Format Datetime For (FromDate And ToDate)
                string fd_1 = model.FromDate1;
                string[] fd1 = fd_1.Split('/');
                string fromdate1 = fd1[2] + '-' + fd1[1] + '-' + fd1[0];
                ViewData["FromDate1"] = fromdate1;

                string fd_2 = model.FromDate2;
                string[] fd2 = fd_2.Split('/');
                string fromdate2 = fd2[2] + '-' + fd2[1] + '-' + fd2[0];
                ViewData["FromDate2"] = fromdate2;

                string td_1 = model.ToDate1;
                string[] td1 = td_1.Split('/');
                string todate1 = td1[2] + '-' + td1[1] + '-' + td1[0];
                ViewData["ToDate1"] = todate1;

                string td_2 = model.ToDate2;
                string[] td2 = td_2.Split('/');
                string todate2 = td2[2] + '-' + td2[1] + '-' + td2[0];
                ViewData["ToDate2"] = todate2;

               
                if (frd1 > trd1 || frd2 > trd2)
                {
                    model.ErrorMessage = "Todate must be larger than Fromdate!";
                    
                }
                return View(model);

            }
        }
        public IActionResult Report3()
         {
                return View();
         }
    }
}
