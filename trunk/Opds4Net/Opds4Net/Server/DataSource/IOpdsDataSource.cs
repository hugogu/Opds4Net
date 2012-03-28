using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace Opds4Net.Server
{
    /// <summary>
    /// An abstraction of opds data source. There will be a lot ways to implement it.
    /// </summary>
    public interface IOpdsDataSource
    {
        /// <summary>
        /// Get opds entries according to a category id.
        /// If there are sub-categories, show them first.
        /// If the given categories id represents a leaf category. Shows the books in it.
        /// </summary>
        /// <param name="request">An instance of OpdsCategoryItemsRequest represents the data you want to fetch.</param>
        /// <returns>The opds entries. If the category Id is not given, returns all root categories.</returns>
        OpdsItemsResult GetItems(IDataRequest request);
    }
}
