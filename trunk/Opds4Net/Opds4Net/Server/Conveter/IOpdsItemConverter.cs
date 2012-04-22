using System;
using Opds4Net.Reflection;

namespace Opds4Net.Server
{
    /// <summary>
    /// Converts data items to syndication items
    /// </summary>
    public interface IOpdsItemConverter
    {
        /// <summary>
        /// Gets or sets the IOpdsItemConverterComponentFactory 
        /// </summary>
        IOpdsItemConverterComponentFactory ComponentFactory { get; set; }

        /// <summary>
        /// Raise on a syndication item is generated.
        /// </summary>
        event EventHandler<ItemGeneratedEventArgs> ItemGenerated;

        /// <summary>
        /// Get opds entries according to a category id.
        /// If there are sub-categories, show them first.
        /// If the given categories id represents a leaf category. Shows the books in it.
        /// </summary>
        /// <param name="data">An instance of DataResponse represents the data you want to fetch.</param>
        /// <returns>The opds entries. If the category Id is not given, returns all root categories.</returns>
        OpdsItemsResult GetItems(OpdsDataSource data);
    }
}
