using BOC.Areas.E_Library.Data;
using BOC.Areas.E_Library.Models;
using BOC.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;



namespace BOC.Areas.E_Library.Controllers
{
    [Area("E-Library")]//Declare Area
    public class SearchController : Controller
    {
        //
        private IHostingEnvironment Environment;
        public UriConfig UriConfig { get; }
        public SearchController(Microsoft.Extensions.Options.IOptions<UriConfig> _UriConfig,IHostingEnvironment Environment)
        {
            UriConfig = _UriConfig.Value;
            this.Environment = Environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult NoSearchResult()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Search _model)
        {

            return View();
        }
        [HttpGet]
        public IActionResult Index1()
        {
            return View();
        }
        public string _splitString(string pattern)
        {
            int n = 0;
            string _result = string.Empty;
            foreach(var _char in pattern)
            {
                if (_char.Equals("x"))
                {
                    _result = pattern.Substring(n + 1, pattern.Length - (n + 1));
                }

                n +=1;
            }
            return _result;
        }

        [HttpGet]
        public IActionResult DocumentList(string ErrorDownload, string DocProfileID)
        {
            ViewBag.Result = (ErrorDownload == "YES" ? "Unable to download" : string.Empty);
       
            ProfileDetail GetSearch_eLib_Issue_Department_List = GetSearch_eLib_Issue_Department_List_Get(ErrorDownload, DocProfileID);
            return View(GetSearch_eLib_Issue_Department_List);
        }

        ProfileDetail GetSearch_eLib_Issue_Department_List_Get(string ErrorDownload, string DocProfileID)
        {
            string _DocProfileID = ReturnValueFromString(DocProfileID, "x");
            ViewData["DocProfileID"] = _DocProfileID;
            string uri_GetSearch_eLib_Profile_Detail = UriConfig.uri_GetSearch_eLib_Profile_Detail;

            //Get List document List
            ProfileDetail GetSearch_eLib_Issue_Department_List = new DataAPI().GetObjectAPI<ProfileDetail>(uri_GetSearch_eLib_Profile_Detail,
                                      HttpContext.Session.GetString("Token"), HttpMethod.Post, false, "Data",
                                     "DocProfileID",
                                    _DocProfileID).Result;
            if (GetSearch_eLib_Issue_Department_List != null)
                if (GetSearch_eLib_Issue_Department_List.Attached_Files != null)
                    foreach (var items in GetSearch_eLib_Issue_Department_List.Attached_Files)
                    {
                        var item = items.OriginalFileName;
                    }

            //Get List Comment API
            string uri_GetSearch_eLib_Comment_Get = UriConfig.uri_GetSearch_eLib_Comment_Get;
            List<eLib_Comment_Get> Get_List_eLib_Comment_Get = new DataAPI().
            GetListOjectAPI<eLib_Comment_Get>(uri_GetSearch_eLib_Comment_Get,
            HttpContext.Session.GetString("Token"),
            HttpMethod.Post,
            false,
            "Data",
            "DocProfileID",
            _DocProfileID).Result;
            if (Get_List_eLib_Comment_Get != null)
                ViewData["List_eLib_Comment_Get"] = Get_List_eLib_Comment_Get;
            string DocCommentID = HttpContext.Request.Query["DocCommentID"].ToString();
            string Comment = HttpContext.Request.Query["Comment"].ToString();

            return GetSearch_eLib_Issue_Department_List;
        }

        [HttpGet]
        public IActionResult M_DocumentList(string ErrorDownload, string DocProfileID)
        {
            ViewBag.Result = (ErrorDownload == "YES" ? "Unable to download" : string.Empty);
            ProfileDetail GetSearch_eLib_Issue_Department_List = GetSearch_eLib_Issue_Department_List_Get(ErrorDownload, DocProfileID);
            return View(GetSearch_eLib_Issue_Department_List);
        }

        [HttpPost]
        public IActionResult DocumentList(int id, string DocProfileID, string comment, string comment_new, string[] comment_reply, string DocCommentID)
        {
            ViewBag.Result = string.Empty;

            ProfileDetail GetSearch_eLib_Issue_Department_List = GetSearch_eLib_Issue_Department_List_Post(id, DocProfileID, comment, comment_new, comment_reply, DocCommentID,"Desktop");


            return View(GetSearch_eLib_Issue_Department_List);
        }


