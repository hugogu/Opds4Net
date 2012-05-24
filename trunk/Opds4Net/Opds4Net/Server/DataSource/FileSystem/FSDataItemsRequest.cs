using System.Diagnostics;
using System.IO;
using Opds4Net.Util;

namespace Opds4Net.Server.FileSystem
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class FSDataItemsRequest : DataItemsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolder"></param>
        public FSDataItemsRequest(string rootFolder)
        {
            RootFolder = rootFolder;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual object GetFileOpdsInfo(string path, string id)
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
