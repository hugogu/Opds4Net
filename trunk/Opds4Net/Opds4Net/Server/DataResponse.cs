using System.Collections.Generic;
using Opds4Net.Model;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class DataResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IOpdsData> Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}
