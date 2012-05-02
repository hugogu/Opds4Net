using System;
using System.IO;
using Opds4Net.Util;

namespace Opds4Net.Server.FileSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class FSDetailRequest : IDataRequest
    {
        private string rootFolder;

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolder"></param>
        public FSDetailRequest(string rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OpdsData Process()
        {
            var id = Id ?? String.Empty;
            var path = Path.Combine(rootFolder, id);

            if (!File.Exists(path))
                throw new ArgumentException("id");

            return new OpdsData()
            {
                Data = new []
                {
                    FileSystemHelper.GetFileOpdsInfo(path, id)
                },
                TotalCount = 1
            };
        }
    }
}
