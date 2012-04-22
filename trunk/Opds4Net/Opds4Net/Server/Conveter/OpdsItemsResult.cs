using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace Opds4Net.Server
{
    /// <summary>
    /// Represents the result that IOpdsItemConverter instance produced.
    /// </summary>
    public class OpdsItemsResult
    {
        /// <summary>
        /// Default constructor of OpdsItemsResult to make sure the Result Items is empty instead of null by default.
        /// </summary>
        public OpdsItemsResult()
        {
            Items = new SyndicationItem[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public OpdsItemsResult(IEnumerable<SyndicationItem> items)
        {
            Items = items;
        }

        /// <summary>
        /// Gets or sets the generated Syndication Items.
        /// </summary>
        public IEnumerable<SyndicationItem> Items { get; set; }
    }
}
