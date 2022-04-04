using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Models
{
	public class MenuModel
	{
		public string ResultCode { get; set; }
		public string Message { get; set; }
		public int TotalRecord { get; set; }
		public List<Info> Data { get; set; }
	
	}

	public class Info
	{
		public string id { get; set; }
		public string text { get; set; }
		public bool expanded { get; set; }
		public string image { get; set; }
		public int FunctionID { get; set; }
		public List<MenuInfo> items { get; set; }
		public string URL { get; set; }
		public string FTP_ServerIP { get; set; }
		public string FTP_Folder { get; set; }
		public string FTP_FileName { get; set; }
	}

	public class MenuInfo
	{
		public int FunctionID { get; set; }
		public string id { get; set; }
		public string text { get; set; }
		public bool expanded { get; set; }
		public string image { get; set; }
		public string URL { get; set; }
		public string FTP_ServerIP { get; set; }
		public string FTP_Folder { get; set; }
		public string FTP_FileName { get; set; }

	}

}

