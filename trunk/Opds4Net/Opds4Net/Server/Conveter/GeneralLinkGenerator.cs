using System;
using System.ComponentModel.Composition;
using Opds4Net.Model;
using Opds4Net.Reflection;
using Opds4Net.Reflection.Extension;
using Opds4Net.Util;

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
        /// <param name="data"></param>
        /// <param name="opdsLinkRelation"></param>
        /// <param name="opdsLinkMediaType"></param>
        /// <param name="propertyAccessor"></param>
        /// <param name="nameMapping"></param>
        /// <returns></returns>
        public virtual OpdsLink Generate(object data, string opdsLinkRelation, string opdsLinkMediaType, IPropertyAccessor propertyAccessor, OpdsNames nameMapping)
        {
            if (nameMapping == null)
                nameMapping = new OpdsNames();

            switch (opdsLinkRelation)
            {
                // Download
                case OpdsRelations.OpenAcquisition:
                    {
                        var id = data.GetProperty(propertyAccessor, nameMapping.DownloadLinkId, nameMapping.Id).ToNullableString();
                        if (!String.IsNullOrEmpty(id))
                        {
                            return GetDownloadLink(id, data.GetProperty(nameMapping.Title, propertyAccessor).ToNullableString());
                        }

                        return null;
                    }
                case OpdsRelations.Buy:
                    {
                        var price = data.GetProperty(nameMapping.Price, propertyAccessor);
                        if (price != null)
                        {
                            var id = data.GetProperty(propertyAccessor, nameMapping.BuyLinkId, nameMapping.Id).ToNullableString();
                            if (!String.IsNullOrEmpty(id))
                            {
                                return GetBuyLink(id,
                                    data.GetProperty(nameMapping.Title, propertyAccessor).ToNullableString(),
                                    Convert.ToDecimal(price));
                            }
                        }

                        return null;
                    }
                case OpdsRelations.Alternate:
                    {
                        if (OpdsMediaType.AcquisitionFeed == opdsLinkMediaType ||
                            OpdsMediaType.NavigationFeed == opdsLinkMediaType)
                        {
                            var id = data.GetProperty(nameMapping.Id, propertyAccessor).ToNullableString();
                            if (!String.IsNullOrEmpty(id))
                            {
                                return GetNavigationLink(id, data.GetProperty(nameMapping.Title, propertyAccessor).ToNullableString());
                            }
                        }
                        else if (OpdsMediaType.Entry == opdsLinkMediaType)
                        {
                            var id = data.GetProperty(propertyAccessor, nameMapping.DetailLinkId, nameMapping.Id).ToNullableString();
                            if (!String.IsNullOrEmpty(id))
                            {
                                // 详细页链接的Id和书籍的Id可能并没有对应关系。仅当没有提供详细页Id时，使用书籍的Id。
                                return GetDetailLink(id, data.GetProperty(nameMapping.Title, propertyAccessor).ToNullableString());
                            }
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }

                        return null;
                    }
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private OpdsLink GetNavigationLink(string id, string title)
        {
            if (String.IsNullOrEmpty(NavigationLinkPattern))
                throw new InvalidOperationException("NavigationLinkPattern is not set.");

            var link = new OpdsLink()
            {
                Title = title,
                // We don't know the media type of Uri point to.
                // It could either be a acquisition link or a navigation link.
                MediaType = OpdsMediaType.Feed,
                RelationshipType = OpdsRelations.Alternate,
                Uri = new Uri(String.Format(WebRequestHelper.CurrentHostUri + NavigationLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute)
            };
            OnNavigationLinkGenerated(link);

            return link;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private OpdsLink GetDetailLink(string id, string title)
        {
            if (String.IsNullOrEmpty(DetailLinkPattern))
                throw new InvalidOperationException("DetailLinkPattern is not set.");

            var link = new OpdsLink()
            {
                Title = title,
                MediaType = OpdsMediaType.Entry,
                RelationshipType = OpdsRelations.Alternate,
                Uri = new Uri(String.Format(WebRequestHelper.CurrentHostUri + DetailLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute)
            };
            OnDetailLinkGenerated(link);

            return link;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private OpdsLink GetDownloadLink(string id, string title)
        {
            if (String.IsNullOrEmpty(DownloadLinkPattern))
                return null;

            var link = new OpdsLink()
            {
                Title = title,
                RelationshipType = OpdsRelations.OpenAcquisition,
                Uri = new Uri(String.Format(WebRequestHelper.CurrentHostUri + DownloadLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute)
            };
            OnDownloadLinkGenerated(link);

            return link;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private OpdsLink GetBuyLink(string id, string title, decimal price)
        {
            if (String.IsNullOrEmpty(BuyLinkPattern))
                return null;

            var link = new OpdsLink()
            {
                Title = title,
                RelationshipType = OpdsRelations.Buy,
                Uri = new Uri(String.Format(WebRequestHelper.CurrentHostUri + BuyLinkPattern, Uri.EscapeDataString(id)), UriKind.RelativeOrAbsolute),
            };
            link.Prices.Add(new OpdsPrice(price));
            OnBuyLinkGenerated(link);

            return link;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        protected virtual void OnNavigationLinkGenerated(OpdsLink link)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        protected virtual void OnDetailLinkGenerated(OpdsLink link)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        protected virtual void OnBuyLinkGenerated(OpdsLink link)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        protected virtual void OnDownloadLinkGenerated(OpdsLink link)
        {
        }
    }
}
