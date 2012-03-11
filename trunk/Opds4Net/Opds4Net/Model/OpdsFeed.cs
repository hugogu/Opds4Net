using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Opds4Net.Util;

namespace Opds4Net.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsFeed : SyndicationFeed
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <returns></returns>
        public new static OpdsFeed Load(XmlReader xmlReader)
        {
            return SyndicationFeed.Load<OpdsFeed>(xmlReader);
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
        public OpdsFeed() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public OpdsFeed(IEnumerable<SyndicationItem> items)
            : base(items) { }

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

        protected override bool TryParseAttribute(string name, string ns, string value, string version)
        {
            return base.TryParseAttribute(name, ns, value, version);
        }

        protected override bool TryParseElement(XmlReader reader, string version)
        {
            return base.TryParseElement(reader, version);
        }

        protected override void WriteAttributeExtensions(XmlWriter writer, string version)
        {
            base.WriteAttributeExtensions(writer, version);
        }

        protected override void WriteElementExtensions(XmlWriter writer, string version)
        {
            base.WriteElementExtensions(writer, version);
        }
    }
}
