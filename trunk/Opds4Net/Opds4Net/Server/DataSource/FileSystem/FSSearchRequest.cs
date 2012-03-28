using System;

namespace Opds4Net.Server.FileSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class FSSearchRequest : FSDataItemsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolder"></param>
        public FSSearchRequest(string rootFolder)
            : base(rootFolder)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DataResponse Process()
        {
            return base.Process();
        }
    }
}
