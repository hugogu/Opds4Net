namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataRequest
    {
        /// <summary>
        /// Identify the resource requested.
        /// For a category request, it represents category id; for a detail request, it represents book id.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Processes the request to produce the data used to generate syndication item later.
        /// </summary>
        /// <returns></returns>
        DataResponse Process();
    }
}
