using System;
using System.IO;

namespace Opds4Net.Server.FileSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class FSDetailRequest : FSDataItemsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolder"></param>
        public FSDetailRequest(string rootFolder)
            : base(rootFolder)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override OpdsData Process()
        {
            var id = Id ?? String.Empty;
            var path = Path.Combine(RootFolder, id);

            if (!File.Exists(path))
                throw new ArgumentException("id");

            return new OpdsData()
            {
                Data = new []
                {
                    GetFileOpdsInfo(path, id)
                },
                TotalCount = 1
            };
        }
    }
}
