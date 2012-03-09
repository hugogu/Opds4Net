using System;
using System.ComponentModel.Composition;
using System.ServiceModel.Syndication;
using Opds4Net.Model;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IOpdsLinkGenerator))]
    public class GeneralLinkGenerator : IOpdsLinkGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        public string NavigationLinkPattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DetailLinkPattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DownloadLinkPattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationPattern"></param>
        /// <param name="detailPattern"></param>
        /// <param name="downloadPattern"></param>
        [ImportingConstructor]
        public GeneralLinkGenerator(
            [Import("NavigationLinkPattern")]string navigationPattern,
            [Import("DetailLinkPattern")]string detailPattern,
            [Import("DownloadLinkPattern")]string downloadPattern)
        {
            NavigationLinkPattern = navigationPattern;
            DetailLinkPattern = detailPattern;
            DownloadLinkPattern = downloadPattern;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public SyndicationLink GetNavigationLink(string id, string title)
        {
            var link = new OpdsLink()
            {
                Title = title,
                MediaType = OpdsMediaType.NavigationFeed,
                Uri = new Uri(String.Format(NavigationLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute)
            };

            return link;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public SyndicationLink GetDetailLink(string id, string title)
        {
            var link = new OpdsLink()
            {
                Title = title,
                MediaType = OpdsMediaType.Entry,
                Uri = new Uri(String.Format(DetailLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute)
            };

            return link;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public SyndicationLink GetDownloadLink(string id, string title)
        {
            var link = new OpdsLink()
            {
                Title = title,
                Uri = new Uri(String.Format(DownloadLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute)
            };

            return link;
        }
    }
}