       public ProfileDetail GetSearch_eLib_Issue_Department_List_Post(int id, string DocProfileID, string comment, string comment_new, string[] comment_reply, string DocCommentID,string _action)
        {
            string _DocProfileID = ReturnValueFromString(DocProfileID, "x");
            string _comment_reply = string.Empty;
            foreach (string item in comment_reply)
            {
                if (item != null)
                {
                    _comment_reply = item;
                }
            }
            // API DocumentList         
            string uri_GetSearch_eLib_Profile_Detail = UriConfig.uri_GetSearch_eLib_Profile_Detail;
            ProfileDetail GetSearch_eLib_Issue_Department_List = new DataAPI().GetObjectAPI<ProfileDetail>(uri_GetSearch_eLib_Profile_Detail,
                                     HttpContext.Session.GetString("Token"), HttpMethod.Post, false, "Data",
                                    "DocProfileID",
                                    _DocProfileID).Result;


            if (int.TryParse(comment, out int value))
            {

                string uri_GetSearch_eLib_Comment_Reply = UriConfig.uri_GetSearch_eLib_Comment_Reply;
                eLib_Comment_New Get_eLib_Comment_Reply = new DataAPI().GetStringAPI(uri_GetSearch_eLib_Comment_Reply,
                                                                                 HttpContext.Session.GetString("Token"),
                                                                                 HttpMethod.Post,
                                                                                 false,
                                                                                 "DocCommentID",
                                                                                  comment,
                                                                                 "Comment",
                                                                                 _comment_reply).Result;
            }

            else
            {

                if (comment == "New")
                {

                    string uri_GetSearch_eLib_Comment_New = UriConfig.uri_GetSearch_eLib_Comment_New;
                    eLib_Comment_New Get_eLib_Comment_New = new DataAPI().GetStringAPI(uri_GetSearch_eLib_Comment_New,
                                                                                     HttpContext.Session.GetString("Token"),
                                                                                     HttpMethod.Post,
                                                                                     false,
                                                                                     "DocProfileID",
                                                                                     _DocProfileID,
                                                                                     "Comment",
                                                                                     comment_new).Result;
                }
                if (comment == "QA")
                {
                    if(_action == "Mobile")
                    {
                        HttpContext.Response.Redirect("/E-Library/Search/M_QA?DocProfileID=" + new Random().Next().ToString() + "8c11dbe0884a34c2a631f25c7b872e65a2ec2bb4adcb7df9af0678ba03cec69fb669586ax" + _DocProfileID);

                    }
                    else
                    {
                        HttpContext.Response.Redirect("/E-Library/Search/QA?DocProfileID=" + new Random().Next().ToString() + "8c11dbe0884a34c2a631f25c7b872e65a2ec2bb4adcb7df9af0678ba03cec69fb669586ax" + _DocProfileID);

                    }

                }

            }

            string uri_GetSearch_eLib_Comment_Get = UriConfig.uri_GetSearch_eLib_Comment_Get;
            List<eLib_Comment_Get> Get_List_eLib_Comment_Get = new DataAPI().
            GetListOjectAPI<eLib_Comment_Get>(uri_GetSearch_eLib_Comment_Get,
            HttpContext.Session.GetString("Token"),
            HttpMethod.Post,
            false,
            "Data",
            "DocProfileID",
            _DocProfileID).Result;

            if (Get_List_eLib_Comment_Get != null)
                ViewData["List_eLib_Comment_Get"] = Get_List_eLib_Comment_Get;

            return GetSearch_eLib_Issue_Department_List;

        }

        [HttpPost]
        public IActionResult M_DocumentList(int id, string DocProfileID, string comment, string comment_new, string[] comment_reply, string DocCommentID)
        {
            ViewBag.Result = string.Empty;
            ProfileDetail GetSearch_eLib_Issue_Department_List = GetSearch_eLib_Issue_Department_List_Post(id, DocProfileID, comment, comment_new, comment_reply, DocCommentID,"Mobile");

            return View(GetSearch_eLib_Issue_Department_List);
        }

