using System.Diagnostics;
using System.IO;
using Opds4Net.Server;

namespace Opds4Net.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileSystemHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static object GetFileOpdsInfo(string path, string id)
        {
            var fileInfo = new FileInfo(path);
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(path);

            return new
            {
                Id = id,
                Title = fileInfo.Name,
                Summary = fileVersionInfo.FileDescription,
                UpdateTime = fileInfo.LastWriteTime,
                OpdsDataType = OpdsDataType.Detial,
                Length = fileInfo.Length,
                CreateTime = fileInfo.CreationTime,
                Language = fileVersionInfo.Language,
                CopyRight = fileVersionInfo.LegalCopyright,
                Publisher = fileVersionInfo.CompanyName,
                AuthorName = fileVersionInfo.CompanyName,
                MediaType = OpdsHelper.DetectFileMimeType(path)
            };
        }
    }
}
