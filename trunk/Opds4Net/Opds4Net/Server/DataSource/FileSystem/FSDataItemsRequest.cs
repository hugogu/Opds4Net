namespace Opds4Net.Server.FileSystem
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class FSDataItemsRequest : DataItemsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolder"></param>
        public FSDataItemsRequest(string rootFolder)
        {
            RootFolder = rootFolder;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RootFolder { get; set; }
    }
}
