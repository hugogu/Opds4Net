using System;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemDetailRequest : IDataRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataResponse Process()
        {
            throw new NotImplementedException();
        }
    }
}
