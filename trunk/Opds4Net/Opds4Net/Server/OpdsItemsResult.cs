using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsItemsResult
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SyndicationItem> Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}
