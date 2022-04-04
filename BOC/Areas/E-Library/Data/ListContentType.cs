using BOC.Areas.E_Library.Models;
using System.Collections.Generic;

namespace BOC.Areas.E_Library.Data
{
    public class ListContentType
    {
       
        public static List<ContentType> _lstContentType()
        {

            List<ContentType> _lstContentType = new List<ContentType>();
            _lstContentType.Add(new ContentType { Name = "pdf", ContentTypeName = "application/pdf" });
            _lstContentType.Add(new ContentType { Name = "txt", ContentTypeName = "text/html" });
            _lstContentType.Add(new ContentType { Name = "xlsx", ContentTypeName = "application/vnd.ms-excel" });
            _lstContentType.Add(new ContentType { Name = "png", ContentTypeName = "image/png" });
            _lstContentType.Add(new ContentType { Name = "jpeg", ContentTypeName = "image/jpeg" });


            _lstContentType.Add(new ContentType { Name = "mpeg", ContentTypeName = "audio/mpeg" });
            _lstContentType.Add(new ContentType { Name = "mp4", ContentTypeName = "video/mp4" });


            _lstContentType.Add(new ContentType { Name = "mpeg", ContentTypeName = "video/mpeg" });
            _lstContentType.Add(new ContentType { Name = "wma", ContentTypeName = "x-ms-wma" });
            _lstContentType.Add(new ContentType { Name = "realaudio", ContentTypeName = "vnd.rn-realaudio" });
            _lstContentType.Add(new ContentType { Name = "wav", ContentTypeName = "x-wav" });
            _lstContentType.Add(new ContentType { Name = "gif", ContentTypeName = "image/gif" });
            _lstContentType.Add(new ContentType { Name = "gif", ContentTypeName = "image/tiff" });
            _lstContentType.Add(new ContentType { Name = "gif", ContentTypeName = "image/x-icon" });


            _lstContentType.Add(new ContentType { Name = "doc", ContentTypeName = "application/msword" });
            _lstContentType.Add(new ContentType { Name = "ppt", ContentTypeName = "application/vnd.ms-powerpoint" });
            _lstContentType.Add(new ContentType { Name = "gif", ContentTypeName = "image/x-icon" });


            return _lstContentType;
        }

    }
}
