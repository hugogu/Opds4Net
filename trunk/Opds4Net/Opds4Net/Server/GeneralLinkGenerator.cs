﻿using System;
using System.ComponentModel.Composition;
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
        public string BuyLinkPattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationPattern"></param>
        /// <param name="detailPattern"></param>
        /// <param name="downloadPattern"></param>
        /// <param name="buyPattern"></param>
        [ImportingConstructor]
        public GeneralLinkGenerator(
            [Import("NavigationLinkPattern")]string navigationPattern,
            [Import("DetailLinkPattern")]string detailPattern,
            [Import("DownloadLinkPattern")]string downloadPattern,
            [Import("BuyLinkPattern")]string buyPattern)
        {
            NavigationLinkPattern = navigationPattern;
            DetailLinkPattern = detailPattern;
            DownloadLinkPattern = downloadPattern;
            BuyLinkPattern = buyPattern;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public OpdsLink GetNavigationLink(string id, string title)
        {
            if (String.IsNullOrEmpty(NavigationLinkPattern))
                throw new InvalidOperationException("NavigationLinkPattern is not set.");

            var link = new OpdsLink()
            {
                Title = title,
                MediaType = OpdsMediaType.NavigationFeed,
                RelationshipType = OpdsRelations.Alternate,
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
        public OpdsLink GetDetailLink(string id, string title)
        {
            if (String.IsNullOrEmpty(DetailLinkPattern))
                throw new InvalidOperationException("DetailLinkPattern is not set.");

            var link = new OpdsLink()
            {
                Title = title,
                MediaType = OpdsMediaType.Entry,
                RelationshipType = OpdsRelations.Alternate,
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
        public OpdsLink GetDownloadLink(string id, string title)
        {
            if (String.IsNullOrEmpty(DownloadLinkPattern))
                throw new InvalidOperationException("DownloadLinkPattern is not set.");

            var link = new OpdsLink()
            {
                Title = title,
                RelationshipType = OpdsRelations.OpenAcquisition,
                Uri = new Uri(String.Format(DownloadLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute)
            };

            return link;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public OpdsLink GetBuyLink(string id, string title, decimal price)
        {
            if (String.IsNullOrEmpty(BuyLinkPattern))
                throw new InvalidOperationException("BuyLinkPattern is not set.");

            var link = new OpdsLink()
            {
                Title = title,
                RelationshipType = OpdsRelations.Buy,
                Uri = new Uri(String.Format(BuyLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute),
            };
            link.Prices.Add(new OpdsPrice(price));

            return link;
        }
    }
}
