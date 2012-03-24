using Opds4Net.Reflection;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpdsItemConverter
    {
        /// <summary>
        /// 
        /// </summary>
        IOpdsLinkGenerator LinkGenerator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IDataTypeDetector TypeDetector { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IAdapterFactory AdapterFactory { get; set; }

        /// <summary>
        /// Get opds entries according to a category id.
        /// If there are sub-categories, show them first.
        /// If the given categories id represents a leaf category. Shows the books in it.
        /// </summary>
        /// <param name="data">An instance of DataResponse represents the data you want to fetch.</param>
        /// <returns>The opds entries. If the category Id is not given, returns all root categories.</returns>
        OpdsItemsResult GetItems(NamingDataSource data);
    }
}
