using System.Collections.Generic;

namespace BOC.Areas.E_Library.Models
{
    public class _eLib_Confirm_read_Understand
    {
       
        public string Message { get; set; }
        public string Success { get; set; }
        public string ResultCode { get; set; }
        public string TotalRecord { get; set; }
        public List<_ls_QA> _ls_QA { get; set; }
    }
    public class _ls_QA
    {
        public string DocProfileID { get; set; }
        public string QADetailID { get; set; }
        public string UserID { get; set; }
        public string Question_html { get; set; }
        public string A_Answer { get; set; }
        public string B_Answer { get; set; }
        public string C_Answer { get; set; }
        public string D_Answer { get; set; }
        public string Correct_Answer { get; set; }
        public string User_Answer { get; set; }
       


    }

}