        [HttpGet]
        public IActionResult QA(string DocProfileID)
        {
            eLib_Confirm_read_Understand Get_List_eLib_Comment_Get = Get_List_eLib_Comment_Get_QA(DocProfileID,out string _DocProfileID);

            if (Get_List_eLib_Comment_Get== null)
            {

                return Redirect("/E-Library/Search/DocumentList?DocProfileID=" + new Random().Next().ToString() + "8c11dbe0884a34c2a631f25c7b872e65a2ec2bb4adcb7df9af0678ba03cec69fb669586ax" + _DocProfileID);
            }
            //Ls_QA null
            else
            {
                if (Get_List_eLib_Comment_Get.ls_QA.Count == 0)
                    return Redirect("/E-Library/Search/DocumentList?DocProfileID=" + new Random().Next().ToString() + "8c11dbe0884a34c2a631f25c7b872e65a2ec2bb4adcb7df9af0678ba03cec69fb669586ax" + _DocProfileID);
            }
          
            return View(Get_List_eLib_Comment_Get);
        }

        public eLib_Confirm_read_Understand Get_List_eLib_Comment_Get_QA(string DocProfileID,out string _DocProfileID)
        {
           
            _DocProfileID = ReturnValueFromString(DocProfileID, "x");
            HttpContext.Session.SetString("Response", "NoAnswer");
            string uri_eLib_Confirm_read_Understand = UriConfig.uri_eLib_Confirm_read_Understand;
            eLib_Confirm_read_Understand Get_List_eLib_Comment_Get = new DataAPI().GetObjectAPI<eLib_Confirm_read_Understand>
                            (uri_eLib_Confirm_read_Understand,
                            HttpContext.Session.GetString("Token"),
                            HttpMethod.Post,
                            false,
                            "Data",
                            "DocProfileID",
                            _DocProfileID).Result;
            return Get_List_eLib_Comment_Get;
        }

        [HttpGet]
        public IActionResult M_QA(string DocProfileID)
        {
            eLib_Confirm_read_Understand Get_List_eLib_Comment_Get = Get_List_eLib_Comment_Get_QA(DocProfileID, out string _DocProfileID);

            //QA Deleted dATA null
            if (Get_List_eLib_Comment_Get == null)
            {

                return Redirect("/E-Library/Search/M_DocumentList?DocProfileID=" + new Random().Next().ToString() + "8c11dbe0884a34c2a631f25c7b872e65a2ec2bb4adcb7df9af0678ba03cec69fb669586ax" + _DocProfileID);
            }
            //Ls_QA null
            else
            {
                if (Get_List_eLib_Comment_Get.ls_QA.Count == 0)
                    return Redirect("/E-Library/Search/M_DocumentList?DocProfileID=" + new Random().Next().ToString() + "8c11dbe0884a34c2a631f25c7b872e65a2ec2bb4adcb7df9af0678ba03cec69fb669586ax" + _DocProfileID);
            }

            return View(Get_List_eLib_Comment_Get);
        }

        [HttpPost]
        public IActionResult QA(string[] CheckAnswer, string DocProfileID)
        {

            eLib_Confirm_read_Understand Get_List_eLib_Comment_Get = Get_List_eLib_Comment_Get_QA_Post(CheckAnswer,
                                                                                                      DocProfileID, out
                                                                                                      eLib_Confirm_read_Understand Get_eLib_QuestionAnswer_Update);
            ViewBag.Result = Get_eLib_QuestionAnswer_Update.Data;
            foreach (var item in Get_List_eLib_Comment_Get.ls_QA)
            {
                string _response = string.Empty;

                foreach (var _item in item.Correct_Answer)
                {
                    if (_item.ToString().Contains("0"))
                    {
                        _response = _response + string.Empty;
                    }
                    else
                    {
                        _response = _response + _item;
                    }

                }
                item.Correct_Answer = _response;
            }

            return View(Get_List_eLib_Comment_Get);
        }

