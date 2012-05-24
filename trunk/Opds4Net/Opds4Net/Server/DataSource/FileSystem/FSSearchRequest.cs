using System;
using System.Collections.Generic;
using System.IO;
using Opds4Net.Util;

namespace Opds4Net.Server.FileSystem
{
    /// <summary>
    /// 文件系统文件搜索请求
    /// </summary>
    public class FSSearchRequest : FSDataItemsRequest
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolder"></param>
        public FSSearchRequest(string rootFolder)
            : base(rootFolder)
        {
        }

        /// <summary>
        /// 处理文件搜索请求
        /// </summary>
        /// <returns></returns>
        public override OpdsData Process()
        {
            var id = Id ?? String.Empty;
            var root = Path.Combine(RootFolder, id);
            var items = new List<object>();
            var result = new OpdsData() { Data = items };

            foreach(var file in Directory.GetFiles(root, KeyWord, SearchOption.AllDirectories))
            {
                if (OpdsHelper.IsFileSupported(file))
                {
                    // Path will be used as Id.
                    var item = GetFileOpdsInfo(file, Path.Combine(id, Path.GetFileName(file)));
                    items.Add(item);
                }
            }

            return result;
        }
    }
}
