using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Opds4Net.Util;

namespace Opds4Net.Model
{
    /// <summary>
    /// Represents an opds feed.
    /// </summary>
    public class OpdsFeed : SyndicationFeed
    {
        /// <summary>
        /// Loads an Opds4Net.Model.OpdsFeed-derived instance from the specified System.Xml.XmlReader.
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <returns> An Opds4Net.Model.OpdsFeed-derived instance that contains the feed.</returns>
        public new static OpdsFeed Load(XmlReader xmlReader)
        {
            return SyndicationFeed.Load<OpdsFeed>(xmlReader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public string GetLinkValue(string relation)
        {
            var searchLink = Links.SingleOrDefault(l => relation.Equals(l.RelationshipType, StringComparison.OrdinalIgnoreCase));
            if (searchLink == null || searchLink.Uri == null)
                return null;

            return searchLink.Uri.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relation"></param>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="mediaType"></param>
        public void SetLinkValue(string relation, string url, string title, string mediaType)
        {
            var searchLink = Links.SingleOrDefault(l => relation.Equals(l.RelationshipType, StringComparison.OrdinalIgnoreCase));
            if (searchLink != null)
                searchLink.Uri = new Uri(url, UriKind.RelativeOrAbsolute);
            else
                Links.Add(new SyndicationLink(new Uri(url, UriKind.RelativeOrAbsolute), relation, title, mediaType, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        public string SearchUri
        {
            get { return GetLinkValue("search"); }
            set { SetLinkValue("search", value, "搜索", OpdsMediaType.NavigationFeed); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NextUri
        {
            get { return GetLinkValue("next"); }
            set { SetLinkValue("next", value, "下一页", OpdsMediaType.NavigationFeed); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PreviousUri
        {
            get { return GetLinkValue("previous"); }
            set { SetLinkValue("previous", value, "上一页", OpdsMediaType.NavigationFeed); }
        }

        /// <summary>
        /// 
        /// </summary>
        public new IEnumerable<OpdsItem> Items
        {
            get { return base.Items.Cast<OpdsItem>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public OpdsFeed()
        {
            InitializeOpdsNamespaces();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public OpdsFeed(IEnumerable<SyndicationItem> items)
            : base(items)
        {
            InitializeOpdsNamespaces();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SyndicationItem CreateItem()
        {
            return new OpdsItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public SyndicationLink FindLink(FeedLinkRelation relation)
        {
            return Links.SingleOrDefault(l => l.RelationshipType == relation.GetXmlEnumName());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ns"></param>
        /// <param name="value"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override bool TryParseAttribute(string name, string ns, string value, string version)
        {
            return base.TryParseAttribute(name, ns, value, version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override bool TryParseElement(XmlReader reader, string version)
        {
            return base.TryParseElement(reader, version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="version"></param>
        protected override void WriteAttributeExtensions(XmlWriter writer, string version)
        {
            base.WriteAttributeExtensions(writer, version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="version"></param>
        protected override void WriteElementExtensions(XmlWriter writer, string version)
        {
            base.WriteElementExtensions(writer, version);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeOpdsNamespaces()
        {
            foreach (var namespaceDefinition in OpdsNamespaces.GetAll())
            {
                AttributeExtensions.Add(namespaceDefinition.Key, namespaceDefinition.Value);
            }
        }
    }
}
