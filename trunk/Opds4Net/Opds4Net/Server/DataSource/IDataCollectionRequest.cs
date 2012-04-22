namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataCollectionRequest : IDataRequest
    {
        /// <summary>
        /// Page index of the result.
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// Amount of the entries want to get.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// How the result set ordered.
        /// </summary>
        string OrderBy { get; set; }

        /// <summary>
        /// Order direction or the result data.
        /// </summary>
        bool OrderDirection { get; set; }
    }
}
