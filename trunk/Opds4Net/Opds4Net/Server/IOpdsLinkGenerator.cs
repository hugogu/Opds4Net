using System.ServiceModel.Syndication;
using Opds4Net.Model;

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
        /// <param name="title"></param>
        /// <returns></returns>
        OpdsLink GetNavigationLink(string id, string title);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        OpdsLink GetDetailLink(string id, string title);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        OpdsLink GetDownloadLink(string id, string title);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        OpdsLink GetBuyLink(string id, string title, decimal price);
    }
}
