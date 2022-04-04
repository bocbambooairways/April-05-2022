using Renci.SshNet;
using System;
using System.IO;

namespace BOC.Areas.E_Library.Models
{
    public static class Extension_DownloadFile
    {
        public static void DownloadFile(this SftpClient _sftpClient,
                                      string remotefilepath, 
                                      string localfilePath)
        {
            try
            {
                
                FileStream _stream = File.Create(localfilePath);
                _sftpClient.DownloadFile(remotefilepath, _stream);

               
            }
            catch (Exception exception)
            {
              
            }
            finally { _sftpClient.Disconnect(); }
        }
    }
}
