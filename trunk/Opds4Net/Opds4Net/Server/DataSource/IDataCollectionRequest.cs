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
        /// Aamout of the entries want to get.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// How the result set ordered.
        /// </summary>
        string OrderBy { get; set; }

        /// <summary>
        /// Order direction
        /// </summary>
        bool OrderDirection { get; set; }
    }
}
