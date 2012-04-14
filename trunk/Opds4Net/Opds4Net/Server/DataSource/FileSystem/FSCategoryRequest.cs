using System;
using System.Collections.Generic;
using System.IO;
using Opds4Net.Util;

namespace Opds4Net.Server.FileSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class FSCategoryRequest : FSDataItemsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolder"></param>
        public FSCategoryRequest(string rootFolder)
            : base(rootFolder)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DataResponse Process()
        {
            var id = Id ?? String.Empty;
            var root = Path.Combine(RootFolder, id);
            var items = new List<object>();
            var result = new DataResponse() { Data = items };

            if (!Directory.Exists(root))
                throw new ArgumentException("id");

            foreach (var path in Directory.GetFiles(root))
            {
                if (OpdsHelper.IsFileSupported(path))
                {
                    // Path will be used as Id.
                    var item = FileSystemHelper.GetFileOpdsInfo(path, Path.Combine(id, Path.GetFileName(path)));
                    items.Add(item);
                }
            }

            foreach (var path in Directory.GetDirectories(root))
            {
                var directoryInfo = new DirectoryInfo(path);

                items.Add(new
                {
                    Id = Path.Combine(id, Path.GetFileName(path)),
                    Title = directoryInfo.Name,
                    UpdateTime = directoryInfo.LastWriteTime,
                    OpdsDataType = OpdsDataType.Category,
                });
            }

            return result;
        }
    }
}
