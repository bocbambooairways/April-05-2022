using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class CallAPI
    {
       
        public static string FTP_FileLocation_Generator(string t_DataType, string t_filename, string t_token)
        {
            //Tạo FTP_FileLocation_Generator để lấy tên file và upload lên sftp
            Url sftplocation = new Url();
            string url = sftplocation.Get("FTP_FileLocation_Generator");
            HttpClient ClientFTP = new HttpClient();
            var nnvc = new List<KeyValuePair<string, string>>();
            nnvc.Add(new KeyValuePair<string, string>("DataType", t_DataType));
            nnvc.Add(new KeyValuePair<string, string>("FileName", t_filename));
            ClientFTP.DefaultRequestHeaders.Add("Authorization", t_token);
            var _req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nnvc) };

            string _Content;
            HttpResponseMessage _res;
            _res = ClientFTP.SendAsync(_req).Result;
            _Content = _res.Content.ReadAsStringAsync().Result;
            return _Content;
        }

        public static string BagHS_Found_Get(int t_BagFound_ID, string t_FromDate, string t_ToDate, string t_Station, string t_KeySearch,string t_token)
        {
            Url misbag = new Url();
            string uri = misbag.Get("BagHS_Found_Get");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagFound_ID", t_BagFound_ID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("Airport", t_Station));
            nvc.Add(new KeyValuePair<string, string>("KeySearch", t_KeySearch));
            nvc.Add(new KeyValuePair<string, string>("FromDate", t_FromDate));
            nvc.Add(new KeyValuePair<string, string>("ToDate", t_ToDate));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
        }

        public static string BagHS_Found_AttachedFile_Update(int t_BagFound_ID, string t_fileloc, string t_status, string t_token)
        {
            
            Url BagMiss = new Url();
            string url = BagMiss.Get("BagHS_Found_AttachedFile_Update");

            HttpClient ClientBagHS = new HttpClient();
            var _nvc = new List<KeyValuePair<string, string>>();
            _nvc.Add(new KeyValuePair<string, string>("BagFound_ID", t_BagFound_ID.ToString()));
            _nvc.Add(new KeyValuePair<string, string>("FileLoc_ID", t_fileloc));
            _nvc.Add(new KeyValuePair<string, string>("Status", t_status));
            ClientBagHS.DefaultRequestHeaders.Add("Authorization", t_token);
            var reqBagHS = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(_nvc) };
            string rs = "OK";
            try
            {
                string ContentBagHS;
                HttpResponseMessage resBagHS;
                resBagHS = ClientBagHS.SendAsync(reqBagHS).Result;
                ContentBagHS = resBagHS.Content.ReadAsStringAsync().Result;
 
            }
            catch (Exception ex)
            {
                SaveLog.WriteLog(ex.Message);
            }
            return rs;



        }

        public static string BagHS_Found_AttachedFile_Get(int t_BagFound_ID, string t_token)
        {
            Url BagMiss = new Url();
            string url = BagMiss.Get("BagHS_Found_AttachedFile_Get");

            HttpClient ClientBagHS = new HttpClient();
            var _nvc = new List<KeyValuePair<string, string>>();
            _nvc.Add(new KeyValuePair<string, string>("BagFound_ID", t_BagFound_ID.ToString()));
            ClientBagHS.DefaultRequestHeaders.Add("Authorization", t_token);
            var reqBagHS = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(_nvc) };

            string ContentBagHS;
            HttpResponseMessage resBagHS;
            resBagHS = ClientBagHS.SendAsync(reqBagHS).Result;
            ContentBagHS = resBagHS.Content.ReadAsStringAsync().Result;
            return ContentBagHS;

        }
        public static string BagHS_Found_Edit(int t_BagFound_ID, string t_token)
        {
            Url BagMiss = new Url();
            string url = BagMiss.Get("BagHS_Found_Edit");

            HttpClient ClientBagHS = new HttpClient();
            var _nvc = new List<KeyValuePair<string, string>>();
            _nvc.Add(new KeyValuePair<string, string>("BagFound_ID", t_BagFound_ID.ToString()));
            ClientBagHS.DefaultRequestHeaders.Add("Authorization", t_token);
            var reqBagHS = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(_nvc) };

            string ContentBagHS;
            HttpResponseMessage resBagHS;
            resBagHS = ClientBagHS.SendAsync(reqBagHS).Result;
            ContentBagHS = resBagHS.Content.ReadAsStringAsync().Result;
            return ContentBagHS;

        }

        public static string BagHS_Found_Update(int t_BagFound_ID,string t_HS_No,string t_FDate,string t_Station,string t_Remark,string t_Item,string t_Quantity,string t_TotalAmount,string t_Currency,string t_Status,string t_BagDescLst,  string t_token)
        {
          

            Url misbag = new Url();
            string uri = misbag.Get("BagHS_Found_Update");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagFound_ID", t_BagFound_ID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("HS_No", t_HS_No));
            nvc.Add(new KeyValuePair<string, string>("HS_Date", t_FDate));
            nvc.Add(new KeyValuePair<string, string>("Airport", t_Station));
            nvc.Add(new KeyValuePair<string, string>("Remark", t_Remark));
            nvc.Add(new KeyValuePair<string, string>("BrandName", t_Item));
            nvc.Add(new KeyValuePair<string, string>("Qty", t_Quantity));
            nvc.Add(new KeyValuePair<string, string>("Total", t_TotalAmount));
            nvc.Add(new KeyValuePair<string, string>("Currency", t_Currency));
            nvc.Add(new KeyValuePair<string, string>("Status", t_Status));
            nvc.Add(new KeyValuePair<string, string>("BagDescription", t_BagDescLst));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
            

        }

        public static string BagHS_Found_Get(int t_BagFound_ID, string t_Airport,string t_FromDate, string t_ToDate, string t_token)
        {
            Url misbag = new Url();
            string uri = misbag.Get("BagHS_Found_Get");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagFound_ID", t_BagFound_ID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("Airport", t_Airport));
            nvc.Add(new KeyValuePair<string, string>("FromDate", t_FromDate));
            nvc.Add(new KeyValuePair<string, string>("ToDate", t_ToDate));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;

        }
        public static string MisBagProfileGet(string t_PNR, string t_FltNo, string t_BagMiss_ID, string t_token)
        {
            
            Url misbag = new Url();
            string uri = misbag.Get("MisBagProfileGet");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("PNR", t_PNR));
            nvc.Add(new KeyValuePair<string, string>("FltNo", t_FltNo));
            nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", t_BagMiss_ID));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
        }

        public static string MisBagProfileUpdate(int t_BagMiss_ID,string t_PNR, string t_FltNo,string t_Type,string t_FullName,string t_Email,string t_Address,string t_Phone,string t_BBCNo,string t_Remark,string t_Status,  string t_token)
        {
            Url misbag = new Url();
            string uri = misbag.Get("MisBagProfileUpdate");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", t_BagMiss_ID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("PNR", t_PNR));
            nvc.Add(new KeyValuePair<string, string>("FltNo", t_FltNo));
            nvc.Add(new KeyValuePair<string, string>("Type", t_Type));
            nvc.Add(new KeyValuePair<string, string>("FullName", t_FullName));
            nvc.Add(new KeyValuePair<string, string>("Email", t_Email));
            nvc.Add(new KeyValuePair<string, string>("Address", t_Address));
            nvc.Add(new KeyValuePair<string, string>("Phone", t_Phone));
            nvc.Add(new KeyValuePair<string, string>("BBCNo", t_BBCNo));
            nvc.Add(new KeyValuePair<string, string>("Remark", t_Remark));
            nvc.Add(new KeyValuePair<string, string>("Status", t_Status));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
        }

        public static string MisBagDetaiGet(int t_BagMissDetail_ID, int t_BagMiss_ID, string t_token)
        {
            Url misbagprofile = new Url();
            string url = misbagprofile.Get("MisBagDetaiGet");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagMissDetail_ID", t_BagMissDetail_ID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", t_BagMiss_ID.ToString()));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
        }

        public static string MisBagDetaiUpdate(int t_BagMissDetail_ID, int t_BagMiss_ID,string t_Item,string t_UseDate,string t_Quantity,string t_TotalAmount,string t_Currency,string t_Remark,string t_Status, string t_BagDescLst, string t_token)
        {
            Url misbag = new Url();
            string uri = misbag.Get("MisBagDetailUpdate");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagMissDetail_ID", t_BagMissDetail_ID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", t_BagMiss_ID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("Item", t_Item));
            nvc.Add(new KeyValuePair<string, string>("UseDate", t_UseDate));
            nvc.Add(new KeyValuePair<string, string>("Qty", t_Quantity));
            nvc.Add(new KeyValuePair<string, string>("Total", t_TotalAmount));
            nvc.Add(new KeyValuePair<string, string>("Currency", t_Currency));
            nvc.Add(new KeyValuePair<string, string>("Remark", t_Remark));
            nvc.Add(new KeyValuePair<string, string>("Status", "OK"));
            nvc.Add(new KeyValuePair<string, string>("BagDescription", t_BagDescLst));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
        }

        public static string MisBagDetailDelete(int t_BagMissDetail_ID,string t_token)
        {
            Url BaggageMiss = new Url();
            string uri = BaggageMiss.Get("MisBagDetailDelete");

            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagMissDetail_ID", t_BagMissDetail_ID.ToString()));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
        }
        public static string MissBagDescriptionGet(int t_BagMissDetail_ID, string t_token)
        {
            Url misbagprofiledesc = new Url();
            string url = misbagprofiledesc.Get("MissBagDescriptionGet");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagMissDetail_ID", t_BagMissDetail_ID.ToString()));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var reqdesc = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage resdesc;
            resdesc = Client.SendAsync(reqdesc).Result;
            Content = resdesc.Content.ReadAsStringAsync().Result;
            return Content;
        }

        public static string BagHS_Miss_AttachedFile_Get(int t_BagMiss_ID, string t_token)
        {
            Url BagMissInfo = new Url();
            string urlBagMissInfo = BagMissInfo.Get("BagHS_Miss_AttachedFile_Get");
            HttpClient ClientHS = new HttpClient();
            var nvcget = new List<KeyValuePair<string, string>>();
            nvcget.Add(new KeyValuePair<string, string>("BagMiss_ID", t_BagMiss_ID.ToString()));
            ClientHS.DefaultRequestHeaders.Add("Authorization", t_token);
            var reqHS = new HttpRequestMessage(HttpMethod.Post, urlBagMissInfo) { Content = new FormUrlEncodedContent(nvcget) };

            string ContentHS;
            HttpResponseMessage resHS;
            resHS = ClientHS.SendAsync(reqHS).Result;
            ContentHS = resHS.Content.ReadAsStringAsync().Result;
            return ContentHS;

        }

        public static string BagHS_Miss_AttachedFile_Update(int t_BagMiss_ID, string t_fileloc, string t_status, string t_token)
        {
            Url BagMiss = new Url();
            string url = BagMiss.Get("BagHS_Miss_AttachedFile_Update");
            HttpClient ClientBagHS = new HttpClient();
            var mvc = new List<KeyValuePair<string, string>>();
            mvc.Add(new KeyValuePair<string, string>("BagMiss_ID", t_BagMiss_ID.ToString()));
            mvc.Add(new KeyValuePair<string, string>("FileLoc_ID", t_fileloc));
            mvc.Add(new KeyValuePair<string, string>("Status", t_status));
            ClientBagHS.DefaultRequestHeaders.Add("Authorization", t_token);
            var reqBagHS = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(mvc) };

            string ContentBagHS;
            HttpResponseMessage resBagHS;
            resBagHS = ClientBagHS.SendAsync(reqBagHS).Result;
            ContentBagHS = resBagHS.Content.ReadAsStringAsync().Result;
            return ContentBagHS;

        }

        public static string MisBagProfileFinish(int t_BagMiss_ID,string t_token)
        {
            Url account = new Url();
            string uri = account.Get("MisBagProfileFinish");

            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("BagMiss_ID", t_BagMiss_ID.ToString()));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
        }

        public static void UploadSFTPFile(string host, string username, string password, string sourcefile, string destinationpath, int port)
        {
            try
            {
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    client.ChangeDirectory(destinationpath);
                    using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, Path.GetFileName(sourcefile));
                    }
                }
            }
            catch (Exception ex)
            {
                SaveLog.WriteLog(ex.Message);
            }
        }

        public static string KiemTraDate(string t_date)
        {
            DateTime kq = new DateTime();
            bool kiemtra = DateTime.TryParseExact(t_date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out kq);
            if (kiemtra == true)
                return t_date;
            else
                return "ERROR";
        }

        public static void Empty(string directory)
        {
            foreach (string fileToDelete in System.IO.Directory.GetFiles(directory))
            {
                
                System.IO.File.Delete(fileToDelete);
            }
            foreach (string subDirectoryToDeleteToDelete in System.IO.Directory.GetDirectories(directory))
            {
                System.IO.Directory.Delete(subDirectoryToDeleteToDelete, true);
            }
        }

        public static void DeleteAllFromFolder(string directory)
        {
         
            DirectoryInfo d = new DirectoryInfo(directory);
            foreach (FileInfo f in d.GetFiles())
            {
               
                if (f.CreationTime.AddHours(1) < DateTime.Now)
                    File.Delete(f.FullName);
            }
        }

        public static void DeleteFileAfter3Months(string directory)
        {
            DirectoryInfo source = new DirectoryInfo(directory);

            // Get info of each file into the directory
            foreach (FileInfo fi in source.GetFiles())
            {
                var creationTime = fi.CreationTime;

                if (creationTime < (DateTime.Now - new TimeSpan(90, 0, 0, 0)))
                {
                    fi.Delete();
                }
            }
        }

        private static async Task DelayAsync()
        {
            await Task.Delay(1000);
        }

       
    }
}
