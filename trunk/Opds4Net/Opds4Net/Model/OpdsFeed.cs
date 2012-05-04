using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string SearchUri
        {
            get { return Links.GetLinkValue("search"); }
            set { Links.SetLinkValue("search", value, "搜索", OpdsMediaType.AcquisitionFeed); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NextUri
        {
            get { return Links.GetLinkValue("next"); }
            set { Links.SetLinkValue("next", value, "下一页", OpdsMediaType.NavigationFeed); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PreviousUri
        {
            get { return Links.GetLinkValue("previous"); }
            set { Links.SetLinkValue("previous", value, "上一页", OpdsMediaType.NavigationFeed); }
        }

        /// <summary>
        /// Total count of result set. Usually used in search result feed.
        /// </summary>
        public int TotalResults { get; set; }

        /// <summary>
        /// Page size of search result.
        /// </summary>
        public int ItemsPerPage { get; set; }

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
        /// <returns></returns>
        protected override SyndicationLink CreateLink()
        {
            return new OpdsLink();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public SyndicationLink FindLink(string relation)
        {
            return Links.SingleOrDefault(l => l.RelationshipType == relation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override bool TryParseElement(XmlReader reader, string version)
        {
            if (reader.IsReadingElementOf(OpdsNamespaces.OpenSearch.Value, "totalResults"))
            {
                TotalResults = reader.ReadElementContentAsInt();
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.OpenSearch.Value, "itemsPerPage"))
            {
                ItemsPerPage = reader.ReadElementContentAsInt();
            }
            else
            {
                return base.TryParseElement(reader, version);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="version"></param>
        protected override void WriteElementExtensions(XmlWriter writer, string version)
        {
            if (TotalResults > 0)
                writer.WriteElementString("totalResults", OpdsNamespaces.OpenSearch.Value, TotalResults.ToString());
            if (ItemsPerPage > 0)
                writer.WriteElementString("itemsPerPage", OpdsNamespaces.OpenSearch.Value, ItemsPerPage.ToString());

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
