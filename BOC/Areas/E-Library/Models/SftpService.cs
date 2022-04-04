using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.IO;

namespace BOC.Areas.E_Library.Models
{
    public class SftpService
    {
        //private readonly ILogger<SftpService> _logger;
        private readonly SftpConfig _config;

        public SftpService( SftpConfig sftpConfig)
        {
            //_logger = logger;
            _config = sftpConfig;
        }
        public void DownloadFile(string remoteFilePath, string localFilePath)
        {
            using var client = new SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password);
            try
            {
                client.Connect();
                using var s = File.Create(localFilePath);
                client.DownloadFile(remoteFilePath, s);
                //_logger.LogInformation($"Finished downloading file [{localFilePath}] from [{remoteFilePath}]");
            }
            catch (Exception exception)
            {
                //_logger.LogError(exception, $"Failed in downloading file [{localFilePath}] from [{remoteFilePath}]");
            }
            finally
            {
                client.Disconnect();
            }
        }

    }
}