        eLib_Confirm_read_Understand Get_List_eLib_Comment_Get_QA_Post(string[] CheckAnswer,
                                                                     string DocProfileID,out
                                                                     eLib_Confirm_read_Understand Get_eLib_QuestionAnswer_Update)
        {
            string _CheckAnswer = string.Empty;
            string _Answer = string.Empty;
            string _TotalAnswer = string.Empty;
            string _check = string.Empty;
            string _so = string.Empty;
            string _pattern = "A_1";
            string question1 = string.Empty;

            string uri_eLib_Confirm_read_Understand = UriConfig.uri_eLib_Confirm_read_Understand;
            eLib_Confirm_read_Understand Get_List_eLib_Comment_Get = new DataAPI().GetObjectAPI<eLib_Confirm_read_Understand>
                            (uri_eLib_Confirm_read_Understand,
                            HttpContext.Session.GetString("Token"),
                            HttpMethod.Post,
                            false,
                            "Data",
                            "DocProfileID",
                            DocProfileID).Result;


            List<ls_QA> Data = new List<ls_QA>();
            string[] answerquestion = new string[Get_List_eLib_Comment_Get.ls_QA.Count];
            //question
            for (int z = 0; z < Get_List_eLib_Comment_Get.ls_QA.Count; z++)
            {
                //from user answer
                for (int n = 0; n < CheckAnswer.Length; n++)
                {
                    if (ReturnValueFromString(CheckAnswer[n], "_").Equals((z + 1).ToString()))
                    {
                        if (CheckAnswer[n].Contains("A"))
                        {
                            _CheckAnswer = _CheckAnswer + "A";
                        }
                        else if (CheckAnswer[n].Contains("B"))
                        {
                            _CheckAnswer = _CheckAnswer + "B";
                        }
                        else if (CheckAnswer[n].Contains("C"))
                        {
                            _CheckAnswer = _CheckAnswer + "C";
                        }
                        else if (CheckAnswer[n].Contains("D"))
                        {
                            _CheckAnswer = _CheckAnswer + "D";
                        }
                    }

                    _Answer = _CheckAnswer;
                }

                if (_Answer != string.Empty)
                {
                    //bring in array to result
                    for (int i = 1; i <= 4; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                if (_Answer.Contains("A"))
                                {
                                    _TotalAnswer = _TotalAnswer + "A";
                                }
                                else
                                {
                                    _TotalAnswer = _TotalAnswer + "0";

                                }
                                break;


                            case 2:
                                if (_Answer.Contains("B"))
                                {
                                    _TotalAnswer = _TotalAnswer + "B";
                                }
                                else
                                {
                                    _TotalAnswer = _TotalAnswer + "0";

                                }
                                break;


                            case 3:
                                if (_Answer.Contains("C"))
                                {
                                    _TotalAnswer = _TotalAnswer + "C";
                                }
                                else
                                {
                                    _TotalAnswer = _TotalAnswer + "0";

                                }
                                break;


                            case 4:
                                if (_Answer.Contains("D"))
                                {
                                    _TotalAnswer = _TotalAnswer + "D";
                                }
                                else
                                {
                                    _TotalAnswer = _TotalAnswer + "0";

                                }
                                break;
                        }

                    }

                    answerquestion[z] = _TotalAnswer;
                    _CheckAnswer = string.Empty;
                    _TotalAnswer = string.Empty;
                }
                else
                {
                    answerquestion[z] = "0000";

                }

                Data.Add(new ls_QA
                {
                    DocProfileID = DocProfileID,
                    QADetailID = Get_List_eLib_Comment_Get.ls_QA[z].QADetailID,
                    UserID = Get_List_eLib_Comment_Get.ls_QA[z].UserID,
                    User_Answer = answerquestion[z],
                    Question_html = Get_List_eLib_Comment_Get.ls_QA[z].Question_html,
                    A_Answer = Get_List_eLib_Comment_Get.ls_QA[z].A_Answer,
                    B_Answer = Get_List_eLib_Comment_Get.ls_QA[z].B_Answer,
                    C_Answer = Get_List_eLib_Comment_Get.ls_QA[z].C_Answer,
                    D_Answer = Get_List_eLib_Comment_Get.ls_QA[z].D_Answer

                });

            }

            HttpContext.Session.SetString("Response", "Answer");
            ViewData["Response"] = Data;

            var _JsonObjectAnswer = JsonConvert.SerializeObject(Data);

