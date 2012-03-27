using System;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class DataItemsRequest : IDataRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public DataItemsRequest()
        {
            PageSize = 10;
            PageIndex = 1;
        }

        /// <summary>
        /// Category Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Page index of the result.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Aamout of the entries want to get.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// How the result set ordered.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Order direction
        /// </summary>
        public bool OrderDirection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DataResponse Process()
        {
            return new DataResponse();
        }
    }
}
