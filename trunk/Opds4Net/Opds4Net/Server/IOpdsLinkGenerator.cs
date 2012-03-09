using System.ServiceModel.Syndication;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpdsLinkGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SyndicationLink GetNavigationLink(string id, string title);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SyndicationLink GetDetailLink(string id, string title);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        SyndicationLink GetDownloadLink(string id, string title);
    }
}
