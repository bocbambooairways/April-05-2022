using BOC.Areas.BaggageMiss.Models;
using BOC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BOC.Controllers
{
    public class BagFoundController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IViewRenderService _viewRenderService;
        private readonly IFileProvider fileProvider;
        private readonly IConfiguration configuration;
        List<FileAttach_HS> _attchList = new List<FileAttach_HS>();
        private object lst_attached;
        public object AttachFile { get; private set; }

        public BagFoundController(IWebHostEnvironment webHostEnvironment, IViewRenderService viewRenderService, IConfiguration configuration, IFileProvider fileProvider)
        {
            _webHostEnvironment = webHostEnvironment;
            _viewRenderService = viewRenderService;
            this.fileProvider = fileProvider;
            this.configuration = configuration;

           
        }
        public List<BagMissHS_Model> GetBagMiss_Hs(int t_bagfound_id, string t_airport, string t_fromDate, string t_toDate)
        {
            if (t_airport == null || t_airport == "")
            {
                t_airport = string.Empty;
            }
            if (t_fromDate == null || t_fromDate == "")
            {
                //Lấy ngày đầu tiên của tháng
                var dateAndTimefrom = DateTime.Now;
                //DateTime date = new DateTime();
                var firstDayOfMonth = new DateTime(dateAndTimefrom.Year, dateAndTimefrom.Month, 1);
                t_fromDate = (firstDayOfMonth.Date).ToString("yyyy-MM-dd");

            }
            if (t_toDate == null || t_toDate == "")
            {

                var dateAndTimeto = DateTime.Now;
                t_toDate = (dateAndTimeto.Date).ToString("yyyy-MM-dd");

            }
            //Get Session Token
            var token = HttpContext.Session.GetString("Token");
            //Call api to get BagHS_Found
            string Content = CallAPI.BagHS_Found_Get(Int32.Parse(t_bagfound_id.ToString()), t_airport.ToString(), t_fromDate.ToString(), t_toDate.ToString(), token);
            var oData = JObject.Parse(Content);
            if (oData["ResultCode"].ToString() == "0")
            {
                List<BagMissHS_Model> lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BagMissHS_Model>>(oData["Data"].ToString());
                return lst;
            }
            else
            {
                //Set Session Error When Api Fail
                HttpContext.Session.SetString("ErrorMessage", oData["Message"].ToString());
                return null;
            }

        }
        public List<BaggageMissDescModel_HS> GetMissDesc_HS(string t_BagMissDetail_ID)
        {
            t_BagMissDetail_ID = t_BagMissDetail_ID == null ? "0" : t_BagMissDetail_ID;
            //Get Session Token
            var token = HttpContext.Session.GetString("Token");
            List<BaggageMissDescModel_HS> missDesc = new List<BaggageMissDescModel_HS>();
            //Get path url api
            Url misbagprofiledesc = new Url();
            string url = misbagprofiledesc.Get("MissBagDescriptionGet");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagMissDetail_ID", t_BagMissDetail_ID));
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var reqdesc = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage resdesc;
            resdesc = Client.SendAsync(reqdesc).Result;
            Content = resdesc.Content.ReadAsStringAsync().Result;
            var oData = JObject.Parse(Content);
            if (oData["ResultCode"].ToString() == "0")
            {
                BaggageMissDesc_HS lstdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissDesc_HS>(Content);
                missDesc.Add(lstdesc.Data);
                var arraybag = lstdesc.Data.BagDesc.ToArray();
                ViewBag.MissBagDescHS = lstdesc.Data.BagDesc;
                // Save  Session object BagDesc
                SessionHelper.SetObjectAsJson(HttpContext.Session, "BagDescription", arraybag);
                return missDesc;
            }
            else
            {
                //Set Session Error When Api Fail
                HttpContext.Session.SetString("ErrorMessage", oData["Message"].ToString());
                return null;
            }
        }
        public IActionResult Index()
        {


            //Get Session Object Attach File
            var _attachfileList = SessionHelper.GetObjectFromJson<List<FileAttach_HS>>(HttpContext.Session, "Found_AttachFileHS");
            List<FileAttach_HS> _lstfileattached = new List<FileAttach_HS>();
            FileAttach_HS fa = new FileAttach_HS();
            _lstfileattached.Add(fa);
            // Save  Session object Found_AttachFileHS
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Found_AttachFileHS", _lstfileattached);
            HttpContext.Session.SetString("BagFound_ID", "0");


            ViewModelHS mymodel = new ViewModelHS();
            mymodel.BagDesc = GetMissDesc_HS("-1");
            mymodel.Table_HS = GetBagMiss_Hs(0, null, null, null);

            return View(mymodel);
        }


        [HttpPost]
        public IActionResult Index(string t_FromDate, string t_ToDate, string t_Station, string t_KeySearch)
        {

            string strMess = string.Empty;
            ViewModelHS mymodel = new ViewModelHS();
            if (t_FromDate == "" || t_FromDate == null)
            {
                mymodel.ErrorMessage = "FromDate not be empty.";
                //Get BagDesc For Add New
                mymodel.BagDesc = GetMissDesc_HS("0");
                return View(mymodel);
            }

            if (t_ToDate == "" || t_ToDate == null)
            {
                mymodel.ErrorMessage = "ToDate not be empty.";
                //Get BagDesc For Add New
                mymodel.BagDesc = GetMissDesc_HS("0");
                return View(mymodel);
            }

            DateTime fd = DateTime.ParseExact(t_FromDate, "dd/MM/yyyy", null);
            DateTime td = DateTime.ParseExact(t_ToDate, "dd/MM/yyyy", null);
            if (fd > td)
            {
                mymodel.ErrorMessage = "ToDate must be larger than FromDate.";
                ViewData["FromDate"] = fd.ToString();
                // Save  Session FromDate
                HttpContext.Session.SetString("FromDate", t_FromDate);
                ViewData["ToDate"] = td.ToString();
                // Save  Session ToDate
                HttpContext.Session.SetString("ToDate", t_ToDate);
                return View(mymodel);
            }

            t_Station = t_Station == null ? "" : t_Station;
            t_KeySearch = t_KeySearch == null ? "" : t_KeySearch;

            //Get Session Token
            var token = HttpContext.Session.GetString("Token");
            //Goi API de lay ho so found theo ngay
            string Content = CallAPI.BagHS_Found_Get(0, fd.ToString("yyyy-MM-dd"), td.ToString("yyyy-MM-dd"), t_Station, t_KeySearch, token);
            JObject oData = JObject.Parse(Content);
            string str = oData.SelectToken("Data").ToString();
            List<BagMissHS_Model> lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BagMissHS_Model>>(str);
            // Save  Session object for mymodel.Table_HS
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Table_HS", lst);
            if (lst != null)
            {
                strMess = "OK";
                // Save  Session FromDate && ToDate
                HttpContext.Session.SetString("FromDate", fd.ToString());
                HttpContext.Session.SetString("ToDate", td.ToString());

                // Get  Session object Table_HS
                List<BagMissHS_Model> _tablehs = SessionHelper.GetObjectFromJson<List<BagMissHS_Model>>(HttpContext.Session, "Table_HS");
                mymodel.Table_HS = _tablehs;

            }
            else
            {

                mymodel.ErrorMessage = oData.SelectToken("Message").ToString();
                // Save  Session FromDate && ToDate
                HttpContext.Session.SetString("FromDate", fd.ToString());
                HttpContext.Session.SetString("ToDate", td.ToString());
                mymodel.Table_HS = null;

            }
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(mymodel.Table_HS);

            return Json(new { mess = strMess, obj = data });
        }

        [HttpPost]
        public IActionResult NewAndEdit(ViewModelHS model)
        {

            model.Remark = model.Remark == null ? "" : model.Remark;

            //Định dạng xóa bỏ dấu chấm (đối với Phone), dấu phẩy (đối với PC) trong chuỗi string-> chuyển về string bt
            string _amount = model.TotalAmount;
            model.TotalAmount = _amount.Replace(",", "").Replace(".", "").Replace(" ", "");



            model.BagFound_ID = model.BagFound_ID == null ? "0" : model.BagFound_ID;
            HttpContext.Session.SetString("BagFound_ID", model.BagFound_ID);

            //Định dạng lại model radio selected(cắt bỏ và thay thế ký tự thừa)
            model.RadioSelected = model.RadioSelected.Replace("\"", string.Empty);
            model.RadioSelected = model.RadioSelected.Replace("[", string.Empty);
            model.RadioSelected = model.RadioSelected.Replace("]", "").Trim('"').TrimEnd(',');
            string[] arrRadioSelected = model.RadioSelected.Split(',');

            string strMess = string.Empty;
            string MessageEr = string.Empty;


            model.Remark = model.Remark == "undefined" ? "" : model.Remark;


            string FDate = DateTime.ParseExact(model.DatePicker, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //Get Session Token
            var token = HttpContext.Session.GetString("Token");



            //Dựng biến flag để kiểm tra 2 xem action là new hay edit
            int flag = Int32.Parse(model.BagFound_ID.ToString());
            //Gọi hàm Lấy Danh Sách BagDesc với các Raio User check chọn
            string BagDescLst = GetBagDescListCheck(arrRadioSelected);

            //Goi api để update BagHS_Found
            try
            {

                string Content = CallAPI.BagHS_Found_Update(Int32.Parse(model.BagFound_ID.ToString()), "", FDate.ToString(), model.Station.ToString(),
                model.Remark.ToString(), model.Item.ToString(), model.Quantity.ToString(), model.TotalAmount, model.Currency.ToString(), "OK", BagDescLst, token);
                BaggageMissDesc_HS obj = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissDesc_HS>(Content);


                if (obj.ResultCode != 0)
                {
                    var result = JObject.Parse(Content);
                    strMess = result["Message"].ToString();
                    return Json(new { mess = strMess });
                }

                var host = configuration["SFTP:Host"];
                var port = configuration["SFTP:Port"];
                var username = configuration["SFTP:Username"];
                var password = configuration["SFTP:Password"];
                string subPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/BagHS_Found/");
                bool exists = System.IO.Directory.Exists(subPath);

                if (flag == 0)
                {

                    //Get Session Object Attach File
                    var _attachfileList = SessionHelper.GetObjectFromJson<List<FileAttach_HS>>(HttpContext.Session, "Found_AttachFileHS");
                    if (_attachfileList.Count > 1)
                    {
                        foreach (var item in _attachfileList)
                        {
                            if (item.Status == "OK" && item.BagMiss_ID == 0)
                            {
                                string rs = CallAPI.BagHS_Found_AttachedFile_Update(obj.Data.BagFound_ID, item.FileLoc_ID.ToString(), "OK", token);
                                if (rs == "OK")
                                {
                                    strMess = "OK";
                                }
                                else
                                {
                                    strMess = "ER";
                                    MessageEr = "Save Baggage Fail";
                                }


                            }
                            if (item.Status == "XX" && item.BagMiss_ID == 0)
                            {
                                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/BagHS_Found/" + item.sysFileName.ToString());
                                if (System.IO.File.Exists(path))//check file exsit or not 
                                {
                                    // If file found, delete it    
                                    System.IO.File.Delete(path);

                                }

                            }
                        }


                        return Json(new { mess = strMess, er = MessageEr });
                    }
                    if (_attachfileList.Count <= 1)
                    {

                        strMess = "OK";
                        return Json(new { mess = strMess });

                    }



                }
                else
                {

                    //Get Session Object Attach File
                    var _attachfileList = SessionHelper.GetObjectFromJson<List<FileAttach_HS>>(HttpContext.Session, "Found_AttachFileHS");

                    foreach (var item in _attachfileList)
                    {
                        if (item.Status == "OK" && item.FileLoc_ID != 0)
                        {
                            string rs = CallAPI.BagHS_Found_AttachedFile_Update(obj.Data.BagFound_ID, item.FileLoc_ID.ToString(), "OK", token);
                            if (rs == "OK")
                            {
                                strMess = "OK";
                            }
                            else
                            {
                                strMess = "ER";
                                MessageEr = "Save Baggage Fail";
                            }


                        }
                        if (item.Status == "XX" && item.FileLoc_ID != 0)
                        {
                            CallAPI.BagHS_Found_AttachedFile_Update(obj.Data.BagFound_ID, item.FileLoc_ID.ToString(), "XX", token);
                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/BagHS_Found/" + item.sysFileName.ToString());
                            if (System.IO.File.Exists(path))//check file exsit or not 
                            {
                                // If file found, delete it    
                                System.IO.File.Delete(path);

                            }
                        }

                    }

                    //Save Session object Found_AttachFileHS
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Found_AttachFileHS", "");
                    return Json(new { mess = strMess, er = MessageEr });

                }

            }
            catch (Exception ex)
            {
                return Json(new { mess = ex.Message });
                //SaveLog.WriteLog(ex.Message);
                //SaveLog.WriteLog(ex.StackTrace);
            }
            //finally
            //{
            //    //SaveLog.WriteLog("4:Sucess");

            //}

            return Json(new { mess = "OK" });

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
        public async Task<IActionResult> ShowModalEdit(string t_bagfound_id)
        {
            try
            {

                string strMess = string.Empty;
                //Get Session Token
                var token = HttpContext.Session.GetString("Token");
                List<BaggageMissDescModel_HS> missDesc = new List<BaggageMissDescModel_HS>();
                //Goi ham lay BagHS_Found_Edit
                string Content = CallAPI.BagHS_Found_Edit(Int32.Parse(t_bagfound_id), token);
                BaggageMissDesc_HS lstdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissDesc_HS>(Content);
                strMess = lstdesc.Message;
                if (lstdesc.ResultCode == 0)
                {
                    //Save Session BagFound_ID
                    HttpContext.Session.SetString("BagFound_ID", t_bagfound_id.ToString());
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


                    //Connect Api to get File Name Attached
                    string Contentbag = CallAPI.BagHS_Found_AttachedFile_Get(Int32.Parse(t_bagfound_id.ToString()), token);


                    JObject ser = JObject.Parse(Contentbag);
                    Int32 _Result = (int)ser.SelectToken("ResultCode");
                    var ser2 = ser.SelectToken("Data");


                    if (_Result == 0)
                    {

                        string Message = ser.SelectToken("Message").ToString();
                        if (_Result == 0)
                        {

                            List<JToken> data = new List<JToken>(ser2.Children());
                            //Lưu lại danh sách file đính kèm vô Session Object
                            List<FileAttach_HS> FileAttach_HS = new List<FileAttach_HS>();
                            foreach (var item in data)
                            {
                                FileAttach_HS fattach_hs = new FileAttach_HS();
                                fattach_hs.BagMiss_ID = Int32.Parse(item.SelectToken("BagMiss_ID").ToString());
                                fattach_hs.FileLoc_ID = Int32.Parse(item.SelectToken("FileLoc_ID").ToString());
                                fattach_hs.FileName = item.SelectToken("FileName").ToString();
                                fattach_hs.sysFileName = item.SelectToken("sysFileName").ToString();
                                fattach_hs.Status = "OK";
                                FileAttach_HS.Add(fattach_hs);

                            }

                            // Save  Session object Found_AttachFileHS
                            SessionHelper.SetObjectAsJson(HttpContext.Session, "Found_AttachFileHS", FileAttach_HS);

                            //Render View To Open Modal Edit
                            strMess = "Successful.";
                            // Save  Session strMess
                            HttpContext.Session.SetString("Mess", strMess);
                            ViewBag.MissBagDescEdit = lstdesc.Data;
                            var result = await this.RenderViewAsync("ShowModalEdit", lstdesc, true);
                            return Json(new { mess = strMess, rs = result });

                        }
                        else
                        {

                            strMess = "Successful.";

                            ViewBag.MissBagDescEdit = lstdesc.Data;
                            var result = await this.RenderViewAsync("ShowModalEdit", lstdesc, true);
                            return Json(new { mess = strMess, rs = result });
                        }
                    }
                    else
                    {
                        List<FileAttach_HS> FileAttach_HS = new List<FileAttach_HS>();
                        FileAttach_HS fa = new FileAttach_HS();
                        FileAttach_HS.Add(fa);
                        strMess = "Successful.";
                        ViewBag.MissBagDescEdit = lstdesc.Data;
                        // Save  Session object Found_AttachFileHS
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "Found_AttachFileHS", FileAttach_HS);
                        var result = await this.RenderViewAsync("ShowModalEdit", lstdesc, true);
                        return Json(new { mess = strMess, rs = result });
                    }


                }


                return Json(new { mess = strMess });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public IActionResult RemoveAttachFile_HS(ViewModelHS model)
        {
            //Goi lai Session Object
            List<FileAttach_HS> _attachfileList = SessionHelper.GetObjectFromJson<List<FileAttach_HS>>(HttpContext.Session, "Found_AttachFileHS");
            string filexoa = model.FileRemove;
            foreach (var item in _attachfileList)
            {
                if (item.sysFileName == filexoa)
                {
                    item.Status = "XX";
                }
            }
            // SAVE Session object Found_AttachFileHS
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Found_AttachFileHS", _attachfileList);
            lst_attached = Newtonsoft.Json.JsonConvert.SerializeObject(_attachfileList);
            return Json(new { obj = lst_attached, mess = "OK" });

        }

        [HttpPost]
        public async Task<IActionResult> UploadAttachFile_HS(ViewModelHS model)
        {


            var host = configuration["SFTP:Host"];
            var port = configuration["SFTP:Port"];
            var username = configuration["SFTP:Username"];
            var password = configuration["SFTP:Password"];
            string strMess = string.Empty;
            //Get Session Token
            var token = HttpContext.Session.GetString("Token");
            if (model.Files.Count < 0)
            {
                strMess = "Upload fail";
                return Json(new { mess = strMess });// Khai bao loi
            }



            //Goi lai Session Object
            List<FileAttach_HS> _attachfileList = SessionHelper.GetObjectFromJson<List<FileAttach_HS>>(HttpContext.Session, "Found_AttachFileHS");

            foreach (var file in model.Files)
            {
                string filethat = file.FileName.ToString();
                string Content = CallAPI.FTP_FileLocation_Generator("BAGHS_FOUND", filethat, token);

                var oData = JObject.Parse(Content);
                string Message = oData["Message"].ToString();

                if (oData["ResultCode"].ToString() != "0")
                {
                    strMess = Message;
                    return Json(new { mess = strMess });
                }

                string fname = oData["Data"]["FileName"].ToString();
                Int32 file_loc = Int32.Parse(oData["Data"]["ID"].ToString());

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/BagHS_Found/", fname);


                //Upload file len web server
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    try
                    {
                        await file.CopyToAsync(stream);
                    }
                    catch (Exception ex)
                    {
                        strMess = ex.Message;
                        return Json(new { mess = strMess });
                    }
                }



                //upload file len SFTP
                var filePaths = new List<string>();

                using (var client = new SftpClient(host, Int16.Parse(port), username, password))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        try
                        {
                            //Call Api Upload SFTP File
                            CallAPI.UploadSFTPFile(host, username, password, path, "HS_Found_Bag", Int32.Parse(port));

                        }
                        catch (Exception ex)
                        {
                            strMess = ex.Message;
                            return Json(new { mess = strMess });
                        }




                        FileAttach_HS fd = new FileAttach_HS();
                        fd.FileLoc_ID = file_loc;
                        fd.BagMiss_ID = 0;
                        fd.FileName = filethat;
                        fd.sysFileName = fname;
                        fd.Status = "OK";
                        _attachfileList.Add(fd);



                    }
                }
                // SAVE Session object Found_AttachFileHS
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Found_AttachFileHS", _attachfileList);

                lst_attached = Newtonsoft.Json.JsonConvert.SerializeObject(_attachfileList);

            }

            return Json(new { obj = lst_attached, mess = "Sucess." });

        }

        [HttpPost]
        public IActionResult ClearTrash_HS(ViewModelHS model)// CHẠY VÔ KHI USER CANCEL HỦY
        {
            //Goi lai Session Object
            List<FileAttach_HS> _attchfileList = SessionHelper.GetObjectFromJson<List<FileAttach_HS>>(HttpContext.Session, "Found_AttachFileHS");
            string strMess = string.Empty;
            //Get FileLoc_ID
            var file_loc = HttpContext.Session.GetString("FileLoc_ID");
            //Get Session Token
            var token = HttpContext.Session.GetString("Token");



            foreach (var item in _attchfileList)
            {
                if (item.BagMiss_ID == 0)
                {

                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/BagHS_Found/" + item.sysFileName.ToString());
                    if (System.IO.File.Exists(path))//check file exsit or not 
                    {
                        // If file found, delete it
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch (Exception ex)
                        {
                            SaveLog.WriteLog(ex.Message);
                        }

                    }
                }



            }
            // Reset Session object Found_AttachFileHS
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Found_AttachFileHS", "");

            return Json(new { mess = "OK" });

        }

        private void DownloadFiles(string FileName)
        {


            //Đọc file json lấy thông tin đăng nhập SFTP Server
            var Host = configuration["SFTP:Host"];
            var Port = configuration["SFTP:Port"];
            var Username = configuration["SFTP:Username"];
            var Password = configuration["SFTP:Password"];
            String RemoteFileName = "/upload/BaggageMiss/" + FileName;
            String LocalDestinationFilename = "";
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            LocalDestinationFilename = Path.Combine(contentRootPath, "wwwroot", "images/BaggageMiss/" + FileName);


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

        private bool CheckIfFileExistsOnServer(string t_host, int t_port, string t_folder, string t_username, string t_password)
        {
            using (var sftp = new SftpClient(t_host, t_port, t_username, t_password))
            {
                try
                {
                    sftp.Connect();
                    //return sftp.Exists(t_folder);
                    return true;
                }
                catch (Exception ex)
                {
                    SaveLog.WriteLog(ex.Message);
                    return false;
                }
            }
        }
        private void DownloadFilesAttached(string FileName)
        {
            //Đọc file json lấy thông tin đăng nhập SFTP Server
            var Host = configuration["SFTP:Host"];
            var Port = configuration["SFTP:Port"];
            var Username = configuration["SFTP:Username"];
            var Password = configuration["SFTP:Password"];
            string RemoteFileName = "/upload/HS_Found_Bag/" + FileName;
            // Kiểm tra sự tồn tại của tập tin
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string filePath = Path.Combine(contentRootPath, "wwwroot", "data/BagHS_Found/" + FileName);

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