            string uri_eLib_QuestionAnswer_Update = UriConfig.uri_eLib_QuestionAnswer_Update;
            Get_eLib_QuestionAnswer_Update = new DataAPI()._GetObjecReturn<eLib_Confirm_read_Understand>
                           (uri_eLib_QuestionAnswer_Update,
                           HttpContext.Session.GetString("Token"),
                           "Data",
                           _JsonObjectAnswer).Result;
            return Get_List_eLib_Comment_Get;

        }


        [HttpPost]
        public IActionResult M_QA(string[] CheckAnswer, string DocProfileID)
        {

            eLib_Confirm_read_Understand Get_List_eLib_Comment_Get = Get_List_eLib_Comment_Get_QA_Post(CheckAnswer,
                                                                                          DocProfileID, out
                                                                                          eLib_Confirm_read_Understand Get_eLib_QuestionAnswer_Update);


            ViewBag.Result = Get_eLib_QuestionAnswer_Update.Data;

            foreach (var item in Get_List_eLib_Comment_Get.ls_QA)
            {
                string _response = string.Empty;

                foreach (var _item in item.Correct_Answer)
                {
                    if (_item.ToString().Contains("0"))
                    {
                        _response = _response + string.Empty;
                    }
                    else
                    {
                        _response = _response + _item;
                    }

                }
                item.Correct_Answer = _response;
            }

            return View(Get_List_eLib_Comment_Get);
        }
        public string ReturnValueFromString(string _pattern, string _char)
        {
            string result = string.Empty;
            if(_pattern!=null)
            for (int i = 0; i < _pattern.Length; i++)
            {
                if (_pattern[i].ToString() == _char)
                {
                    result = _pattern.Substring(i + 1, _pattern.Length - (i + 1)).ToString();
                }
            }
            return result;  
        }
        [HttpGet]
        public IActionResult SearchResult(int id, Search model)
        {
           
            //More
            string uri_GetSearch_eLib_Search = UriConfig.uri_GetSearch_eLib_Search;
            List<SearchResult> GetSearch_eLib_Division_List = new DataAPI().GetListOjectAPI<SearchResult>(uri_GetSearch_eLib_Search,
                                                            HttpContext.Session.GetString("Token"),
                                                            HttpMethod.Post,false,"Data",
                                                            "UnRead",
                                                            (model.CheckRead == true ? "YES" : "NO"),
                                                            "Newest",
                                                            (model.CheckNews == true ? "YES" : "NO"),
                                                            "KeySearch",
                                                             model.KeySearch,
                                                             "Author",
                                                              model.Author,
                                                              "ISBN",
                                                              model.isdn,
                                                              "DocDivID",
                                                               model.DocDivID,
                                                              "PublishID",
                                                               model.PublishID,
                                                              "FromDate",
                                                              model.ReceivedDate,
                                                              "ToDate",
                                                               model.PublishDate).Result;


            int _count = int.Parse((GetSearch_eLib_Division_List).Count.ToString());
            int i = 0;
            foreach (var item in GetSearch_eLib_Division_List)
            {
                i += 1;
                item.ID = i;
            }
            HttpContext.Session.SetListData("SearchResult", GetSearch_eLib_Division_List);
            if (GetSearch_eLib_Division_List != null)
                return View();
            return RedirectToAction("NoSearchResult",
                                     "Search",
                                     new
                                     {
                                         area = "E-Library"
                                     });

        }
        [HttpPost]
        public IActionResult SearchResult(Search model)
        {
            //More
            string uri_GetSearch_eLib_Search = UriConfig.uri_GetSearch_eLib_Search;
            List<SearchResult> GetSearch_eLib_Division_List = new DataAPI().GetListOjectAPI<SearchResult>(uri_GetSearch_eLib_Search,
                                                            HttpContext.Session.GetString("Token"),
                                                            HttpMethod.Post,
                                                            false,
                                                            "Data",
                                                            "UnRead",
                                                            (model.CheckRead == true ? "YES" : "NO"),
                                                            "Newest",
                                                            (model.CheckNews == true ? "YES" : "NO"),
                                                            "KeySearch",
                                                            (model.KeySearch==null?string.Empty: model.KeySearch),
                                                            "Author",
                                                            (model.Author==null?string.Empty:model.Author),
                                                            "ISBN",
                                                            (model.isdn==null?string.Empty:model.isdn),
                                                            "DocDivID",
                                                            (model.DocDivID==null?"0":model.DocDivID),
                                                           "PublishID",
                                                            (model.PublishID==null?"0":model.PublishID),
                                                           "FromDate",
                                                           (model.ReceivedDate==null?string.Empty:model.ReceivedDate),
                                                           "ToDate",
                                                           (model.PublishDate==null?string.Empty:model.PublishDate)).Result;



            if (GetSearch_eLib_Division_List != null)
            {
                int i = 0;
                foreach (var item in GetSearch_eLib_Division_List)
                {
                    i += 1;
                    item.ID = i;
                }
                HttpContext.Session.SetListData("SearchResult", GetSearch_eLib_Division_List);

                return View();

            }
            else
            {
                return RedirectToAction("NoSearchResult",
                                         "Search",
                                         new
                                         {
                                             area = "E-Library"
                                         });
            }

        }

