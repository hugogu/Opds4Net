namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class OpdsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public OpdsRequest()
        {
            PageSize = 10;
            PageIndex = 1;
        }

        /// <summary>
        /// Page index of the result.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Aamout of the entries want to get.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// How the result set ordered.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Order direction
        /// </summary>
        public bool OrderDirection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal protected abstract OpdsResponse Process();
    }
}
