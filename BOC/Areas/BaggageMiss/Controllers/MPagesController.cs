using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BOC.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using BOC.Areas.BaggageMiss.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Renci.SshNet;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace BOC.Areas.BaggageMiss.Controllers
{
    [Area("BaggageMiss")]
    public class MPagesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IViewRenderService _viewRenderService;
        private readonly IFileProvider fileProvider;
        private readonly IConfiguration configuration;
        public MPagesController(IWebHostEnvironment webHostEnvironment, IViewRenderService viewRenderService, IConfiguration configuration, IFileProvider fileProvider)
        {
            _webHostEnvironment = webHostEnvironment;
            _viewRenderService = viewRenderService;
            this.fileProvider = fileProvider;
            this.configuration = configuration;
        }
        public IActionResult SetLang(string t_flag)
        {
            string culture = t_flag == null ? "VN" : t_flag;
            Response.Cookies.Append(
               CookieRequestCultureProvider.DefaultCookieName,
               CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
               new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
           );
            // Set Session Upload is null cho trường hợp đổi ngôn ngữ bất kỳ sau khi upload
            HttpContext.Session.SetString("Upload", string.Empty);
            // Save  Session Language
            HttpContext.Session.SetString("Lang", culture);
            //Get Session BagMissDetail_Id
            var bagmissdetail_id = HttpContext.Session.GetString("BagMissDetail_ID");
            return RedirectToAction("Index");
        }
        public List<BaggageMissContactModel> GetMissContacts(string t_bagmiss_id)
        {
            if (t_bagmiss_id == null || t_bagmiss_id == "")
            {
                t_bagmiss_id = "0";
            }
            List<BaggageMissContactModel> missContacts = new List<BaggageMissContactModel>();
            //Get Session Token
            var token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
            //Get Session PNR
            var pnr = HttpContext.Session.GetString("PNR");
            //Get Session FltNo
            var fltno = HttpContext.Session.GetString("FltNo");
            //Get MissBagProfile
            string Content = CallAPI.MisBagProfileGet(pnr, fltno, t_bagmiss_id, token);
            BaggageMissContact model = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissContact>(Content);
            if (model.Data.FullName != null)
            {
                ViewData["CustName"] = model.Data.FullName;

            }
            // Save  Session PNR & FltNo
            HttpContext.Session.SetString("FltNo", model.Data.FltNo);
            HttpContext.Session.SetString("PNR", model.Data.PNR);
            // Save  Session Status
            HttpContext.Session.SetString("Status", model.Data.Status);
            // Save  Session BBCNo
            HttpContext.Session.SetString("BBCNo", model.Data.BBCNo);
            // Save  Session FltDate
            HttpContext.Session.SetString("FltDate", model.Data.Date);
            //// Save  Session BagMiss_ID
            //HttpContext.Session.SetString("BagMiss_ID", model.Data.BagMiss_ID.ToString());
            // Save  Session ProfileNo
            HttpContext.Session.SetString("ProfileNo", model.Data.ProfileNo.ToString());
            missContacts.Add(model.Data);
            return missContacts;

        }
        public List<BaggageMissDetailModel> GetMissDetails(string t_bagmiss_id, string t_bagmissdetail_id)
        {

            //t_bagmissdetail_id = "0";
            //Get Session Token
            var token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");

            List<BaggageMissDetailModel> missDetails = new List<BaggageMissDetailModel>();

            if (t_bagmiss_id != null)
            {
                HttpContext.Session.SetString("BagMiss_ID", t_bagmiss_id.ToString());


                string Content = CallAPI.MisBagDetaiGet(Int32.Parse(t_bagmissdetail_id.ToString()), Int32.Parse(t_bagmiss_id.ToString()), token);
                BaggageMissDetail lst = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissDetail>(Content);
                // Save  Session BagMissDetail_ID
                if (lst.Data != null)
                {
                    //HttpContext.Session.SetString("BagMissDetail_ID", lst.Data[0].BagMissDetail_ID.ToString());
                    missDetails = lst.Data;
                    for (int i = 0; i < missDetails.Count; i++)
                    {
                        missDetails[i].ID = i + 1;
                    }
                    ViewBag.BagMiss_ID = t_bagmiss_id.ToString();
                }
                else
                {
                    HttpContext.Session.SetString("BagMissDetail_ID", "0");
                    missDetails = null;
                }
            }
            return missDetails;
        }
        public List<BaggageMissDescModel> GetMissDesc(string t_bagmiss_id)
        {

            //Get Session Token
            var token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
            //Get Session BagMiss_ID
            var bagmiss_id = HttpContext.Session.GetString("BagMiss_ID");
            //Get Session BagMissDetail_ID
            t_bagmiss_id = t_bagmiss_id == null ? "0" : t_bagmiss_id.ToString();

            List<BaggageMissDescModel> missDesc = new List<BaggageMissDescModel>();
            string Content = CallAPI.MissBagDescriptionGet(Int32.Parse(t_bagmiss_id.ToString()), token);
            BaggageMissDesc lstdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissDesc>(Content);
            missDesc.Add(lstdesc.Data);
            var arraybag = lstdesc.Data.BagDesc.ToArray();
            foreach (var i in arraybag)
            {
                var FileName = i.sysFileName;
                if (FileName != "")
                {
                    DownloadFiles(FileName);
                }

            }
            ViewBag.MissBagDesc = lstdesc.Data.BagDesc;
            // Save  Session object BagDesc
            SessionHelper.SetObjectAsJson(HttpContext.Session, "BagDescription", arraybag);
            return missDesc;
        }
        private void DownloadFiles(string t_FileName)
        {
            //Đọc file json lấy thông tin đăng nhập SFTP Server
            var Host = configuration["SFTP:Host"];
            var Port = configuration["SFTP:Port"];
            var Username = configuration["SFTP:Username"];
            var Password = configuration["SFTP:Password"];
            String RemoteFileName = "/upload/BaggageMiss/" + t_FileName;
            String LocalDestinationFilename = "";
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            LocalDestinationFilename = Path.Combine(contentRootPath, "wwwroot", "images/BaggageMiss/" + t_FileName);


            //Compare Time Create File In Local And SFTP
            DateTime timelocalfile = System.IO.File.GetCreationTime(LocalDestinationFilename);
            DateTime timesftpfile = System.IO.File.GetCreationTime(RemoteFileName);
            if (timelocalfile <= timesftpfile)
            {


                using (var sftp = new SftpClient(Host, Int16.Parse(Port), Username, Password))
                {
                    sftp.Connect();

                    using (var file = System.IO.File.OpenWrite(LocalDestinationFilename))
                    {
                        sftp.DownloadFile(RemoteFileName, file);
                    }

                    sftp.Disconnect();
                }
            }

        }
        public List<FileDisplay> GetFileAttach(string t_bagmiss_id)
        {

            //Get Session Token
            var token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
            //Get Session BagMiss_ID
            t_bagmiss_id = t_bagmiss_id != null ? t_bagmiss_id : HttpContext.Session.GetString("BagMiss_ID");


            if (t_bagmiss_id != null)
            {
                //Call api to get BagHS_Miss
                string ContentHS = CallAPI.BagHS_Miss_AttachedFile_Get(Int32.Parse(t_bagmiss_id.ToString()), token);
                JObject ser = JObject.Parse(ContentHS);
                Int32 _Result = (int)ser.SelectToken("ResultCode");
                if (_Result == 0)
                {
                    var ser2 = ser.SelectToken("Data");
                    List<JToken> data = new List<JToken>(ser2.Children());
                    List<FileDisplay> filedisplay = new List<FileDisplay>();
                    foreach (var item in data)
                    {
                        FileDisplay fd = new FileDisplay();
                        fd.FileLoc_ID = Int32.Parse(item.SelectToken("FileLoc_ID").ToString());
                        if (fd.FileLoc_ID > 0)
                        {
                            //Set Session id_loc
                            HttpContext.Session.SetString("FileLoc_ID", fd.FileLoc_ID.ToString());
                        }
                        fd.FileName = item.SelectToken("FileName").ToString();
                        fd.SysFileName = item.SelectToken("sysFileName").ToString();
                        fd.LastUserUpdate = item.SelectToken("LastUserUpdate").ToString();
                        fd.LastUpdate = item.SelectToken("LastUpdate").ToString();
                        filedisplay.Add(fd);
                    }
                    // Save  Session object AttachFile
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Miss_AttachFileHS", filedisplay);

                    return filedisplay;
                }
                else
                {
                    return null;
                }



            }

            return null;


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


        public IActionResult Index(string t_flag, string t_bagmiss_id, string t_action)
        {
            
            t_flag = t_flag == null ? HttpContext.Session.GetString("Lang") : t_flag;
            if (t_flag == "EN")
            {
                //Save Session Flag
                HttpContext.Session.SetString("Lang", "EN");
            }
            else
            {
                //Save Session Flag
                HttpContext.Session.SetString("Lang", "VN");
            }
            if (t_bagmiss_id == null || t_bagmiss_id == "0")
            {
                t_bagmiss_id = HttpContext.Session.GetString("BagMiss_ID");
            }
            else
            {
                HttpContext.Session.SetString("BagMiss_ID", t_bagmiss_id);
            }
            if (t_action == "0" && t_bagmiss_id != "0")
            {
                ////Remove Session BagMissID
                //HttpContext.Session.Remove("BagMiss_ID");
                ////Remove Session BagMissID
                //HttpContext.Session.Remove("BagMissDetail_ID");
                t_bagmiss_id = "0";

            }
            ViewModel mymodel = new ViewModel();


            var t_bagmissdetail_id = HttpContext.Session.GetString("BagMissDetail_ID") == null ? "0" : HttpContext.Session.GetString("BagMissDetail_ID");


            ////Get Session object BagDescription

            mymodel.MissContacts = GetMissContacts(t_bagmiss_id);
            
            //mymodel.MissDetails = GetMissDetails(t_bagmiss_id, t_bagmissdetail_id);
            mymodel.MissDetails = GetMissDetails(t_bagmiss_id, "0");

            var delete = HttpContext.Session.GetString("Delete");
            var upload = HttpContext.Session.GetString("UPLOADED");

            if (t_bagmissdetail_id != "0" || delete == "YES")
            {

                if (upload == "OK")
                {
                    ViewBag.FileDisplay = SessionHelper.GetObjectFromJson<List<FileDisplay>>(HttpContext.Session, "Miss_AttachFileHS");

                }



            }
            if (t_bagmiss_id != "0" && t_bagmissdetail_id != "0")
            {

                ViewBag.FileDisplay = GetFileAttach(t_bagmiss_id);
            }


            if (t_bagmiss_id == "0")
            {
                mymodel.MissDesc = GetMissDesc("0");
            }
            else
            {
                mymodel.MissDesc = GetMissDesc(t_bagmiss_id);
            }
            mymodel.MissDesc = GetMissDesc("0");
            ViewBag.FileDisplay = GetFileAttach(t_bagmiss_id);
            //Get Session FltNo And PNR And FltDate
            ViewBag.FltNo = HttpContext.Session.GetString("FltNo");
            ViewBag.PNR = HttpContext.Session.GetString("PNR");
            ViewBag.FltDate = HttpContext.Session.GetString("FltDate");
            //SaveLog.WriteLog("FltNo:" + ViewBag.FltNo.ToString());
            //SaveLog.WriteLog("PNR:" + ViewBag.PNR.ToString());
            return View(mymodel);

        }
        [HttpPost]
        public IActionResult UpdateProfile(string t_FullName, string t_Email, string t_Phone, string t_Address, string t_MembershipNo, string t_Remark)
        {
            t_MembershipNo = t_MembershipNo == null ? "" : t_MembershipNo;
            t_Remark = t_Remark == null ? "" : t_Remark;
            //Get Session Token
            var t_token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
            //Get Session BagMiss_ID
            var t_bagmiss_id = HttpContext.Session.GetString("BagMiss_ID") == null ? "0" : HttpContext.Session.GetString("BagMiss_ID");
            //Get Session PNR
            var t_pnr = HttpContext.Session.GetString("PNR");
            //Get Session FltNo
            var t_fltno = HttpContext.Session.GetString("FltNo");
            var t_type = "MISS";
            var t_bbcno = t_MembershipNo;
            //Get Session Status
            var t_status = HttpContext.Session.GetString("Status");
            string strMess = "Save customer info is fail!";

            if (t_status == "OK")
            {
                //Save Session Argree
                HttpContext.Session.SetString("Argree", "Yes");
                string Content = CallAPI.MisBagProfileUpdate(Int32.Parse(t_bagmiss_id.ToString()), t_pnr, t_fltno, t_type, t_FullName, t_Email, t_Address, t_Phone, t_bbcno, t_Remark, t_status, t_token);
                var lst = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissContact>(Content);
                if (lst.Data != null)
                {
                    string BagMiss_ID = lst.Data.BagMiss_ID.ToString();
                    if (lst.ResultCode == 0)
                    {

                        //Save Session BagMiss_ID
                        HttpContext.Session.SetString("BagMiss_ID", BagMiss_ID);

                        var rs = BagMiss_ID.ToString();
                        strMess = lst.Message;
                        return Json(new { mess = "OK", result = rs });

                    }
                    else
                    {
                        //Set Session BagMiss_ID khi tạo mới không thành công
                        HttpContext.Session.SetString("BagMiss_ID", "");
                        strMess = lst.Message;
                    }
                }
                else
                {
                    //Set Session BagMiss_ID khi tạo mới không thành công
                    HttpContext.Session.SetString("BagMiss_ID", "");
                    strMess = lst.Message;

                }

            }


            return Json(new { mess = strMess });
        }

      

        [HttpPost]
        public IActionResult Baggage_Information_Update(string t_Item, string t_Quantity, string t_TotalAmount, string t_Currency, string t_Remark, String t_RadioSelected, string t_Action)
        {
            try
            {
                var t_BagMissDetail_ID = string.Empty;
                int action = Int32.Parse(t_Action);
                if (action == 0)
                {
                    t_BagMissDetail_ID = "0";
                }
                else
                {
                    t_BagMissDetail_ID = HttpContext.Session.GetString("BagMissDetailForUpdate_ID");

                }
                var lang = HttpContext.Session.GetString("Lang");
                string strMess = "Update baggage description is fail!";
                t_TotalAmount = t_TotalAmount.Replace(",", "").Replace(".", "").Replace(" ", "");
                t_Remark = t_Remark == null ? "" : t_Remark;
                //Get Session Token
                var t_token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");

                //Get Session BagMiss_ID
                var t_BagMiss_ID = HttpContext.Session.GetString("BagMiss_ID") == null ? "0" : HttpContext.Session.GetString("BagMiss_ID");



                //Định dạng lại RadioSelected(cắt bỏ và thay thế ký tự thừa)
                t_RadioSelected = t_RadioSelected.Replace("\"", string.Empty);
                t_RadioSelected = t_RadioSelected.Replace("[", string.Empty);
                t_RadioSelected = t_RadioSelected.Replace("]", "").Trim('"').TrimEnd(',');
                string[] arrRadioSelected = t_RadioSelected.Split(',');
                //Gọi hàm Lấy Danh Sách BagDesc với các Raio User check chọn
                string t_BagDescLst = GetBagDescListCheck(arrRadioSelected);

                //Get Session Status
                var t_status = HttpContext.Session.GetString("Status");
                if (t_status == "OK")
                {

                    string Content = CallAPI.MisBagDetaiUpdate(Int32.Parse(t_BagMissDetail_ID.ToString()), Int32.Parse(t_BagMiss_ID.ToString()), t_Item, "1900-01-01", t_Quantity, t_TotalAmount, t_Currency, t_Remark, t_status, t_BagDescLst, t_token);
                    var lst = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissDesc>(Content);
                    //// Save  Session BagMissDetail_ID
                    //HttpContext.Session.SetString("BagMissDetail_ID", lst.Data.BagMissDetail_ID.ToString());

                    Int32 _Result = Int32.Parse(lst.ResultCode.ToString());
                    if (_Result != 0)
                    {
                        strMess = lst.Message.ToString();
                        return Json(new { mess = strMess });
                    }
                    HttpContext.Session.SetString("BagMiss_ID", t_BagMiss_ID);


                }

                return Json(new { mess = "OK" });
            }
            catch (Exception ex)
            {
                var strMess = ex.Message.ToString();
                return Json(new { mess = strMess });
            }


        }

        public string GetBagDescListCheck(String[] arrRadioSelected)
        {

            //Get Session object BagDescription
            List<BagDesc> _BagDescList = SessionHelper.GetObjectFromJson<List<BagDesc>>(HttpContext.Session, "BagDescription");
            foreach (var item in _BagDescList)
            {

                foreach (var i in arrRadioSelected)
                {
                    if (item.BagDesc_ID == Int32.Parse(i.ToString()))
                    {
                        item.UserCheck = true;
                    }

                }


            }
            string BagDescLst = JsonConvert.SerializeObject(_BagDescList);
            return BagDescLst;
        }
        [HttpPost]
        public IActionResult Baggage_Information_Confirm()
        {

            string strMess = string.Empty;

            //Get Session BagMiss_ID
            string t_BagMiss_ID = HttpContext.Session.GetString("BagMiss_ID");
            //Get Session Token
            string t_token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
            // Call api của anh Thọ để báo kết quả confirm email
            string t_Content = CallAPI.MisBagProfileFinish(Int32.Parse(t_BagMiss_ID.ToString()), t_token);

            var oData = JObject.Parse(t_Content);
            var Result = oData["ResultCode"].ToString();
            var Message = oData["Message"].ToString();
            if (Result == "0")
            {
                strMess = Result;


            }
            else
            {
                strMess = Message;
            }

            return Json(new { mess = strMess });
        }
        [HttpPost]
        public async Task<IActionResult> BaggageDescEdit(string t_bagmissdetail_id)
        {
            try
            {
                ////Get Session BagMiss_ID
                //string t_BagMiss_ID = HttpContext.Session.GetString("BagMiss_ID");
                string strMess = string.Empty;
                //Get Session Token
                string t_token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
                List<BaggageMissDescModel> missDesc = new List<BaggageMissDescModel>();
                string Content = CallAPI.MissBagDescriptionGet(Int32.Parse(t_bagmissdetail_id.ToString()), t_token);
                BaggageMissDesc lstdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissDesc>(Content);
                if (lstdesc.ResultCode == 0)
                {
                    missDesc.Add(lstdesc.Data);
                    var arraybag = lstdesc.Data.BagDesc.ToArray();
                    foreach (var i in arraybag)
                    {
                        var FileName = i.sysFileName;
                        if (FileName != "")
                        {
                            DownloadFiles(FileName);
                        }

                    }

                    strMess = "OK";
                    ViewBag.MissBagDescEdit = lstdesc.Data;
                    var result = await this.RenderViewAsync("BaggageDescEdit", lstdesc, true);
                    // Save  Session BagMissDetailForUpdate_ID
                    HttpContext.Session.SetString("BagMissDetailForUpdate_ID", t_bagmissdetail_id.ToString());
                    return Json(new { mess = strMess, rs = result });
                }
                else
                {
                    strMess = "Get baggage description fail!";
                }
                return Json(new { mess = strMess });

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpPost]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {

            try
            {
                string strMess = string.Empty;
                var host = configuration["SFTP:Host"];
                var port = configuration["SFTP:Port"];
                var username = configuration["SFTP:Username"];
                var password = configuration["SFTP:Password"];
                //Get Session Token
                string t_token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
                //Get Session BagMiss_ID And Create Folder With BagMiss_ID
                string t_BagMiss_ID = HttpContext.Session.GetString("BagMiss_ID");
                string subPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/BagMiss/");

                if (files.Count > 0)
                {
                    foreach (var formFile in files)
                    {
                        //Tạo FTP_FileLocation_Generator để lấy tên file và upload lên sftp
                        string Content = CallAPI.FTP_FileLocation_Generator("BAGHS_MISS", formFile.FileName.ToString(), t_token);
                        var oData = JObject.Parse(Content);
                        if (oData["ResultCode"].ToString() != "0")
                        {
                            strMess = oData["Message"].ToString();
                            return Json(new { mess = strMess });
                        }
                        else
                        {
                            var t_fname = oData["Data"]["FileName"].ToString();
                            var t_id_loc = oData["Data"]["ID"].ToString();
                            //Save Session id_loc
                            HttpContext.Session.SetString("FileLoc_ID", t_id_loc.ToString());
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/BagMiss/", t_fname.ToString());
                            try
                            {
                                using (var stream = new FileStream(path, FileMode.Create))
                                {

                                    await formFile.CopyToAsync(stream);

                                }
                            }
                            catch (Exception ex)
                            {
                                strMess = ex.Message;
                                return Json(new { mess = strMess });
                            }

                            using (var client = new SftpClient(host, Int16.Parse(port), username, password))
                            {
                                client.Connect();
                                if (client.IsConnected)
                                {

                                    try
                                    {
                                        //Call Api Upload SFTP File
                                        CallAPI.UploadSFTPFile(host, username, password, path, "HS_Miss_Bag", Int32.Parse(port));

                                    }
                                    catch (Exception ex)
                                    {
                                        strMess = ex.Message;
                                        return Json(new { mess = strMess });
                                    }

                                    string ContentBagHS = CallAPI.BagHS_Miss_AttachedFile_Update(Int32.Parse(t_BagMiss_ID.ToString()), t_id_loc, "OK", t_token);
                                    var oData_ = JObject.Parse(ContentBagHS);
                                    if (oData_["ResultCode"].ToString() == "0")
                                    {

                                        string ContentHS = CallAPI.BagHS_Miss_AttachedFile_Get(Int32.Parse(t_BagMiss_ID), t_token);
                                        JObject ser = JObject.Parse(ContentHS);
                                        Int32 _Result = (int)ser.SelectToken("ResultCode");
                                        if (_Result == 0)
                                        {
                                            var ser2 = ser.SelectToken("Data");
                                            List<JToken> data = new List<JToken>(ser2.Children());
                                            List<FileDisplay> filedisplay = new List<FileDisplay>();
                                            foreach (var item in data)
                                            {
                                                FileDisplay fd = new FileDisplay();
                                                fd.FileName = formFile.FileName.ToString();
                                                fd.SysFileName = item.SelectToken("sysFileName").ToString();
                                                fd.LastUserUpdate = item.SelectToken("LastUserUpdate").ToString();
                                                fd.LastUpdate = item.SelectToken("LastUpdate").ToString();
                                                filedisplay.Add(fd);
                                            }
                                            // Save  Session object AttachFile
                                            SessionHelper.SetObjectAsJson(HttpContext.Session, "Miss_AttachFileHS", filedisplay);
                                            HttpContext.Session.SetString("UPLOADED","OK");
                                            strMess = "OK";
                                            return RedirectToAction("Index");

                                        }
                                        else
                                        {
                                            strMess = "ERROR";
                                        }

                                    }
                                    else
                                    {
                                        strMess = oData_["Message"].ToString();
                                    }
                                }
                                else
                                {
                                    strMess = "ERROR";
                                }

                            }

                        }

                    }

                }

                // Lưu Session Upload File
                HttpContext.Session.SetString("UPLOADED", "OK");
                return RedirectToAction("Index");



            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpPost]
        public IActionResult BaggageMiss_Delete(string t_bagmissdetail_id)
        {
            string strMess = string.Empty;
            //Get Session Token
            var t_token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
            string Content = CallAPI.MisBagDetailDelete(Int32.Parse(t_bagmissdetail_id.ToString()), t_token);
            var oData = JObject.Parse(Content);

            var Result = oData["ResultCode"].ToString();
            var Message = oData["Message"].ToString();
            if (Result == "0")
            {
                strMess = "OK";


            }
            else
            {
                strMess = Message;
            }

            return Json(new { mess = strMess });
        }

        [HttpPost]
        public IActionResult Delete_FileAttach(string t_fpath)
        {
            string strMess = string.Empty;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var filePath = Path.Combine(contentRootPath, "wwwroot", "data/BagMiss/" + t_fpath);



            System.IO.DirectoryInfo di = new DirectoryInfo(filePath);

            // Check if file exists with its full path    
            if (System.IO.File.Exists(filePath))
            {
                // If file found, delete it    
                System.IO.File.Delete(filePath);
                strMess = "OK";
                // Lưu Session Delete để load lại trang còn handle được
                HttpContext.Session.SetString("Delete", "YES");
                //Còn vấn đề xóa file trên SFTP Server thông qua API của anh Thọ
                //Get Session Token
                string token = HttpContext.Session.GetString("Token");
                ////Get Session id_loc
                //string id_loc=HttpContext.Session.GetString("FileLoc_ID");
                //Get Session BagMiss_ID 
                string BagMiss_ID = HttpContext.Session.GetString("BagMiss_ID");

                //Goi lai Session Object
                List<FileDisplay> _attachfileList = SessionHelper.GetObjectFromJson<List<FileDisplay>>(HttpContext.Session, "Miss_AttachFileHS");
                foreach (var item in _attachfileList)
                {
                    if (item.SysFileName == t_fpath)
                    {
                        string ContentBagHS = CallAPI.BagHS_Miss_AttachedFile_Update(Int32.Parse(BagMiss_ID.ToString()), item.FileLoc_ID.ToString(), "XX", token);
                    }
                }
                return Json(new { mess = "OK" });
                
            


            }
            else
            {
                strMess = "Delete attach file fail!";

            }
            return Json(new { mess = strMess });
        }


        private bool CheckIfFileExistsOnServer(string t_host, int t_port, string t_folder, string t_username, string t_password)
        {
            using (var sftp = new SftpClient(t_host, t_port, t_username, t_password))
            {
                try
                {
                    sftp.Connect();
                    return true;
                }
                catch (Exception ex)
                {
                    SaveLog.WriteLog(ex.Message.ToString());
                    return false;
                }
            }
        }
        private void DownloadFilesAttached(string t_FileName)
        {
            //Đọc file json lấy thông tin đăng nhập SFTP Server
            var Host = configuration["SFTP:Host"];
            var Port = configuration["SFTP:Port"];
            var Username = configuration["SFTP:Username"];
            var Password = configuration["SFTP:Password"];
            string RemoteFileName = "/upload/HS_Miss_Bag/" + t_FileName;
            // Kiểm tra sự tồn tại của tập tin
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string filePath = Path.Combine(contentRootPath, "wwwroot", "data/BagMiss/" + t_FileName);

            if (!System.IO.File.Exists(filePath) && CheckIfFileExistsOnServer(Host, Int16.Parse(Port), filePath, Username, Password) == true)
            {

                using (var sftp = new SftpClient(Host, Int16.Parse(Port), Username, Password))
                {
                    sftp.Connect();
                    try
                    {
                        using (var file = System.IO.File.OpenWrite(filePath))
                        {
                            sftp.DownloadFile(RemoteFileName, file);
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveLog.WriteLog(ex.Message);
                    }


                    sftp.Disconnect();
                }

            }

        }

        [HttpPost]
        public IActionResult ViewAttached(string t_FileName)
        {
            DownloadFilesAttached(t_FileName);
            return Json(new { mess = "OK", fname = t_FileName.ToString() });
        }
    }
}