        [HttpPost]
        public IActionResult M_SearchResult(Search model,
                                            string M_Page,
                                            string currentpage,
                                            string next,
                                            string previous, string _pagesize
                                            )
        {

            //default
            ViewBag.currentpage = 1;
            //default or clicked
            if (string.IsNullOrEmpty(_pagesize))
            {
                ViewBag.pagesize = 10;
            }
            else
            {
                ViewBag.pagesize = int.Parse(_pagesize);
            }
            //More
            if (M_Page != null)
            {
                string uri_GetSearch_eLib_Search = UriConfig.uri_GetSearch_eLib_Search;
                List<SearchResult> GetSearch_eLib_Division_List = new DataAPI().GetListOjectAPI<SearchResult>(uri_GetSearch_eLib_Search,
                                                                HttpContext.Session.GetString("Token"),
                                                                HttpMethod.Post,
                                                                false,
                                                                "Data",
                                                                "UnRead",
                                                                (model.CheckRead == true ? "YES" : "NO"),
                                                                "Newest",
                                                                (model.CheckNews == true ? "YES" : "NO"),
                                                                "KeySearch",
                                                                (model.KeySearch == null ? string.Empty : model.KeySearch),
                                                                "Author",
                                                                (model.Author == null ? string.Empty : model.Author),
                                                                "ISBN",
                                                                (model.isdn == null ? string.Empty : model.isdn),
                                                                "DocDivID",
                                                                (model.DocDivID == null ? "0" : model.DocDivID),
                                                               "PublishID",
                                                                (model.PublishID == null ? "0" : model.PublishID),
                                                               "FromDate",
                                                               (model.ReceivedDate == null ? string.Empty : model.ReceivedDate),
                                                               "ToDate",
                                                               (model.PublishDate == null ? string.Empty : model.PublishDate)).Result;



                if (GetSearch_eLib_Division_List != null)
                {

                    float totalNumsize = (GetSearch_eLib_Division_List.Count / (float)(_pagesize == null ? 10 : int.Parse(_pagesize)));
                    int numSize = (int)Math.Ceiling(totalNumsize);

                    ViewBag.numSize = numSize;
                    if (next == "next")
                    {
                        if (int.Parse(currentpage) < numSize)
                        {
                            ViewBag.currentpage = int.Parse(currentpage) + 1;

                        }
                        else
                        {
                            ViewBag.currentpage = ViewBag.numSize;
                        }
                    }
                    if (previous == "previous")
                    {
                        if (int.Parse(currentpage) > 0)
                        {
                            ViewBag.currentpage = int.Parse(currentpage) - 1;

                        }
                        else
                        {
                            ViewBag.currentpage = ViewBag.numSize;
                        }
                    }

                    ViewBag.Total = GetSearch_eLib_Division_List.Count;
                    int i = 0;
                    foreach (var item in GetSearch_eLib_Division_List)
                    {
                        i += 1;
                        item.ID = i;
                    }
                    HttpContext.Session.SetListData("SearchResult", GetSearch_eLib_Division_List);
                    List<SearchResult> Get_Pagination_M_SearchResult = _Pagination_M_SearchResult((int)ViewBag.currentpage, ViewBag.pagesize,
                                                                                GetSearch_eLib_Division_List);


                    return View(Get_Pagination_M_SearchResult);

                }
                else
                {
                    return RedirectToAction("M_NoSearchResult",
                                             "Search",
                                             new
                                             {
                                                 area = "E-Library"
                                             });
                }
            }
            else
            {
                List<SearchResult> lst_SearchResult = HttpContext.Session.GetListData<List<SearchResult>>("SearchResult");
                int i = 0;
                foreach (var item in lst_SearchResult)
                {
                    i += 1;
                    item.ID = i;
                }
                float totalNumsize = (lst_SearchResult.Count / (float)(_pagesize == null ? 10 : int.Parse(_pagesize)));
                int numSize = (int)Math.Ceiling(totalNumsize);
                ViewBag.numSize = numSize;
                if (next == "next")
                {
                    if (int.Parse(currentpage) < numSize)
                    {
                        ViewBag.currentpage = int.Parse(currentpage) + 1;

                    }
                    else
                    {
                        ViewBag.currentpage = ViewBag.numSize;
                    }
                }
                if (previous == "previous")
                {
                    //KHI THAY ĐỔI GIÁ TRỊ PAGE 10 20 30
                    if (int.Parse(currentpage) > 0 && int.Parse(currentpage) <= numSize)
                    {
                        ViewBag.currentpage = int.Parse(currentpage) - 1;

                    }
                    else
                    {
                        ViewBag.currentpage = ViewBag.numSize;
                    }
                }
                ViewBag.Total = lst_SearchResult.Count;
                //pagination
                List<SearchResult> Get_Pagination_M_SearchResult = _Pagination_M_SearchResult((int)ViewBag.currentpage, ViewBag.pagesize,
                                                                            lst_SearchResult);

                return View(Get_Pagination_M_SearchResult);
            }

        }

