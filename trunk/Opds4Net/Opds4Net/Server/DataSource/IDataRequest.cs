namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataRequest
    {
        /// <summary>
        /// Identify the request.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DataResponse Process();
    }
}
