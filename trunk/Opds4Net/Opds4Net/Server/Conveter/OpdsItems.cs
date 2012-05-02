using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace Opds4Net.Server
{
    /// <summary>
    /// Represents the result that IOpdsItemConverter instance produced.
    /// </summary>
    public class OpdsItems
    {
        /// <summary>
        /// Default constructor of OpdsItems to make sure the Result Items is empty instead of null by default.
        /// </summary>
        public OpdsItems()
        {
            Items = new SyndicationItem[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public OpdsItems(IEnumerable<SyndicationItem> items)
        {
            Items = items;
        }

        /// <summary>
        /// Gets or sets the generated Syndication Items.
        /// </summary>
        public virtual IEnumerable<SyndicationItem> Items { get; set; }
    }
}