        public List<SearchResult> _Pagination_M_SearchResult(int currentPage,
                                                             int pagesize,
                                                         List<SearchResult> lst)
        {
    
        List<SearchResult> _lstSearchResult = new List<SearchResult>();
         _lstSearchResult =          (from item in lst
                                      select item)
                                     .OrderBy(i => i.ID)
                                     .Skip((currentPage - 1) * pagesize)
                                     .Take(pagesize).ToList();
        return _lstSearchResult;
        }

        [HttpGet]
        public IActionResult GetSearchFolder(string ID)
        {
            List<SearchResult> GetSearch_eLib_Of_Folder = GetSearchFolder_Get(ID);
            if (GetSearch_eLib_Of_Folder != null)
            {
                int i = 0;
                foreach (var item in GetSearch_eLib_Of_Folder)
                {
                    i += 1;
                    item.ID = i;
                }
                HttpContext.Session.SetListData("SearchOfFolder", GetSearch_eLib_Of_Folder);
                return View(GetSearch_eLib_Of_Folder);
            }
            else
            {
                return Redirect("/E-Library/Folder/Index");
            }
        }

      public  List<SearchResult> GetSearchFolder_Get(string ID)
        {
            string _ID = ReturnValueFromString(ID, "x");
            string uri_GetSearch_eLib_Of_Folder = UriConfig.uri_GetSearch_eLib_Of_Folder;

            List<SearchResult> GetSearch_eLib_Of_Folder = new DataAPI().GetListOjectAPI<SearchResult>(UriConfig.uri_GetSearch_eLib_Of_Folder,
                                                            HttpContext.Session.GetString("Token"),
                                                            HttpMethod.Post,
                                                            false,
                                                            "Data",
                                                            "FolderID",
                                                            _ID).Result;
            return GetSearch_eLib_Of_Folder;
        }

        [HttpGet]
        public IActionResult M_GetSearchFolder(string ID)
        {
            List<SearchResult> GetSearch_eLib_Of_Folder = GetSearchFolder_Get(ID);
            if (GetSearch_eLib_Of_Folder != null)
            {
                int i = 0;
                foreach (var item in GetSearch_eLib_Of_Folder)
                {
                    i += 1;
                    item.ID = i;
                }
                HttpContext.Session.SetListData("SearchOfFolder", GetSearch_eLib_Of_Folder);
                return View(GetSearch_eLib_Of_Folder);
            }
            else
            {
                return Redirect("/E-Library/Folder/M_Index");
            }
        }

        [HttpGet]
        public object Get_eLib_Search(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(@HttpContext.Session.GetListData<List<SearchResult>>("SearchResult"), loadOptions);
        }

