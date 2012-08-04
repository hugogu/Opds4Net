using System;
using System.Globalization;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Serialization;
using Opds4Net.Util;

namespace Opds4Net.Model
{
    /// <summary>
    /// Represents an opds entry.
    /// </summary>
    public class OpdsItem : SyndicationItem
    {
        #region Dublin Core Element
        /// <summary>
        /// The language of the book
        /// </summary>
        [XmlElement("language")]
        public string Language { get; set; }

        /// <summary>
        /// The time when the publisher publish the book.
        /// </summary>
        [XmlElement("issued")]
        public string Issued { get; set; }

        /// <summary>
        /// Unique identifier of the book. In the format of urn:isbn:xxxxxxxxxxxxxxxx.
        /// </summary>
        [XmlElement("identifier")]
        public string ISBN { get; set; }

        /// <summary>
        /// The publisher.
        /// </summary>
        [XmlElement("publisher")]
        public string Publisher { get; set; }

        /// <summary>
        /// Gets and sets Source property defined by Dublin Core
        /// </summary>
        [XmlElement("source")]
        public string Source { get; set; }

        /// <summary>
        /// The relevance score of the entry. Used in a search result. value range from 0 to 1.
        /// Default to -1 means not defined;
        /// </summary>
        [XmlElement("score")]
        public double Relevance { get; set; }

        /// <summary>
        /// Defined the size or duration information of the resource.
        /// In Dublin Core it is recommended to be the Content-Length of the resource.
        /// But in OPDS, the Content-Length is represents by the size attribute of acquisition Link.
        /// So the extent here is recommended to be the word count information.
        /// </summary>
        [XmlElement("extent")]
        public string Extent { get; set; }

        /// <summary>
        /// Gets the cover image url of the syndication item.
        /// </summary>
        [XmlIgnore]
        public string Cover
        {
            get { return Links.GetLinkValue(OpdsRelations.Cover); }
        }

        /// <summary>
        /// Gets the thumbnail of the syndication item.
        /// </summary>
        [XmlIgnore]
        public string Thumbnail
        {
            get { return Links.GetLinkValue(OpdsRelations.Thumbnail); }
        }

        #endregion

        /// <summary>
        /// Default constructor of OpdsItem
        /// </summary>
        public OpdsItem()
        {
            Relevance = -1.0;
            InitializeNamespaces(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public new static OpdsItem Load(XmlReader reader)
        {
            return Load<OpdsItem>(reader);
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
        /// <param name="reader"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override bool TryParseElement(XmlReader reader, string version)
        {
            if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore.Value, "language"))
            {
                Language = reader.ReadElementContentAsString();
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore.Value, "issued"))
            {
                Issued = reader.ReadElementContentAsString();
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore.Value, "identifier"))
            {
                ISBN = reader.ReadElementContentAsString();
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore.Value, "publisher"))
            {
                Publisher = reader.ReadElementContentAsString();
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.Relevance.Value, "score"))
            {
                Relevance = reader.ReadElementContentAsDouble();
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore.Value, "extent"))
            {
                Extent = reader.ReadElementContentAsString();
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore.Value, "source"))
            {
                Source = reader.ReadElementContentAsString();
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
            // There is no need to provide prefix when writing Element.
            // As long as the namespaces defined in OpdsFeed properly.
            // The prefix will be added automatically.
            if (!String.IsNullOrWhiteSpace(Language))
                writer.WriteElementString("language", OpdsNamespaces.DublinCore.Value, Language);
            if (!String.IsNullOrWhiteSpace(Issued))
                writer.WriteElementString("issued", OpdsNamespaces.DublinCore.Value, Issued);
            if (!String.IsNullOrWhiteSpace(ISBN))
            {
                writer.WriteStartElement("identifier", OpdsNamespaces.DublinCore.Value);
                if (ISBN.StartsWith("urn:"))
                {
                    writer.WriteAttributeString("type", OpdsNamespaces.Xsi.Value, "URI");
                }
                writer.WriteValue(ISBN);
                writer.WriteEndElement();
            }
            if (!String.IsNullOrWhiteSpace(Publisher))
                writer.WriteElementString("publisher", OpdsNamespaces.DublinCore.Value, Publisher);
            if (Relevance >= 0)
                writer.WriteElementString("score", OpdsNamespaces.Relevance.Value, Convert.ToString(Relevance, CultureInfo.InvariantCulture));
            if (!String.IsNullOrWhiteSpace(Extent))
                writer.WriteElementString("extent", OpdsNamespaces.DublinCore.Value, Extent);
            if (!String.IsNullOrWhiteSpace(Source))
                writer.WriteElementString("source", OpdsNamespaces.DublinCore.Value, Source);

            base.WriteElementExtensions(writer, version);
        }

        /// <summary>
        /// If you want to add your own namespaces.
        /// Use RegistNamespace of OpdsNamespaces.
        /// Because this method is called from constructor,
        /// it should not be a virtual method.
        /// </summary>
        private static T InitializeNamespaces<T>(T item)
            where T : SyndicationItem
        {
            foreach (var @namespace in OpdsNamespaces.GetAll())
            {
                item.AttributeExtensions[@namespace.Key] = @namespace.Value;
            }

            return item;
        }
    }
}