        [HttpGet]
        public object Get_eLib_Of_Folderh(DataSourceLoadOptions loadOptions)
        {
            
            return DataSourceLoader.Load(@HttpContext.Session.GetListData<List<SearchResult>>("SearchOfFolder"), loadOptions);

        }
       
        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            string uri_GetSearch_eLib_Division_List = UriConfig.uri_GetSearch_eLib_Division_List;
            IEnumerable<Search> GetSearch_GetSearch_eLib_Division_List = new DataAPI().GetListAPIWithoutParams<Search>(uri_GetSearch_eLib_Division_List,
                                                                      HttpContext.Session.GetString("Token"),HttpMethod.Post,false,"Data").Result;
            return DataSourceLoader.Load(GetSearch_GetSearch_eLib_Division_List, loadOptions);
        }

        [HttpGet]
        public object GetIssue(DataSourceLoadOptions loadOptions)
        {
            string uri_GetSearch_eLib_Issue_Department_List = UriConfig.uri_GetSearch_eLib_Issue_Department_List;
            IEnumerable<Search> GetSearch_eLib_Issue_Department_List = new DataAPI().GetListAPIWithoutParams<Search>(uri_GetSearch_eLib_Issue_Department_List,
                                                                     HttpContext.Session.GetString("Token"),
                                                                     HttpMethod.Post,
                                                                     false,
                                                                     "Data").Result;
            return DataSourceLoader.Load(GetSearch_eLib_Issue_Department_List, loadOptions);
        }

        [HttpGet]
        public IActionResult DownloadFile([FromQuery] string DocProfileID, string ServerID,string Folder, string FileName)
        {
            string _applicationType = Combine_DownloadFile(DocProfileID,
                                                  ServerID,
                                                  Folder,
                                                  FileName, out
                                                  string contentPath,
                                                  out string _DocProfileID);
          
            if (_applicationType!=String.Empty)
            return File(System.IO.File.ReadAllBytes(string.Concat(contentPath, FileName)),
                       _applicationType);
            return Redirect("/E-Library/Search/DocumentList?ErrorDownload=YES&DocProfileID=" + new Random().Next().ToString() + "8c11dbe0884a34c2a631f25c7b872e65a2ec2bb4adcb7df9af0678ba03cec69fb669586ax" + _DocProfileID);
        }


        string Combine_DownloadFile([FromQuery] string DocProfileID,
                                                string ServerID,
                                                string Folder,
                                                string FileName,out
                                                string contentPath,out string _DocProfileID)
        {
            _DocProfileID = ReturnValueFromString(DocProfileID, "=");

            string wwpath = this.Environment.WebRootPath;
            contentPath = string.Concat(wwpath, @"\css\E-Library\Data\");

            new DataAPI().DownloadFile(new SftpConfig
            {
                Host = ServerID,
                Port = 2211,
                UserName = "sftpuser",
                Password = "rR#ky37CDHaSkRL"
            }, string.Concat(Folder, FileName),
            string.Concat(contentPath, FileName));

            string _applicationType = string.Empty;

            foreach (var item in ListContentType._lstContentType())
            {
                if (FileName.Contains(item.Name.ToString()))
                    _applicationType = item.ContentTypeName;
            }
            return _applicationType;
        }



        [HttpGet]
        public IActionResult M_DownloadFile([FromQuery] string DocProfileID, string ServerID, string Folder, string FileName)
        {
            string _applicationType = Combine_DownloadFile(DocProfileID,
                                                   ServerID,
                                                   Folder,
                                                   FileName, out
                                                   string contentPath,
                                                   out string _DocProfileID);

            if (_applicationType != String.Empty)
                return File(System.IO.File.ReadAllBytes(string.Concat(contentPath, FileName)),
                           _applicationType);
            return Redirect("/E-Library/Search/M_DocumentList?ErrorDownload=YES&DocProfileID=" + new Random().Next().ToString() + "8c11dbe0884a34c2a631f25c7b872e65a2ec2bb4adcb7df9af0678ba03cec69fb669586ax" + _DocProfileID);
        }
        public IActionResult M_Index()
        {
            return View();
        }
        public IActionResult M_NoSearchResult()
        {
            return View();
        }
  
    }
}
